using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace SeeloewenCraft
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
            Game.client = new Client();
            await Game.client.Connect(tbIp.Text, Convert.ToInt32(tbPort.Text));

            if (Game.client.isConnected)
            {
                //If the connection was successful, initialize the world
                Game.world = new World(wndMenu, DateTime.Now.Microsecond.ToString(), 0, true, 0, "", MultiplayerType.CLIENT);
                Game.client.Initialize();
                wndMenu.Hide();
                Close();
            }
            else
            {
                MessageBox.Show($"Failed to connect to the server: {Game.client.connectionException.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
