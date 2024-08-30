
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

            if (ItemRegister.GenerateItem(id) == null)
            {
                MessageBox.Show("Invalid command syntax: item id was not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Game.world.player.inventory.AddItem(id, amount, ItemRegister.GenerateItem(id).tag, out int remainingAmount);
            if (remainingAmount > 0)
            {
                MessageBox.Show("Warning: Not all items were added to your inventory since it's full.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"Succesfully gave {amount}x {id} to player.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
