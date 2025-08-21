using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft.game.core.legacy
{

    public class NotificationGui : Gui
    {
        //Controls
        ScrollViewer svNotifications = new ScrollViewer() { Width = 357, Height = 430, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        ListView lvNotifications = new ListView() { Background = new SolidColorBrush(Colors.Transparent) };
        Button btnClose = new Button() { Width = 160, Height = 30, Content = "Close", FontSize = 16 };
        Button btnClearAll = new Button() { Width = 160, Height = 30, Content = "Clear All", FontSize = 16 };

        public NotificationGui(int height, int width, int top, int left, string id) : base(height, width, top, left, id)
        {
            //Setup gui
            tblHeader.Text = "Notifications";

            Canvas.SetTop(tblHeader, 12);
            Canvas.SetLeft(tblHeader, 10);

            Canvas.SetLeft(svNotifications, 9);
            Canvas.SetTop(svNotifications, 53);
            cvsGui.Children.Add(svNotifications);

            Canvas.SetTop(btnClose, 492);
            Canvas.SetLeft(btnClose, 192);
            cvsGui.Children.Add(btnClose);

            Canvas.SetTop(btnClearAll, 492);
            Canvas.SetLeft(btnClearAll, 20);
            cvsGui.Children.Add(btnClearAll);

            btnClearAll.Click += btnClearAll_Click;
            btnClose.Click += btnClose_Click;

            svNotifications.Content = lvNotifications;
        }

        public void AddNotification(string message)
        {
            //Create components
            Border bdrNotification = new Border() { Width = 342, Height = 75, BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(3) };
            Canvas cvsNotification = new Canvas() { Background = new SolidColorBrush(Colors.DarkGray) };
            TextBlock tblNotification = new TextBlock() { FontSize = 18, Width = 250, Height = 50, TextWrapping = TextWrapping.Wrap };
            Canvas cvsImage = new Canvas() { Width = 50, Height = 50 };

            //Setup necessary components for showing the message
            bdrNotification.Child = cvsNotification;

            Canvas.SetTop(tblNotification, 10);
            Canvas.SetLeft(tblNotification, 75);
            cvsNotification.Children.Add(tblNotification);

            Canvas.SetTop(cvsImage, 10);
            Canvas.SetLeft(cvsImage, 10);
            cvsNotification.Children.Add(cvsImage);

            tblNotification.Text = message;

            lvNotifications.Items.Add(bdrNotification);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Hide the notification list gui
            NotificationHandler.HideGui();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //Ask the user whether they really want to clear the notification list
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to clear all notifications?", "Clear Notifications", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                //Clear the notification list
                case MessageBoxResult.Yes:
                    lvNotifications.Items.Clear();
                    break;
            }
        }
    }

    public class UnchiselerGui : Gui
    {

        TextBlock tblUnchisel = new TextBlock() { Text = "Unchisel a block:", FontSize = 20, FontWeight = FontWeights.DemiBold };
        Button btnUnchisel = new Button() { Content = "Break down", Width = 125, Height = 30, FontSize = 16 };

        public UnchiselerGui(int height, int width, int top, int left, string id) : base(height, width, top, left, id)
        {
            //Setup the gui
            inventory = new Inventory(1, 1, false);

            //TODO: render unchiseler gui next to inventory

            btnUnchisel.Click += BtnUnchisel_Click;
        }

        private void BtnUnchisel_Click(object sender, RoutedEventArgs e)
        {
            Unchisel();
        }

        private void Unchisel()
        {
            //Check if there's an item in the slot and the item is a chiselitem
            if (!inventory.slotList[0].IsEmpty())
            {
                Item item = ItemRegister.GenerateItem(inventory.slotList[0].itemId);
                bool unchiselSuccess = false;
                int successItems = 0;

                if (item is ChiseledItem chisItem && chisItem.isChiseled)
                {
                    for (int i = 0; i < inventory.slotList[0].amount; i++)
                    {
                        //Add the output to the inventory
                        foreach (Item outItem in chisItem.Unchisel())
                        {
                            Game.world.player.inventory.AddItem(outItem.id, 1, outItem.tag, out int remainingItems);

                            if (remainingItems > 0)
                            {
                                inventory.slotList[0].Remove(successItems);
                                MessageBox.Show("This item cannot be unchiseled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        successItems++;
                    }

                    unchiselSuccess = true;
                }
                else
                {
                    MessageBox.Show("This item cannot be unchiseled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //If an item was unchiseled, clear the slot
                if (unchiselSuccess) inventory.slotList[0].Remove(inventory.slotList[0].amount);

            }
            else
            {
                MessageBox.Show("Please select an item to unchisel!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}