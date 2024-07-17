using System.IO;
using System.Windows;

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
                    wndMenu.world = new World(wndMenu, tbWorldName.Text, true, wndMenu.worldVersion, wndMenu.gameVersion, wndMenu.log);
                    wndMenu.Hide();
                    wndMenu.world.wndGame.Show();
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
