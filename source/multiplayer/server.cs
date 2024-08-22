using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Security.Cryptography.Pkcs;

namespace SeeloewenCraft;

public class Server
{
    public TcpListener server;
    public TcpClient client;
    public NetworkStream stream;
    public bool isConnected;

    public async void Start(int port)
    {
        Log.Write($"Starting server on port {port}...", "Info");

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Game.isServer = true;
            
        }
        catch (Exception ex)
        {
            Log.Write("Failed to start the server!", "Info");
            Log.Write($"{ex.Message}", "Info");
            return;
        }

        Log.Write("The server was successfully started.", "Info");


        client = await server.AcceptTcpClientAsync();
        stream = client.GetStream();
        if (stream != null)
        {
            isConnected = true;
            Log.Write("The connection with a client was successfully established.", "Info");
        }

        ReceiveData();
    }

    public async Task ReceiveData()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                byte[] lengthBuffer = new byte[sizeof(int)];
                int lengthBytesRead = await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                if (lengthBytesRead == 0) break;

                byte[] messageBuffer = new byte[messageLength];
                int messageBytesRead = await stream.ReadAsync(messageBuffer, 0, messageBuffer.Length);
                string receivedMessage = Encoding.ASCII.GetString(messageBuffer, 0, messageBytesRead);

                await NetworkHandler.HandleData(receivedMessage);
                Log.Write($"Received data: {receivedMessage}", "Info");
            }
            catch (Exception ex)
            {
                Log.Write($"Could not receive data from clients: {ex.Message}", "Info");
                break;
            }
        }
    }

    public async Task SendData(string data)
    {
        if(stream != null)
        {
            try
            {
                byte[] messageBytes = Encoding.ASCII.GetBytes(data);

                byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);
                await stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);

                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                Log.Write($"Send data to server: {data}", "Info");
            }
            catch (Exception ex)
            {
                Log.Write($"Could not send data to clients: {ex.Message}", "Info");
            }
        }
    }
}
