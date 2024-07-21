using System;
using System.Collections.Generic;
using static System.Environment;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class World
    {
        //References
        public wndGame wndGame;
        public System.Windows.Forms.Timer tmrMovement = new System.Windows.Forms.Timer();
        public List<Chunk> loadedChunkList = new List<Chunk>();
        public List<Chunk> totalChunkList = new List<Chunk>();
        public List<Inventory> inventoryList = new List<Inventory>();
        public List<BlockContainerList> blockContainerList = new List<BlockContainerList>();
        public List<Gui> guiList = new List<Gui>();
        public List<CraftingRecipe> craftingRecipeList = new List<CraftingRecipe>();
        public Images images;
        public LootTables lootTables;
        public wndMenu wndMenu;
        public Log log;
        public Player player;
        public WaterHandler waterHandler;
        public ClickHandler clickHandler;
        public DebugMenu debugMenu;
        public GameLoop gameLoop;
        public RecipeCreator recipeCreator;
        public NotificationHandler notificationHandler;
        public WorldRenderer worldRenderer;

        //Constants
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);
        private string worldName;
        public int worldVersion;
        public string gameVersion;
        public string worldDirectory = "";
        public int lightRange = 7; //The range that all light sources use

        //Variables
        public bool finishedLoading = false;
        public bool returnToMenu = false;
        public bool showBlockInfo = false;
        public int nightState = 0;


        //-- Constructor --//

        public World(wndMenu wndMenu, string worldName, bool isNew, int worldVersion, string gameVersion, Log log)
        {
            //Set world name and create game and links
            this.worldName = worldName;
            this.worldVersion = worldVersion;
            this.gameVersion = gameVersion;
            this.wndMenu = wndMenu;
            this.log = log;

            //Create objects
            wndGame = new wndGame(this);
            images = new Images(this);
            lootTables = new LootTables(this);
            waterHandler = new WaterHandler(this);
            clickHandler = new ClickHandler(this);
            debugMenu = new DebugMenu(this);
            gameLoop = new GameLoop(this, 25);
            recipeCreator = new RecipeCreator(this);
            notificationHandler = new NotificationHandler(this);
            recipeCreator.CreateRecipes();

            worldRenderer = new WorldRenderer(wndGame);

            worldDirectory = $"{wndMenu.worldDirectory}\\{worldName}";

            if (!isNew && GetWorldVersion(worldName) < worldVersion)
            {
                MessageBoxResult result = MessageBox.Show("You are trying to load an outdated world. This may lead to corruption or other issues. You have been warned! Do you wish to continue?", "Load outdated world", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        log.Write("You are loading an outdated world. This may cause issues or corruption.", "Info");
                        CreateGame(worldName, isNew);
                        break;
                }
            }
            else
            {
                CreateGame(worldName, isNew);
            }
        }


        //-- Custom Methods --//

        //creates and returns chunk and adds to total chunk list

        public void SetBlock(Block block, int posX, int posY)
        {
            GetBlock(posX, posY).PlaceNewBlock(block);
        }

        public Chunk CreateChunk(int index)
        {
            Chunk newChunk = new Chunk(this, index);
            totalChunkList.Add(newChunk);
            return newChunk;
        }

        public void UnloadChunk(Chunk chunk)
        {
            loadedChunkList.Remove(chunk);
        }


        public Chunk LoadChunk(Chunk chunk)
        {
            chunk.SetContainerList();
            chunk.RenderChunk();
            loadedChunkList.Add(chunk);
            return chunk;
        }

        public Chunk LoadChunk(int index)
        {
            foreach (Chunk chunk in totalChunkList)
            {
                if (chunk.index == index)
                {
                    chunk.SetContainerList();
                    chunk.RenderChunk();
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

        public void RefreshTextures()
        {
            images = new Images(this);
            foreach (Inventory inventory in inventoryList)
            {
                foreach (InventorySlot slot in inventory.slotList)
                {
                    foreach (Item item in slot.items)
                    {
                        item.SetTexture();
                    }
                }
            }
            foreach (Chunk chunk in loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    block.SetTexture();
                    block.blockContainer.cvsBlock.Background = block.image;
                }
            }

            log.Write("Refreshing Textures for items and blocks!", "Info");
        }

        public int GetWorldVersion(string worldName)
        {
            //Check if the world settings file exists
            if (File.Exists($"{worldDirectory}/world_settings.json"))
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/world_settings.json");

                return documentToken.GetInt("/world_version");
            }
            else if (File.Exists($"{worldDirectory}/settings.txt"))
            {
                log.Write("Detected old saving system, can't load", "Error");
                return 0;
            }
            else
            {
                log.Write("Could not read world version from settings file because the settings file was not found", "Error");
                return 0;
            }
        }

        public void GenerateBlockContainer()
        {
            for (int i = 0; i < 5; i++)
            {
                blockContainerList.Add(new BlockContainerList(this));
            }
        }

        public void CreateGame(string worldName, bool isNew)
        {
            log.Write($"Beginning to load game for world {worldName}", "Info");

            //Check if the world directory exists and create it otherwise
            if (!Directory.Exists($"{wndMenu.worldDirectory}\\{worldName}"))
            {
                Directory.CreateDirectory($"{wndMenu.worldDirectory}\\{worldName}");
                log.Write($"Created directory for world {worldName}: {wndMenu.worldDirectory}\\{worldName}", "Info");
            }
            worldDirectory = $"{wndMenu.worldDirectory}\\{worldName}";
            log.Write($"Set directory for world {worldName} to {worldDirectory}", "Info");

            //write world version to settings.json
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("world_version");
                writer.WriteValue(worldVersion);

                writer.WriteEndObject();

                writer.WriteToFile($"{worldDirectory}/world_settings.json");
            }

            //Check if the player position exists
            bool loadedPlayerPosExists = File.Exists($"{worldDirectory}/player_position.json");
            int playerPosX = 0;
            int playerPosY = 0;

            //Load the player position if possible
            if (loadedPlayerPosExists)
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/player_position.json");
                try
                {
                    playerPosX = documentToken.GetInt("/pos_x");
                    playerPosY = documentToken.GetInt("/pos_y");
                    log.Write($"Read player position from file: x{playerPosX} y{playerPosY}", "Info");
                }
                catch (Exception ex)
                {
                    loadedPlayerPosExists = false;
                    log.Write($"Could not read player position from file: {ex.Message}", "Error");
                }

            }
            else
            {
                log.Write("Player position file does not exist, skipping...", "Info");
            }

            //Create the game components
            GenerateBlockContainer();
            GenerateChunks(loadedPlayerPosExists ? (playerPosX / 8000) - 2 : 0);

            //When the world is loaded, display the debug information
            DisplayDebugInformation();

            //Create the player
            CreatePlayer(loadedPlayerPosExists, playerPosX, playerPosY);
            player.inventory = new Inventory(this, true, 9, 4);
            inventoryList.Add(player.inventory);
            player.inventory.hotbarSlotList[0].SelectSlot();

            //Load the player inventory if the world is not new
            if (!isNew)
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/player_inventory.json");
                player.inventory = Inventory.LoadFromJson(documentToken, this);
                inventoryList.Add(player.inventory);

                player.inventory.UpdateHotbar();
            }
            else
            {
                if (Settings.enableHammer) player.inventory.AddItem(new HammerItem(this));
                for (int i = 0; i < 64; i++)
                {
                    player.inventory.AddItem(new TorchItem(this));
                    player.inventory.AddItem(new WaterItem(this));
                    player.inventory.AddItem(new Plant2Item(this));
                    player.inventory.AddItem(new ChiselerItem(this));
                    player.inventory.AddItem(new AlphaCrafterItem(this));
                    player.inventory.AddItem(new ChestItem(this));
                    player.inventory.AddItem(new CobbleStoneItem_StairTopLeft(this));
                    player.inventory.AddItem(new UnchiselerItem(this));

                }
            }

            finishedLoading = true;
            log.Write($"Loading of world {worldName} completed!", "Info");

            //Start the main timer
            tmrMovement.Interval = 16;
            tmrMovement.Tick += tmrMovement_Tick;
            tmrMovement.Start();

            //Start the game loop timer
            gameLoop.Start();
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

        public void CreatePlayer(bool isLoaded, int playerPosX, int playerPosY)
        {

            if (!isLoaded)
            {
                //Calculate y position where the player starts
                int yPos = 0;
                foreach (Block block in loadedChunkList[2].blockList.blocks)
                {
                    if (block.xPos == 4 && block is GrassBlock)
                    {
                        yPos = block.yPos;
                        break;
                    }
                }

                //Create the player and add it to the world canvas
                player = new Player(this, 20050, Math.Max((yPos*1000) - 1900, 2000));
                log.Write("Generated player at start position", "Info");
            }
            else
            {
                player = new Player(this, playerPosX, playerPosY);
                log.Write("Generated player at loaded position", "Info");
            }
            wndGame.cvsWorld.Children.Add(player.cvsPlayer);
            Panel.SetZIndex(player.cvsPlayer, 1);
            wndGame.relativeSvPos = wndGame.svWorld.VerticalOffset;
            wndGame.defaultSvPos = wndGame.svWorld.VerticalOffset;
        }

        private void GenerateChunks(int j)
        {
            //Load or generate the chunks
            int c = 0;
            for (int i = Math.Max(j, 0); i < Math.Max(j + 5, 0); i++)
            {

                loadedChunkList.Add(GetChunk(i));
                c++;
            }

            for (int i = Math.Min(j + 4, -1); i >= c + Math.Min(j, -5); i--)
            {
                loadedChunkList.Add(GetChunk(i));
            }

            worldRenderer.AddChunk(GetLoadedChunk(j));
            worldRenderer.AddChunk(GetLoadedChunk(j + 1));
            worldRenderer.AddChunk(GetLoadedChunk(j + 2));
            worldRenderer.AddChunk(GetLoadedChunk(j + 3));
            worldRenderer.AddChunk(GetLoadedChunk(j + 4));

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

        public void DisplayDebugInformation()
        {
            //Show the debug information for the world in the debug menu
            debugMenu.tblGameStats.Text = "";
            debugMenu.AddLine(debugMenu.tblGameStats, $"SeeloewenCraft {wndMenu.gameVersion} ({wndMenu.versionDate})");
            debugMenu.AddLine(debugMenu.tblGameStats, $"worldName: {worldName}");
            debugMenu.AddLine(debugMenu.tblGameStats, $"worldVersion: {worldVersion}");
        }

        public void SetNight(int nightState)
        {
            this.nightState = nightState;

            //Update all blocks in the currently loaded chunks
            foreach (Chunk chunk in loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    block.blockContainer.SetNightState(nightState);
                }
            }

            //Set background color of world canvas based on night state
            switch (nightState)
            {
                case 0:
                    wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 188, 244, 247));
                    break;
                case 1:
                    wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 150, 195, 198));
                    break;
                case 2:
                    wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 113, 146, 148));
                    break;
                case 3:
                    wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 75, 98, 99));
                    break;
                case 4:
                    wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 38, 49, 49));
                    break;
                case 5:
                    wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 10, 12, 13));
                    break;
            }
        }

        //-- Event Handlers --//
        private void tmrMovement_Tick(object sender, EventArgs e)
        {
            //Movement timer, ticks at a rate of approximitely 60 fps (every 16 ms)
            player.DoPhysicsStep(wndGame.pressedKeys.Contains(Settings.cMoveLeft), wndGame.pressedKeys.Contains(Settings.cMoveRight), wndGame.pressedKeys.Contains(Settings.cJump), 0.016);

            worldRenderer.playerPosX = (double)player.posX / 1000;
            worldRenderer.playerPosY = (double)player.posY / 1000;

            worldRenderer.Render();


        }
    }
}
