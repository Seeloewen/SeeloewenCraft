using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Newtonsoft.Json;


namespace SeeloewenCraft
{
    public partial class wndGame : Window
    {
        //References
        public World world;
        public HashSet<Key> pressedKeys = new HashSet<Key>();
        public Canvas cvsInvItem = new Canvas();
        public TextBlock tblInvItem = new TextBlock();
        public Point mousePosition;

        //Variables
        public int goLeftAmount = 10;
        public int goRightAmount = 10;
        public double relativeSvPos = 0;
        public double defaultSvPos = 0;

        //-- Constructor --//

        public wndGame()
        {
            world = Game.world;
            InitializeComponent();
            ApplyVideoSettings();

            //Setup canvas for the item currently held in inventory
            cvsGame.Children.Add(cvsInvItem);
            cvsInvItem.Width = 55;
            cvsInvItem.Height = 55;
            Panel.SetZIndex(cvsInvItem, 5);

            //Setup textblock for the item currently held in inventory
            tblInvItem.FontSize = 18;
            Canvas.SetTop(tblInvItem, 40);
            Canvas.SetLeft(tblInvItem, 45);
            cvsInvItem.Children.Add(tblInvItem);
        }

        //-- Custom Methods --//

        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            //OpenTK Code here...
        }
        private void HandleKeyPresses()
        {
            if (pressedKeys.Contains(Settings.cShowInv)) //E key
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

                    foreach (Gui gui in removeGuiList)
                    {
                        if (gui.id == "sc:inventory")
                        {
                            gui.inventory.Hide();
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
                    Canvas.SetLeft(world.player.inventory.inventoryGui.cvsGui, 290);
                    world.player.inventory.Show();
                }
            }
            if (pressedKeys.Contains(Key.D1)) //Num Key 1 (Not numpad)
            {
                //Select Hotbar Slot 1
                world.player.inventory.hotbarSlotList[0].Select();
            }
            if (pressedKeys.Contains(Key.D2)) //Num Key 2 (Not numpad)
            {
                //Select Hotbar Slot 2
                world.player.inventory.hotbarSlotList[1].Select();
            }
            if (pressedKeys.Contains(Key.D3)) //Num Key 3 (Not numpad)
            {
                //Select Hotbar Slot 3
                world.player.inventory.hotbarSlotList[2].Select();
            }
            if (pressedKeys.Contains(Key.D4)) //Num Key 4 (Not numpad)
            {
                //Select Hotbar Slot 4
                world.player.inventory.hotbarSlotList[3].Select();
            }
            if (pressedKeys.Contains(Key.D5)) //Num Key 5 (Not numpad)
            {
                //Select Hotbar Slot 5
                world.player.inventory.hotbarSlotList[4].Select();
            }
            if (pressedKeys.Contains(Key.D6)) //Num Key 6 (Not numpad)
            {
                //Select Hotbar Slot 6
                world.player.inventory.hotbarSlotList[5].Select();
            }
            if (pressedKeys.Contains(Key.D7)) //Num Key 7 (Not numpad)
            {
                //Select Hotbar Slot 7
                world.player.inventory.hotbarSlotList[6].Select();
            }
            if (pressedKeys.Contains(Key.D8)) //Num Key 8 (Not numpad)
            {
                //Select Hotbar Slot 8
                world.player.inventory.hotbarSlotList[7].Select();
            }
            if (pressedKeys.Contains(Key.D9)) //Num Key 9 (Not numpad)
            {
                //Select Hotbar Slot 9
                world.player.inventory.hotbarSlotList[8].Select();
            }
            if (pressedKeys.Contains(Key.Escape)) //Num Key 9 (Not numpad)
            {
                if (!world.HasOpenGui(false))
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
                            gui.inventory.Hide();
                        }
                        else
                        {
                            gui.Hide();
                        }
                    }
                }
            }
            else if (pressedKeys.Contains(Settings.cNotifications))
            {
                //Open notification list gui
                if (NotificationHandler.gui.isOpen)
                {
                    NotificationHandler.HideGui();
                }
                else
                {
                    NotificationHandler.ShowGui();
                }
            }
            if (pressedKeys.Contains(Settings.cToggleDebug))
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
                Log.Write($"Could not get rectangle for canvas {canvas.Uid}: {ex.Message}", "Warning");
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
                Log.Write($"Could not get rectangle for border {border.Uid}: {ex.Message}", "Warning");
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
                Log.Write($"Could not get rectangle for grid {grid.Uid}: {ex.Message}", "Warning");
                return new Rect(1, 1, 1, 1);
            }
        }

        public void ApplyVideoSettings()
        {
            if (world != null)
            {
                //Apply resolution

                //If a custom resolution is used
                if (Settings.resolution == "Custom")
                {
                    Resize(Settings.customResX, Settings.customResY, false);

                    //Allow user to change resolution for themselves
                    ResizeMode = ResizeMode.CanResize;
                }
                else //If a preset resolution is used
                {
                    ResizeMode = ResizeMode.CanMinimize;

                    switch (Settings.resolution)
                    {
                        case "320x180":
                            Resize(640, 360, false);
                            break;
                        case "640x360":
                            Resize(640, 360, false);
                            break;
                        case "1280x720":
                            Resize(1280, 720, false);
                            break;
                        case "1920x1080":
                            Resize(1920, 1080, false);
                            break;
                        case "2560x1440":
                            Resize(2560, 1440, false);
                            break;
                        case "3840x2160":
                            Resize(3840, 2160, false);
                            break;
                    }
                }

                //Apply video mode
                if (Settings.videoMode == "Windowed")
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                    btnClose.Visibility = Visibility.Hidden;
                }
                else if (Settings.videoMode == "Borderless")
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Normal;
                    btnClose.Visibility = Visibility.Visible;
                }
                else if (Settings.videoMode == "Fullscreen")
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    btnClose.Visibility = Visibility.Visible;
                }
            }
        }
        public void Resize(double width, double height, bool scaleOnly)
        {
            if (!scaleOnly)
            {
                WindowState = WindowState.Normal;
                Width = width;
                Height = height;
            }

            //Calculate new scaling based on resolution
            double scaleX = width / 1280;
            double scaleY = height / 720;

            //Apply scaling
            ScaleTransform scaleTransform = new ScaleTransform(scaleX, scaleY);
            cvsGame.LayoutTransform = scaleTransform;
        }

        public void ShowInvItem(InventorySlot slot)
        {
            cvsInvItem.Visibility = Visibility.Visible;
            cvsInvItem.Background = slot.cvsItem.Background;
            Canvas.SetLeft(cvsInvItem, mousePosition.X + 5);
            Canvas.SetTop(cvsInvItem, mousePosition.Y + 5);
            tblInvItem.Text = slot.Amount.ToString();
        }

        public void HideInvItem()
        {
            cvsInvItem.Visibility = Visibility.Hidden;
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

            InventorySlot selectedSlot = world.GetSelectedInvSlot();

            //Make the canvas follow the mouse
            if (selectedSlot != null)
            {
                Canvas.SetLeft(cvsInvItem, mousePosition.X + 5);
                Canvas.SetTop(cvsInvItem, mousePosition.Y + 5);
            }
        }


        private void wndGame1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //If the setting to save worlds on closing is enabled
            if (world.finishedLoading)
            {
                world.tmrMovement.Stop();
                if (Settings.saveWorldOnClose)
                {
                    world.Save();
                    world.gameLoop.tmrGameLoop.Stop();
                }
            }

            //Save the user settings
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                Settings.Save(writer);
                writer.WriteToFile($"{FolderUtil.gameFolder}\\clientSettings.json");
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
            world.wndMenu.wndSettings = new wndSettings(world.wndMenu, false);
            world.wndMenu.wndSettings.ShowDialog();
        }

        private void btnSaveWorld_Click(object sender, RoutedEventArgs e)
        {
            world.Save();
            if (world.gameLoop.tmrGameLoop.IsRunning) world.gameLoop.tmrGameLoop.Stop();

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
                newSlot = (world.player.inventory.GetSelectedHotbarIndex() + 1) % 9;
            }
            else
            {
                newSlot = (world.player.inventory.GetSelectedHotbarIndex() - 1) % 9;
                if (newSlot == -1) newSlot = 8;
            }
            world.player.inventory.hotbarSlotList[newSlot].Select();

            e.Handled = true;
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            //Show the notification list gui
            NotificationHandler.ShowGui();
            bdrMenu.Visibility = Visibility.Hidden;
        }

        private void wndGame1_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //If custom resolution is enabled, save the new resolution
            if (Settings.resolution == "Custom")
            {
                Settings.customResX = Convert.ToInt32(e.NewSize.Width);
                Settings.customResY = Convert.ToInt32(e.NewSize.Height);
            }

            //Rescale the game based on the new values
            Resize(e.NewSize.Width, e.NewSize.Height, true);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //What did you expect this button to do?
            Close();
        }

        private void btnToggleRenderer_Click(object sender, RoutedEventArgs e)
        {
            //DEBUG - Toggle visibility of WPF renderer for OpenGL testing
            cvsWorld.Visibility = cvsWorld.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
    }
}

