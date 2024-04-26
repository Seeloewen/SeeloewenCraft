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
        private int splashTextSize = 0;
        public string version = "Alpha 1.1.1-Dev";
        public string gameDirectory;
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
            tblVersion.Text = string.Format("Version {0}", version);

            //Check if the game directory exists and create it otherwise
            if (!Directory.Exists(string.Format("{0}/SeeloewenCraft/", appData)))
            {
                Directory.CreateDirectory(string.Format("{0}/SeeloewenCraft/", appData));
            }
            gameDirectory = string.Format("{0}/SeeloewenCraft/", appData);
        }

        //-- Event Handlers --//

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //Show the world selection window
            wndLoadWorld = new wndLoadWorld(this) { Owner = this };
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
            wndSettings = new wndSettings { Owner = this };
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
    }
}
