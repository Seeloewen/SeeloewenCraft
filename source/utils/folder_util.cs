using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace SeeloewenCraft
{
    public static class FolderUtil
    {
        private static string appData;
        public static string gameFolder;
        public static string worldsFolder;
        public static string logsFolder;
        public static string texturepacksFolder;
        public static string? modFolder;

        public static void InitializeDirectories(Log log)
        { 
            appData = GetFolderPath(SpecialFolder.ApplicationData);

            //Check if the game directory exists and create it otherwise
            if (!Directory.Exists($"{appData}\\SeeloewenCraft"))
            {
                Directory.CreateDirectory($"{appData}\\SeeloewenCraft");
                log.Write($"Created game directory {appData}\\SeeloewenCraft", "Info");
            }
            gameFolder = $"{appData}\\SeeloewenCraft";
            log.Write($"Set game directory to {gameFolder}", "Info");

            //Check if the world directory exists
            if (!Directory.Exists($"{gameFolder}\\worlds"))
            {
                Directory.CreateDirectory($"{gameFolder}\\worlds");
                log.Write($"Created world directory {gameFolder}\\worlds", "Info");
            }
            worldsFolder = $"{gameFolder}\\worlds";
            log.Write($"Set world directory to {gameFolder}\\worlds", "Info");

            //Check if the log directory exists
            if (!Directory.Exists($"{gameFolder}\\logs"))
            {
                Directory.CreateDirectory($"{gameFolder}\\logs");
                log.Write($"Set log directory to {gameFolder}\\logs", "Info");
            }
            logsFolder = $"{gameFolder}\\logs";
            log.Write($"Set game directory to {logsFolder}", "Info");

            //Check if the texturepack directory exists, create it otherwise
            if (!Directory.Exists($"{gameFolder}\\texturepacks"))
            {
                Directory.CreateDirectory($"{gameFolder}\\texturepacks");
                log.Write($"Set texturepack directory to {gameFolder}\\texturepack", "Info");
            }
            texturepacksFolder = $"{gameFolder}\\texturepacks";
            log.Write($"Set texturepack directory to {texturepacksFolder}", "Info");
        }
    }


}
