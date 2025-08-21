using SeeloewenCraft.game.networking;
using System.Windows;

namespace SeeloewenCraft.launcher
{
    public partial class wndMultiplayer : Window
    {
        wndMenu wndMenu;

        public wndMultiplayer(wndMenu wndMenu)
        {
            InitializeComponent();
            this.wndMenu = wndMenu;
        }

        private void btnConnectToServer_Click(object sender, RoutedEventArgs e)
        {
            //Show the server connection screen
            wndServer wndServer = new wndServer(wndMenu);
            wndServer.ShowDialog();

            Close();
        }

        private void btnHostServer_Click(object sender, RoutedEventArgs e)
        {
            //Show the world selection window
            wndLoadWorld wndLoadWorld = new wndLoadWorld(wndMenu, MultiplayerType.SERVER);
            wndLoadWorld.ShowDialog();

            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbAgreeWarning_Click(object sender, RoutedEventArgs e)
        {
            btnConnectToServer.IsEnabled = (bool)cbAgreeWarning.IsChecked;
            btnHostServer.IsEnabled = (bool)cbAgreeWarning.IsChecked;
        }
    }
}
