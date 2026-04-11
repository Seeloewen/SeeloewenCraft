using LibNoise.Primitive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.commands;
using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.events;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.core.world.generation;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace SeeloewenCraft.game.core.world
{
    public class World
    {
        public List<Chunk> loadedChunkList = new List<Chunk>(); //All chunks that are currently loaded and updated
        public List<Chunk> totalChunkList = new List<Chunk>(); //All chunks that were ever loaded
        public List<IGuiData> guiData = new List<IGuiData>(); //Currently displayed guis

        public readonly SimplexPerlin heightNoise;
        public readonly SimplexPerlin humidityNoise;
        public readonly SimplexPerlin temperatureNoise;

        public Player player { get => entityManager.player; }
        public EntityManager entityManager;

        private readonly string name;
        public string gameVersion;
        public string worldDirectory = "";
        public string multiplayerDirectory = "";
        public int worldSpawnX;
        public int worldSpawnY;
        public int seed { private set; get; }

        public Gamemode gamemode = Gamemode.Survival;
        public MultiplayerType multiplayerType = MultiplayerType.OFFLINE;
        public DayTime dayTime = DayTime.DAY;

        private World(string name, int seed, MultiplayerType multiplayerType)
        {
            Game.world = this;
            this.name = name;
            this.multiplayerType = multiplayerType;

            //Load the seed
            this.seed = seed != 0 ? seed : new Random(DateTime.Now.Millisecond).Next();
            heightNoise = new SimplexPerlin();
            humidityNoise = new SimplexPerlin();
            temperatureNoise = new SimplexPerlin();

            heightNoise.Seed = this.seed;
            humidityNoise.Seed = this.seed + 1;
            temperatureNoise.Seed = this.seed - 1;

            CraftingHandler.Init();
            BlockRegister.Init();
            ItemRegister.Init();
            DungeonRoomRegister.Init();
            GameEventHandler.Init();
            ChatHandler.Init();
            CraftingHandler.LoadRecipes();

            InitWorldDirectory();
        }

        public static World CreateWorld(string name, int seed, MultiplayerType multiplayerType)
        {
            World w = new World(name, seed, multiplayerType);
            w.Start(false);
            if (NetworkHandler.IsMultiplayer()) w.InitMultiplayer();

            return w;
        }

        public static World LoadWorld(string name, MultiplayerType multiplayerType)
        {
            World w = new World(name, 0, multiplayerType);

            if (!CheckWorldVersion(w, Game.WORLD_VERSION)) throw new Exception(); //Check if the world version is compatible
            if (int.TryParse(w.LoadWorldSetting("seed"), out int s)) w.seed = s;

            w.Start(true);
            if (NetworkHandler.IsMultiplayer()) w.InitMultiplayer();

            return w;
        }

        public void Start(bool loaded)
        {
            Log.Write($"Starting game for world {name}...", LogType.WORLD_GENERATION, LogLevel.INFO);

            InitEntityManager(loaded);

            if(!loaded) CreateStartChunks(); //Create the start chunks if the world was never loaded before

            (int x, int y) playerStartPos = FindPlayerStartPos(loaded);

            //this isnt exactly true as posX=-1000 should be chunk -1 but is loaded as chunk 0,
            //however in first rendering tick it should get fixed automatically
            LoadStartChunks(playerStartPos.x / 8000);

            CreatePlayer(playerStartPos.x, playerStartPos.y);
            player.currentChunk = playerStartPos.x / 8000;

            if (StartOptions.startCreative) SetGamemode(Gamemode.Creative);

            //Load the player inventory if the world is not new
            InitPlayerInventory(loaded);

            GameEventHandler.Register(new DayNightCycleEvent());
            GameEventHandler.Register(new AutoSaveEvent(Settings.autoSaveInterval * 60000));

            //When the world is loaded, display the debug information
            DisplayDebugInformation();

            SaveWorldSettings();
            Log.Write($"Successfully initialized game for world {name}!", LogType.GENERAL, LogLevel.INFO);
        }

        private void InitMultiplayer()
        {
            if (multiplayerType == MultiplayerType.SERVER)
            {
                NetworkHandler.StartServer(5000);
                GameEventHandler.Register(new ClientConnectedCheckEvent());
            }
            else if (multiplayerType == MultiplayerType.CLIENT)
            {
                GameEventHandler.Register(new SendConnectionStateEvent());
            }

            GameEventHandler.Register(new EntitySyncEvent());
        }

        public static World Get() => Game.world;

        private static bool CheckWorldVersion(World w, int currentVersion) //return true if proceed
        {
            //Try loading the world version
            int worldVersion;
            if (File.Exists($"{w.worldDirectory}/world_settings.json"))
            {
                JObject settingsObj = JsonUtil.ObjectFromFile($"{w.worldDirectory}/world_settings.json");
                worldVersion = settingsObj.Get<int>("world_version");
            }
            else if (File.Exists($"{w.worldDirectory}/settings.txt"))
            {
                Log.Write("Detected old saving system, can't load the world. Please use an older version of the game", LogType.GENERAL, LogLevel.ERROR);
                return false;
            }
            else
            {
                Log.Write("Could not read world version from settings file because the settings file was not found", LogType.GENERAL, LogLevel.ERROR);
                return false;
            }

            //If the world version is outdated, show are warning as it may cause corruption
            if (worldVersion < currentVersion)
            {
                MessageBoxResult result = MessageBox.Show("You are trying to load an outdated world. This may lead to corruption or other issues. You have been warned! Do you wish to continue?", "Load outdated world", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Log.Write("You are loading an outdated world, this may cause issues or corruption", LogType.GENERAL, LogLevel.WARNING);
                        return true;
                }
            }

            return false;
        }

        public void SetGamemode(Gamemode gamemode)
        {
            this.gamemode = gamemode;
            player.SetGamemode(gamemode);
        }

        public void SaveEntities()
        {
            entityManager.ToJson().ToFile($"{worldDirectory}/entities.json");
        }

        public void Save()
        {
            if (!NetworkHandler.IsClient())
            {
                //Save all chunks and entities
                totalChunkList.ForEach(c => c.Save());

                if (NetworkHandler.IsServer()) NetworkHandler.SendData(MultiplayerPacketType.REQUEST, "player_information", "");

                player.SaveInventory(worldDirectory);
                player.SavePosition(worldDirectory);
                SaveEntities();
            }
        }

        public string LoadWorldSetting(string settingName)
        {
            //Check if the world settings file exists and retrieve the value
            if (File.Exists($"{worldDirectory}/world_settings.json"))
            {
                JObject settingsObj = JsonUtil.ObjectFromFile($"{worldDirectory}/world_settings.json");
                if (settingsObj.ContainsKey(settingName))
                {
                    return settingsObj.Get<string>($"{settingName}");
                }
            }

            return null;
        }

        public void AddEntity(Entity entity, bool networkCall = false)
        {
            if (entity.id == player.id && networkCall) return;

            //Save the entity and send it to others on the network if necessary
            entityManager.Add(entity);

            if (networkCall) return;
            NetworkHandler.SendData(MultiplayerPacketType.CREATE_ENTITY, $"{entity.ToJson()}");
        }

        public void RemoveEntity(int id) => entityManager.Remove(id);

        public void SetBlock(int posX, int posY, Block b) => SetBlock((posX % 8 + 8) % 8, posY, posX >= 0 ? posX / 8 : (posX - 7) / 8, b);
        public void SetBlock(int cPosX, int posY, int cIndex, Block b) => GetChunk(cIndex).SetBlock(b, cPosX, posY);
        public void SetBlock(PositionData posData, Block b) => SetBlock(posData.x, posData.y, posData.ci, b);

        public Block GetBlock(int posX, int posY) //global x
        {
            int chunkIndex = posX >= 0
                ? posX / 8
                : (posX - 7) / 8;
            int x = (posX % 8 + 8) % 8;
            Chunk c = GetChunk(chunkIndex);
            if(c == null) return null;
            Block b =  c.GetBlock(x, posY);

            return b;
        }

        public Block GetBlock(int posX, int posY, int cIndex)
        {
            Chunk c = GetChunk(cIndex);
            Block b = c.GetBlock(posX, posY);

            return b;
        }

        public Chunk CreateChunk(int index)
        {
            Chunk newChunk = new Chunk(index);
            totalChunkList.Add(newChunk);
            return newChunk;
        }

        //Searches the total chunk list for the chunk with the specified index. If not found, create a new one
        public Chunk GetChunk(int index) => totalChunkList.Find(c => c.index == index);

        private Chunk GetLoadedChunk(int index) => loadedChunkList.Find(c => c.index == index); //Only used internally 

        internal void LoadChunk(Chunk chunk) => loadedChunkList.Add(chunk);

        internal void UnloadChunk(Chunk chunk) => loadedChunkList.Remove(chunk);

        private void SaveWorldSettings()
        {
            //write world version to settings.json
            JObject obj = new JObject()
            {
                { "world_version", Game.WORLD_VERSION},
                { "seed", seed}
            };
            obj.ToFile($"{worldDirectory}/world_settings.json");
        }

        private void InitWorldDirectory()
        {
            //Check if the world directory exists and create it otherwise
            if (multiplayerType != MultiplayerType.CLIENT)
            {
                if (!Directory.Exists($"{FolderUtil.worldsFolder}\\{name}"))
                {
                    Directory.CreateDirectory($"{FolderUtil.worldsFolder}\\{name}");
                    Log.Write($"Created directory for world {name} ({FolderUtil.worldsFolder}\\{name})", LogType.GENERAL, LogLevel.INFO);
                }
                worldDirectory = $"{FolderUtil.worldsFolder}\\{name}";
                Log.Write($"Set directory for world {name} to {worldDirectory}", LogType.GENERAL, LogLevel.INFO);
            }

            //Check if the world's multiplayer directory exists and create it otherwise
            if (multiplayerType == MultiplayerType.SERVER)
            {
                if (!Directory.Exists($"{worldDirectory}\\multiplayer"))
                {
                    Directory.CreateDirectory($"{worldDirectory}\\multiplayer");
                    Log.Write($"Created multiplayer directory for world {name} ({worldDirectory}\\multiplayer)", LogType.NETWORK, LogLevel.INFO);
                }
                multiplayerDirectory = $"{worldDirectory}\\multiplayer";
                Log.Write($"Set multiplayer directory for world {name} to {multiplayerDirectory}", LogType.GENERAL, LogLevel.INFO);
            }
        }

        private void InitEntityManager(bool loaded)
        {
            entityManager = loaded
                    ? new EntityManager(JsonUtil.ObjectFromFile($"{worldDirectory}/entities.json"))
                    : new EntityManager();
        }

        private void InitPlayerInventory(bool loaded)
        {
            if (loaded)
            {
                JObject inventoryToken = JsonUtil.ObjectFromFile($"{worldDirectory}/player_inventory.json");
                player.inventory = Inventory.FromJson(inventoryToken, true);
            }
            else
            {
                player.inventory = new Inventory(9, 4);
                player.inventory.InitHotbar();
            }
        }

        public void CreatePlayer(int playerPosX, int playerPosY)
        {
            entityManager.player = new Player(playerPosX, playerPosY);
            entityManager.player.SetId(Game.playerId);
            AddEntity(player);

            Log.Write($"Created player at position x{playerPosX} y{playerPosY}", LogType.GENERAL, LogLevel.INFO);
        }

        private (int x, int y) FindPlayerStartPos(bool loaded)
        {
            //Load the player position if possible
            if (loaded)
            {
                JObject playerPosObj = JsonUtil.ObjectFromFile($"{worldDirectory}/player_position.json");
                int x = playerPosObj.Get<int>("pos_x");
                int y = playerPosObj.Get<int>("pos_y");
                Log.Write($"Loaded player start position at x{x} y{y}", LogType.GENERAL, LogLevel.INFO);
                return (x, y);
            }
            else
            {
                //Calculate y position where the player starts
                int yPos = 0;

                foreach (Block block in GetChunk(3).blockList.blocks)
                {
                    if (block.posX == 4 && block.isSolid)
                    {
                        yPos = block.posY;
                        break;
                    }
                }

                yPos = Math.Max((yPos * 1000) - 1900, 2000);

                Log.Write($"Found player spawn point at position x{28050} y{yPos}", LogType.GENERAL, LogLevel.INFO);
                worldSpawnX = 28050;
                worldSpawnY = yPos;
                return (28050, yPos);
            }
        }

        private void LoadStartChunks(int midChunk)
        {
            for (int i = midChunk - 3; i <= midChunk + 3; i++)
            {
                Chunk c = GetChunk(i);
                LoadChunk(c);
            }
        }

        private void CreateStartChunks()
        {
            for (int i = 0; i <= 7; i++)
            {
                Chunk c = CreateChunk(i);
            }
        }

        public void MoveLoadedChunks(Direction dir)
        {
            if (dir.IsRight())
            {
                player.currentChunk++;

                Chunk chunk = GetLoadedChunk(loadedChunkList[0].index);
                UnloadChunk(chunk);

                int newIndex = loadedChunkList[5].index + 1;
                Chunk newChunk = GetChunk(newIndex) ?? CreateChunk(newIndex);
                LoadChunk(newChunk);

                //Sort the chunklist again
                loadedChunkList = loadedChunkList.OrderBy(obj =>  obj.index).ToList();

                //Send the chunk on the network
                NetworkHandler.SendData(MultiplayerPacketType.CREATE_CHUNK, $"{newChunk.index}");

                //If it's a server, additionally send the chunk to all clients
                if (NetworkHandler.IsServer())
                {
                    foreach (Block block in newChunk.blockList.blocks)
                    {
                        NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, block.id.ToString(), newChunk.index.ToString(), block.posX.ToString(), block.posY.ToString());
                    }
                }
            }
            else if (dir.IsLeft())
            {
                player.currentChunk--;

                Chunk chunk = GetLoadedChunk(loadedChunkList[6].index);
                UnloadChunk(chunk);

                int newIndex = loadedChunkList[0].index - 1;
                Chunk newChunk = GetChunk(newIndex) ?? CreateChunk(newIndex);
                LoadChunk(newChunk);

                //Sort the list again
                loadedChunkList = loadedChunkList.OrderBy(obj => obj.index).ToList();

                //Send the chunk on the network
                NetworkHandler.SendData(MultiplayerPacketType.CREATE_CHUNK, $"{newChunk.index}");

                //If it's a server, additionally send the chunk to all clients
                if (NetworkHandler.IsServer())
                {
                    foreach (Block block in newChunk.blockList.blocks)
                    {
                        NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, block.id.ToString(), newChunk.index.ToString(), block.posX.ToString(), block.posY.ToString());
                    }
                }
            }
        }

        public void DisplayDebugInformation()
        {
            //Show the debug information for the world in the debug menu
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "Version", $"{Game.GAME_VERSION} ({Game.VERSION_DATE})");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "worldName", $"{name}");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "worldVersion", $"{Game.WORLD_VERSION}");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "seed", $"{seed}");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "fps");
        }

        public void SetDayTime(DayTime dayTime)
        {
            this.dayTime = dayTime;

            //Change skycolor of the renderer based on the current day time
            Renderer.skyColor = dayTime switch
            {
                DayTime.SUNRISE1 => SkyColors.SUNRISE1_SUNSET4_COLOR,
                DayTime.SUNRISE2 => SkyColors.SUNRISE2_SUNSET3_COLOR,
                DayTime.SUNRISE3 => SkyColors.SUNRISE3_SUNSET2_COLOR,
                DayTime.SUNRISE4 => SkyColors.SUNRISE4_SUNSET1_COLOR,
                DayTime.SUNSET1 => SkyColors.SUNRISE4_SUNSET1_COLOR,
                DayTime.SUNSET2 => SkyColors.SUNRISE3_SUNSET2_COLOR,
                DayTime.SUNSET3 => SkyColors.SUNRISE2_SUNSET3_COLOR,
                DayTime.SUNSET4 => SkyColors.SUNRISE1_SUNSET4_COLOR,
                DayTime.NIGHT => SkyColors.NIGHT_COLOR,
                _ => SkyColors.DAY_COLOR
            };
        }

        public void Update(double dt, bool blockUpdate)
        {
            GameEventHandler.Update(dt);
            CraftingHandler.Update(dt);

            if (blockUpdate)
            {
                foreach (Chunk c in loadedChunkList)
                {
                    foreach (Block block in c.blockList.blocks)
                    {
                        block.DoUpdate(1 / 20d);
                    }
                }
            }

            entityManager.DoStep((int)(1.0 / dt));

            GameCamera.SetCamCenterPhysicsCoord(player.posX + 237, player.posY + 950);

            int chunk = player.GetChunkIndex();
            if (chunk != player.currentChunk)
            {
                if (chunk > player.currentChunk)
                {
                    MoveLoadedChunks(Direction.RIGHT);
                    Log.Write($"move chunks right {chunk} {player.currentChunk}", LogType.GENERAL, LogLevel.WARNING);
                }
                else
                {
                    MoveLoadedChunks(Direction.LEFT);
                    Log.Write($"move chunks left {chunk} {player.currentChunk}", LogType.GENERAL, LogLevel.WARNING);
                }
            }
        }
    }
}
