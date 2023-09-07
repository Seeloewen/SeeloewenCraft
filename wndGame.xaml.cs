using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        public Player player;
        public Point mousePosition;
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);
        private string worldName;
        public string worldDirectory = "";
        private bool goLeft = false;
        private bool goRight = false;
        public int goLeftAmount = 10;
        public int goRightAmount = 10;
        public double relativeSvPos = 0;
        public double defaultSvPos = 0;


        //-- Constructor --//

        public wndGame(string worldName, bool isNew)
        {
            InitializeComponent();

            //Set world name and create game
            this.worldName = worldName;
            CreateGame(worldName, isNew);
        }


        //-- Custom Methods --//

        public void CreateGame(string worldName, bool isNew)
        {
            //Check if the world directory exists and create it otherwise
            if (!Directory.Exists(string.Format("{0}/SeeloewenCraft/{1}", appData, worldName)))
            {
                Directory.CreateDirectory(string.Format("{0}/SeeloewenCraft/{1}", appData, worldName));
            }
            worldDirectory = string.Format("{0}/SeeloewenCraft/{1}", appData, worldName);

            //Create the game components
            GenerateChunks();
            CreatePlayer();
            player.inventory = new Inventory(this, 0, true);
            inventoryList.Add(player.inventory);
            player.inventory.hotbarSlotList[0].SelectSlot();


            //Start the main timer
            tmrMovement.Interval = 16;
            tmrMovement.Tick += tmrMovement_Tick;
            tmrMovement.Start();
        }

        public void CreatePlayer()
        {
            //Calculate y position where the player starts
            //WIP
            int yPos = 0;
            foreach (Block block in chunkList[2].blockList)
            {
                if (block.xPos == 5 && block is GrassBlock)
                {
                    yPos = block.yPos * 50 - 150;
                }
            }

            //Create the player and add it to the world canvas
            player = new Player(this, 600, yPos);
            cvsWorld.Children.Add(player.cvsPlayer);
            cvsWorld.Children.Add(player.cvsPlayerHitbox);
            cvsWorld.Children.Add(player.cvsGravityHitbox);
            Panel.SetZIndex(player.cvsPlayer, 1);
            Panel.SetZIndex(player.cvsPlayerHitbox, 2);
            Panel.SetZIndex(player.cvsGravityHitbox, 3);
            relativeSvPos = svWorld.VerticalOffset;
            defaultSvPos = svWorld.VerticalOffset;
        }

        private void GenerateChunks()
        {
            //Create the starter chunks and add them to the world canvas
            for (int i = 0; i < 5; i++)
            {
                chunkList.Add(new Chunk(this, i));
            }
            cvsWorld.Children.Add(GetChunk(0).grdChunk);
            Canvas.SetLeft(GetChunk(0).grdChunk, -400);
            cvsWorld.Children.Add(GetChunk(1).grdChunk);
            Canvas.SetLeft(GetChunk(1).grdChunk, 0);
            cvsWorld.Children.Add(GetChunk(2).grdChunk);
            Canvas.SetLeft(GetChunk(2).grdChunk, 400);
            cvsWorld.Children.Add(GetChunk(3).grdChunk);
            Canvas.SetLeft(GetChunk(3).grdChunk, 800);
            cvsWorld.Children.Add(GetChunk(4).grdChunk);
            Canvas.SetLeft(GetChunk(4).grdChunk, 1200);
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
            if (pressedKeys.Contains(Key.Space) && player.isOnFloor() == true) //Space key
            {
                //Make the player jump
                player.Jump();
            }
            if (pressedKeys.Contains(Key.E)) //E key
            {

                int amountInventoriesOpen = 0;

                foreach(Inventory inventory in inventoryList)
                {
                    if(inventory.isShown == true)
                    {
                        amountInventoriesOpen++;
                    }
                }

                //Toggle inventory visibility
                if (amountInventoriesOpen >= 1)
                {
                    foreach(Inventory inventory in inventoryList)
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

        public Rect GetRectangle(Border border)
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

        public Rect GetRectangle(Grid grid)
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

        //-- Event Handlers --//

        private void tmrMovement_Tick(object sender, EventArgs e)
        {
            //Movement timer, ticks at a rate of approximitely 60 fps (every 16 ms)
            //Go in the specified direction for as long as the statement is true
            if (goLeft == true)
            {
                player.MoveLeft(goLeftAmount);
            }
            else if (goRight == true)
            {
                player.MoveRight(goRightAmount);

            }
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
                //Show help message
                MessageBox.Show("List of commands (For debug purposes only!):\n/help - Shows this page\n/showblockinfo - Shows Block info like coords or type\n/hideblockinfo - Hides the Block info\n/about - Shows about window\n/generateplayer - Runs the generation method of player\n/toggleinv - Opens or closes the inventory\n/showdevcontrols - Shows the controls only meant for developing\n/hidedevcontrols - Hides the development controls\n/give chest - Gives the player a chest item", "/help");
            }
            else if (tbDebug.Text == "/showblockinfo")
            {
                //Display block info (position, name, etc.) for each block that is currently rendered
                //WIP - Needs to also show block info on new chunks
                foreach (Chunk chunk in chunkList)
                {
                    foreach (Block block in chunk.blockList)
                    {
                        block.ShowBlockInfo();
                    }
                }
                MessageBox.Show("Block info is now shown.", "/showblockinfo");
            }
            else if (tbDebug.Text == "/hideblockinfo")
            {
                //Hide block info for each block that is currently rendered
                //WIP - Needs to also hide block info on new chunks
                foreach (Chunk chunk in chunkList)
                {
                    foreach (Block block in chunk.blockList)
                    {
                        block.HideBlockInfo();
                    }
                }
                MessageBox.Show("Block info is now hidden.", "/hideblockinfo");
            }
            else if (tbDebug.Text == "/about")
            {
                //Show 'About' message
                MessageBox.Show(string.Format("You are running SeeloewenCraft Version {0} - This version is not meant to be publicly shared and shall only be used for private purposes.", ""), "/about");
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
            else if (tbDebug.Text == "/changelog")
            {
                MessageBox.Show("Changelog:\n\nAlpha 1.0.1 - 31.08.2023\r\n- Fixed blocks not loading when going into old chunks\r\n- Fixed chunks resetting when unloading them\n\nAlpha 1.0.0 - 31.08.2023\r\n- The project is now called SeeloewenCraft\r\n- Added the /changelog command\r\n- Made some code optimisations\r\n- Clicking on a hotbar slot now selects it\r\n- You will now also see a border when trying to place a block\r\n- Player hitbox now looks a little better (still no model though)\r\n- Player will now always spawn on the floor\r\n- Fixed diamond veins being too big\r\n- Possibly fixed camera breaking when glitching in walls", "/changelog");
            }
            else if (tbDebug.Text == "/give chest")
            {
                player.inventory.AddItem(new ChestItem(this, 0));
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
            player.MoveRight(10);

        }

        private void btnPlayerLeft_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use
            //Move player to the left by 5
            player.MoveLeft(10);
        }

        private void btnPlayerDown_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use
            //Move player down by 5
            player.MoveDown(5);
        }

        private void btnPlayerUp_Click(object sender, RoutedEventArgs e)
        {
            //Development control, not meant for normal use
            //Make the player jump
            player.Jump();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //Add pressed key to the collection if it isn't in there already
            if (!pressedKeys.Contains(e.Key))
            {
                pressedKeys.Add(e.Key);

                //Handle the key press events
                HandleKeyPresses();
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
            foreach(Inventory inventory in inventoryList)
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Dev, remove later - save chunks manually
            foreach (Chunk chunk in chunkList)
            {
                chunk.SaveChunk();
            }
            player.inventory.SaveInventory(worldDirectory);
        }

        private void wndGame1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //If the setting to save worlds on closing is enabled
            if (Properties.Settings.Default.saveWorldOnClose == true)
            {
                //Save all chunks and the inventory of the player
                foreach (Chunk chunk in chunkList)
                {
                    chunk.SaveChunk();
                }
                player.inventory.SaveInventory(worldDirectory);
            }
        }
    }
}

