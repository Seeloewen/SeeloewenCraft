using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        private static void HandleSetBlockCommand(string[] args)
        {
            if (args.Length != 4)
            {
                HandleSystemMessage("Invalid command syntax: incorrect number of arguments");
                return;
            }
            string id = args[0];
            try
            {
                int posX = int.Parse(args[1]);
                int posY = int.Parse(args[2]);
                int chunkID = int.Parse(args[3]);

                Block block = BlockRegister.Get(id);

                if (block == null)
                {
                    HandleSystemMessage("Invalid command syntax: block id was not found");
                    return;
                }

                World.Get().SetBlock(new PositionData(posX, posY, chunkID), block);
                HandleSystemMessage($"Successfully placed block {id} at x{posX} and y{posY} in chunk {chunkID}");
            }
            catch
            {
                HandleSystemMessage($"Invalid command syntax: can't parse coordinates to int");
                return;
            }
        }
    }
}
