namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        public static void HandleFlyCommand(string[] args)
        {
            if (args.Length == 1)
            {
                Game.world.player.flying = !Game.world.player.flying;
                NotificationHandler.ShowNotification($"Flying status has been set to {Game.world.player.flying}", 3000, Images.Air.GetTexture());
            }
            else
            {
                try
                {
                    Game.world.player.flying = bool.Parse(args[1]);
                    NotificationHandler.ShowNotification($"Flying status has been set to {Game.world.player.flying}", 3000, Images.Air.GetTexture());
                }
                catch
                {
                    NotificationHandler.ShowNotification($"Invalid command syntax: Couldn't parse flying status to bool", 3000, Images.Air.GetTexture());
                }
            }
        }

    }
}
