using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SeeloewenCraft.NetworkHandler;

namespace SeeloewenCraft
{
    public class IdTcpClient : TcpClient
    {
        public int id = 0;
        public string nickname = "NONE";
        public bool isConnected = true;
    }

    public class Client
    {
        private IdTcpClient client;
        private NetworkStream stream;
        public bool isConnected = false;
        public Exception connectionException;

        public async Task Connect(string address, int port)
        {
            client = new IdTcpClient();
            Log.Write("Connecting to server...", LogType.NETWORK, LogLevel.INFO);

            try
            {
                //Try to connect to the server
                await client.ConnectAsync(address, port);
                stream = client.GetStream();

                if (stream != null)
                {
                    connectionException = new Exception("Could not get NetworkStream");
                    isConnected = true;
                }
            }
            catch (Exception ex)
            {
                connectionException = ex;
                Log.Write($"Failed to connect to the server: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
                return;
            }
        }

        public async void Initialize()
        {
            if (stream != null)
            {
                //Check if the stream exists to confirm that the connection was successful
                Log.Write("The connection with the server was successfully established", LogType.NETWORK, LogLevel.INFO);

                //Send a request to the server to do an initial load, which gets all blocks in all chunks and their content
                await Game.client.SendData(CreatePacket(MultiplayerPacketType.PLAYER_INFORMATION, Game.playerId.ToString(), Settings.nickname, ""));
                await Game.client.SendData(CreatePacket(MultiplayerPacketType.INITIAL_LOAD, ""));

                using (JsonWriter writer = JsonWriter.Create())
                {
                    Game.world.player.SaveToJson(writer);
                    await SendData(CreatePacket(MultiplayerPacketType.CREATE_ENTITY, writer.ToString()));
                }
                Game.world.wndGame.Show();
            }
            else
            {
                Log.Write("Failed to connect to the server!", LogType.NETWORK, LogLevel.ERROR);
                return;
            }

            //Start receiving data from the server
            ReceiveData();
        }

        public async Task SendData(NetworkPacket packet)
        {
            //Send the data to the server
            if (stream != null)
            {
                try
                {
                    //Get the bytes of the data and the length of the data
                    byte[] dataBytes = packet.GetBytes();
                    byte[] lengthBytes = BitConverter.GetBytes(dataBytes.Length);

                    //First send the length of the data, then send the actual data
                    await stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
                    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);

                    //Log.Write($"Sent data to server: {BitConverter.ToString(dataBytes).Replace("-", " ")}.", "Info");
                }
                catch (Exception ex)
                {
                    Log.Write($"Could not send data to server: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
                }
            }
        }

        public async Task ReceiveData()
        {
            //Receive data from the server
            Exception e = null;

            while (true)
            {
                try
                {
                    //Get the length of the following packet
                    byte[] lengthPacket = await ReceivePacket(sizeof(int), stream);

                    if (lengthPacket.Length >= 4) //The packet is invalid if the length is below 4 bytes
                    {
                        int dataLength = BitConverter.ToInt32(lengthPacket);

                        //Read data into the buffer and copy data from buffer to receivedData
                        byte[] receivedData = await ReceivePacket(dataLength, stream);

                        if (receivedData.Length >= dataLength && receivedData.Length >= 4) //If the data wasn't read correctly and isn't long enough, the packet is invalid
                        {
                            //Get the type bytes and convert it to type
                            int typeLength = BitConverter.ToInt32(receivedData, 0);

                            if (receivedData.Length >= 4 + typeLength)
                            {
                                string typeString = Encoding.ASCII.GetString(receivedData, 4, typeLength);
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

                                //Log.Write($"Received data from server: {BitConverter.ToString(receivedData).Replace("-", " ")}.", "Info");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    e = ex;
                    Log.Write($"Could not receive data from server: {ex.Message}\n{ex.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
                    break;
                }
            }

            MessageBox.Show($"Lost connection to the server! {e.Message}\n{e.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Game.world.wndGame.Close();
            Game.world.wndMenu.Show();
        }

        public void SendPlayerInformation()
        {
            //Get the inventory and send it to the server to save it
            JsonWriter writer = JsonWriter.Create();
            Game.world.player.inventory.SaveToJson(writer);
            NetworkHandler.SendData(MultiplayerPacketType.PLAYER_INFORMATION, writer.ToString());
        }
    }
}