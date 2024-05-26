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
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            wndMenu wndMenu = new wndMenu();

            switch (e.Args.Length)
            {
                case 0:
                    wndMenu.Show();
                    break;
                case 1:
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

                    wndGame wndGame = new wndGame(wndMenu, "debug", true, wndMenu.worldVersion, wndMenu.gameVersion);
                    wndGame.Show();
                    wndMenu.Owner = wndGame;

                    break;
            }

        }
    }
}