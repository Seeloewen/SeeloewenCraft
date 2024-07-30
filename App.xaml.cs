using System.IO;
using System.Windows;
using static System.Environment;

namespace SeeloewenCraft
{
    public partial class App : Application
    {
        wndMenu wndMenu;

        public App() : base()
        {
            //add custom dipatcher method for unhandled exceptions to save them to the log
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //parse command line arguments to start option variables
            StartOptions.Parse(e.Args);

            //Initiliaze Game
            FolderUtil.InitializeDirectories();
            wndMenu = new wndMenu();

            //Do start options
            if (StartOptions.modded)
            {
                ModLoader.LoadMods();
            }

            if (!StartOptions.skipMenu)
            {
                wndMenu.Show();
            }
            else
            {
                string appData = GetFolderPath(SpecialFolder.ApplicationData);

                if (!Directory.Exists(string.Format("{0}/SeeloewenCraft/", appData)))
                {
                    Directory.CreateDirectory(string.Format("{0}/SeeloewenCraft/", appData));
                }
                string gameDirectory = string.Format("{0}/SeeloewenCraft/", appData);

                if (Directory.Exists(string.Format("{0}/worlds/{1}", gameDirectory, "debug")))
                {
                    Directory.Delete(string.Format("{0}/worlds/{1}", gameDirectory, "debug"), true);
                }

                World world = new World(wndMenu, "Debug", true, wndMenu.worldVersion, wndMenu.gameVersion);
                world.wndGame.Show();
                wndMenu.world = world;
                wndMenu.Owner = world.wndGame;
            }

            if (StartOptions.showLog)
            {
                Log.Show();
            }

        }
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Create crash dump
            Log.Write("An unhandled exception has occured. Creating crash dump...", "Error");
            Log.CreateCrashDump(wndMenu, e.Exception);

            //Save log before the game exits if enabled
            if (Settings.saveLogOnExit)
            {
               Log.Save(FolderUtil.logsFolder, false);
            }
        }

    }

    class StartOptions
    {
        public static bool skipMenu;
        public static bool showLog;
        public static bool modded;
        public static void Parse(string[] args)
        {
            foreach (string arg in args)
            {
                switch (arg.ToUpper())
                {
                    case "-SKIPMENU":
                        skipMenu = true;
                        break;
                    case "-SHOWLOG":
                        showLog = true;
                        break;
                    case "-MODDED":
                        modded = true;
                        break;
                }
            }
        }
    }

}