using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.notifications;
using System;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        public static void HandlePingCommand(string[] args)
        {
            if (NetworkHandler.IsMultiplayer())
            {
                NetworkHandler.SendData(MultiplayerPacketType.REQUEST, DateTime.Now.ToString());

                if (NetworkHandler.IsServer())
                {
                    HandleSystemMessage("You can see the ping to your clients in the log.");
                }
            }
            else
            {
                HandleSystemMessage("This command only works in Multiplayer!");
            }
        }
    }
}