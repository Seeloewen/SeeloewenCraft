using System;
using System.Collections.Generic;
using static System.Environment;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using SeeloewenCraft.entity;
using System.Windows.Documents;
using SeeloewenCraft.game.graphics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace SeeloewenCraft
{
    public class World
    {

        //References
        public wndMenu wndMenu;
        public System.Windows.Forms.Timer tmrMovement = new System.Windows.Forms.Timer();
        public List<Chunk> loadedChunkList = new List<Chunk>();
        public List<Chunk> totalChunkList = new List<Chunk>();
        public List<Inventory> inventoryList = new List<Inventory>();
        public List<Gui> guiList = new List<Gui>();
        public List<CraftingRecipe> craftingRecipeList = new List<CraftingRecipe>();
        public Player player { get => entityManager.player; }
        public List<IGuiData> guiDatas = new List<IGuiData>();
        public WaterHandler waterHandler;
        public ClickHandler clickHandler;
        public DebugMenu_old debugMenu;
        public GameLoop gameLoop;
        public RecipeCreator recipeCreator;
        public EntityManager entityManager;


        //Constants
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);
        private string worldName;
        public int worldVersion;
        public string gameVersion;
        public string worldDirectory = "";
        public string multiplayerDirectory = "";
        public int lightRange = 7; //The range that all light sources use
        public int worldSpawnX;
        public int worldSpawnY;
        public int seed;

        //Variables
        public bool finishedLoading = false;
        public bool returnToMenu = false;
        public bool showBlockInfo = false;
        public int nightState = 0;
        public Gamemode gamemode = Gamemode.Survival;
        public MultiplayerType multiplayerType;
        public int currentChunk; //idk if this works in mp

        //-- Constructor --//

        public World(wndMenu wndMenu, string worldName, int seed, bool isNew, int worldVersion, string gameVersion, MultiplayerType multiplayerType)
        {


            int i = GL.GenTexture();

            //Set world name and create game and links
            this.worldName = worldName;
            this.worldVersion = worldVersion;
            this.gameVersion = gameVersion;
            this.wndMenu = wndMenu;
            this.seed = seed;
            this.multiplayerType = multiplayerType;

            if (seed == 0)
            {
                this.seed = new Random(DateTime.Now.Millisecond).Next();
            }

            Game.world = this;


            //Create objects
            waterHandler = new WaterHandler();
            clickHandler = new ClickHandler();
            debugMenu = new DebugMenu_old();
            gameLoop = new GameLoop(25);
            recipeCreator = new RecipeCreator();

            Renderer.Init();

            //Actually initialize the game
            InitGame(worldName, isNew, worldVersion);

            NotificationHandler.Init();

            if (StartOptions.startCreative)
            {
                SetGamemode(Gamemode.Creative);
            }

            //Only start a server when requested
            if (multiplayerType == MultiplayerType.SERVER)
            {
                NetworkHandler.StartServer(5000);
            }

            //Game.world.wndGame.Show();

            GLFW.Init();

        }

        //-- Custom Methods --//

        //return true if proceed
        private bool CheckWorldVersion(int currentWorldVersion)
        {
            int worldVersion;
            if (File.Exists($"{worldDirectory}/world_settings.json"))
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/world_settings.json");

                worldVersion = documentToken.GetInt("/world_version");
            }
            else if (File.Exists($"{worldDirectory}/settings.txt"))
            {
                Log.Write("Detected old saving system, can't load the world. Please use an older version of the game", LogType.GENERAL, LogLevel.ERROR);
                return false;
            }
            else
            {
                Log.Write("Could not read world version from settings file because the settings file was not found", LogType.GENERAL, LogLevel.ERROR);
                return false;
            }

            if (worldVersion < currentWorldVersion)
            {
                MessageBoxResult result = MessageBox.Show("You are trying to load an outdated world. This may lead to corruption or other issues. You have been warned! Do you wish to continue?", "Load outdated world", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Log.Write("You are loading an outdated world, this may cause issues or corruption", LogType.GENERAL, LogLevel.WARNING);
                        return true;
                }
                return false;
            }
            return true;
        }

        public void InitGame(string worldName, bool isNew, int worldVersion)
        {
            Log.Write($"Initializing game for world {worldName}...", LogType.WORLD_GENERATION, LogLevel.INFO);

            InitWorldDirectory();

            //Try to load the seed if it is not a new world
            if (!isNew)
            {
                string? seedString = LoadWorldSetting("seed");

                seed = seedString != null ? int.Parse(seedString) : new Random(DateTime.Now.Millisecond).Next();
            }

            recipeCreator.CreateRecipes();

            RoomLibrary.CreateDungeonRooms();

            if (!isNew && !CheckWorldVersion(worldVersion))
            {
                throw new Exception();
            }

            SaveWorldSettings();

            InitEntityManager(!isNew);

            (int x, int y) playerStartPos = FindPlayerStartPos(!isNew);

            //this isnt exactly true as posX=-1000 should be chunk -1 but is loaded as chunk 0,
            //however in first rendering tick it should get fixed automatically
            LoadStartChunks(playerStartPos.x / 8000);

            CreatePlayer(playerStartPos.x, playerStartPos.y);

            //When the world is loaded, display the debug information
            DisplayDebugInformation();

            //Load the player inventory if the world is not new
            InitPlayerInventory(!isNew);





            Log.Write($"Successfully initialized game for world {worldName}!", LogType.GENERAL, LogLevel.INFO);




            //Start the main timer

            //Start the game loop timer
            gameLoop.Start();

            finishedLoading = true;
        }

        public void SetGamemode(Gamemode gamemode)
        {
            this.gamemode = gamemode;
            player.SetGamemode(gamemode);
        }

        public void SaveEntities()
        {
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.WriteStartObject();
                entityManager.SaveToJson(writer);
                writer.WriteEndObject();
                writer.WriteToFile($"{worldDirectory}/entities.json");
            }
        }

        public void Save()
        {
            if (!Game.IsClient())
            {
                //Save all chunks and the inventory of the player
                foreach (Chunk chunk in Game.world.totalChunkList)
                {
                    //Stop all running crafting timers
                    foreach (Block block in chunk.blockList.blocks)
                    {
                        if (block.craftingHandler != null && block.craftingHandler.tmrCrafting.IsRunning)
                        {
                            block.craftingHandler.tmrCrafting.Stop();
                        }
                    }

                    chunk.Save();
                }

                if (Game.IsServer())
                {
                    NetworkHandler.SendData(MultiplayerPacketType.REQUEST, "player_information", "");
                }

                player.SaveInventory(worldDirectory);
                player.SavePosition(worldDirectory);
                SaveEntities();
            }
        }

        public string? LoadWorldSetting(string settingName)
        {
            if (File.Exists($"{worldDirectory}/world_settings.json"))
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/world_settings.json");
                if (documentToken.ContainsKey(settingName))
                {
                    return documentToken.GetString($"/{settingName}");
                }
            }

            return null;
        }


        public void AddEntity(Entity entity)
        {
            /*//Game.world.wndGame.cvsWorld.Children.Add(entity.texture);
            Panel.SetZIndex(entity.texture, 1);
            worldRenderer.AddEntity(entity);*/

            using (JsonWriter writer = JsonWriter.Create())
            {
                entity.SaveToJson(writer);
                NetworkHandler.SendData(MultiplayerPacketType.CREATE_ENTITY, $"{writer.ToString()}");
            }

            entityManager.Add(entity);
        }

        public void AddEntity_Multiplayer(Entity entity)
        {
            if (entity.id == player.id)
            {
                return;
            }

            entityManager.Add(entity);
        }

        public void RemoveEntity(int id)
        {
            entityManager.Remove(id);
        }

        public void SetBlock(Block block, int posX, int posY)
        {
            GetBlock(posX, posY).SetBlock(block);
        }

        public void SetBlock_Multiplayer(Block block, int cIndex, int x, int y)
        {
            //Check if the chunk exists before placing a block there, if not, create it
            if (GetTotalChunk(cIndex) == null)
            {
                CreateChunk(cIndex);
            }

            //Set the block in the chunk
            GetTotalChunk(cIndex).SetBlock(block, x, y);
        }

        public Chunk CreateChunk(int index)
        {
            Chunk newChunk = new Chunk(index);
            totalChunkList.Add(newChunk);
            return newChunk;
        }

        public void UnloadChunk(Chunk chunk)
        {
            loadedChunkList.Remove(chunk);
        }


        public void LoadChunk(Chunk chunk)
        {
            loadedChunkList.Add(chunk);
        }

        public Chunk LoadChunk(int index)
        {
            foreach (Chunk chunk in totalChunkList)
            {
                if (chunk.index == index)
                {
                    loadedChunkList.Add(chunk);
                    return chunk;
                }
            }
            return CreateChunk(index);
        }

        public Chunk GetChunk(int index)
        {
            //Searches the total chunk list for the chunk with the specified index. If not found, create a new one
            foreach (Chunk chunk in totalChunkList)
            {
                if (chunk.index == index)
                {
                    return chunk;
                }
            }
            return CreateChunk(index);
        }

        public Block GetBlock(int posX, int posY)
        {
            int chunkIndex = posX >= 0
                ? posX / 8
                : (posX - 7) / 8;
            int x = (posX % 8 + 8) % 8;
            Chunk c = GetChunk(chunkIndex);
            Block b = c.GetBlock(x, posY);

            return b;
        }



        private void SaveWorldSettings()
        {
            //write world version to settings.json
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("world_version");
                writer.WriteValue(worldVersion);

                writer.WritePropertyName("seed");
                writer.WriteValue(seed);

                writer.WriteEndObject();

                writer.WriteToFile($"{worldDirectory}/world_settings.json");
            }
        }

        private void InitWorldDirectory()
        {
            //Check if the world directory exists and create it otherwise
            if (multiplayerType != MultiplayerType.CLIENT)
            {
                if (!Directory.Exists($"{FolderUtil.worldsFolder}\\{worldName}"))
                {
                    Directory.CreateDirectory($"{FolderUtil.worldsFolder}\\{worldName}");
                    Log.Write($"Created directory for world {worldName} ({FolderUtil.worldsFolder}\\{worldName})", LogType.GENERAL, LogLevel.INFO);
                }
                worldDirectory = $"{FolderUtil.worldsFolder}\\{worldName}";
                Log.Write($"Set directory for world {worldName} to {worldDirectory}", LogType.GENERAL, LogLevel.INFO);
            }

            if (multiplayerType == MultiplayerType.SERVER)
            {
                //Check if the world's multiplayer directory exists and create it otherwise
                if (!Directory.Exists($"{worldDirectory}\\multiplayer"))
                {
                    Directory.CreateDirectory($"{worldDirectory}\\multiplayer");
                    Log.Write($"Created multiplayer directory for world {worldName} ({worldDirectory}\\multiplayer)", LogType.NETWORK, LogLevel.INFO);
                }
                multiplayerDirectory = $"{worldDirectory}\\multiplayer";
                Log.Write($"Set multiplayer directory for world {worldName} to {multiplayerDirectory}", LogType.GENERAL, LogLevel.INFO);
            }
        }

        private void InitEntityManager(bool loaded)
        {
            entityManager = loaded
                    ? new EntityManager(JsonUtil.ReadFile($"{worldDirectory}/entities.json"))
                    : new EntityManager();
        }

        private void InitPlayerInventory(bool loaded)
        {
            if (loaded)
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/player_inventory.json");
                player.inventory = Inventory.LoadFromJson(documentToken, true);
            }
            else
            {
                player.inventory = new Inventory(9, 4, true);
                player.inventory.InitHotbar();
                player.inventory.AddItem("sc:diamond_scythe_item", 1, "durability=507");
                player.inventory.AddItem("sc:diamond_hammer_item", 1, "durability=507");
                player.inventory.AddItem("sc:dirt_item", 64, "");
            }

            inventoryList.Add(player.inventory);
            player.inventory.hotbarSlotList[0].Select();
        }

        public bool HasOpenGui(bool ignoreInventory)
        {
            foreach (Gui gui in guiList)
            {
                if (gui.isOpen)
                {
                    if (gui.id == "sc:inventory")
                    {
                        if (!ignoreInventory)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CreatePlayer(int playerPosX, int playerPosY)
        {
            entityManager.player = new Player(playerPosX, playerPosY);
            entityManager.player.SetId(Game.playerId);
            entityManager.player.tblId.SetAlignedText(Settings.nickname);
            AddEntity(player);

            //Game.world.wndGame.relativeSvPos = //Game.world.wndGame.svWorld.VerticalOffset;
            //Game.world.wndGame.defaultSvPos = //Game.world.wndGame.svWorld.VerticalOffset;

            Log.Write($"Created player at position x{playerPosX} y{playerPosY}", LogType.GENERAL, LogLevel.INFO);
        }

        private (int x, int y) FindPlayerStartPos(bool loaded)
        {

            //Load the player position if possible
            if (loaded)
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/player_position.json");
                int x = documentToken.GetInt("/pos_x");
                int y = documentToken.GetInt("/pos_y");
                Log.Write($"Loaded player start position at x{x} y{y}", LogType.GENERAL, LogLevel.INFO);
                return (x, y);
            }
            else
            {
                //Calculate y position where the player starts
                int yPos = 0;
                foreach (Block block in GetChunk(3).blockList.blocks)
                {
                    if (block.xPos == 4 && block.isSolid)
                    {
                        yPos = block.yPos;
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
            currentChunk = midChunk;
            for (int i = midChunk - 3; i <= midChunk + 3; i++)
            {
                Chunk c = GetChunk(i);
                LoadChunk(c);
            }
        }

        public async void MoveLoadedChunks(Direction dir)
        {
            if (dir.IsRight())
            {
                currentChunk++;

                Chunk chunk = Game.world.GetLoadedChunk(Game.world.loadedChunkList[0].index);
                Game.world.UnloadChunk(chunk);
                Chunk newChunk = Game.world.GetChunk(Game.world.loadedChunkList[5].index + 1);
                Game.world.LoadChunk(newChunk);

                //Sort the chunklist again
                Game.world.loadedChunkList = Game.world.loadedChunkList.OrderBy(obj => obj.index).ToList();

                //Send the chunk on the network
                NetworkHandler.SendData(MultiplayerPacketType.CREATE_CHUNK, $"{newChunk.index}");

                //If it's a server, additionally send the chunk to all clients
                if (Game.IsServer())
                {
                    foreach (Block block in newChunk.blockList.blocks)
                    {
                        NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, block.id.ToString(), newChunk.index.ToString(), block.xPos.ToString(), block.yPos.ToString());
                    }
                }
            }
            else if (dir.IsLeft())
            {
                currentChunk--;
                Chunk chunk = Game.world.GetLoadedChunk(Game.world.loadedChunkList[6].index);
                Game.world.UnloadChunk(chunk);
                Chunk newChunk = Game.world.GetChunk(Game.world.loadedChunkList[0].index - 1);
                Game.world.LoadChunk(newChunk);

                //Sort the list again
                Game.world.loadedChunkList = Game.world.loadedChunkList.OrderBy(obj => obj.index).ToList();

                //Send the chunk on the network
                NetworkHandler.SendData(MultiplayerPacketType.CREATE_CHUNK, $"{newChunk.index}");

                //If it's a server, additionally send the chunk to all clients
                if (Game.IsServer())
                {
                    foreach (Block block in newChunk.blockList.blocks)
                    {
                        NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, block.id.ToString(), newChunk.index.ToString(), block.xPos.ToString(), block.yPos.ToString());
                    }
                }
            }
        }

        public Chunk GetLoadedChunk(int index)
        {
            //Returns a chunk from the list based on the given index
            foreach (Chunk chunk in loadedChunkList)
            {
                if (chunk.index == index)
                {
                    return chunk;
                }
            }
            return null;
        }

        public Chunk GetTotalChunk(int index)
        {
            //Returns a chunk from the list based on the given index
            foreach (Chunk chunk in totalChunkList)
            {
                if (chunk.index == index)
                {
                    return chunk;
                }
            }
            return null;
        }

        public void DisplayDebugInformation()
        {
            //Show the debug information for the world in the debug menu
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "Version", $"{Game.GAME_VERSION} ({Game.VERSION_DATE})");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "worldName", $"{worldName}");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "worldVersion", $"{worldVersion}");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "seed", $"{seed}");
            DebugMenu.AddLine(DebugMenu.Section.WORLD, "fps");

        }

        public void SetNight(int nightState)
        {
            this.nightState = nightState;

            //TODO change light levels

            //Set background color of world canvas based on night state
            switch (nightState)
            {
                case 0:
                    //Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 188, 244, 247));
                    break;
                case 1:
                    //Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 150, 195, 198));
                    break;
                case 2:
                    //Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 113, 146, 148));
                    break;
                case 3:
                    //Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 75, 98, 99));
                    break;
                case 4:
                    //Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 38, 49, 49));
                    break;
                case 5:
                    //Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 10, 12, 13));
                    break;
            }
        }

        public InventorySlot GetSelectedInvSlot()
        {
            InventorySlot selectedSlot;

            foreach (Inventory inventory in inventoryList)
            {
                selectedSlot = inventory.GetSelectedInvSlot();
                if (selectedSlot != null)
                {
                    return selectedSlot;
                }
            }

            return null;
        }

        //-- Event Handlers --//

        public void doGameTick(double dt, bool blockUpdate)
        {


            if(blockUpdate)
            {
                foreach (Chunk chunk in loadedChunkList)
                {
                    foreach (Block block in chunk.blockList.blocks)
                    {
                        block.DoUpdate();
                    }
                }
            }

            entityManager.DoStep((int)(1.0 / dt));


            GameCamera.SetCamCenterPhysicsCoord(player.posX + 237, player.posY + 950);

            {
                int chunk = player.GetChunkIndex();
                if (chunk != currentChunk)
                {
                    if (chunk > currentChunk)
                    {
                        MoveLoadedChunks(Direction.RIGHT);
                        Log.Write($"move chunks right {chunk} {currentChunk}", LogType.GENERAL, LogLevel.WARNING);
                    }
                    else
                    {
                        MoveLoadedChunks(Direction.LEFT);
                        Log.Write($"move chunks left {chunk} {currentChunk}", LogType.GENERAL, LogLevel.WARNING);
                    }
                }
            }

        }

    }

    public enum Gamemode
    {
        Survival,
        Creative
    }
}
