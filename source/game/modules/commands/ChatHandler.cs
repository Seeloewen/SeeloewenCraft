using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.notifications;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SeeloewenCraft.game.core.commands
{
    public static partial class ChatHandler
    {
        private static readonly Dictionary<string, Action<string[]>> commandMap = new()
        {
            {"give", HandleGiveCommand },
            {"setblock", HandleSetBlockCommand },
            {"spawn", HandleSpawnCommand },
            {"gamemode", HandleGamemodeCommand },
            {"tp", HandleTPCommand },
            {"heal", HandleHealCommand },
            {"damage", HandleDamageCommand },
            {"hp", HandleHPCommand },
            {"fly", HandleFlyCommand },
            {"help", HandleHelpCommand },
            {"ping", HandlePingCommand }
        };

        private static List<string> messages;

        public static void Init()
        {
            messages = new List<string>();
        }

        public static string[] GetMessages() => messages.ToArray();

        public static void HandlePlayerMessage(string message, string playerName)
        {
            if (message[0] == '/') //In case of command, don't display the message but handle the command
            {
                HandleCommand(message);
                return;
            }

            messages.Add($"{playerName}: {message}");
        }

        public static void HandleSystemMessage(string message)
        {
            messages.Add(message);
        }

        private static void HandleCommand(string command)
        {
            Log.Write($"Handling command '{command}'...", LogType.GENERAL, LogLevel.INFO);

            //Remove the slash at the beginning
            command = command.ToLower().Remove(0, 1);

            //Split up the command into args so it can be handled
            string cmd = command.Split(' ')[0];
            string[] args = command.Split(' ').Skip(1).Take(command.Length - 2).ToArray();

            if (commandMap.TryGetValue(cmd, out Action<string[]> a))
            {
                a.Invoke(args);
            }
            else
            {
                HandleSystemMessage("Unknown command. Type /help for a list of available commands.");
            }
        }
    }
}
