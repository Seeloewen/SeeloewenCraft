using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleTPCommand(string[] args)
        {
            try
            {
                int posX = int.Parse(args[1]);
                int posY = int.Parse(args[2]);

                Game.world.player.posX = posX;
                Game.world.player.posY = posY;
                MessageBox.Show($"Succesfully teleported player to position x={posX}, y={posY}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch 
            {
                MessageBox.Show("Invalid command syntax: can't parse coordinates to int", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

    }
}
