using System.IO;
using System.Windows;

namespace SeeloewenCraft
{
    public partial class App : Application
    {
        //entry point of program
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //add custom dipatcher method for unhandled exceptions to save them to the log
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            //Show the version in log
            Log.Write($"SeeloewenCraft Version {Game.GAME_VERSION} ({Game.VERSION_DATE})", "Info");

            //parse command line arguments to start option variables
            StartOptions.Parse(e.Args);

            //Initialize folders
            FolderUtil.InitializeDirectories();

            //load mods if enabled
            if (StartOptions.modded)
            {
                ModLoader.LoadMods();
            }

            wndMenu wndMenu = new wndMenu();

            // if start option "-skipmenu" is disabled, proceed like normal
            if (!StartOptions.skipMenu)
            {
                wndMenu.Show();
            }
            else // if start option "-skipmenu" is enabled
            {
                //delete world "debug" if it exists
                if (Directory.Exists($"{FolderUtil.worldsFolder}/debug"))
                {
                    Directory.Delete($"{FolderUtil.worldsFolder}/debug", true);
                }

                //create new world with name "debug"
                World world = new World(wndMenu, "Debug", true, Game.WORLD_VERSION, Game.GAME_VERSION);
            }

            //show start log on start of program if enabled through start options
            if (StartOptions.showLog)
            {
                Log.Show();
            }

        }
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Create crash dump
            Log.Write("An unhandled exception has occured. Creating crash dump...", "Error");
            Log.CreateCrashDump(e.Exception);
            Game.ShowException(e.Exception);

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
        public static bool startCreative;
        public static bool disableLighting;
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
                    case "-STARTCREATIVE":
                        startCreative = true;
                        break;
                    case "-DISABLELIGHTING":
                        disableLighting = true;
                        break;
                }
            }
        }
    }

}