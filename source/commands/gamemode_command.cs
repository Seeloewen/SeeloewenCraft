using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleGamemodeCommand(string[] args)
        {
            if (args.Length != 2)
            {
                MessageBox.Show("Invalid command syntax: incorrect number of arguments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string gamemode = args[1];

            //Set gamemode based on given input
            if (gamemode == "0" || gamemode == "survival")
            {
                Game.world.SetGamemode(Gamemode.Survival);
                MessageBox.Show("Gamemode was changed to survival mode", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (gamemode == "1" || gamemode == "creative")
            {
                Game.world.SetGamemode(Gamemode.Creative);
                MessageBox.Show("Gamemode was changed to creative mode", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid command syntax: gamemode type not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
