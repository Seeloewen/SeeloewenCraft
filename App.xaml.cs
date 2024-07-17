using System.Windows;
using System.IO;
using static System.Environment;

namespace SeeloewenCraft
{
    public partial class App : Application
    {
        wndMenu wndMenu = new wndMenu();

        public App() : base()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Create crash dump
            wndMenu.log.Write("A crash has occured and an unhandled exception has occured. Creating crash dump...", "Error");
            wndMenu.log.CreateCrashDump(e.Exception);

            //Save log before the game exits if enabled
            if (wndMenu.settings.saveLogOnExit)
            {
                wndMenu.log.Save(wndMenu.logDirectory, false);
            }
        }

        class StartOptions
        {
            public static bool skipMenu;
            public static bool showLog;
            public static bool modded;
            public static void Init(string[] args)
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

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            StartOptions.Init(e.Args);

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

                World world = new World(wndMenu, "Debug", true, wndMenu.worldVersion, wndMenu.gameVersion, wndMenu.log);
                world.wndGame.Show();
                wndMenu.Owner = world.wndGame;
            }

            if (StartOptions.showLog)
            {
                wndMenu.log.Show();
            }

        }
    }
}