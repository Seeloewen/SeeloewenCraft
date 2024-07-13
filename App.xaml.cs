using SeeloewenCraft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using static System.Environment;

namespace Random_2D_Terrain_Generator_2._0
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {

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

            Console.WriteLine(StartOptions.skipMenu);
            Console.WriteLine(StartOptions.showLog);

            if (StartOptions.modded)
            {
                ModLoader.LoadMods();
            }

            wndMenu wndMenu = new wndMenu();

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

                World world = new World(wndMenu, "debug", true, wndMenu.worldVersion, wndMenu.gameVersion, wndMenu.log);
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