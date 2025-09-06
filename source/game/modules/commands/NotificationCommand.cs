using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.legacy;
using System.Windows.Media;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleNotificationCommand(string[] args)
        {
            //Check if the command has enough args
            if (args.Length < 4)
            {
                NotificationHandler.ShowNotification("Invalid command syntax: incorrect number of arguments", 3000);
                return;
            }

            //Try to parse the time to an int
            int time;

            try
            {
                time = int.Parse(args[1]);
            }
            catch
            {
                NotificationHandler.ShowNotification("Invalid command syntax: can't parse amount to int", 3000);
                return;
            }

            //Get the image
            ImageBrush image = ItemRegister.Get(args[2]).image;

            //Append all parts of the message to the message string
            string message = args[3];

            for (int i = 4; i < args.Length; i++)
            {
                message += $" {args[i]}";
            }

            //Show the notification
            NotificationHandler.ShowNotification(message, time);
        }
    }
}
