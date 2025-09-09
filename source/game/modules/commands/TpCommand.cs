using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleTPCommand(string[] args)
        {
            try
            {
                int posX = int.Parse(args[1]);
                int posY = int.Parse(args[2]);

                Game.world.player.posX = posX;
                Game.world.player.posY = posY;
                NotificationHandler.Notify("sc:dirt_item", $"Succesfully teleported player to position x={posX}, y={posY}");
            }
            catch
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: can't parse coordinates to int");
                return;
            }
        }

    }
}
