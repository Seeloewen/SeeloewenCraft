using System;
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
            Game.client = new Client();
            Game.client.Connect(tbIp.Text, Convert.ToInt32(tbPort.Text));

            Game.world = new World(wndMenu, DateTime.Now.Microsecond.ToString(), 0, true, 0, "", MultiplayerType.CLIENT);
            wndMenu.Hide();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
