using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SeeloewenCraft.game.networking;
using System;
using System.Text.RegularExpressions;

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
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "Please specify an IP and a port!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await box.ShowAsync();
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
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "This feature is temporarily unavailable.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await box.ShowAsync();
                //Game.Create(DateTime.Now.Microsecond.ToString(), 0, true, MultiplayerType.CLIENT, wndMenu);
                //NetworkHandler.client.Initialize();
                Close();
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to connect to the server: {NetworkHandler.client.connectionException.Message}", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await box.ShowAsync();
            }

            btnConnect.Content = "Connect";
            btnClose.IsEnabled = true;
            btnConnect.IsEnabled = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbPort_PreviewTextInput(object sender, TextInputEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
