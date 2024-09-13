
using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleGiveCommand(string[] args)
        {
            if (!(args.Length == 2 || args.Length == 3))
            {
                NotificationHandler.ShowNotification("Invalid command syntax: incorrect number of arguments", 3000, Images.Arrow.GetTexture());
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
                    NotificationHandler.ShowNotification("Invalid command syntax: couldn't parse amount to int", 3000, Images.Arrow.GetTexture());
                    return;
                }
            }

            if (ItemRegister.GenerateItem(id) == null)
            {
                NotificationHandler.ShowNotification("Invalid command syntax: item id was not found", 3000, Images.Arrow.GetTexture());
                return;
            }

            Game.world.player.inventory.AddItem(id, amount, ItemRegister.GenerateItem(id).tag, out int remainingAmount);
            if (remainingAmount > 0)
            {
                NotificationHandler.ShowNotification("Warning: Not all items were added (Full Inventory)", 3000, Images.Arrow.GetTexture());
            }
            else
            {
                NotificationHandler.ShowNotification($"Succesfully gave {amount}x {id} to player.", 3000, Images.Arrow.GetTexture());
            }
        }
    }
}
