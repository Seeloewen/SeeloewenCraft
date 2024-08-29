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

namespace SeeloewenCraft
{
    public class World
    {
        //References
        public wndMenu wndMenu;
        public wndGame wndGame;
        public System.Windows.Forms.Timer tmrMovement = new System.Windows.Forms.Timer();
        public List<Chunk> loadedChunkList = new List<Chunk>();
        public List<Chunk> totalChunkList = new List<Chunk>();
        public List<Inventory> inventoryList = new List<Inventory>();
        public List<BlockContainerList> blockContainerList = new List<BlockContainerList>();
        public List<Gui> guiList = new List<Gui>();
        public List<CraftingRecipe> craftingRecipeList = new List<CraftingRecipe>();
        public LootTables lootTables;
        public Player player;
        public WaterHandler waterHandler;
        public ClickHandler clickHandler;
        public DebugMenu debugMenu;
        public GameLoop gameLoop;
        public RecipeCreator recipeCreator;
        public NotificationHandler notificationHandler;
        public WorldRenderer worldRenderer;
        public List<Entity> entities;
        public List<Entity> toDieEntities;

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
        public Gamemode gamemode = Gamemode.Survival;

        //-- Constructor --//

        public World(wndMenu wndMenu, string worldName, bool isNew, int worldVersion, string gameVersion)
        {
            //Set world name and create game and links
            this.worldName = worldName;
            this.worldVersion = worldVersion;
            this.gameVersion = gameVersion;
            this.wndMenu = wndMenu;
            Game.world = this;

            //Initialize textures before anything else
            Images.Init();

            //Create objects
            wndGame = new wndGame();
            lootTables = new LootTables();
            waterHandler = new WaterHandler();
            clickHandler = new ClickHandler();
            debugMenu = new DebugMenu();
            gameLoop = new GameLoop(25);
            recipeCreator = new RecipeCreator();
            notificationHandler = new NotificationHandler();
            worldRenderer = new WorldRenderer();

            //Actually initialize the game
            InitGame(worldName, isNew, worldVersion);

            if (StartOptions.startCreative)
            {
                SetGamemode(Gamemode.Creative);
            }

            Game.world.wndGame.Show();
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
                Log.Write("Detected old saving system, can't load", "Error");
                return false;
            }
            else
            {
                Log.Write("Could not read world version from settings file because the settings file was not found", "Error");
                return false;
            }

            if (worldVersion < currentWorldVersion)
            {
                MessageBoxResult result = MessageBox.Show("You are trying to load an outdated Game.world. This may lead to corruption or other issues. You have been warned! Do you wish to continue?", "Load outdated world", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Log.Write("You are loading an outdated Game.world. This may cause issues or corruption.", "Warning");
                        return true;
                }
                return false;
            }
            return true;
        }

        public void InitGame(string worldName, bool isNew, int worldVersion)
        {
            Log.Write($"Beginning to init game for world {worldName}", "Info");

            recipeCreator.CreateRecipes();

            RoomLibrary.CreateDungeonRooms();

            InitWorldDirectory();

            if (!isNew && !CheckWorldVersion(worldVersion))
            {
                throw new Exception();
            }

            SaveWorldSettings();

            InitEntityList(!isNew);

            GenerateBlockContainer();

            (int x, int y) playerStartPos = FindPlayerStartPos(!isNew);

            //this isnt exactly true as posX=-1000 should be chunk -1 but is loaded as chunk 0,
            //however in first rendering tick it should get fixed automatically
            LoadStartChunks(playerStartPos.x / 8000);

            CreatePlayer(playerStartPos.x, playerStartPos.y);

            //When the world is loaded, display the debug information
            DisplayDebugInformation();

            //Load the player inventory if the world is not new
            InitPlayerInventory(!isNew);

            Log.Write($"Completed initializing of world {worldName}!", "Info");


            //this is a temporary fix to avoid all item entities getting collected by the player
            //before the first render call; probably because the collision system works through
            //checking for collision between the rendered rectangles, should be improved 
            {
                worldRenderer.playerPosX = (double)player.posX / 1000;
                worldRenderer.playerPosY = (double)player.posY / 1000;

                worldRenderer.Render();
            }


            //Start the main timer
            tmrMovement.Interval = 16;
            tmrMovement.Tick += tmrMovement_Tick;
            tmrMovement.Start();

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
                writer.WritePropertyName("entities");
                writer.WriteStartArray();
                foreach (Entity entity in entities)
                {
                    entity.SaveToJson(writer);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
                writer.WriteToFile($"{worldDirectory}/entities.json");
            }
        }


        public Entity GetEntity(long id)
        {
            foreach (Entity entity in entities)
            {
                if (entity.id == id)
                {
                    return entity;
                }
            }
            return null;
        }


        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
            Game.world.wndGame.cvsWorld.Children.Add(entity.texture);
            Panel.SetZIndex(entity.texture, 1);
            worldRenderer.AddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                entities.Remove(entity);
                Game.world.wndGame.cvsWorld.Children.Remove(entity.texture);
                worldRenderer.Remove(entity);
            }
        }

        public void SetBlock(Block block, int posX, int posY)
        {
            GetBlock(posX, posY).SetBlock(block);
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
            worldRenderer.RemoveChunk(chunk);
        }


        public void LoadChunk(Chunk chunk)
        {
            chunk.SetContainerList();
            chunk.RenderChunk();
            loadedChunkList.Add(chunk);
            worldRenderer.AddChunk(chunk);
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
            //Refresh the images to use the new textures
            Images.Init();

            //Update all textures in inventories
            foreach (Inventory inventory in inventoryList)
            {
                foreach (InventorySlot slot in inventory.slotList)
                {
                    if (!slot.IsEmpty())
                    {
                        slot.cvsItem.Background = ItemRegister.GenerateItem(slot.itemId).image;
                    }
                }
            }

            //Update all loaded block textures
            foreach (Chunk chunk in loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    block.SetTexture();
                    block.blockContainer.cvsBlock.Background = block.image;
                }
            }

            Log.Write("Refreshing Textures for items and blocks!", "Info");
        }

        public void GenerateBlockContainer()
        {
            for (int i = 0; i < 8; i++)
            {
                blockContainerList.Add(new BlockContainerList());
            }
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

                writer.WriteEndObject();

                writer.WriteToFile($"{worldDirectory}/world_settings.json");
            }
        }

        private void InitWorldDirectory()
        {
            //Check if the world directory exists and create it otherwise
            if (!Directory.Exists($"{FolderUtil.worldsFolder}\\{worldName}"))
            {
                Directory.CreateDirectory($"{FolderUtil.worldsFolder}\\{worldName}");
                Log.Write($"Created directory for world {worldName}: {FolderUtil.worldsFolder}\\{worldName}", "Info");
            }
            worldDirectory = $"{FolderUtil.worldsFolder}\\{worldName}";
            Log.Write($"Set directory for world {worldName} to {worldDirectory}", "Info");
        }

        private void InitEntityList(bool loaded)
        {
            toDieEntities = new List<Entity>();
            entities = new List<Entity>();
            if (loaded)
            {
                JsonToken listToken = JsonUtil.ReadFile($"{worldDirectory}/entities.json").GetToken("/entities");
                int l = listToken.GetArrayLength();
                for (int i = 0; i < l; i++)
                {
                    AddEntity(Entity.LoadFromJson(listToken.GetToken($"/{i}")));
                }
            }
        }

        private void InitPlayerInventory(bool loaded)
        {
            if (loaded)
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/player_inventory.json");
                player.inventory = Inventory.LoadFromJson(documentToken);
                inventoryList.Add(player.inventory);
            }
            else
            {
                player.inventory = new Inventory(9, 4);
                player.inventory.InitHotbar();
                if (Settings.enableHammer) player.inventory.AddItem("sc:stone_hammer_item", 1);
                player.inventory.AddItem("sc:torch_item", 64);
                player.inventory.AddItem("sc:water_item", 64);
                player.inventory.AddItem("sc:potted_cactus_item", 64);
                player.inventory.AddItem("sc:chiseler_item", 64);
                player.inventory.AddItem("sc:crafting_table_item", 64);
                player.inventory.AddItem("sc:chest_item", 64);
                player.inventory.AddItem("sc:cobblestone_stairtopleft_item", 64);
                player.inventory.AddItem("sc:unchiseler_item", 64);
                player.inventory.AddItem("sc:spruce_door_item", 64);
            }
            player.inventory.UpdateHotbar();
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

            player = new Player(playerPosX, playerPosY);

            Game.world.wndGame.cvsWorld.Children.Add(player.texture);
            Panel.SetZIndex(player.texture, 1);
            Game.world.wndGame.relativeSvPos = Game.world.wndGame.svWorld.VerticalOffset;
            Game.world.wndGame.defaultSvPos = Game.world.wndGame.svWorld.VerticalOffset;

            Log.Write($"Created player at position x:{playerPosX}, y:{playerPosY}", "Info");
        }

        private (int x, int y) FindPlayerStartPos(bool loaded)
        {

            //Load the player position if possible
            if (loaded)
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{worldDirectory}/player_position.json");
                int x = documentToken.GetInt("/pos_x");
                int y = documentToken.GetInt("/pos_y");
                Log.Write($"loaded player start point at: x{x} y{y}", "Info");
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

                Log.Write($"found player spawn point at pos: x: {20050}, y: {yPos}", "Info");
                return (28050, Math.Max((yPos * 1000) - 1900, 2000));
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

        public void MoveLoadedChunks(Direction dir)
        {
            if (dir.IsRight())
            {
                Chunk removeChunk = Game.world.GetLoadedChunk(Game.world.loadedChunkList[0].index);
                Game.world.loadedChunkList.Remove(removeChunk);
                worldRenderer.RemoveChunk(removeChunk);
                Chunk addChunk = Game.world.GetChunk(Game.world.loadedChunkList[5].index + 1);
                Game.world.LoadChunk(addChunk);


                //Sort the chunklist again
                Game.world.loadedChunkList = Game.world.loadedChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                worldRenderer.Render();
            }
            else if (dir.IsLeft())
            {
                Chunk chunk = Game.world.GetLoadedChunk(Game.world.loadedChunkList[6].index);
                Game.world.UnloadChunk(chunk);
                Game.world.LoadChunk(Game.world.GetChunk(Game.world.loadedChunkList[0].index - 1));

                //Sort the list again
                Game.world.loadedChunkList = Game.world.loadedChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                worldRenderer.Render();
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

        public void DisplayDebugInformation()
        {
            //Show the debug information for the world in the debug menu
            debugMenu.tblGameStats.Text = "";
            debugMenu.AddLine(debugMenu.tblGameStats, $"SeeloewenCraft {Game.GAME_VERSION} ({Game.VERSION_DATE})");
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
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 188, 244, 247));
                    break;
                case 1:
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 150, 195, 198));
                    break;
                case 2:
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 113, 146, 148));
                    break;
                case 3:
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 75, 98, 99));
                    break;
                case 4:
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 38, 49, 49));
                    break;
                case 5:
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 10, 12, 13));
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

        private void tmrMovement_Tick(object sender, EventArgs e)
        {
            player.HandleInputs();
            player.OnUpdate(63); // tps: 1/0.016

            List<ItemEntity> pickedUpEntities = new List<ItemEntity>();

            foreach (Entity entity in entities)
            {
                entity.OnUpdate(63);
                if (entity is ItemEntity itemEntity && entity.lifeTime > 300 && entity.posX < player.posX + player.sizeX && entity.posX + entity.sizeX > player.posX && entity.posY < player.posY + player.sizeY && entity.posY + entity.sizeY > player.posY)
                {
                    player.inventory.AddItem(itemEntity.item.id, 1, out int remainingItem);
                    if (remainingItem == 0)
                    {
                        toDieEntities.Add(itemEntity);
                    }
                }
            }

            foreach (Entity entity in toDieEntities)
            {
                RemoveEntity(entity);
            }

            worldRenderer.playerPosX = (double)player.posX / 1000;
            worldRenderer.playerPosY = (double)player.posY / 1000;

            worldRenderer.Render();
        }
    }

    public enum Gamemode
    {
        Survival,
        Creative
    }
}
