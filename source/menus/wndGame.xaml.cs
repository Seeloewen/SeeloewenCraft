using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;


namespace SeeloewenCraft
{
    public partial class wndGame : Window
    {
        //References
        public HashSet<Key> pressedKeys = new HashSet<Key>();
        public World world;
        public Point mousePosition;

        //Variables
        private bool goLeft = false;
        private bool goRight = false;
        public int goLeftAmount = 10;
        public int goRightAmount = 10;
        public double relativeSvPos = 0;
        public double defaultSvPos = 0;

        //-- Constructor --//

        public wndGame(World world)
        {
            InitializeComponent();
            this.world = world;
        }

        //-- Custom Methods --//
        private void HandleKeyPresses()
        {
            if (pressedKeys.Contains(world.settings.cShowInv)) //E key
            {
                //Check how many guis are open
                int openGuis = 0;

                foreach (Gui gui in world.guiList)
                {
                    if (gui.isOpen == true)
                    {
                        openGuis++;
                    }
                }

                //Toggle gui visibility
                if (openGuis >= 1)
                {
                    //Create a list of all the guis that need to be closed
                    List<Gui> removeGuiList = [.. world.guiList];

                    foreach(Gui gui in removeGuiList)
                    {
                        if (gui.id == "sc:inventory")
                        {
                            gui.inventory.HideInventory();
                        }
                        else
                        {
                            gui.Hide();
                        }
                    }
                }
                else
                {
                    Canvas.SetTop(world.player.inventory.inventoryGui.cvsGui, 175);
                    world.player.inventory.ShowInventory();
                }
            }
            if (pressedKeys.Contains(Key.D1)) //Num Key 1 (Not numpad)
            {
                //Select Hotbar Slot 1
                world.player.inventory.hotbarSlotList[0].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D2)) //Num Key 2 (Not numpad)
            {
                //Select Hotbar Slot 2
                world.player.inventory.hotbarSlotList[1].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D3)) //Num Key 3 (Not numpad)
            {
                //Select Hotbar Slot 3
                world.player.inventory.hotbarSlotList[2].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D4)) //Num Key 4 (Not numpad)
            {
                //Select Hotbar Slot 4
                world.player.inventory.hotbarSlotList[3].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D5)) //Num Key 5 (Not numpad)
            {
                //Select Hotbar Slot 5
                world.player.inventory.hotbarSlotList[4].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D6)) //Num Key 6 (Not numpad)
            {
                //Select Hotbar Slot 6
                world.player.inventory.hotbarSlotList[5].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D7)) //Num Key 7 (Not numpad)
            {
                //Select Hotbar Slot 7
                world.player.inventory.hotbarSlotList[6].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D8)) //Num Key 8 (Not numpad)
            {
                //Select Hotbar Slot 8
                world.player.inventory.hotbarSlotList[7].SelectSlot();
            }
            if (pressedKeys.Contains(Key.D9)) //Num Key 9 (Not numpad)
            {
                //Select Hotbar Slot 9
                world.player.inventory.hotbarSlotList[8].SelectSlot();
            }
            if (pressedKeys.Contains(Key.Escape)) //Num Key 9 (Not numpad)
            {
                if(!world.HasOpenGui(false))
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
                else
                {
                    //Create a list of all the guis that need to be closed
                    List<Gui> removeGuiList = [.. world.guiList];

                    foreach (Gui gui in removeGuiList)
                    {
                        if (gui.id == "sc:inventory")
                        {
                            gui.inventory.HideInventory();
                        }
                        else
                        {
                            gui.Hide();
                        }
                    }
                }
            }
            else if(pressedKeys.Contains(world.settings.cNotifications))
            {
                //Open notification list gui
                if(world.notificationHandler.gui.isOpen)
                {
                    world.notificationHandler.HideGui();
                }
                else
                {
                    world.notificationHandler.ShowGui();
                }
            }
            if (pressedKeys.Contains(world.settings.cToggleDebug))
            {
                //Open debug menu
                if (world.debugMenu.isEnabled)
                {
                    world.debugMenu.Hide();
                }
                else
                {
                    world.debugMenu.Show();
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
                world.log.Write($"Could not get rectangle for canvas {canvas.Uid}: {ex.Message}", "Warning");
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
                world.log.Write($"Could not get rectangle for border {border.Uid}: {ex.Message}", "Warning");
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
                world.log.Write($"Could not get rectangle for grid {grid.Uid}: {ex.Message}", "Warning");
                return new Rect(1, 1, 1, 1);
            }
        }

        //-- Event Handlers --//
        
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //Add pressed key to the collection if it isn't in there already
            if (world.debugMenu.tbDebug.IsKeyboardFocused == false)
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
            foreach (Inventory inventory in world.inventoryList)
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
            if (world.finishedLoading)
            {
                world.tmrMovement.Stop();
                if (world.settings.saveWorldOnClose == true)
                {
                    //Save all chunks and the inventory of the player
                    foreach (Chunk chunk in world.totalChunkList)
                    {
                        chunk.Save();
                    }
                    world.player.SaveInventory(world.worldDirectory);
                    world.player.SavePosition(world.worldDirectory);
                }
            }

            //Check if the user wants to return to the menu, else close the entire app
            if (world.returnToMenu)
            {
                world.wndMenu.Show();
            }
            else
            {
                world.wndMenu.Close();
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
            world.wndMenu.wndSettings = new wndSettings(world.wndMenu, world.settings);
            world.wndMenu.wndSettings.ShowDialog();
        }

        private void btnSaveWorld_Click(object sender, RoutedEventArgs e)
        {
            //Save all chunks and the inventory of the player
            foreach (Chunk chunk in world.totalChunkList)
            {
                chunk.Save();
            }
            world.player.SaveInventory(world.worldDirectory);
            world.player.SavePosition(world.worldDirectory);

            //Show confirmation
            MessageBox.Show("Successfully saved the World!", "Save World", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnExitToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            //Show the main menu window and close the game window
            world.returnToMenu = true;
            Close();
        }

        //disables scrolling with the mouse wheel
        private void svWorld_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Select the hotbar slots
            int newSlot;
            if (e.Delta < 0)
            {
                newSlot = (world.player.inventory.GetSelectedIndex() + 1) % 9;
            }
            else
            {
                newSlot = (world.player.inventory.GetSelectedIndex() - 1) % 9;
                if (newSlot == -1) newSlot = 8;
            }
            world.player.inventory.hotbarSlotList[newSlot].SelectSlot();

            e.Handled = true;
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            //Show the notification list gui
            world.notificationHandler.ShowGui();
            bdrMenu.Visibility = Visibility.Hidden;
        }
    }
}

