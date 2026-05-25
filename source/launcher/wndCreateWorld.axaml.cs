using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SeeloewenCraft.game;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using System.IO;
using System.Text.RegularExpressions;

namespace SeeloewenCraft.launcher
{
    public partial class wndCreateWorld : Window
    {
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

        private void btnCreateWorld_Click(object sender, RoutedEventArgs e)
        {
            //Check if the world name isn't blank
            if (!string.IsNullOrEmpty(tbWorldName.Text))
            {
                //Check if the world already exists
                if (!Directory.Exists(Path.Combine(FolderUtil.worldsFolder, tbWorldName.Text)))
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
                    var box = MessageBoxManager.GetMessageBoxStandard("Error", "A world with the specified name already exists.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                    box.ShowAsync();
                }
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "Please enter a world name!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                box.ShowAsync();
            }
        }

        private void cbSeed_Click(object sender, RoutedEventArgs e)
        {
            tbSeed.IsEnabled = (bool)cbSeed.IsChecked;
        }

        private void tbSeed_PreviewTextInput(object sender, TextInputEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
