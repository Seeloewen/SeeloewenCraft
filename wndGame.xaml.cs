using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
        public List<Chunk> chunkList = new List<Chunk>();
        public List<Inventory> inventoryList = new List<Inventory>();
        public Images images;
        public wndMenu wndMenu;
        public wndSettings wndSettings;
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


        //-- Constructor --//

        public wndGame(wndMenu wndMenu, string worldName, bool isNew, int worldVersion, string gameVersion)
        {
            InitializeComponent();

            //Set world name and create game
            this.worldName = worldName;
            this.worldVersion = worldVersion;
            this.gameVersion = gameVersion;
            this.wndMenu = wndMenu;
            images = new Images(this);

            if (!isNew && GetWorldVersion(worldName) < worldVersion)
            {
                MessageBoxResult result = MessageBox.Show("You are trying to load an outdated world. This may lead to corruption or other issues. You have been warned! Do you wish to continue?", "Load outdated world", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
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
            foreach (Chunk chunk in chunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    block.SetTexture();
                    block.blockContainer.cvsBlock.Background = block.image;
                }
            }
        }

        public int GetWorldVersion(string worldName)
        {
            //Check if the world settings file exists
            if (File.Exists(string.Format("{0}/SeeloewenCraft/worlds/{1}/settings.txt", appData, worldName)))
            {
                try
                {
                    string[] fileContent = File.ReadAllLines(string.Format("{0}/SeeloewenCraft/worlds/{1}/settings.txt", appData, worldName));
                    int worldVersion = Convert.ToInt32(fileContent[1].Replace("worldVersion=", ""));
                    return worldVersion;
                }
                catch
                {
                    Console.WriteLine("[Error] Could not read worldVersion from settings file.");
                }
            }
            return 0;
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
            //Check if the world directory exists and create it otherwise
            if (!Directory.Exists(string.Format("{0}/SeeloewenCraft/worlds/{1}", appData, worldName)))
            {
                Directory.CreateDirectory(string.Format("{0}/SeeloewenCraft/worlds/{1}", appData, worldName));
            }
            worldDirectory = string.Format("{0}/SeeloewenCraft/worlds/{1}", appData, worldName);

            //Check if the world settings file exists and create it otherwise
            if (!File.Exists($"{worldDirectory}/settings.txt"))
            {
                List<string> worldSettings = new List<string>
                {
                    $"#SeeloewenCraft World Settings",
                    $"worldVersion={worldVersion}"
                };
                File.WriteAllLines($"{worldDirectory}/settings.txt", worldSettings);
            }

            bool loadedPlayerPosExists = File.Exists($"{worldDirectory}/playerPosition.txt");
            double playerPosX = 0;
            double playerPosY = 0;

            if (loadedPlayerPosExists)
            {
                string[] coords = File.ReadAllLines($"{worldDirectory}/playerPosition.txt");
                try
                {
                    playerPosX = Double.Parse(coords[0]);
                    playerPosY = Double.Parse(coords[1]);
                }
                catch
                {
                    loadedPlayerPosExists = false;
                    Console.WriteLine("player coords file incorrect format(use log for this)");
                }

            }

            //Create the game components
            GenerateBlockContainer();
            GenerateChunks(loadedPlayerPosExists ? ((int)playerPosX / 8) - 2 : 0);
            CreatePlayer(loadedPlayerPosExists, playerPosX, playerPosY);
            player.inventory = new Inventory(this, 0, true);
            inventoryList.Add(player.inventory);
            player.inventory.hotbarSlotList[0].SelectSlot();

            //Start the main timer
            tmrMovement.Interval = 16;
            tmrMovement.Tick += tmrMovement_Tick;
            tmrMovement.Start();

            //Load the player inventory if the world is not new
            if (!isNew)
            {
                player.inventory.LoadInventory(worldDirectory, 0);
            }
            else
            {
                //Give the player a hammer -- !! Only temporary until Crafting is implemented !!
                player.inventory.AddItem(new HammerItem(this, 0, null));
            }

            finishedLoading = true;
        }

        public void CreatePlayer(bool isLoaded, double playerPosX, double playerPosY)
        {

            if (!isLoaded)
            {
                //Calculate y position where the player starts
                //WIP
                int yPos = 0;
                foreach (Block block in chunkList[2].blockList.blocks)
                {
                    if (block.xPos == 5 && block is GrassBlock)
                    {
                        yPos = block.yPos * 50 - 150;
                        yPos = (block.yPos - 3) * 50;
                    }
                }

                //Create the player and add it to the world canvas
                player = new Player(this, 602, yPos + 5);

            }
            else
            {
                player = new Player(this, 602, (int)(playerPosY * 50) - 50);
                player.MoveHorizontal((int)Math.Round((playerPosX % 8.0) * 50) - 252);
            }
            cvsWorld.Children.Add(player.cvsPlayer);
            Panel.SetZIndex(player.cvsPlayer, 1);
            relativeSvPos = svWorld.VerticalOffset;
            defaultSvPos = svWorld.VerticalOffset;
        }

        private void GenerateChunks(int j)
        {

            int c = 0;
            for (int i = Math.Max(j, 0); i < Math.Max(j + 5, 0); i++)
            {
                chunkList.Add(new Chunk(this, i));
                c++;
            }

            int temp = Math.Min(j + 4, -1);
            int temp2 = c + Math.Min(j, -5);
            for (int i = temp; i >= temp2; i--)
            {
                chunkList.Add(new Chunk(this, i));
            }



            cvsWorld.Children.Add(GetChunk(j).grdChunk);
            Canvas.SetLeft(GetChunk(j).grdChunk, -400);
            cvsWorld.Children.Add(GetChunk(j + 1).grdChunk);
            Canvas.SetLeft(GetChunk(j + 1).grdChunk, 0);
            cvsWorld.Children.Add(GetChunk(j + 2).grdChunk);
            Canvas.SetLeft(GetChunk(j + 2).grdChunk, 400);
            cvsWorld.Children.Add(GetChunk(j + 3).grdChunk);
            Canvas.SetLeft(GetChunk(j + 3).grdChunk, 800);
            cvsWorld.Children.Add(GetChunk(j + 4).grdChunk);
            Canvas.SetLeft(GetChunk(j + 4).grdChunk, 1200);
        }

        public Chunk GetChunk(int index)
        {
            //Returns a chunk from the list based on the given index
            foreach (Chunk chunk in chunkList)
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
            catch
            {
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
            catch
            {
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
            catch
            {
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
            foreach (Chunk chunk in chunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) + 50);
            }

            //Sort the chunklist to account for the chunk movement
            chunkList = chunkList.OrderBy(chunk => Canvas.GetLeft(chunk.grdChunk)).ToList();

            //Check if the chunk has moved too far and move the chunk on the far right all the way to the left
            if (Canvas.GetLeft(chunkList[2].grdChunk) == 800)
            {
                chunkList.Remove(GetChunk(chunkList[4].index));
                chunkList.Add(new Chunk(this, chunkList[0].index - 1));
                cvsWorld.Children.Add(chunkList[4].grdChunk);
                Canvas.SetLeft(chunkList[4].grdChunk, -400);

                //Sort chunklist again
                chunkList = chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }


        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use

            //Move all chunks 50 to the left
            foreach (Chunk chunk in chunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) - 50);
            }

            //Sort the chunklist to account for the chunk movement
            chunkList = chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

            //Check if the chunk has moved too far and move the chunk on the far left all the way to the right
            if (Canvas.GetLeft(chunkList[2].grdChunk) == 0)
            {
                chunkList.Remove(GetChunk(chunkList[0].index));
                chunkList.Add(new Chunk(this, chunkList[3].index + 1));
                cvsGame.Children.Add(chunkList[4].grdChunk);
                Canvas.SetLeft(chunkList[4].grdChunk, 1200);

                //Sort chunklist again
                chunkList = chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
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
                foreach (Chunk chunk in chunkList)
                {
                    chunk.showBlockInfo();
                }
                MessageBox.Show("Block info is now shown.", "/showblockinfo");
                showBlockInfo = true;
            }
            else if (tbDebug.Text == "/hideblockinfo")
            {
                //Hide block info for each block that is currently rendered
                //WIP - Needs to also hide block info on new chunks
                foreach (Chunk chunk in chunkList)
                {
                    chunk.hideBlockInfo();

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
                    foreach (Chunk chunk in chunkList)
                    {
                        chunk.Save();
                    }
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
            foreach (Chunk chunk in chunkList)
            {
                chunk.Save();
            }
            player.inventory.SaveInventory(worldDirectory);
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

