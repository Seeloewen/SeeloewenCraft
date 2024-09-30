using System;
using System.Windows;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SeeloewenCraft;

public class Server
{
    public TcpListener server;
    public List<AdvancedTcpClient> clients = new List<AdvancedTcpClient>();
    private static int nextClientId = 0;

    public async void Start(int port)
    {
        Log.Write($"Starting server on port {port}...", "Info");

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
        }
        catch (Exception ex)
        {
            Log.Write($"Failed to start the server: {ex.Message}", "Error");
            return;
        }

        Log.Write("The server was successfully started!", "Info");

        while (true)
        {
            //Get the a server from an incoming connection
            TcpClient tcpClient = await server.AcceptTcpClientAsync();

            //Convert it to an advanced client and assign an id
            AdvancedTcpClient client = new AdvancedTcpClient();
            client.Client = tcpClient.Client;
            client.id = nextClientId;
            nextClientId++;

            //Get the stream and check if the stream is not null, which means that the connection was successful
            if (client.GetStream() != null)
            {
                clients.Add(client);
                Log.Write("The connection with client was successfully established.", "Info");
            }

            //Start receiving data from the client
            ReceiveData(client);
        }
    }

    public async Task ReceiveData(AdvancedTcpClient client)
    {
        while (true)
        {
            try
            {
                //First get the length of the data so the server knows how many bytes the following actual data is long
                byte[] lengthBuffer = new byte[sizeof(int)];
                int lengthBytesRead = await client.GetStream().ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                int dataLength = BitConverter.ToInt32(lengthBuffer, 0);

                //Get the data bytes based on the previously received length
                byte[] dataBuffer = new byte[dataLength];
                int dataBytesRead = await client.GetStream().ReadAsync(dataBuffer, 0, dataBuffer.Length);
                string receivedData = Encoding.ASCII.GetString(dataBuffer, 0, dataBytesRead);

                //Log.Write($"Received data from client #{client.id}: {receivedData}.", "Info");

                //Handle the data
                await NetworkHandler.HandleData(client, receivedData);
            }
            catch (Exception ex)
            {
                Log.Write($"Could not receive data from client #{client.id}: {ex.Message}", "Error");
                break;
            }
        }
    }

    public async Task SendData(MultiplayerPacketType type, string data)
    {
        data = $"{type};{data}";

        //Send the data to all connected clients
        foreach (AdvancedTcpClient client in clients)
        {
            if (client.GetStream() != null)
            {            
                try
                {
                    //Convert the data and the length of the data to bytes
                    byte[] dataBytes = Encoding.ASCII.GetBytes(data);
                    byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

                    //First send the length of the data, then send the actual data
                    await client.GetStream().WriteAsync(lengthBytes, 0, lengthBytes.Length);
                    await client.GetStream().WriteAsync(dataBytes, 0, dataBytes.Length);

                    //Log.Write($"Sent data to client #{client.id}: {data}.", "Info");
                }
                catch (Exception ex)
                {
                    Log.Write($"Could not send data to client #{client.id}: {ex.Message}", "Error");
                }
            }
        }
    }

    public async Task SendDataSingleClient(int clientId, MultiplayerPacketType type, string data)
    {
        data = $"{type};{data}";

        //Send the data to a specific client
        AdvancedTcpClient client = GetClient(clientId);

        if (client != null && client.GetStream() != null)
        {
            try
            {
                //Convert the data and the length of the data to bytes
                byte[] dataBytes = Encoding.ASCII.GetBytes(data);
                byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

                //First send the length of the data, then send the actual data
                await client.GetStream().WriteAsync(lengthBytes, 0, lengthBytes.Length);
                await client.GetStream().WriteAsync(dataBytes, 0, dataBytes.Length);

                //Log.Write($"Sent data to single client #{client.id}: {data}.", "Info");
            }
            catch (Exception ex)
            {
                Log.Write($"Could not send data to client #{client.id}: {ex.Message}", "Error");
            }
        }
    }

    public async Task SendDataExceptClients(int clientId, MultiplayerPacketType type, string data)
    {
        data = $"{type};{data}";
        //Send the data to all connected clients except the clients mentioned
        foreach (AdvancedTcpClient client in clients)
        {
            if (clientId != client.id && client.GetStream() != null)
            {
                try
                {
                    //Convert the data and the length of the data to bytes
                    byte[] dataBytes = Encoding.ASCII.GetBytes(data);
                    byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

                    //First send the length of the data, then send the actual data
                    await client.GetStream().WriteAsync(lengthBytes, 0, lengthBytes.Length);
                    await client.GetStream().WriteAsync(dataBytes, 0, dataBytes.Length);

                    //Log.Write($"Sent data to client #{client.id} (except {clientId}): {data}.", "Info");
                }
                catch (Exception ex)
                {
                    Log.Write($"Could not send data to client #{client.id}: {ex.Message}", "Error");
                }
            }
        }
    }

    public AdvancedTcpClient GetClient(int id)
    {
        //Go through all clients and return the correct one
        foreach (AdvancedTcpClient client in clients)
        {
            if (client.id == id)
            {
                return client;
            }
        }

        return null;
    }
}
