using SeeloewenCraft.game;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.networking;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace SeeloewenCraft.launcher
{

    public partial class wndServer : Window
    {
        wndMenu wndMenu;

        public wndServer(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;

            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbIp.Text) || string.IsNullOrEmpty(tbPort.Text))
            {
                MessageBox.Show("Please specify an IP and a port!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Temporarily disable input
            btnConnect.Content = "Connecting...";
            btnClose.IsEnabled = false;
            btnConnect.IsEnabled = false;

            //Try to connect with the server
            NetworkHandler.client = new Client();
            await NetworkHandler.client.Connect(tbIp.Text, Convert.ToInt32(tbPort.Text));

            if (NetworkHandler.client.isConnected)
            {
                //If the connection was successful, initialize the world
                wndMenu.Hide();
                MessageBox.Show("This feature is temporarily unavailable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //Game.Create(DateTime.Now.Microsecond.ToString(), 0, true, MultiplayerType.CLIENT, wndMenu);
                //NetworkHandler.client.Initialize();
                Close();
            }
            else
            {
                MessageBox.Show($"Failed to connect to the server: {NetworkHandler.client.connectionException.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            btnConnect.Content = "Connect";
            btnClose.IsEnabled = true;
            btnConnect.IsEnabled = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbPort_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
