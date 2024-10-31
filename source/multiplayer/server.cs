using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static SeeloewenCraft.NetworkHandler;
using System.Linq;

namespace SeeloewenCraft;

public class Server
{
    public TcpListener server;
    public List<IdTcpClient> clients = new List<IdTcpClient>();
    private static int nextClientId = 0;

    public async void Start(int port)
    {
        Log.Write($"Starting server on port {port}...", LogType.NETWORK, LogLevel.INFO);

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
        }
        catch (Exception ex)
        {
            Log.Write($"Failed to start the server: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
            return;
        }

        Log.Write("The server was successfully started!", LogType.NETWORK, LogLevel.INFO);

        while (true)
        {
            //Get the a server from an incoming connection
            TcpClient tcpClient = await server.AcceptTcpClientAsync();

            //Convert it to an advanced client and assign an id
            IdTcpClient client = new IdTcpClient();
            client.Client = tcpClient.Client;
            client.id = nextClientId;
            nextClientId++;

            //Get the stream and check if the stream is not null, which means that the connection was successful
            if (client.GetStream() != null)
            {
                clients.Add(client);
                Log.Write($"The connection with client #{client.id} was successfully established", LogType.NETWORK, LogLevel.INFO);
            }

            //Start receiving data from the client
            ReceiveData(client);
        }
    }

    public async Task ReceiveData(IdTcpClient client)
    {
        while (true && clients.Contains(client))
        {
            try
            {
                //Get the length of the following packet
                byte[] lengthPacket = await ReceivePacket(sizeof(int), client.GetStream());
                if (lengthPacket.Length < 4) return; //The packet is invalid if the length is below 4 bytes
                int dataLength = BitConverter.ToInt32(lengthPacket);

                //Read data into the buffer and copy data from buffer to receivedData
                byte[] receivedData = await ReceivePacket(dataLength, client.GetStream());
                if (receivedData.Length < dataLength) return; //If the data wasn't read correctly and isn't long enough, the packet is invalid

                //Get the type bytes and convert it to type
                int typeLength = BitConverter.ToInt32(receivedData, 0);
                if (receivedData.Length < 4) break;
                string typeString = Encoding.ASCII.GetString(receivedData, 4, typeLength);
                if (receivedData.Length < 4 + typeLength) break;
                Enum.TryParse(typeString, out MultiplayerPacketType type);

                //Go through the remaining bytes and read the string length first, then the string based on the length
                List<string> contentList = new List<string>();
                int index = 4 + typeLength;
                while (index < receivedData.Length)
                {
                    //Get string length
                    int stringLength = BitConverter.ToInt32(receivedData, index);
                    index += 4;

                    //Get the bytes for the message starting from the index with determined length
                    byte[] stringBytes = new byte[stringLength];
                    Array.Copy(receivedData, index, stringBytes, 0, stringLength);
                    index += stringLength;

                    //Convert bytes to string
                    string str = Encoding.UTF8.GetString(stringBytes);
                    contentList.Add(str);
                }

                string[] content = contentList.ToArray();

                //Create the packet from previously determined information
                NetworkPacket packet = CreatePacket(type, content);

                //Handle the data
                await HandleData(client, packet);

                //Log.Write($"Received data from client #{client.id}: {$"{type} ({content.ToString()})"}.", "Info");
            }
            catch (Exception ex)
            {
                Log.Write($"Could not receive data from client #{client.id}: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
                break;
            }
        }
    }

    public async Task SendData(NetworkPacket packet, IdTcpClient client)
    {
        //Get the bytes of the data and the length of the data
        byte[] dataBytes = packet.GetBytes();
        byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

        //First send the length of the data as a packet, then send the actual data packet
        await client.GetStream().WriteAsync(lengthBytes, 0, lengthBytes.Length);
        await client.GetStream().WriteAsync(dataBytes, 0, dataBytes.Length);

        //Log.Write($"Sent data to client #{client.id}: {BitConverter.ToString(dataBytes).Replace("-", " ")}.", "Info");
    }

    public async Task SendDataToAll(NetworkPacket packet)
    {
        //Send the data to all connected clients
        for (int i = 0; i < clients.Count; i++)
        {
            try
            {
                if (clients[i].GetStream() != null)
                {
                    SendData(packet, clients[i]);
                }
            }
            catch (Exception ex)
            {
                Log.Write($"Could not send data to client #{clients[i].id}: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);

                if (ex is InvalidOperationException)
                {
                    Disconnect(clients[i], ex.Message);
                }
            }
        }
    }


    public async Task SendDataSingleClient(NetworkPacket packet, int clientId)
    {
        //Send the data to a specific client
        IdTcpClient client = GetClient(clientId);

        if (client != null)
        {
            try
            {
                if (client.GetStream() != null)
                {
                    SendData(packet, client);
                }
            }
            catch (Exception ex)
            {
                Log.Write($"Could not send data to single client #{client.id}: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);

                if (ex is SocketException)
                {
                    Disconnect(client, ex.Message);
                }
            }
        }
    }

    public async Task SendDataExceptClients(NetworkPacket packet, params int[] ignoredIds)
    {
        //Send the data to all connected clients except the clients mentioned
        foreach (IdTcpClient client in clients)
        {
            if (!ignoredIds.Contains(client.id))
            {
                try
                {
                    if (client.GetStream() != null)
                    {
                        SendData(packet, client);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write($"Could not send data to client #{client.id} using except: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);

                    if (ex is SocketException)
                    {
                        Disconnect(client, ex.Message);
                    }
                }
            }
        }
    }

    public IdTcpClient GetClient(int id)
    {
        //Go through all clients and return the correct one
        foreach (IdTcpClient client in clients)
        {
            if (client.id == id)
            {
                return client;
            }
        }

        return null;
    }

    public void Disconnect(IdTcpClient client, string message)
    {
        clients.Remove(client);
        Game.world.entityManager.Remove(client.id);
        Log.Write($"Client #{client.id} ({client.userName}) disconnected: {message}", LogType.NETWORK, LogLevel.WARNING);
    }
}
