using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        private static void HandleGamemodeCommand(string[] args)
        {
            if (args.Length != 1)
            {
                HandleSystemMessage("Invalid command syntax: incorrect number of arguments");
                return;
            }
            string gamemode = args[0];

            //Set gamemode based on given input
            if (gamemode == "0" || gamemode == "survival")
            {
                World.Get().SetGamemode(Gamemode.Survival);
                HandleSystemMessage("Gamemode was changed to survival mode");
            }
            else if (gamemode == "1" || gamemode == "creative")
            {
                World.Get().SetGamemode(Gamemode.Creative);
                HandleSystemMessage("Gamemode was changed to creative mode");
            }
            else
            {
                HandleSystemMessage("Invalid command syntax: gamemode type not found");
            }
        }
    }
}
