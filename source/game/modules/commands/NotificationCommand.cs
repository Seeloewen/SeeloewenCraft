using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.notifications;
using System.Windows.Media;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleNotificationCommand(string[] args)
        {
            //Check if the command has enough args
            if (args.Length < 3)
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: incorrect number of arguments");
                return;
            }

            //Get the image
            string icon = args[1];

            //Append all parts of the message to the message string
            string message = args[2];

            for (int i = 4; i < args.Length; i++)
            {
                message += $" {args[i]}";
            }

            //Show the notification
            NotificationHandler.Notify(icon, message);
        }
    }
}
