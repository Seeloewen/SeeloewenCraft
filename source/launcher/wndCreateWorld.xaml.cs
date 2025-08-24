using SeeloewenCraft.game;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace SeeloewenCraft.launcher
{
    public partial class wndCreateWorld : Window
    {
        //-- Constructor --//

        public wndMenu wndMenu;
        private MultiplayerType multiplayerType;

        public wndCreateWorld(wndMenu wndMenu, MultiplayerType multiplayerType)
        {
            InitializeComponent();

            //Set main window
            this.wndMenu = wndMenu;
            this.multiplayerType = multiplayerType;

            if (StartOptions.seed != 0)
            {
                cbSeed.IsChecked = true;
                tbSeed.Text = StartOptions.seed.ToString();
                tbSeed.IsEnabled = true;
            }
        }

        //-- Event Handlers --//

        private void btnCreateWorld_Click(object sender, RoutedEventArgs e)
        {
            //Check if the world name isn't blank
            if (!string.IsNullOrEmpty(tbWorldName.Text))
            {
                //Check if the world already exists
                if (!Directory.Exists($"{FolderUtil.worldsFolder}/{tbWorldName.Text}"))
                {
                    //Create a new world
                    wndMenu.Hide();
                    Close();

                    string worldName = tbWorldName.Text;
                    int seed = cbSeed.IsChecked == true ? int.Parse(tbSeed.Text) : 0;
                    Game.Create(worldName, seed, true, multiplayerType, wndMenu);
                }
                else
                {
                    MessageBox.Show("A world with the specified name already exists.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Please enter a world name!", "Error");
            }
        }

        private void cbSeed_Click(object sender, RoutedEventArgs e)
        {
            tbSeed.IsEnabled = (bool)cbSeed.IsChecked;
        }

        private void tbSeed_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
