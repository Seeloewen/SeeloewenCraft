using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using SeeloewenCraft.launcher;
using System;
using System.IO;

namespace SeeloewenCraft
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void Exit()
        {
            var lifetime = Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            lifetime?.Shutdown();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Log.Write(
                $"SeeloewenCraft Version {Game.GAME_VERSION} ({Game.VERSION_DATE})",
                LogType.GENERAL,
                LogLevel.INFO);

            FolderUtil.InitializeDirectories();

            Game.playerId = GetPlayerId();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                wndMenu wndMenu = new wndMenu();

                if (!StartOptions.skipMenu)
                {
                    desktop.MainWindow = wndMenu;

                    wndMenu.Show();
                }
                else
                {
                    if (Directory.Exists(Path.Combine(FolderUtil.worldsFolder, "Debug")))
                    {
                        Directory.Delete(Path.Combine(FolderUtil.worldsFolder, "Debug"), true);
                    }

                    if (StartOptions.showLog)
                    {
                        Log.Show();
                    }

                    Game.Create(
                        "Debug",
                        StartOptions.seed,
                        true,
                        MultiplayerType.OFFLINE,
                        wndMenu);
                }
            }

            if (StartOptions.showLog)
            {
                Log.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Write(
                    "An unhandled exception has occured. Creating crash dump...",
                    LogType.GENERAL,
                    LogLevel.ERROR);

                Log.CreateCrashDump(ex);

                Game.ShowException(ex);

                if (Settings.saveLogOnExit)
                {
                    Log.Save(FolderUtil.logsFolder, false);
                }
            }
        }


        private int GetPlayerId()
        {
            int playerId;

            if (StartOptions.playerId != 0)
            {
                playerId = StartOptions.playerId;

                Log.Write($"Used player id from start options: {playerId}", LogType.GENERAL, LogLevel.INFO);
            }
            else if (File.Exists(FolderUtil.playerInfoFile))
            {
                try
                {
                    //Read the ID from the file
                    JObject playerInfo = JObject.Parse(File.ReadAllText(FolderUtil.playerInfoFile));

                    playerId = playerInfo.Get<int>("player_id");

                    Log.Write($"Read player id from file: {playerId}", LogType.GENERAL, LogLevel.INFO);
                }
                catch (Exception ex)
                {
                    //If reading from the json fails, delete the file and generate a new id. If the player had a previous ID that he wants back, he will need to
                    //Contact players he played with as his ID is saved in their worlds to match inventories
                    Log.Write($"Could not get player id from file, generating new one... ({ex.Message})", LogType.GENERAL, LogLevel.WARNING);
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

                JObject playerObj = new JObject()
                {
                    { "_comment", "This ID is used to identify you in multiplayer sessions. DO NOT CHANGE IT UNLESS YOU KNOW WHAT YOU ARE DOING."},
                    {"player_id",  playerId}
                };

                File.WriteAllText(FolderUtil.playerInfoFile, playerObj.ToString());
                Log.Write($"Generated player id {playerId}", LogType.GENERAL, LogLevel.INFO);
            }

            return playerId;
        }
    }

    public class StartOptions
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