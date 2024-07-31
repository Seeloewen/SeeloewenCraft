using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleSpawnCommand(string[] args)
        {
            if (args.Length != 4)
            {
                MessageBox.Show("Invalid command syntax: incorrect number of arguments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Entity entity = EntityRegister.GenerateEntity(args[1], world);
            if (entity == null)
            {
                MessageBox.Show("Invalid command syntax: entity id was not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                entity.posX = int.Parse(args[2]);
                entity.posY = int.Parse(args[3]);
            } 
            catch
            {
                MessageBox.Show("Invalid command syntax: couldn't parse coordinates to int", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            world.AddEntity(entity);
            MessageBox.Show($"Successfully spawned entity {entity.id} at x{entity.posX} y{entity.posY}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
