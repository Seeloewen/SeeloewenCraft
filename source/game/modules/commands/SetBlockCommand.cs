using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleSetBlockCommand(string[] args)
        {
            if (args.Length != 5)
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: incorrect number of arguments");
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
                    NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: block id was not found");
                    return;
                }

                Game.world.SetBlock(block, posX + 8 * chunkID, posY);
                NotificationHandler.Notify("sc:stone_block", $"Successfully placed block {id} at x{posX} and y{posY} in chunk {chunkID}");
            }
            catch
            {
                NotificationHandler.Notify("sc:bedrock_item", $"Invalid command syntax: can't parse coordinates to int");
                return;
            }
        }
    }
}
