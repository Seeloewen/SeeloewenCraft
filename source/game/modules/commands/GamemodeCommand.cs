using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleGamemodeCommand(string[] args)
        {
            if (args.Length != 2)
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: incorrect number of arguments");
                return;
            }
            string gamemode = args[1];

            //Set gamemode based on given input
            if (gamemode == "0" || gamemode == "survival")
            {
                World.Get().SetGamemode(Gamemode.Survival);
                NotificationHandler.Notify("sc:grass_item", "Gamemode was changed to survival mode");
            }
            else if (gamemode == "1" || gamemode == "creative")
            {
                World.Get().SetGamemode(Gamemode.Creative);
                NotificationHandler.Notify("sc:diamond_item", "Gamemode was changed to creative mode");
            }
            else
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: gamemode type not found");
            }
        }
    }
}
