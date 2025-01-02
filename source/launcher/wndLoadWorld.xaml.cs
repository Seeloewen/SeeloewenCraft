using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SeeloewenCraft
{
    public partial class wndLoadWorld : Window
    {

        public List<string> worldList = new List<string>();
        public wndMenu wndMenu;
        public wndCreateWorld wndCreateWorld;
        private MultiplayerType multiplayerType;

        //-- Constructor --//
        public wndLoadWorld(wndMenu wndMenu, MultiplayerType multiplayerType)
        {
            InitializeComponent();

            //Set the main menu
            this.wndMenu = wndMenu;
            this.multiplayerType = multiplayerType;

            //Get the world list
            LoadWorlds();
        }

        //-- Custom Methods --//

        private void LoadWorlds()
        {
            //Get all worlds
            string[] worlds = Directory.GetDirectories(FolderUtil.worldsFolder);

            //List the worlds in the combobox
            foreach (string world in worlds)
            {
                cbxWorld.Items.Add(world.Replace(FolderUtil.worldsFolder + "\\", ""));
            }
        }

        //-- Event Handlers --//

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbxWorld.Text) == false)
            {
                World world = new World(wndMenu, cbxWorld.Text, 0, false, Game.WORLD_VERSION, Game.GAME_VERSION, multiplayerType); 
                wndMenu.Hide();
                Close();
            }
            else
            {
                MessageBox.Show("Please select a world you want to play or create a new one", "Error");
            }
        }

        private void btnCreateNewWorld_Click(object sender, RoutedEventArgs e)
        {
            //Show the window for creating a new world
            wndCreateWorld = new wndCreateWorld(wndMenu, multiplayerType);
            Close();
            wndCreateWorld.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxWorld.Text))
            {
                //Ask the user if they want to delete the world
                MessageBoxResult result = MessageBox.Show("You will not be able to recover deleted worlds. Are you sure you want to delete this world?", "Delete world", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Directory.Delete($"{FolderUtil.worldsFolder}\\{cbxWorld.Text}", true);
                            MessageBox.Show($"Successfully deleted world {cbxWorld.Text}!", "Delete world", MessageBoxButton.OK, MessageBoxImage.Information);
                            cbxWorld.Items.Remove(cbxWorld.Text);
                        }
                        catch (Exception ex)
                        {
                            Log.Write($"Could not delete world {cbxWorld.SelectedItem}: {ex.Message}\n{ex.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
                        }
                        Log.Write($"Deleted world {cbxWorld.SelectedItem}", LogType.GENERAL, LogLevel.INFO);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Please select the world that you want to delete!", "Delete world", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
