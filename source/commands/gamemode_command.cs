using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleGamemodeCommand(string[] args)
        {
            if (args.Length != 2)
            {
                NotificationHandler.ShowNotification("Invalid command syntax: incorrect number of arguments", 3000, Images.Emerald.GetTexture());
                return;
            }
            string gamemode = args[1];

            //Set gamemode based on given input
            if (gamemode == "0" || gamemode == "survival")
            {
                Game.world.SetGamemode(Gamemode.Survival);
                NotificationHandler.ShowNotification("Gamemode was changed to survival mode", 3000, Images.Emerald.GetTexture());
            }
            else if (gamemode == "1" || gamemode == "creative")
            {
                Game.world.SetGamemode(Gamemode.Creative);
                NotificationHandler.ShowNotification("Gamemode was changed to creative mode", 3000, Images.Emerald.GetTexture());
            }
            else
            {
                NotificationHandler.ShowNotification("Invalid command syntax: gamemode type not found", 3000, Images.Emerald.GetTexture());
            }
        }
    }
}
