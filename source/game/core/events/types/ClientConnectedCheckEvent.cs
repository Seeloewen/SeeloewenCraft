using SeeloewenCraft.game.networking;

namespace SeeloewenCraft.game.core.events
{
    internal class ClientConnectedCheckEvent : GameEvent
    {
        public ClientConnectedCheckEvent() : base(20000) { } //Should only be called by the client

        protected override void Run()
        {
            //Checks for each client if it is listed as connected. If not, disconnect it properly
            for (int i = 0; i < NetworkHandler.server.clients.Count; i++)
            {
                IdTcpClient client = NetworkHandler.server.clients[i];

                if (!client.isConnected)
                {
                    NetworkHandler.server.Disconnect(client, "Timed out");
                }
                else
                {
                    client.isConnected = false;
                }
            }
        }
    }
}
