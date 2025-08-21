using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.legacy;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleGiveCommand(string[] args)
        {
            if (!(args.Length == 2 || args.Length == 3))
            {
                NotificationHandler.ShowNotification("Invalid command syntax: incorrect number of arguments", 3000);
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
                    NotificationHandler.ShowNotification("Invalid command syntax: couldn't parse amount to int", 3000);
                    return;
                }
            }

            if (ItemRegister.GenerateItem(id) == null)
            {
                NotificationHandler.ShowNotification("Invalid command syntax: item id was not found", 3000);
                return;
            }

            Game.world.player.inventory.AddItem(id, amount, ItemRegister.GenerateItem(id).tag, out int remainingAmount);
            if (remainingAmount > 0)
            {
                NotificationHandler.ShowNotification("Warning: Not all items were added (Full Inventory)", 3000);
            }
            else
            {
                NotificationHandler.ShowNotification($"Succesfully gave {amount}x {id} to player.", 3000);
            }
        }
    }
}
