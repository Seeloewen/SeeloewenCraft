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
using static System.Environment;

namespace SeeloewenCraft
{
    public partial class wndMenu : Window
    {

        private System.Windows.Forms.Timer tmrSplashText = new System.Windows.Forms.Timer();
        private wndLoadWorld wndLoadWorld;
        private wndSettings wndSettings;
        public wndGame wndGame;
        private int splashTextSize = 0;
        public int worldVersion = 1;
        public string gameVersion = "Alpha 1.1.3";
        public string gameDirectory;
        public string texturepackDirectory;
        public string selectedTexturepack;
        public int texturepackVersion;
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);

        //-- Constructor --//

        public wndMenu()
        {
            InitializeComponent();

            //Setup the splash text timer
            tmrSplashText.Tick += tmrSplashText_Tick;
            tmrSplashText.Interval = 50;
            tmrSplashText.Start();

            //Show the version
            tblVersion.Text = string.Format("Version {0}", gameVersion);

            //Check if the game directory exists and create it otherwise
            if (!Directory.Exists(string.Format("{0}/SeeloewenCraft/", appData)))
            {
                Directory.CreateDirectory(string.Format("{0}/SeeloewenCraft/", appData));
            }

            //Check if the world directory exists
            if (!Directory.Exists(string.Format("{0}/SeeloewenCraft/worlds", appData)))
            {
                Directory.CreateDirectory(string.Format("{0}/SeeloewenCraft/worlds", appData));
            }

            gameDirectory = string.Format("{0}/SeeloewenCraft/", appData);
            wndSettings = new wndSettings(this);
        }

        //-- Event Handlers --//

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //Show the world selection window
            wndLoadWorld = new wndLoadWorld(this);
            wndLoadWorld.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Close the game
            Close();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Show the settings window
            wndSettings = new wndSettings(this);
            wndSettings.ShowDialog();
        }

        private void tmrSplashText_Tick(object sender, EventArgs e)
        {
            splashTextSize++;
            //Increase or decrease the splash text size based on the current number
            if (splashTextSize >= 0 && splashTextSize < 15)
            {
                tblAlpha.FontSize -= 0.3;
            }
            else if(splashTextSize >= 15 && splashTextSize < 30)
            {
                tblAlpha.FontSize += 0.3;
            }
            else if(splashTextSize == 30)
            {
                //Reset the number
                splashTextSize = 0;
            }
        }

        private void wndMenu1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Close the app
            Application.Current.Shutdown();
        }
    }
}
