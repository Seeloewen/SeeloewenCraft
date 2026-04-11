using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        private static void HandleGiveCommand(string[] args)
        {
            if (!(args.Length == 1 || args.Length == 2))
            {
                HandleSystemMessage("Invalid command syntax: incorrect number of arguments");
                return;
            }
            string id = args[0];
            int amount = 1;
            if (args.Length > 2)
            {
                try
                {
                    amount = int.Parse(args[1]);
                }
                catch
                {
                    HandleSystemMessage("Invalid command syntax: couldn't parse amount to int");
                    return;
                }
            }

            if (!ItemRegister.Exists(id))
            {
                HandleSystemMessage("Invalid command syntax: item id was not found");
                return;
            }

            int remaining = Player.Get().inventory.Add(id, amount, ItemRegister.Get(id).tag);
            if (remaining > 0)
            {
                HandleSystemMessage("Warning: Not all items were added (Full Inventory)");
            }
            else
            {
                HandleSystemMessage($"Succesfully gave {amount}x {id} to player.");
            }
        }
    }
}
