using System;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public static class NetworkHandler
    {
        public static async void StartServer()
        {
            Game.server = new Server();
            Game.server.Start(5000);
        }

        public static async void SendData(string data)
        {
            //Send the data either as client or server. If it's neither, don't send anything
            if (Game.isServer)
            {
                if (Game.server.clients.Count > 0)
                {
                    Game.server.SendData(data); //Sends the data to all clients
                }
            }
            else if (Game.isClient)
            {
                Game.client.SendData(data); //Sends the data only to the server
            }
        }

        public static async Task HandleData(AdvancedTcpClient client, string data)
        {
            string[] args = data.Split(';');

            switch (args[0])
            {
                //Handle the action based on the the first arg
                case "CreateChunk":
                    HandleCreateChunk(client, args);
                    break;
                case "InitialLoad":
                    HandleInitialLoad(client, args);
                    break;
                case "SetBlock":
                    HandleSetBlock(client, args);
                    break;
            }
        }

        public async static void HandleInitialLoad(AdvancedTcpClient client, string[] args) //Only executed by the server
        {
            //Go through each chunk and each block and send it to the client that requested it
            if (Game.isServer)
            {
                foreach (Chunk chunk in Game.world.loadedChunkList)
                {
                    foreach (Block block in chunk.blockList.blocks)
                    {
                        await Game.server.SendDataSingleClient(client.id, $"SetBlock;{block.id};{chunk.index};{block.xPos};{block.yPos}");
                    }
                }
            }
        }

        public async static void HandleSetBlock(AdvancedTcpClient client, string[] args) //Handled by both server and clients
        {
            Block block = BlockRegister.GenerateBlock(args[1]);
            int cIndex = Convert.ToInt32(args[2]);
            int xPos = Convert.ToInt32(args[3]);
            int yPos = Convert.ToInt32(args[4]);

            //Set the block using the multiplayer method (more info in method)
            Game.world.SetBlockMultiplayer(block, cIndex, xPos, yPos);

            //If the server receives the call, redirect it to the other clients
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients([client.id], $"SetBlock;{block.id};{cIndex};{block.xPos};{block.yPos}");
            }
        }

        public async static void HandleCreateChunk(AdvancedTcpClient client, string[] args) //Handled by both server and clients
        {
            //Create the new chunk on current instance
            int index = Convert.ToInt32(args[1]);
            Game.world.CreateChunk(index);

            //If the server receives the message, it should additionally send the chunk back to all clients
            if (Game.isServer)
            {
                foreach (Block block in Game.world.GetChunk(index).blockList.blocks)
                {
                    Game.server.SendData($"SetBlock;{block.id};{index};{block.xPos};{block.yPos}");
                }
            }
        }
    }
}
