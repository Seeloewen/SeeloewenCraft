
using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleGiveCommand(string[] args)
        {
            if (!(args.Length == 2 || args.Length == 3))
            {
                MessageBox.Show("Invalid command syntax: incorrect number of arguments", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string id = args[1];
            int amount = 1;
            if (args.Length > 2)
            {
                try
                {
                    amount = int.Parse(args[2]);
                }
                catch
                {
                    MessageBox.Show("Invalid command syntax: can't parse amount to int", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            for (int i = 0; i < amount; i++)
            {
                Item item = ItemRegister.GenerateItem(id, world);
                if(i == 0 && item == null)
                {
                    MessageBox.Show("Invalid command syntax: item id was not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                world.player.inventory.AddItem(item);
            }
            MessageBox.Show($"Succesfully gave {amount}x {id} to player.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
