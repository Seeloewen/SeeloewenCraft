using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeeloewenCraft
{

    public partial class wndLoadWorld : Window
    {

        public List<string> worldList = new List<string>();
        public wndMenu wndMenu;
        public wndCreateWorld wndCreateWorld;

        //-- Constructor --//
        public wndLoadWorld(wndMenu wndMenu)
        {
            InitializeComponent();
            
            //Set the main menu
            this.wndMenu = wndMenu;

            //Get the world list
            LoadWorlds();
        }

        //-- Custom Methods --//

        private void LoadWorlds()
        {
            //Get all worlds
            string[] worlds = Directory.GetDirectories(wndMenu.worldDirectory);

            //List the worlds in the combobox
            foreach(string world in worlds)
            {
                cbxWorld.Items.Add(world.Replace(wndMenu.worldDirectory + "\\", ""));
            }
        }

        //-- Event Handlers --//

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(cbxWorld.Text) == false)
            {
                wndMenu.wndGame = new wndGame(wndMenu, cbxWorld.Text, false, wndMenu.worldVersion, wndMenu.gameVersion, wndMenu.log);
                if (wndMenu.wndGame.finishedLoading)
                {
                    wndMenu.Hide();
                    wndMenu.wndGame.Show();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Please select a world you want to play or create a new one", "Error");
            }
        }

        private void btnCreateNewWorld_Click(object sender, RoutedEventArgs e)
        {
            //Show the window for creating a new world
            wndCreateWorld = new wndCreateWorld(wndMenu);
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
                            Directory.Delete($"{wndMenu.worldDirectory}\\{cbxWorld.Text}",true);
                            MessageBox.Show($"Successfully deleted world {cbxWorld.Text}!", "Delete world", MessageBoxButton.OK, MessageBoxImage.Information);
                            cbxWorld.Items.Remove(cbxWorld.Text);
                        }
                        catch (Exception ex)
                        {
                            wndMenu.log.Write($"Could not delete world {cbxWorld.SelectedItem}: {ex.Message}", "Info");
                        }
                        wndMenu.log.Write($"Deleted world {cbxWorld.SelectedItem}", "Info");
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
