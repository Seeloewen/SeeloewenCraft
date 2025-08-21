using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.networking;
using System;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        public static void HandlePingCommand(string[] args)
        {
            if (NetworkHandler.IsMultiplayer())
            {
                NetworkHandler.SendData(MultiplayerPacketType.REQUEST, DateTime.Now.ToString());

                if (NetworkHandler.IsServer())
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