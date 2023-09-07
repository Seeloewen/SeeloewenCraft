using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public partial class wndCreateWorld : Window
    {
        //-- Constructor --//

        public wndMenu wndMenu;
        public wndGame wndGame;

        public wndCreateWorld(wndMenu wndMenu)
        {
            InitializeComponent();

            //Set main window
            this.wndMenu = wndMenu;
        }

        //-- Event Handlers --//

        private void btnCreateWorld_Click(object sender, RoutedEventArgs e)
        {
            //Check if the world name isn't blank
            if(string.IsNullOrEmpty(tbWorldName.Text) == false)
            {
                //Check if the world already exists
                if(Directory.Exists(string.Format("{0}/{1}", wndMenu.gameDirectory, tbWorldName.Text)) == false)
                {
                    //Create a new world
                    wndGame = new wndGame(tbWorldName.Text, true);
                    wndGame.Show();
                    wndMenu.Owner = wndGame;
                    wndMenu.Close();
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
    }
}
