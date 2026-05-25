using SeeloewenCraft.game.util.logging;
using System.IO;
using static System.Environment;

namespace SeeloewenCraft.game.util
{
    public static class FolderUtil
    {
        public static string appData;
        public static string gameFolder;
        public static string worldsFolder;
        public static string logsFolder;
        public static string texturepacksFolder;
        public static string playerInfoFile;

        public static void InitializeDirectories()
        {
            appData = GetFolderPath(SpecialFolder.ApplicationData);

            //Check if the game directory exists and create it otherwise
            if (!Directory.Exists(Path.Combine(appData, "SeeloewenCraft")))
            {
                Directory.CreateDirectory(Path.Combine(appData, "SeeloewenCraft"));
                Log.Write($"Created game directory {appData}/SeeloewenCraft", LogType.GENERAL, LogLevel.INFO);
            }

            gameFolder = Path.Combine(appData, "SeeloewenCraft");
            Log.Write($"Set game directory to {gameFolder}", LogType.GENERAL, LogLevel.INFO);

            //Check if the world directory exists
            if (!Directory.Exists(Path.Combine(gameFolder, "worlds")))
            {
                Directory.CreateDirectory(Path.Combine(gameFolder, "worlds"));
                Log.Write($"Created world directory {gameFolder}/worlds", LogType.GENERAL, LogLevel.INFO);
            }
            worldsFolder = Path.Combine(gameFolder, "worlds");
            Log.Write($"Set world directory to {gameFolder}/worlds", LogType.GENERAL, LogLevel.INFO);

            //Check if the log directory exists
            if (!Directory.Exists(Path.Combine(gameFolder, "logs")))
            {
                Directory.CreateDirectory(Path.Combine(gameFolder, "logs"));
                Log.Write($"Set log directory to {gameFolder}/logs", LogType.GENERAL, LogLevel.INFO);
            }
            logsFolder = Path.Combine(gameFolder, "logs");
            Log.Write($"Set game directory to {logsFolder}", LogType.GENERAL, LogLevel.INFO);

            //Check if the texturepack directory exists, create it otherwise
            if (!Directory.Exists(Path.Combine(gameFolder, "texturepacks")))
            {
                Directory.CreateDirectory(Path.Combine(gameFolder, "texturepacks"));
                Log.Write($"Set texturepack directory to {gameFolder}/texturepack", LogType.GENERAL, LogLevel.INFO);
            }
            texturepacksFolder = Path.Combine(gameFolder, "texturepacks");
            Log.Write($"Set texturepack directory to {texturepacksFolder}", LogType.GENERAL, LogLevel.INFO);

            playerInfoFile = Path.Combine(gameFolder, "playerInfo.json");
        }
    }


}
