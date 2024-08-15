using System.Windows;
using System.Windows.Media;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleNotificationCommand(string[] args)
        {
            //Check if the command has enough args
            if (args.Length < 4)
            {
                MessageBox.Show("Invalid command syntax: incorrect number of arguments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Invalid command syntax: can't parse amount to int", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Get the image
            ImageBrush image = ItemRegister.GenerateItem(args[2], world).image;

            //Append all parts of the message to the message string
            string message = args[3];

            for (int i = 4; i < args.Length; i++)
            {
                message += $" {args[i]}";
            }

            //Show the notification
            world.notificationHandler.ShowNotification(message, time, image);
        }
    }
}
