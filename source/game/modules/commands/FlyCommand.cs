using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        public static void HandleFlyCommand(string[] args)
        {
            if (args.Length == 1)
            {
                Game.world.player.flying = !Game.world.player.flying;
                NotificationHandler.Notify("sc:glass_item", $"Flying status has been set to {Game.world.player.flying}");
            }
            else
            {
                try
                {
                    Game.world.player.flying = bool.Parse(args[1]);
                    NotificationHandler.Notify("sc:glass_item", $"Flying status has been set to {Game.world.player.flying}");
                }
                catch
                {
                    NotificationHandler.Notify("sc:bedrock_item", $"Invalid command syntax: Couldn't parse flying status to bool");
                }
            }
        }

    }
}
