using SeeloewenCraft.entity;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
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
                case "CreateEntity":
                    HandleCreateEntity(client, args);
                    break;
                case "RemoveEntity":
                    HandleRemoveEntity(client, args);
                    break;
                case "MovePlayer":
                    HandleMovePlayer(client, args);
                    break;
                case "SyncPos":
                    HandleSyncPos(client, args);
                    break;
            }
        }


        public async static void HandleSyncPos(AdvancedTcpClient client, string[] args)
        {
            //Synchronise the position of all entities to ensure their position is correct
            foreach (Entity entity in Game.world.entities)
            {
                if (entity.id == Convert.ToInt32(args[1]) && entity is MovingEntity movEntity)
                {
                    movEntity.HandleSyncData(args);
                }
            }

            //If the server gets the packet, send it all other clients to ensure that the position on them is correct too
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients([client.id], $"SyncPos;{args[1]};{args[2]};{args[3]};{args[4]};{args[5]}");
            }
        }

        public async static void HandleMovePlayer(AdvancedTcpClient client, string[] args)
        {
            foreach (Entity entity in Game.world.entities)
            {
                if (entity.id == Convert.ToInt32(args[1]) && entity is MovingEntity movEntity)
                {
                    movEntity.pressedLeft = Convert.ToBoolean(args[2]);
                    movEntity.pressedRight = Convert.ToBoolean(args[3]);
                    movEntity.pressedUp = Convert.ToBoolean(args[4]);
                    movEntity.pressedSneak = Convert.ToBoolean(args[5]);
                    movEntity.pressedSprint = Convert.ToBoolean(args[6]);
                    movEntity.pressedThrow = Convert.ToBoolean(args[7]);
                }
            }

            //If the server receives the packet, send it to all other clients to make sure the player moves on all of them
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients([client.id], $"MovePlayer;{args[1]};{args[2]};{args[3]};{args[4]};{args[5]};{args[6]};{args[7]}");
            }
        }

        public async static void HandleCreateEntity(AdvancedTcpClient client, string[] args)
        {
            //Add the entity
            Game.world.AddMultiplayerEntity(Entity.LoadFromJson(JsonUtil.ReadString(args[1])));

            //If the server receives the packet, send it to all clients except the one it came from to ensure the entity gets created on all clients
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients([client.id], $"CreateEntity;{args[1]}");
            }
        }

        public async static void HandleRemoveEntity(AdvancedTcpClient client, string[] args)
        {
            //Remove the entity
            Game.world.RemoveMultiplayerEntity(Convert.ToInt32(args[1]));

            //If the server receives the packet, send it to all clients except the one it came from to ensure the entity gets removed on all clients
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients([client.id], $"RemoveEntity;{args[1]}");
            }
        }

        public async static void HandleInitialLoad(AdvancedTcpClient client, string[] args) //Only executed by the server
        {
            //Called when a player connects

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

            //Send all moving entities to the connecting client
            foreach (MovingEntity entity in Game.world.entities)
            {
                if (entity.type != "Player") //Don't sync other players, those get synced seperately
                {
                    using (JsonWriter writer = JsonWriter.Create())
                    {
                        entity.SaveToJson(writer);
                        await Game.server.SendDataSingleClient(client.id, $"CreateEntity;{writer.ToString()}");
                    }
                }
            }

            //Send the player (which is for some reason not in the entity list) to the connecting client
            using (JsonWriter writer = JsonWriter.Create())
            {
                Game.world.player.SaveToJson(writer);
                await Game.server.SendDataSingleClient(client.id, $"CreateEntity;{writer.ToString()}");
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
