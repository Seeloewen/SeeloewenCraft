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
            string[] worlds = Directory.GetDirectories($"{wndMenu.gameDirectory}worlds/");

            //List the worlds in the combobox
            foreach(string world in worlds)
            {
                cbxWorld.Items.Add(world.Replace($"{wndMenu.gameDirectory}worlds/", ""));
            }
        }

        //-- Event Handlers --//

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(cbxWorld.Text) == false)
            {
                wndMenu.wndGame = new wndGame(wndMenu, cbxWorld.Text, false, wndMenu.worldVersion, wndMenu.gameVersion);
                if (wndMenu.wndGame.finishedLoading)
                {
                    wndMenu.Hide();
                    wndMenu.wndGame.ShowDialog();
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
            wndCreateWorld = new wndCreateWorld(wndMenu);
            Close();
            wndCreateWorld.ShowDialog();
        }
    }
}
