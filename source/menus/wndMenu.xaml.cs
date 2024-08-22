using System;
using System.Windows;

namespace SeeloewenCraft
{
    public partial class wndMenu : Window
    {

        //References
        private System.Windows.Forms.Timer tmrSplashText = new System.Windows.Forms.Timer();
        private wndLoadWorld wndLoadWorld;
        public wndSettings wndSettings;
        private SplashTextHandler splashTextHandler;

        //Variables
        private int splashTextSize = 0;

        //-- Constructor --//

        public wndMenu()
        {
            InitializeComponent();

            //Setup the splash text timer
            tmrSplashText.Tick += tmrSplashText_Tick;
            tmrSplashText.Interval = 50;
            tmrSplashText.Start();

            //Display version
            tblVersion.Text = string.Format("Version {0}", Game.GAME_VERSION);

            //set splashtext
            splashTextHandler = new SplashTextHandler(this);
            tblSplashText.Text = splashTextHandler.GetText(); ;

            wndSettings = new wndSettings(this, true);
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
            wndSettings = new wndSettings(this, false);
            wndSettings.ShowDialog();
        }

        private void tmrSplashText_Tick(object sender, EventArgs e)
        {
            splashTextSize++;
            //Increase or decrease the splash text size based on the current number
            if (splashTextSize >= 0 && splashTextSize < 15)
            {
                tblSplashText.FontSize -= 0.3;
            }
            else if (splashTextSize >= 15 && splashTextSize < 30)
            {
                tblSplashText.FontSize += 0.3;
            }
            else if (splashTextSize == 30)
            {
                //Reset the number
                splashTextSize = 0;
            }
        }

        private void wndMenu1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save the log, if enabled
            if (Settings.saveLogOnExit)
            {
                Log.Save(FolderUtil.logsFolder, false);
            }

            //Close the app
            Application.Current.Shutdown();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            wndServer wndServer = new wndServer(this);
            wndServer.ShowDialog();
        }
    }
}
