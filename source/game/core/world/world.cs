using DocumentFormat.OpenXml.Drawing.Charts;
using LibNoise.Primitive;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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
using SeeloewenCraft.game.notifications;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        public NotificationManager notificationManager;

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
            notificationManager = new NotificationManager();

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

            if (!CheckWorldVersion(w, Game.WORLD_VERSION)) throw new Exception($"Incompatible world version."); //Check if the world version is compatible
            if (int.TryParse(w.LoadWorldSetting("seed"), out int s)) w.seed = s;

            w.Start(true);
            if (NetworkHandler.IsMultiplayer()) w.InitMultiplayer();

            return w;
        }

        public void Start(bool loaded)
        {
            Log.Write($"Starting game for world {name}...", LogType.WORLD_GENERATION, LogLevel.INFO);

            InitEntityManager(loaded);

            (int x, int y) playerStartPos;

            if (!loaded)
            {
                CreateSpawnChunks(3);
                playerStartPos = CalcPlayerStartPos();
            }
            else
            { 
                playerStartPos = LoadPlayerStartPos();
                CreateSpawnChunks(playerStartPos.x / 8000);
            }

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
            if (File.Exists(Path.Combine(w.worldDirectory, "world_settings.json")))
            {
                JObject settingsObj = JsonUtil.ObjectFromFile(Path.Combine(w.worldDirectory, "world_settings.json"));
                worldVersion = settingsObj.Get<int>("world_version");
            }
            else if (File.Exists(Path.Combine(w.worldDirectory, "settings.txt")))
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
                var result = ShowOutdatedDialog().GetAwaiter().GetResult();

                switch (result)
                {
                    case ButtonResult.Yes:
                        Log.Write("You are loading an outdated world, this may cause issues or corruption", LogType.GENERAL, LogLevel.WARNING);
                        return true;
                }
            }

            return true;
        }

        private static async Task<ButtonResult> ShowOutdatedDialog()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Load outdated world",
                "You are trying to load an outdated world. This may lead to corruption or other issues. You have been warned! Do you wish to continue?",
                ButtonEnum.YesNo, Icon.Warning);

            return await box.ShowAsync();
        }

        public void SetGamemode(Gamemode gamemode)
        {
            this.gamemode = gamemode;
            player.SetGamemode(gamemode);
        }

        public void SaveEntities()
        {
            entityManager.ToJson().ToFile(Path.Combine(worldDirectory, "entities.json"));
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
            if (File.Exists(Path.Combine(worldDirectory, "world_settings.json")))
            {
                JObject settingsObj = JsonUtil.ObjectFromFile(Path.Combine(worldDirectory, "world_settings.json"));
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

        public void SetBlock(PositionData posData, Block b) => GetChunk(posData.ci).SetBlock(b, posData.x, posData.y);

        public Block GetBlock(PositionData pos)
        {
            Chunk c = GetChunk(pos.ci);
            if (c == null) return null;

            Block b = c.GetBlock(pos.x, pos.y);
            return b;
        }

        public Chunk CreateChunk(int index)
        {
            Chunk newChunk = new Chunk(index);
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
            obj.ToFile(Path.Combine(worldDirectory, "world_settings.json"));
        }

        private void InitWorldDirectory()
        {
            //Check if the world directory exists and create it otherwise
            if (multiplayerType != MultiplayerType.CLIENT)
            {
                if (!Directory.Exists(Path.Combine(FolderUtil.worldsFolder, name)))
                {
                    Directory.CreateDirectory(Path.Combine(FolderUtil.worldsFolder, name));
                    Log.Write($"Created directory for world {name} ({FolderUtil.worldsFolder}/{name})", LogType.GENERAL, LogLevel.INFO);
                }
                worldDirectory = Path.Combine(FolderUtil.worldsFolder, name);
                Log.Write($"Set directory for world {name} to {worldDirectory}", LogType.GENERAL, LogLevel.INFO);
            }

            //Check if the world's multiplayer directory exists and create it otherwise
            if (multiplayerType == MultiplayerType.SERVER)
            {
                if (!Directory.Exists(Path.Combine(FolderUtil.worldsFolder, "multiplayer")))
                {
                    Directory.CreateDirectory(Path.Combine(FolderUtil.worldsFolder, "multiplayer"));
                    Log.Write($"Created multiplayer directory for world {name} ({worldDirectory}/multiplayer)", LogType.NETWORK, LogLevel.INFO);
                }
                multiplayerDirectory = Path.Combine(FolderUtil.worldsFolder, "multiplayer");
                Log.Write($"Set multiplayer directory for world {name} to {multiplayerDirectory}", LogType.GENERAL, LogLevel.INFO);
            }
        }

        private void InitEntityManager(bool loaded)
        {
            entityManager = loaded
                    ? new EntityManager(JsonUtil.ObjectFromFile(Path.Combine(worldDirectory, "entities.json")))
                    : new EntityManager();
        }

        private void InitPlayerInventory(bool loaded)
        {
            if (loaded)
            {
                JObject inventoryToken = JsonUtil.ObjectFromFile(Path.Combine(worldDirectory, "player_inventory.json"));
                player.inventory = Inventory.FromJson(inventoryToken, true);
            }
            else
            {
                player.inventory = new Inventory(9, 4, isPlayerInv: true);
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

        //When creating a world
        private (int x, int y) CalcPlayerStartPos()
        {
            //Calculate y position where the player starts
            int posY = 0;

            foreach (Block block in GetChunk(3).blockList.blocks)
            {
                if (block.posX == 4 && block.isSolid)
                {
                    posY = block.posY;
                    break;
                }
            }

            posY = Math.Max((posY * 1000) - 1900, 2000);

            Log.Write($"Found player spawn point at position x{28050} y{posY}", LogType.GENERAL, LogLevel.INFO);
            worldSpawnX = 28050;
            worldSpawnY = posY;
            return (28050, posY);
        }

        //When loading a world
        private (int x, int y) LoadPlayerStartPos()
        {
            JObject playerPosObj = JsonUtil.ObjectFromFile(Path.Combine(worldDirectory, "player_position.json"));
            int x = playerPosObj.Get<int>("pos_x");
            int y = playerPosObj.Get<int>("pos_y");
            Log.Write($"Loaded player start position at x{x} y{y}", LogType.GENERAL, LogLevel.INFO);
            return (x, y);
        }

        private void LoadStartChunks(int midChunk)
        {
            for (int i = midChunk - 3; i <= midChunk + 3; i++)
            {
                Chunk c = GetChunk(i);
                LoadChunk(c);
            }
        }

        private void CreateSpawnChunks(int midChunk)
        {
            for (int i = midChunk - 3; i <= midChunk + 3; i++)
            {
                CreateChunk(i);
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
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "version", $"{Game.GAME_VERSION} ({Game.VERSION_DATE})");
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
            notificationManager.Update(dt);

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
                }
                else
                {
                    MoveLoadedChunks(Direction.LEFT);
                }
            }
        }
    }
}
