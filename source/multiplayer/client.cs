using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeloewenCraft;

public class Client
{
    public TcpClient client;
    public NetworkStream stream;
    public bool isConnected;

    public async void Connect(string address, int port)
    {
        client = new TcpClient();
        Log.Write("Connecting to server...", "Info");

        try
        {
            await client.ConnectAsync(address, port);
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Log.Write("Failed to connect to the server!", "Info");
            Log.Write($"{ex.Message}", "Info");
            return;
        }

        if (stream != null)
        {
            Log.Write("The connection was successfully established.", "Info");
            isConnected = true;
            Game.isClient = true;
            Game.client.SendData("InitialLoad");
        }
        else
        {
            Log.Write("Failed to connect to the server!", "Info");
            return;
        }

        ReceiveData();
    }

    public async Task SendData(string data)
    {
        if (stream != null)
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
                Log.Write($"Could not send data to server: {ex.Message}", "Info");
            }
        }
    }

    public async Task ReceiveData()
    {
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
                Log.Write($"Could not receive data from server: {ex.Message}", "Info");
                break;
            }
        }
    }
}
