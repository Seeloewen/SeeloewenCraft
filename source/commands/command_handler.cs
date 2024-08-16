using System.Windows;

namespace SeeloewenCraft
{
    public static partial class CommandHandler
    {
        public static void HandleCommand(string command)
        {
            //Remove the slash at the beginning
            command = command.ToLower().Remove(0, 1);

            Log.Write($"Handling command: {command}", "Info");

            //Split up the command into args so it can be handled
            string[] args = command.Split(' ');

            //Handle the given command
            switch (args[0])
            {
                case "give":
                    HandleGiveCommand(args);
                    break;
                case "setblock":
                    HandleSetBlockCommand(args);
                    break;
                case "spawn":
                    HandleSpawnCommand(args);
                    break;
                case "gamemode":
                    HandleGamemodeCommand(args);
                    break;
                case "tp":
                case "teleport":
                    HandleTPCommand(args);
                    break;
                case "heal":
                    HandleHealCommand(args);
                    break;
                case "damage":
                    HandleDamageCommand(args);
                    break;
                case "hp":
                    HandleHPCommand(args);
                    break;
                case "fly":
                    HandleFlyCommand(args);
                    break;
                case "notification":
                    HandleNotificationCommand(args);
                    break;
                case "ping":
                    Write("pong");
                    break;
                case "help":
                    MessageBox.Show("List of commands:" +
                        "\n/help - Shows this menu" +
                        "\n/give [itemId] <amount> - Gives you a specific item (+ optional amount)" +
                        "\n/setblock [blockId] [posX] [posY] <chunkId> - Places a block at a specific location" +
                        "\n/spawn [entityId] [absPosX] [absPosY] - Spawns an entity at a given location" +
                        "\n/gamemode [type] - Switches your gamemode" +
                        "\n/tp [absPosX] [absPosY] - Teleports you to specified position" +
                        "\n/ping - Return pong, used as a test command");
                    break;
                default:
                    Write("Invalid command! Type /help for a list of commands.");
                    break;
            }
        }

        private static void Write(string msg)
        {
            Log.Write($"{msg}", "Info");
        }
    }
}
