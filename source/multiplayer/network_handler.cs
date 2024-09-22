using SeeloewenCraft.entity;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.ViewManagement.Core;

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
                case "PressedChangeEvent":
                    HandlePressedChange(client, args);
                    break;
                case "SyncPos":
                    HandleSyncPos(client, args);
                    break;
                case "AddToInv":
                    HandleAddToInv(client, args);
                    break;
                case "RemoveFromInv":
                    HandleRemoveFromInv(client, args);
                    break;
                case "DamageEntity":
                    //HandleDamageEntity(client, args);
                    break;
                case "HealEntity":
                    //HandleHealEntity(client, args);
                    break;
            }
        }


        //-- Events --//

        public async static void HandleAddToInv(AdvancedTcpClient client, string[] args)
        {
            //Split the received data into the attributes
            int blockX = Convert.ToInt32(args[1]);
            int blockY = Convert.ToInt32(args[2]);
            int blockChunk = Convert.ToInt32(args[3]);
            string itemId = args[4];
            int amount = Convert.ToInt32(args[5]);
            int slotX = Convert.ToInt32(args[6]);
            int slotY = Convert.ToInt32(args[7]);
            string tag = args[8];

            //Get the block which has the inventory and try to add the item to the slot
            Block invBlock = Game.world.GetChunk(blockChunk).GetBlock(blockX, blockY);
            if (invBlock != null && invBlock.blockInventory != null)
            {
                invBlock.blockInventory.GetSlot(slotX, slotY).AddMultiplayer(itemId, tag, amount);
                invBlock.blockInventory.GetSlot(slotX, slotY).Unselect();
            }

            //If the server receives the packet, send it to all other clients to make sure the inventory gets updated on all of them
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients(client.id, $"AddToInv;{args[1]};{args[2]};{args[3]};{args[4]};{args[5]};{args[6]};{args[7]}");
            }
        }

        public async static void HandleRemoveFromInv(AdvancedTcpClient client, string[] args)
        {
            //Split the received data into the attributes
            int blockX = Convert.ToInt32(args[1]);
            int blockY = Convert.ToInt32(args[2]);
            int blockChunk = Convert.ToInt32(args[3]);
            int amount = Convert.ToInt32(args[4]);
            int slotX = Convert.ToInt32(args[5]);
            int slotY = Convert.ToInt32(args[6]);

            //Get the block which has the inventory and try to add the item to the slot
            Block invBlock = Game.world.GetChunk(blockChunk).GetBlock(blockX, blockY);
            if (invBlock != null && invBlock.blockInventory != null)
            {
                invBlock.blockInventory.GetSlot(slotX, slotY).RemoveMultiplayer(amount);
                invBlock.blockInventory.GetSlot(slotX, slotY).Unselect();
            }

            //If the server receives the packet, send it to all other clients to make sure the inventory gets updated on all of them
            if (Game.isServer)
            {
                Game.server.SendDataExceptClients(client.id, $"RemoveFromInv;{args[1]};{args[2]};{args[3]};{args[4]};{args[5]};{args[6]}");
            }
        }


        public async static void HandleSyncPos(AdvancedTcpClient client, string[] args)
        {
            //Synchronize the position of all entities to ensure their position is correct
            try
            {
                /*
                foreach (Entity entity in Game.world.entities)
                {
                    if (entity.id == Convert.ToInt32(args[1]) && entity is MovingEntity movEntity)
                    {
                        movEntity.HandleSyncData(args);
                    }
                }*/

                //TODO: Rework for new entity system
            }
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }

        public async static void HandlePressedChange(AdvancedTcpClient client, string[] args)
        {

            try
            {
                Game.world.entityManager.ReceivePressedChange(args);

                /*
                foreach (Entity entity in Game.world.entities)
                {
                    if (entity.id == Convert.ToInt32(args[1]) && entity is MovingEntity movEntity)
                    {
                        movEntity.pressedLeft = Convert.ToBoolean(args[2]);
                        movEntity.pressedRight = Convert.ToBoolean(args[3]);
                        movEntity.pressedUp = Convert.ToBoolean(args[4]);
                        movEntity.pressedSneak = Convert.ToBoolean(args[5]);
                        movEntity.pressedSprint = Convert.ToBoolean(args[6]);
                    }
                }
                //If the server receives the packet, send it to all other clients to make sure the player moves on all of them
                if (Game.isServer)
                {
                    Game.server.SendDataExceptClients(client.id, $"MovePlayer;{args[1]};{args[2]};{args[3]};{args[4]};{args[5]};{args[6]}");
                }*/

                //TODO: Rework for new entity system/
            }
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }

        public async static void HandleCreateEntity(AdvancedTcpClient client, string[] args)
        {
            if (args.Length != 2) return;

            try
            {
                //Add the entity
                Game.world.AddMultiplayerEntity(Entity.LoadFromJson(JsonUtil.ReadString(args[1])));

                //If the server receives the packet, send it to all clients except the one it came from to ensure the entity gets created on all clients
                if (Game.isServer)
                {
                    Game.server.SendDataExceptClients(client.id, $"CreateEntity;{args[1]}");
                }
            }
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }

        public async static void HandleRemoveEntity(AdvancedTcpClient client, string[] args)
        {
            if (args.Length != 2) return;

            try
            {
                //Remove the entity
                Game.world.RemoveMultiplayerEntity(Convert.ToInt32(args[1]));

                //If the server receives the packet, send it to all clients except the one it came from to ensure the entity gets removed on all clients
                if (Game.isServer)
                {
                    Game.server.SendDataExceptClients(client.id, $"RemoveEntity;{args[1]}");
                }
            }
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }

        public async static void HandleInitialLoad(AdvancedTcpClient client, string[] args) //Only executed by the server
        {
            try
            {
                //Called when a player connects
                if (Game.isServer)
                {
                    //Go through each chunk and each block and send it to the client that requested it

                    foreach (Chunk chunk in Game.world.totalChunkList)
                    {
                        foreach (Block block in chunk.blockList.blocks)
                        {
                            await Game.server.SendDataSingleClient(client.id, $"SetBlock;{block.id};{chunk.index};{block.xPos};{block.yPos}");
                        }
                    }


                    //Send all moving entities to the connecting client

                    /*
                    foreach (Entity entity in Game.world.entities)
                    {
                        if (entity is MovingEntity movEntity)
                        {
                            using (JsonWriter writer = JsonWriter.Create())
                            {
                                movEntity.SaveToJson(writer);
                                await Game.server.SendDataSingleClient(client.id, $"CreateEntity;{writer.ToString()}");
                            }
                        }
                    }*/

                    //TODO: Rework for new multiplayer system

                    //Send the player (which is for some reason not in the entity list) to the connecting client
                    /*using (JsonWriter writer = JsonWriter.Create())
                    {
                        Game.world.player.SaveToJson(writer);
                        await Game.server.SendDataSingleClient(client.id, $"CreateEntity;{writer.ToString()}");
                    }*/
                    Game.world.entityManager.SendInitLoadData(client.id);
                }
            }
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }
        public async static void HandleSetBlock(AdvancedTcpClient client, string[] args) //Handled by both server and clients
        {
            if (args.Length != 5) return;

            try
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
                    Game.server.SendDataExceptClients(client.id, $"SetBlock;{block.id};{cIndex};{block.xPos};{block.yPos}");
                }
            }
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }

        public async static void HandleCreateChunk(AdvancedTcpClient client, string[] args) //Handled by both server and clients
        {
            if (args.Length != 2) return;

            try
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
            catch (Exception e)
            {
                Game.ShowException(e);
            }
        }
    }
}
