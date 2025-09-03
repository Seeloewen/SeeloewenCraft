using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.legacy;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleSetBlockCommand(string[] args)
        {
            if (args.Length != 5)
            {
                NotificationHandler.ShowNotification("Invalid command syntax: incorrect number of arguments", 3000);
                return;
            }
            string id = args[1];
            try
            {
                int posX = int.Parse(args[2]);
                int posY = int.Parse(args[3]);
                int chunkID = int.Parse(args[4]);

                Block block = BlockRegister.Get(id);

                if (block == null)
                {
                    NotificationHandler.ShowNotification("Invalid command syntax: block id was not found", 3000);
                    return;
                }

                Game.world.SetBlock(block, posX + 8 * chunkID, posY);
                NotificationHandler.ShowNotification($"Successfully placed block {id} at x{posX} and y{posY} in chunk {chunkID}", 3000);
            }
            catch
            {
                NotificationHandler.ShowNotification($"Invalid command syntax: can't parse coordinates to int", 3000);
                return;
            }
        }
    }
}
