using System.IO;
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

        public static void InitializeDirectories()
        { 
            appData = GetFolderPath(SpecialFolder.ApplicationData);

            //Check if the game directory exists and create it otherwise
            if (!Directory.Exists($"{appData}\\SeeloewenCraft"))
            {
                Directory.CreateDirectory($"{appData}\\SeeloewenCraft");
                Log.Write($"Created game directory {appData}\\SeeloewenCraft", "Info");
            }
            gameFolder = $"{appData}\\SeeloewenCraft";
            Log.Write($"Set game directory to {gameFolder}", "Info");

            //Check if the world directory exists
            if (!Directory.Exists($"{gameFolder}\\worlds"))
            {
                Directory.CreateDirectory($"{gameFolder}\\worlds");
                Log.Write($"Created world directory {gameFolder}\\worlds", "Info");
            }
            worldsFolder = $"{gameFolder}\\worlds";
            Log.Write($"Set world directory to {gameFolder}\\worlds", "Info");

            //Check if the log directory exists
            if (!Directory.Exists($"{gameFolder}\\logs"))
            {
                Directory.CreateDirectory($"{gameFolder}\\logs");
                Log.Write($"Set log directory to {gameFolder}\\logs", "Info");
            }
            logsFolder = $"{gameFolder}\\logs";
            Log.Write($"Set game directory to {logsFolder}", "Info");

            //Check if the texturepack directory exists, create it otherwise
            if (!Directory.Exists($"{gameFolder}\\texturepacks"))
            {
                Directory.CreateDirectory($"{gameFolder}\\texturepacks");
                Log.Write($"Set texturepack directory to {gameFolder}\\texturepack", "Info");
            }
            texturepacksFolder = $"{gameFolder}\\texturepacks";
            Log.Write($"Set texturepack directory to {texturepacksFolder}", "Info");
        }
    }


}
