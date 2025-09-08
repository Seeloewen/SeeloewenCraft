using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.notifications;
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
                    NotificationHandler.Notify("sc:snowball_item", "You can see the ping to your clients in the log.");
                }
            }
            else
            {
                NotificationHandler.Notify("sc:bedrock_item", "This command only works in Multiplayer!");
            }
        }
    }
}