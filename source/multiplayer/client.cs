using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft;

public class AdvancedTcpClient : TcpClient
{
    public int id;
}

public class Client
{
    public AdvancedTcpClient client;
    public NetworkStream stream;
    public bool isConnected;

    public async void Connect(string address, int port)
    {
        client = new AdvancedTcpClient();
        Log.Write("Connecting to server...", "Info");

        try
        {
            //Try to connect to the server
            await client.ConnectAsync(address, port);
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Log.Write($"Failed to connect to the server: {ex.Message}", "Error");
            return;
        }

        if (stream != null)
        {
            //Check if the stream exists to confirm that the connection was successful
            Log.Write("The connection with the server was successfully established.", "Info");
            isConnected = true;
            Game.isClient = true;

            //Send a request to the server to do an initial load, which gets all blocks in all chunks and their content
            await Game.client.SendData("InitialLoad");
            Game.world.wndGame.Show();
        }
        else
        {
            Log.Write("Failed to connect to the server!", "Error");
            return;
        }

        //Start receiving data from the server
        ReceiveData();
    }

    public async Task SendData(string data)
    {
        //Send the data to the server
        if (stream != null)
        {
            try
            {
                //Get the bytes of the data and the length of the data
                byte[] dataBytes = Encoding.ASCII.GetBytes(data);
                byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

                //First send the length of the data, then send the actual data
                await stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
                await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
            }
            catch (Exception ex)
            {
                Log.Write($"Could not send data to server: {ex.Message}", "Error");
            }
        }
    }

    public async Task ReceiveData()
    {
        //Receive data from the server
        while (true)
        {
            try
            {
                //First get the length of the following data
                byte[] lengthBuffer = new byte[sizeof(int)];
                int lengthBytesRead = await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                int dataLength = BitConverter.ToInt32(lengthBuffer, 0);
                if (lengthBytesRead == 0) break;

                //Get the actual data based on the previously received length
                byte[] dataBuffer = new byte[dataLength];
                int dataBytesRead = await stream.ReadAsync(dataBuffer, 0, dataBuffer.Length);
                string receivedData = Encoding.ASCII.GetString(dataBuffer, 0, dataBytesRead);

                //Handle the data
                await NetworkHandler.HandleData(client, receivedData);
            }
            catch (Exception ex)
            {
                Log.Write($"Could not receive data from server: {ex.Message}", "Error");
                break;
            }
        }
    }
}
