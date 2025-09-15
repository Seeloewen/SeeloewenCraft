using SeeloewenCraft.game.networking;

namespace SeeloewenCraft.game.core.events
{
    internal class SendConnectionStateEvent : GameEvent
    {
        public SendConnectionStateEvent() : base(10000) { } //Should only be called when the game is client

        protected override void Run()
        {
            //Sends a packet to the server that confirms that the connection is still standing
            NetworkHandler.SendData(MultiplayerPacketType.CONNECTION_CONFIRMATION, "");
        }
    }
}
