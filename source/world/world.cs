using System;
using System.Collections.Generic;
using static System.Environment;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Json.Pointer;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text;

namespace SeeloewenCraft
{
    public class World
    {
        //References
        public wndGame wndGame;
        public System.Windows.Forms.Timer tmrMovement = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer tmrWater = new System.Windows.Forms.Timer();
        public List<Chunk> currentChunkList = new List<Chunk>();
        public List<Chunk> totalChunkList = new List<Chunk>();
        public List<Inventory> inventoryList = new List<Inventory>();
        public List<BlockContainerList> blockContainerList = new List<BlockContainerList>();
        public List<Gui> guiList = new List<Gui>();
        public Images images;
        public LootTables lootTables;
        public wndMenu wndMenu;
        public Log log;
        public Player player;
        public WaterHandler waterHandler;
        public ClickHandler clickHandler;
        public DebugMenu debugMenu;

        //Constants
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);
        private string worldName;
        public int worldVersion;
        public string gameVersion;
        public string worldDirectory = "";

        //Variables
        public bool finishedLoading = false;
        public bool returnToMenu = false;
        public bool showBlockInfo = false;


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

        public Chunk GetChunk(int index)
        {
            //Searches the total chunk list for the chunk with the specified index. If not found, create a new one
            foreach (Chunk chunk in totalChunkList)
            {
                if (chunk.index == index)
                {
                    chunk.SetContainerList();
                    chunk.RenderChunk();
                    return chunk;
                }
            }

            Chunk newChunk = new Chunk(this, index);
            totalChunkList.Add(newChunk);
            return newChunk;
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
            foreach (Chunk chunk in currentChunkList)
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
                string documentText = File.ReadAllText($"{worldDirectory}/world_settings.json");
                JToken documentToken = JToken.Parse(documentText);

                return (int)new JsonPointer("/world_version").Evaluate(documentToken);
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
            double playerPosX = 0;
            double playerPosY = 0;

            //Load the player position if possible
            if (loadedPlayerPosExists)
            {
                string documentText = File.ReadAllText($"{worldDirectory}/player_position.json");
                JToken documentToken = JToken.Parse(documentText);

                try
                {
                    playerPosX = (double)new JsonPointer("/pos_x").Evaluate(documentToken);
                    playerPosY = (double)new JsonPointer("/pos_y").Evaluate(documentToken);
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
            GenerateChunks(loadedPlayerPosExists ? ((int)playerPosX / 8) - 2 : 0);

            //When the world is loaded, display the debug information
            DisplayDebugInformation();

            //Create the player
            CreatePlayer(loadedPlayerPosExists, playerPosX, playerPosY);
            player.inventory = new Inventory(this, true);
            inventoryList.Add(player.inventory);
            player.inventory.hotbarSlotList[0].SelectSlot();

            //Load the player inventory if the world is not new
            if (!isNew)
            {
                string documentText = File.ReadAllText($"{worldDirectory}/player_inventory.json");
                JToken documentToken = JToken.Parse(documentText);

                player.inventory = Inventory.LoadFromJson(documentToken, this);
                inventoryList.Add(player.inventory);

                player.inventory.UpdateHotbar();
            }
            else
            {
                //Give the player a hammer -- !! Only temporary until Crafting is implemented !!
                if (wndMenu.wndSettings.enableHammer) player.inventory.AddItem(new HammerItem(this, null));
                for (int i = 0; i < 64; i++)
                {
                    player.inventory.AddItem(new TorchItem(this, null));
                    player.inventory.AddItem(new WaterItem(this, null));
                    player.inventory.AddItem(new Plant2Item(this, null));
                }
            }

            finishedLoading = true;
            log.Write($"Loading of world {worldName} completed!", "Info");

            //Start the main timer
            tmrMovement.Interval = 16;
            tmrMovement.Tick += tmrMovement_Tick;
            tmrMovement.Start();

            //Start the water timer
            tmrWater.Interval = 1000;
            tmrWater.Tick += tmrWater_Tick;
            tmrWater.Start();
        }

        public bool HasOpenGui(bool ignoreInventory)
        {
            foreach (Gui gui in guiList)
            {
                if(gui.isOpen)
                {
                    if(gui.id == "sc:inventory")
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

        public void CreatePlayer(bool isLoaded, double playerPosX, double playerPosY)
        {

            if (!isLoaded)
            {
                //Calculate y position where the player starts
                int yPos = 0;
                foreach (Block block in currentChunkList[2].blockList.blocks)
                {
                    if (block.xPos == 5 && block is GrassBlock)
                    {
                        yPos = (block.yPos - 3) * 50;
                    }
                }

                //Create the player and add it to the world canvas
                player = new Player(this, 602, yPos + 5);
                log.Write("Generated player at start position", "Info");
            }
            else
            {
                player = new Player(this, 602, (int)(playerPosY * 50) - 50);
                player.MoveHorizontal((int)Math.Round((playerPosX % 8.0) * 50) - 252);
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

                currentChunkList.Add(GetChunk(i));
                c++;
            }

            for (int i = Math.Min(j + 4, -1); i >= c + Math.Min(j, -5); i--)
            {
                currentChunkList.Add(GetChunk(i));
            }

            //Add the chunks to the game
            wndGame.cvsWorld.Children.Add(GetFromCurrentChunks(j).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j).grdChunk, -400);
            wndGame.cvsWorld.Children.Add(GetFromCurrentChunks(j + 1).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 1).grdChunk, 0);
            wndGame.cvsWorld.Children.Add(GetFromCurrentChunks(j + 2).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 2).grdChunk, 400);
            wndGame.cvsWorld.Children.Add(GetFromCurrentChunks(j + 3).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 3).grdChunk, 800);
            wndGame.cvsWorld.Children.Add(GetFromCurrentChunks(j + 4).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 4).grdChunk, 1200);
        }

        public Chunk GetFromCurrentChunks(int index)
        {
            //Returns a chunk from the list based on the given index
            foreach (Chunk chunk in currentChunkList)
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

        //-- Event Handlers --//

        private void tmrMovement_Tick(object sender, EventArgs e)
        {
            //Movement timer, ticks at a rate of approximitely 60 fps (every 16 ms)
            player.PhysicsStep(wndGame.pressedKeys.Contains(wndMenu.wndSettings.cMoveLeft), wndGame.pressedKeys.Contains(wndMenu.wndSettings.cMoveRight), wndGame.pressedKeys.Contains(wndMenu.wndSettings.cJump), 0.016);
        }

        private void tmrWater_Tick(object sender, EventArgs e)
        {
            //Update all water blocks accordingly
            waterHandler.DoUpdate();
        }
    }
}
