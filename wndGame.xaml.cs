using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Environment;

namespace SeeloewenCraft
{
    public partial class wndGame : Window
    {
        private System.Windows.Forms.Timer tmrMovement = new System.Windows.Forms.Timer();
        public List<Chunk> currentChunkList = new List<Chunk>();
        public List<Inventory> inventoryList = new List<Inventory>();
        public Images images;
        public wndMenu wndMenu;
        public wndSettings wndSettings;
        public Log log;
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        public Player player;
        public Point mousePosition;
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);
        private string worldName;
        public int worldVersion;
        public string gameVersion;
        public string worldDirectory = "";
        private bool goLeft = false;
        private bool goRight = false;
        public int goLeftAmount = 10;
        public int goRightAmount = 10;
        public double relativeSvPos = 0;
        public double defaultSvPos = 0;
        public bool finishedLoading = false;
        private bool returnToMenu = false;
        public List<BlockContainerList> blockContainerList = new List<BlockContainerList>();
        public bool showBlockInfo = false;
        public List<Chunk> totalChunkList = new List<Chunk>();


        //-- Constructor --//

        public wndGame(wndMenu wndMenu, string worldName, bool isNew, int worldVersion, string gameVersion, Log log)
        {
            InitializeComponent();

            //Set world name and create game
            this.worldName = worldName;
            this.worldVersion = worldVersion;
            this.gameVersion = gameVersion;
            this.wndMenu = wndMenu;
            this.log = log;
            images = new Images(this);
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
            if (File.Exists($"{worldDirectory}\\settings.txt"))
            {
                try
                {
                    string[] fileContent = File.ReadAllLines($"{worldDirectory}\\settings.txt");
                    int worldVersion = Convert.ToInt32(fileContent[1].Replace("worldVersion=", ""));
                    log.Write($"Read world version {fileContent[1].Replace("worldVersion=", "")} from settings file", "Info");
                    return worldVersion;
                }
                catch (Exception ex)
                {
                    log.Write($"Could not read world version from settings file: {ex.Message}", "Error");
                    return 0;
                }
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

            //Check if the world settings file exists and create it otherwise
            if (!File.Exists($"{worldDirectory}\\settings.txt"))
            {
                List<string> worldSettings = new List<string>
                {
                    $"#SeeloewenCraft World Settings",
                    $"worldVersion={worldVersion}"
                };
                File.WriteAllLines($"{worldDirectory}\\settings.txt", worldSettings);
                log.Write($"Created settings file for world {worldName}: {worldDirectory}\\settings.txt", "Info");
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
            CreatePlayer(loadedPlayerPosExists, playerPosX, playerPosY);
            player.inventory = new Inventory(this, 0, true);
            inventoryList.Add(player.inventory);
            player.inventory.hotbarSlotList[0].SelectSlot();

            //Load the player inventory if the world is not new
            if (!isNew)
            {
                string documentText = File.ReadAllText($"{worldDirectory}/player_inventory.json");
                JToken documentToken = JToken.Parse(documentText);

                player.inventory = Inventory.LoadFromJson(documentToken, this);

                player.inventory.LoadInventory(worldDirectory, 0);

                inventoryList.Add(player.inventory);

                player.inventory.UpdateHotbar();
            }
            else
            {
                //Give the player a hammer -- !! Only temporary until Crafting is implemented !!
                if (Properties.Settings.Default.enableHammer) player.inventory.AddItem(new HammerItem(this, 0, null));
                player.inventory.AddItem(new TorchItem(this, 0, null));
            }

            finishedLoading = true;
            log.Write($"Loading of world {worldName} completed!", "Info");

            //Start the main timer
            tmrMovement.Interval = 16;
            tmrMovement.Tick += tmrMovement_Tick;
            tmrMovement.Start();
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
            cvsWorld.Children.Add(player.cvsPlayer);
            Panel.SetZIndex(player.cvsPlayer, 1);
            relativeSvPos = svWorld.VerticalOffset;
            defaultSvPos = svWorld.VerticalOffset;
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
            cvsWorld.Children.Add(GetFromCurrentChunks(j).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j).grdChunk, -400);
            cvsWorld.Children.Add(GetFromCurrentChunks(j + 1).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 1).grdChunk, 0);
            cvsWorld.Children.Add(GetFromCurrentChunks(j + 2).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 2).grdChunk, 400);
            cvsWorld.Children.Add(GetFromCurrentChunks(j + 3).grdChunk);
            Canvas.SetLeft(GetFromCurrentChunks(j + 3).grdChunk, 800);
            cvsWorld.Children.Add(GetFromCurrentChunks(j + 4).grdChunk);
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

        private void HandleKeyPresses()
        {
            if (pressedKeys.Contains(Key.A)) //A key
            {
                //Start going left
                goLeftAmount = 5;
                goLeft = true;
            }
            else
            {
                //Stop going left
                goLeft = false;
            }
            if (pressedKeys.Contains(Key.D)) //D key
            {
                //Start going right
                goRightAmount = 5;
                goRight = true;
            }
            else
            {
                //Stop going right
                goRight = false;
            }
            if (pressedKeys.Contains(Key.E)) //E key
            {

                int amountInventoriesOpen = 0;

                foreach (Inventory inventory in inventoryList)
                {
                    if (inventory.isShown == true)
                    {
                        amountInventoriesOpen++;
                    }
                }

                //Toggle inventory visibility
                if (amountInventoriesOpen >= 1)
                {
                    foreach (Inventory inventory in inventoryList)
                    {
                        inventory.HideInventory();
                    }
                }
                else
                {
                    Canvas.SetTop(player.inventory.grdInventory, 100);
                    player.inventory.ShowInventory();
                }
            }
            if (pressedKeys.Contains(Key.D1)) //Num Key 1 (Not numpad)
            {
                //Select Hotbar Slot 1
                player.inventory.hotbarSlotList[0].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D2)) //Num Key 2 (Not numpad)
            {
                //Select Hotbar Slot 2
                player.inventory.hotbarSlotList[1].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D3)) //Num Key 3 (Not numpad)
            {
                //Select Hotbar Slot 3
                player.inventory.hotbarSlotList[2].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D4)) //Num Key 4 (Not numpad)
            {
                //Select Hotbar Slot 4
                player.inventory.hotbarSlotList[3].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D5)) //Num Key 5 (Not numpad)
            {
                //Select Hotbar Slot 5
                player.inventory.hotbarSlotList[4].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D6)) //Num Key 6 (Not numpad)
            {
                //Select Hotbar Slot 6
                player.inventory.hotbarSlotList[5].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D7)) //Num Key 7 (Not numpad)
            {
                //Select Hotbar Slot 7
                player.inventory.hotbarSlotList[6].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D8)) //Num Key 8 (Not numpad)
            {
                //Select Hotbar Slot 8
                player.inventory.hotbarSlotList[7].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D9)) //Num Key 9 (Not numpad)
            {
                //Select Hotbar Slot 9
                player.inventory.hotbarSlotList[8].SelectSlot();
            }
            if (pressedKeys.Contains(Key.Escape)) //Num Key 9 (Not numpad)
            {
                if (bdrMenu.IsVisible == true)
                {
                    //Hide the game menu and disable it to avoid input
                    bdrMenu.Visibility = Visibility.Hidden;
                    bdrMenu.IsEnabled = false;
                }
                else
                {
                    //Show the game menu and enable it to allow input
                    bdrMenu.Visibility = Visibility.Visible;
                    bdrMenu.IsEnabled = true;
                }
            }
        }

        public void RemoveFromParent(UIElement element)
        {
            //Get the parent of the specified UI element and remove the element from its parent
            var parent = VisualTreeHelper.GetParent(element);
            var parentAsPanel = parent as Panel;
            if (parentAsPanel != null)
            {
                parentAsPanel.Children.Remove(element);
            }
            var parentAsContentControl = parent as ContentControl;
            if (parentAsContentControl != null)
            {
                parentAsContentControl.Content = null;
            }
            var parentAsDecorator = parent as Decorator;
            if (parentAsDecorator != null)
            {
                parentAsDecorator.Child = null;
            }
        }

        public Rect GetRectangle(Canvas canvas)
        {
            try
            {
                //Set the non-adjusted rectangles
                Rect canvasHitbox = new Rect(Canvas.GetLeft(canvas), Canvas.GetTop(canvas), canvas.ActualWidth, canvas.ActualHeight);

                //Convert positions to screen coordinates
                Point canvasPoint = canvas.PointToScreen(new Point(0, 0));

                //Convert to coordinates in the scrollviewer considering scrolling
                Point canvasPosition = svWorld.TranslatePoint(canvasPoint, cvsWorld);

                //Set the new adjusted rectangles
                Rect adjustedCanvasRect = new Rect(canvasPosition.X, canvasPosition.Y, canvasHitbox.Width, canvasHitbox.Height);
                return adjustedCanvasRect;
            }
            catch (Exception ex)
            {
                log.Write($"Could not get rectangle for canvas {canvas.Uid}: {ex.Message}", "Warning");
                return new Rect(1, 1, 1, 1);
            }
        }

        public Rect GetRectangle(Border border)
        {
            try
            {
                //Set the non-adjusted rectangles
                Rect borderHitbox = new Rect(Canvas.GetLeft(border), Canvas.GetTop(border), border.ActualWidth, border.ActualHeight);

                //Convert positions to screen coordinates
                Point borderPoint = border.PointToScreen(new Point(0, 0));

                //Convert to coordinates in the scrollviewer considering scrolling
                Point borderPosition = svWorld.TranslatePoint(borderPoint, cvsWorld);

                //Set the new adjusted rectangles
                Rect adjustedBorderRect = new Rect(borderPosition.X, borderPosition.Y, borderHitbox.Width, borderHitbox.Height);
                return adjustedBorderRect;
            }
            catch (Exception ex)
            {
                log.Write($"Could not get rectangle for border {border.Uid}: {ex.Message}", "Warning");
                return new Rect(1, 1, 1, 1);
            }
        }


        public Rect GetRectangle(Grid grid)
        {
            try
            {
                //Set the non-adjusted rectangles
                Rect gridHitbox = new Rect(Canvas.GetLeft(grid), Canvas.GetTop(grid), grid.ActualWidth, grid.ActualHeight);

                //Convert positions to screen coordinates
                Point gridPoint = grid.PointToScreen(new Point(0, 0));

                //Convert to coordinates in the scrollviewer considering scrolling
                Point gridPosition = svWorld.TranslatePoint(gridPoint, cvsWorld);

                //Set the new adjusted rectangles
                Rect adjustedGridRect = new Rect(gridPosition.X, gridPosition.Y, gridHitbox.Width, gridHitbox.Height);
                return adjustedGridRect;

            }
            catch (Exception ex)
            {
                log.Write($"Could not get rectangle for grid {grid.Uid}: {ex.Message}", "Warning");
                return new Rect(1, 1, 1, 1);
            }
        }

        //-- Event Handlers --//

        private void tmrMovement_Tick(object sender, EventArgs e)
        {
            //Movement timer, ticks at a rate of approximitely 60 fps (every 16 ms)
            player.physicsStep(pressedKeys.Contains(Key.A), pressedKeys.Contains(Key.D), pressedKeys.Contains(Key.Space), 0.016);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use

            //Move all chunks 50 to the right
            foreach (Chunk chunk in currentChunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) + 50);
            }

            //Sort the chunklist to account for the chunk movement
            currentChunkList = currentChunkList.OrderBy(chunk => Canvas.GetLeft(chunk.grdChunk)).ToList();

            //Check if the chunk has moved too far and move the chunk on the far right all the way to the left
            if (Canvas.GetLeft(currentChunkList[2].grdChunk) == 800)
            {
                currentChunkList.Remove(GetFromCurrentChunks(currentChunkList[4].index));
                currentChunkList.Add(new Chunk(this, currentChunkList[0].index - 1));
                cvsWorld.Children.Add(currentChunkList[4].grdChunk);
                Canvas.SetLeft(currentChunkList[4].grdChunk, -400);

                //Sort chunklist again
                currentChunkList = currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }


        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use

            //Move all chunks 50 to the left
            foreach (Chunk chunk in currentChunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) - 50);
            }

            //Sort the chunklist to account for the chunk movement
            currentChunkList = currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

            //Check if the chunk has moved too far and move the chunk on the far left all the way to the right
            if (Canvas.GetLeft(currentChunkList[2].grdChunk) == 0)
            {
                currentChunkList.Remove(GetFromCurrentChunks(currentChunkList[0].index));
                currentChunkList.Add(new Chunk(this, currentChunkList[3].index + 1));
                cvsGame.Children.Add(currentChunkList[4].grdChunk);
                Canvas.SetLeft(currentChunkList[4].grdChunk, 1200);

                //Sort chunklist again
                currentChunkList = currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }

        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            //Currently development control, not meant for normal use yet. Will get an rework at a later point inn development to allow normal use
            //WIP

            //Perform an action based on the entered command
            if (tbDebug.Text == "/help")
            {
                //Show help messaged
                MessageBox.Show("List of commands (For debug purposes only!):\n/help - Shows this page\n/showblockinfo - Shows Block info like coords or type\n/hideblockinfo - Hides the Block info\n/about - Shows about window\n/generateplayer - Runs the generation method of player\n/toggleinv - Opens or closes the inventory\n/showdevcontrols - Shows the controls only meant for developing\n/hidedevcontrols - Hides the development controls\n/give chest - Gives the player a chest item\n/resetview - Reset the scrollviewer location\n/give magmablock - Gives the player a magma block", "/help");
            }
            else if (tbDebug.Text == "/showblockinfo")
            {
                //Display block info (position, name, etc.) for each block that is currently rendered
                //WIP - Needs to also show block info on new chunks
                foreach (Chunk chunk in currentChunkList)
                {
                    chunk.ShowBlockInfo();
                }
                MessageBox.Show("Block info is now shown.", "/showblockinfo");
                showBlockInfo = true;
            }
            else if (tbDebug.Text == "/hideblockinfo")
            {
                //Hide block info for each block that is currently rendered
                //WIP - Needs to also hide block info on new chunks
                foreach (Chunk chunk in currentChunkList)
                {
                    chunk.HideBlockInfo();

                }
                MessageBox.Show("Block info is now hidden.", "/hideblockinfo");
                showBlockInfo = false;
            }
            else if (tbDebug.Text == "/about")
            {
                //Show 'About' message
                wndAbout wndAbout = new wndAbout(wndMenu);
                wndAbout.ShowDialog();
            }
            else if (tbDebug.Text == "/generateplayer")
            {
                //Generate player at preset position, might not work correctly
                player.GeneratePlayer(600, 50);
            }
            else if (tbDebug.Text == "/toggleinv")
            {
                //Toggle inventory visibility - pretty much useless nowadays, was only used for developing the inventory
                if (player.inventory.isShown)
                {
                    player.inventory.HideInventory();
                }
                else
                {
                    player.inventory.ShowInventory();
                }
            }
            else if (tbDebug.Text == "/showdevcontrols")
            {
                //Show all development controls like player or chunk movement buttons
                btnLeft.Visibility = Visibility.Visible;
                btnRight.Visibility = Visibility.Visible;
                btnPlayerDown.Visibility = Visibility.Visible;
                btnPlayerUp.Visibility = Visibility.Visible;
                btnPlayerLeft.Visibility = Visibility.Visible;
                btnPlayerRight.Visibility = Visibility.Visible;
            }
            else if (tbDebug.Text == "/hidedevcontrols")
            {
                //Hide all development controls
                btnLeft.Visibility = Visibility.Hidden;
                btnRight.Visibility = Visibility.Hidden;
                btnPlayerDown.Visibility = Visibility.Hidden;
                btnPlayerUp.Visibility = Visibility.Hidden;
                btnPlayerLeft.Visibility = Visibility.Hidden;
                btnPlayerRight.Visibility = Visibility.Hidden;
            }
            else if (tbDebug.Text.Contains("/give chest"))
            {
                player.inventory.AddItem(new ChestItem(this, 0, null));
            }
            else if (tbDebug.Text.Contains("/give magmablock"))
            {
                player.inventory.AddItem(new MagmaBlockItem(this, 0, null));
            }
            else if (tbDebug.Text == "/resetview")
            {
                svWorld.ScrollToVerticalOffset(player.cvsPlayer.Margin.Top - 400);
            }
            else
            {
                //Show error message if the command is unknown
                MessageBox.Show("Unknown command.", "Error");
            }

            //Clear the chat box after execution
            tbDebug.Clear();
        }

        private void btnPlayerRight_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use
            //Move player to the right by 5
            player.MoveHorizontal(10);

        }

        private void btnPlayerLeft_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use
            //Move player to the left by 5
            player.MoveHorizontal(-10);
        }

        private void btnPlayerDown_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use
            //Move player down by 5
            player.MoveVertical(5);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //Add pressed key to the collection if it isn't in there already
            if (tbDebug.IsKeyboardFocused == false)
            {
                if (!pressedKeys.Contains(e.Key))
                {
                    pressedKeys.Add(e.Key);

                    //Handle the key press events
                    HandleKeyPresses();
                }
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            //Remove the pressed key from the list and handle press events
            pressedKeys.Remove(e.Key);
            HandleKeyPresses();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //Get the current mouse position
            mousePosition = e.GetPosition(cvsGame);
            foreach (Inventory inventory in inventoryList)
            {
                foreach (InventorySlot slot in inventory.slotList)
                {
                    //Go through every slot and check which one is selected
                    if (slot.isSelected == true)
                    {
                        //Remove the canvas from its spot in the inventory
                        RemoveFromParent(slot.items[0].cvsItem);

                        //Add the canvas to the main game canvas to make it follow the mouse
                        cvsGame.Children.Add(slot.items[0].cvsItem);
                        Panel.SetZIndex(slot.items[0].cvsItem, 5);
                        Canvas.SetLeft(slot.items[0].cvsItem, mousePosition.X + 5);
                        Canvas.SetTop(slot.items[0].cvsItem, mousePosition.Y + 5);
                    }
                }
            }
        }


        private void wndGame1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //If the setting to save worlds on closing is enabled
            if (finishedLoading)
            {
                tmrMovement.Stop();
                if (Properties.Settings.Default.saveWorldOnClose == true)
                {
                    //Save all chunks and the inventory of the player
                    foreach (Chunk chunk in currentChunkList)
                    {
                        chunk.Save();
                    }
                    player.SaveInventory(worldDirectory);
                    player.inventory.SaveInventory(worldDirectory);
                    player.SavePosition(worldDirectory);
                }
            }

            //Check if the user wants to return to the menu, else close the entire app
            if (returnToMenu)
            {
                wndMenu.Show();
            }
            else
            {
                wndMenu.Close();
            }
        }

        private void btnBackToGame_Click(object sender, RoutedEventArgs e)
        {
            //Hide the game menu and disable it to avoid input
            bdrMenu.Visibility = Visibility.Hidden;
            bdrMenu.IsEnabled = false;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Show settings window
            wndSettings = new wndSettings(wndMenu);
            wndSettings.ShowDialog();
        }

        private void btnSaveWorld_Click(object sender, RoutedEventArgs e)
        {
            //Save all chunks and the inventory of the player
            foreach (Chunk chunk in totalChunkList)
            {
                chunk.Save();
            }
            player.inventory.SaveInventory(worldDirectory);
            player.SaveInventory(worldDirectory);
            player.SavePosition(worldDirectory);

            //Show confirmation
            MessageBox.Show("Successfully saved the World!", "Save World", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnExitToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            //Show the main menu window and close the game window
            returnToMenu = true;
            Close();
        }

        //disables scrolling with the mouse wheel
        private void svWorld_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Select the hotbar slots
            int newSlot;
            if (e.Delta < 0)
            {
                newSlot = (player.inventory.GetSelectedIndex() + 1) % 9;
            }
            else
            {
                newSlot = (player.inventory.GetSelectedIndex() - 1) % 9;
                if (newSlot == -1) newSlot = 8;
            }
            player.inventory.hotbarSlotList[newSlot].SelectSlot();

            e.Handled = true;
        }
    }
}

