using System;
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
            Log.Write($"SeeloewenCraft Version {Game.GAME_VERSION} ({Game.VERSION_DATE})", LogType.GENERAL, LogLevel.INFO);

            //parse command line arguments to start option variables
            StartOptions.Parse(e.Args);

            //Initialize folders
            FolderUtil.InitializeDirectories();

            Game.playerId = GetPlayerId();

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

                //show start log on start of program if enabled through start options
                if (StartOptions.showLog)
                {
                    Log.Show();
                }

                //create new world with name "debug"
                //World world = new World(wndMenu, "Debug", StartOptions.seed, true, Game.WORLD_VERSION, Game.GAME_VERSION, MultiplayerType.OFFLINE);
                Game.Create("Debug", StartOptions.seed, true, Game.WORLD_VERSION, Game.GAME_VERSION, MultiplayerType.OFFLINE, wndMenu);            
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
            Log.Write("An unhandled exception has occured. Creating crash dump...", LogType.GENERAL, LogLevel.ERROR);
            Log.CreateCrashDump(e.Exception);
            Game.ShowException(e.Exception);

            //Save log before the game exits if enabled
            if (Settings.saveLogOnExit)
            {
                Log.Save(FolderUtil.logsFolder, false);
            }
        }

        private int GetPlayerId()
        {
            int playerId;

            if(StartOptions.playerId != 0)
            {
               playerId = StartOptions.playerId;

                Log.Write($"Used player id from start options: {playerId}", LogType.GENERAL, LogLevel.INFO);
            }
            else if (File.Exists(FolderUtil.playerInfoFile))
            {
                try
                {
                    //Read the ID from the file
                    JsonToken playerInfoToken = JsonUtil.ReadFile(FolderUtil.playerInfoFile);
                    playerId = playerInfoToken.GetInt("/player_id");

                    Log.Write($"Read player id from file: {playerId}", LogType.GENERAL, LogLevel.INFO);
                }
                catch (Exception ex)
                {
                    //If reading from the json fails, delete the file and generate a new id. If the player had a previous ID that he wants back, he will need to
                    //Contact players he played with as his ID is saved in their worlds to match inventories
                    Log.Write("Could not get player id from file, generating new one...", LogType.GENERAL, LogLevel.WARNING);
                    File.Delete(FolderUtil.playerInfoFile);
                    playerId = GetPlayerId();
                }
            }
            else
            {
                //If no ID was specified, get a random integer as the ID
                //Not a very reliable approach as duplicates can occur, but it's highly unlikely and
                //doesn't really matter right now
                playerId = Game.rnd.Next();

                JsonWriter writer = JsonWriter.Create();
                writer.WriteStartObject();
                writer.WritePropertyName("_comment");
                writer.WriteValue("This ID is used to identify you in multiplayer sessions. DO NOT CHANGE IT UNLESS YOU KNOW WHAT YOU ARE DOING.");
                writer.WritePropertyName("player_id");
                writer.WriteValue(playerId);
                writer.WriteEndObject();

                File.WriteAllText(FolderUtil.playerInfoFile, writer.ToString());

                Log.Write($"Generated player id {playerId}", LogType.GENERAL, LogLevel.INFO);
            }

            return playerId;
        }
    }

    class StartOptions
    {
        public static bool skipMenu;
        public static bool showLog;
        public static bool modded;
        public static bool startCreative;
        public static bool disableLighting;
        public static int seed = 0;
        public static int playerId;

        public static void Parse(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
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
                    case "-SEED":
                        seed = int.Parse(args[i + 1]);
                        i++;
                        break;
                    case "-ID":
                        playerId = int.Parse(args[i + 1]);
                        i++;
                        break;
                }
            }
        }
    }

}