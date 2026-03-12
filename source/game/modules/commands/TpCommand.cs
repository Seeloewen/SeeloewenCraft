using SeeloewenCraft.game.core.entities;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        private static void HandleTPCommand(string[] args)
        {
            try
            {
                int posX = int.Parse(args[0]);
                int posY = int.Parse(args[1]);

                Player.Get().posX = posX;
                Player.Get().posY = posY;
                HandleSystemMessage($"Succesfully teleported player to position x={posX}, y={posY}");
            }
            catch
            {
                HandleSystemMessage("Invalid command syntax: can't parse coordinates to int");
                return;
            }
        }

    }
}
