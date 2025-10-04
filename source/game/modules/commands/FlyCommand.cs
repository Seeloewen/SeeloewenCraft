using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        public static void HandleFlyCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Player.Get().flying = !Player.Get().flying;
                HandleSystemMessage($"Your flying status has been set to {Player.Get().flying}");
            }
            else
            {
                try
                {
                    Player.Get().flying = bool.Parse(args[0]);
                    HandleSystemMessage($"Your flying status has been set to {Player.Get().flying}");
                }
                catch
                {
                    HandleSystemMessage($"Invalid command syntax: Couldn't parse flying status to bool");
                }
            }
        }

    }
}
