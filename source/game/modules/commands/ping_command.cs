using System;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        public static void HandlePingCommand(string[] args)
        {
            if (Game.IsMultiplayer())
            {
                NetworkHandler.SendData(MultiplayerPacketType.REQUEST, DateTime.Now.ToString());

                if (Game.IsServer())
                {
                    NotificationHandler.ShowNotification("You can see the ping to your clients in the log.", 3000);
                }
            }
            else
            {
                NotificationHandler.ShowNotification("This command only works in Multiplayer!", 3000);
            }
        }
    }
}