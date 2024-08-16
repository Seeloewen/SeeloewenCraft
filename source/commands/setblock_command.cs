using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleSetBlockCommand(string[] args)
        {
            if (args.Length != 5)
            {
                MessageBox.Show("Invalid command syntax: incorrect number of arguments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string id = args[1];
            try
            {
                int posX = int.Parse(args[2]);
                int posY = int.Parse(args[3]);
                int chunkID = int.Parse(args[4]);

                Block block = BlockRegister.GenerateBlock(id);

                if(block == null)
                {
                    MessageBox.Show("Invalid command syntax: block id was not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Game.world.SetBlock(block , posX + 8 * chunkID, posY);
                MessageBox.Show($"Successfully placed block {id} at x{posX} and y{posY} in chunk {chunkID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Invalid command syntax: can't parse coordinates to int", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }        
        }
    }
}
