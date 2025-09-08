using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.notifications;
using Windows.ApplicationModel.UserDataAccounts;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleGiveCommand(string[] args)
        {
            if (!(args.Length == 2 || args.Length == 3))
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: incorrect number of arguments");
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
                    NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: couldn't parse amount to int");
                    return;
                }
            }

            if (ItemRegister.Get(id) == null)
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: item id was not found");
                return;
            }

            int remaining = Game.world.player.inventory.Add(id, amount, ItemRegister.Get(id).tag);
            if (remaining > 0)
            {
                NotificationHandler.Notify("sc:lantern_item", "Warning: Not all items were added (Full Inventory)");
            }
            else
            {
                NotificationHandler.Notify(id, $"Succesfully gave {amount}x {id} to player.");
            }
        }
    }
}
