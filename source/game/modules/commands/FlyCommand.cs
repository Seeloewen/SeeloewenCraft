using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        public static void HandleFlyCommand(string[] args)
        {
            if (args.Length == 1)
            {
                Player.Get().flying = !Player.Get().flying;
                NotificationHandler.Notify("sc:glass_item", $"Flying status has been set to {Player.Get().flying}");
            }
            else
            {
                try
                {
                    Player.Get().flying = bool.Parse(args[1]);
                    NotificationHandler.Notify("sc:glass_item", $"Flying status has been set to {Player.Get().flying}");
                }
                catch
                {
                    NotificationHandler.Notify("sc:bedrock_item", $"Invalid command syntax: Couldn't parse flying status to bool");
                }
            }
        }

    }
}
