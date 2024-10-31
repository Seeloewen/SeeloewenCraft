using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using static SeeloewenCraft.Game;

namespace SeeloewenCraft
{
    public static partial class NetworkHandler
    {
        //Contains the multiplayer packet type and maps the expected arg length
        public static Dictionary<MultiplayerPacketType, (int length, Action<IdTcpClient, NetworkPacket> action)> packetTypeDictionary = new Dictionary<MultiplayerPacketType, (int length, Action<IdTcpClient, NetworkPacket> action)>() {
        { MultiplayerPacketType.ADD_TO_INV, (8, HandleAddToInv ) },
        { MultiplayerPacketType.REMOVE_FROM_INV, (6, HandleRemoveFromInv) },
        { MultiplayerPacketType.SYNC_POS, (5, HandleSyncPos) }, //1
        { MultiplayerPacketType.PRESSED_CHANGE, (6, HandlePressedChange) },
        { MultiplayerPacketType.CREATE_ENTITY, (1, HandleCreateEntity) },
        { MultiplayerPacketType.REMOVE_ENTITY, (1, HandleRemoveEntity) },
        { MultiplayerPacketType.INITIAL_LOAD, (2, HandleInitialLoad) },
        { MultiplayerPacketType.SET_BLOCK, (4, HandleSetBlock) },
        { MultiplayerPacketType.CREATE_CHUNK, (1, HandleCreateChunk) },
        { MultiplayerPacketType.PING_REQUEST, (1, HandlePingRequest) },
        { MultiplayerPacketType.PING_RESPONSE, (1, HandlePingResponse) },
        { MultiplayerPacketType.DISCONNECT, (1, HandleDisconnect) },
        { MultiplayerPacketType.CONNECTION_CONFIRMATION, (1, HandleConnectionConfirmation) } };


        public static NetworkPacket CreatePacket(MultiplayerPacketType type, params string[] data)
        {
            return new NetworkPacket(type, data);
        }

        public async static Task<byte[]> ReceivePacket(int size, NetworkStream stream)
        {
            //Read data into the buffer and return that buffer. The buffer has predetermined size to fit the packet size exactly.
            byte[] buffer = new byte[size];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            return buffer;
        }

        public static async void StartServer(int port)
        {
            //Start the server on port
            server = new Server();
            server.Start(port);
        }

        public static async void SendData(MultiplayerPacketType type, params string[] data)
        {
            //Send the data either as client or server. If it's neither, don't send anything
            if (IsServer() && server.clients.Count > 0)
            {
                await server.SendDataToAll(CreatePacket(type, data)); //Sends the data to all clients
            }
            else if (IsClient())
            {
                await client.SendData(CreatePacket(type, data)); //Sends the data only to the server
            }
        }

        public static async Task HandleData(IdTcpClient client, NetworkPacket packet)
        {
            if (!packet.isValid) return;

            try
            {
                if (packetTypeDictionary.TryGetValue(packet.type, out var entry))
                {
                    entry.action(client, packet);
                }
                else
                {
                    Log.Write($"Couldn't handle packet {packet.type} ({packet.content.ToString()})", LogType.NETWORK, LogLevel.ERROR);
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
    }
}
