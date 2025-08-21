using SeeloewenCraft.game.core.legacy;

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
                NotificationHandler.ShowNotification($"Succesfully teleported player to position x={posX}, y={posY}", 3000);
            }
            catch
            {
                NotificationHandler.ShowNotification("Invalid command syntax: can't parse coordinates to int", 3000);
                return;
            }
        }

    }
}
