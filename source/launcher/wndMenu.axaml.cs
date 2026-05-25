using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.OpenGL;
using Avalonia.Threading;
using SeeloewenCraft.game;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;

namespace SeeloewenCraft.launcher
{
    public partial class wndMenu : Window
    {

        //References
        private DispatcherTimer tmrSplashText = new DispatcherTimer();
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
            tmrSplashText.Interval = new TimeSpan(0, 0, 0, 0, 50);
            tmrSplashText.Start();

            //Display version
            tblVersion.Text = string.Format("Version {0}", Game.GAME_VERSION);

            //set splashtext
            splashTextHandler = new SplashTextHandler();
            tblSplashText.Text = splashTextHandler.GetText(); ;

            wndSettings = new wndSettings(true);
        }

        //-- Event Handlers --//

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //Show the world selection window
            wndLoadWorld = new wndLoadWorld(this, MultiplayerType.OFFLINE);
            wndLoadWorld.ShowDialog(this);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Close the game
            Close();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Show the settings window
            wndSettings = new wndSettings(false);
            wndSettings.ShowDialog(this);
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

        private void wndMenu_Closing(object sender, WindowClosingEventArgs e)
        {
            //Save the log, if enabled
            if (Settings.saveLogOnExit)
            {
                Log.Save(FolderUtil.logsFolder, false);
            }

            //Close the app
            App.Exit();
        }

        private void btnMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            wndMultiplayer wndMultiplayer = new wndMultiplayer(this);
            wndMultiplayer.ShowDialog(this);
        }
    }
}
