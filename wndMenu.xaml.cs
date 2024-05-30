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
        public Log log;
        private int splashTextSize = 0;
        public int worldVersion = 1;
        public string gameVersion = "Alpha 1.1.4-Dev5";
        public string versionDate = "30.05.2024";
        public string gameDirectory;
        public string texturepackDirectory;
        public string logDirectory;
        public string worldDirectory;
        public string selectedTexturepack;
        public int texturepackVersion;
        private string appData = GetFolderPath(SpecialFolder.ApplicationData);
        private SplashTextHandler splashTextHandler;
        bool setSplashText = false;

        //-- Constructor --//

        public wndMenu()
        {
            InitializeComponent();

            //Setup the splash text timer
            tmrSplashText.Tick += tmrSplashText_Tick;
            tmrSplashText.Interval = 50;
            tmrSplashText.Start();
            log = new Log();
            splashTextHandler = new SplashTextHandler(this);

            //Show the version
            tblVersion.Text = string.Format("Version {0}", gameVersion);
            log.Write($"SeeloewenCraft Alpha Version {gameVersion} ({versionDate})", "Info");

            //set splashtext        
            tblSplashText.Text = splashTextHandler.GetText();;
            
            //Create the game directories
            CreateDirectories();
            
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
                tblSplashText.FontSize -= 0.3;
            }
            else if(splashTextSize >= 15 && splashTextSize < 30)
            {
                tblSplashText.FontSize += 0.3;
            }
            else if(splashTextSize == 30)
            {
                //Reset the number
                splashTextSize = 0;
            }
        }

        private void wndMenu1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save the log, if enabled
            if (Properties.Settings.Default.saveLogOnExit)
            {
                log.Save(logDirectory, false);
            }

            //Close the app
            Application.Current.Shutdown();
        }

        //-- Custom Methods --//

        private void CreateDirectories()
        {
            //Check if the game directory exists and create it otherwise
            if (!Directory.Exists($"{appData}\\SeeloewenCraft"))
            {
                Directory.CreateDirectory($"{appData}\\SeeloewenCraft");
                log.Write($"Created game directory {appData}\\SeeloewenCraft", "Info");
            }
            gameDirectory = $"{appData}\\SeeloewenCraft";
            log.Write($"Set game directory to {gameDirectory}", "Info");

            //Check if the world directory exists
            if (!Directory.Exists($"{gameDirectory}\\worlds"))
            {
                Directory.CreateDirectory($"{gameDirectory}\\worlds");
                log.Write($"Created world directory {gameDirectory}\\worlds", "Info");
            }
            worldDirectory = $"{gameDirectory}\\worlds";
            log.Write($"Set world directory to {gameDirectory}\\worlds", "Info");

            //Check if the log directory exists
            if (!Directory.Exists($"{gameDirectory}\\logs"))
            {
                Directory.CreateDirectory($"{gameDirectory}\\logs");
                log.Write($"Set log directory to {gameDirectory}\\logs", "Info");
            }
            logDirectory = $"{gameDirectory}\\logs";
            log.Write($"Set game directory to {logDirectory}", "Info");

            //Check if the texturepack directory exists, create it otherwise
            if (!Directory.Exists($"{gameDirectory}\\texturepacks"))
            {
                Directory.CreateDirectory($"{gameDirectory}\\texturepacks");
                log.Write($"Set texturepack directory to {gameDirectory}\\texturepack", "Info");
            }
            texturepackDirectory = $"{gameDirectory}\\texturepacks";
            log.Write($"Set texturepack directory to {texturepackDirectory}", "Info");
        }
    }
}
