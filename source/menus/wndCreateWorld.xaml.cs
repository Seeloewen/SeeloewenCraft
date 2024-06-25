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
            if(!string.IsNullOrEmpty(tbWorldName.Text))
            {
                //Check if the world already exists
                if(!Directory.Exists($"{wndMenu.worldDirectory}/{tbWorldName.Text}"))
                {
                    //Create a new world
                    wndMenu.wndGame = new wndGame(wndMenu, tbWorldName.Text, true, wndMenu.worldVersion, wndMenu.gameVersion, wndMenu.log);
                    wndMenu.Hide();
                    wndMenu.wndGame.Show();
                    Close();
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
