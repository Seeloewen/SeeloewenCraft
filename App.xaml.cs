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

        private class StartOptions
        {
            public bool skipMenu;
            public bool showLog;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            StartOptions startOptions = GetStartOptions(e.Args);

            Console.WriteLine(startOptions.skipMenu);
            Console.WriteLine(startOptions.showLog);


            wndMenu wndMenu = new wndMenu();

            if (!startOptions.skipMenu)
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

                wndGame wndGame = new wndGame(wndMenu, "debug", true, wndMenu.worldVersion, wndMenu.gameVersion, wndMenu.log);
                wndGame.Show();
                wndMenu.Owner = wndGame;
            }

            if(startOptions.showLog)
            {
                wndMenu.log.Show();
            }

        }

    


    private StartOptions GetStartOptions(string[] args)
    {
        StartOptions options = new StartOptions();

        foreach (string arg in args)
        {
            switch (arg.ToUpper())
            {
                case "-SKIPMENU":
                    options.skipMenu = true;
                    break;
                case "-SHOWLOG":
                    options.showLog = true;
                    break;
            }
        }

        return options;
    }

}
}