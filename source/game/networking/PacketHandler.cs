using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.notifications;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.IO;

namespace SeeloewenCraft.game.networking
{
    public static partial class NetworkHandler
    {
        public async static void HandleAddToInv(IdTcpClient client, NetworkPacket packet)
        {
            //Split the received data into the attributes
            int blockX = int.Parse(packet.content[0]);
            int blockY = int.Parse(packet.content[1]);
            int blockChunk = int.Parse(packet.content[2]);
            string itemId = packet.content[3];
            int amount = int.Parse(packet.content[4]);
            int slotX = int.Parse(packet.content[5]);
            int slotY = int.Parse(packet.content[6]);
            string tag = packet.content[7];

            //Get the block which has the inventory and try to add the item to the slot
            Block invBlock = World.Get().GetChunk(blockChunk).GetBlock(blockX, blockY);
            if (invBlock != null && invBlock.GetInventory() != null)
            {
                //invBlock.inventory.GetSlot(slotX, slotY).Add_Multiplayer(itemId, tag, amount);
                invBlock.GetInventory().GetSlot(slotX, slotY).Unselect();
            }

            //If the server receives the packet, send it to all other clients to make sure the inventory gets updated on all of them
            if (IsServer())
            {
                server.SendDataExceptClients(packet, client.id);
            }
        }

        public async static void HandleRemoveFromInv(IdTcpClient client, NetworkPacket packet)
        {
            //Split the received data into the attributes
            int blockX = int.Parse(packet.content[0]);
            int blockY = int.Parse(packet.content[1]);
            int blockChunk = int.Parse(packet.content[2]);
            int amount = int.Parse(packet.content[3]);
            int slotX = int.Parse(packet.content[4]);
            int slotY = int.Parse(packet.content[5]);

            //Get the block which has the inventory and try to add the item to the slot
            Block invBlock = World.Get().GetChunk(blockChunk).GetBlock(blockX, blockY);
            if (invBlock != null && invBlock.GetInventory() != null)
            {
                //invBlock.inventory.GetSlot(slotX, slotY).Remove_Multiplayer(amount);
                invBlock.GetInventory().GetSlot(slotX, slotY).Unselect();
            }

            //If the server receives the packet, send it to all other clients to make sure the inventory gets updated on all of them
            if (IsServer())
            {
                server.SendDataExceptClients(packet, client.id);
            }
        }


        public async static void HandleSyncPos(IdTcpClient client, NetworkPacket packet)
        {
            //Synchronize the position of all entities to ensure their position is correct            
            foreach (Entity entity in World.Get().entityManager.entities)
            {
                if (entity.id == Convert.ToInt32(packet.content[0]) && entity is MovingEntity movEntity && entity != Player.Get())
                {
                    movEntity.HandleSyncData(packet.content);
                }
            }

            try
            {
                //world.entityManager.Sync(SyncPosEvent.Create(packet.content[0]));
            }
            catch (Exception e)
            {
                Log.Write($"Error while trying to handle PressedChange event: {e.Message}\n{e.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
            }
        }

        public async static void HandlePressedChange(IdTcpClient client, NetworkPacket packet)
        {
            try
            {
                foreach (Entity entity in World.Get().entityManager.entities)
                {
                    if (entity.id == Convert.ToInt32(packet.content[0]) && entity is MovingEntity movEntity && entity != Player.Get())
                    {
                        movEntity.pressedLeft = Convert.ToBoolean(packet.content[1]);
                        movEntity.pressedRight = Convert.ToBoolean(packet.content[2]);
                        movEntity.pressedUp = Convert.ToBoolean(packet.content[3]);
                        movEntity.pressedSneak = Convert.ToBoolean(packet.content[4]);
                        movEntity.pressedSprint = Convert.ToBoolean(packet.content[5]);
                    }

                    if (entity is Player player)
                    {
                        player.UpdateHeadPosition();

                    }
                }

                //If the server receives the packet, send it to all other clients to make sure the player moves on all of them
                if (IsServer())
                {
                    server.SendDataExceptClients(packet, client.id);
                }

                //world.entityManager.ReceivePressedChange(packet.content);
            }
            catch (Exception e)
            {
                Log.Write($"Error while trying to handle PressedChange event: {e.Message}\n{e.StackTrace}", LogType.NETWORK, LogLevel.ERROR);
            }


        }

        public async static void HandleCreateEntity(IdTcpClient client, NetworkPacket packet)
        {
            //Add the entity
            Entity newEntity = Entity.FromJson(JObject.Parse(packet.content[0]));
            //World.Get().AddEntity_Multiplayer(newEntity);

            //If the server receives the packet, send it to all clients except the one it came from to ensure the entity gets created on all clients
            if (IsServer())
            {
                server.SendDataExceptClients(packet, client.id);
            }
        }

        public async static void HandleRemoveEntity(IdTcpClient client, NetworkPacket packet)
        {
            //Remove the entity
            World.Get().RemoveEntity(int.Parse(packet.content[0]));

            //If the server receives the packet, send it to all clients except the one it came from to ensure the entity gets removed on all clients
            if (IsServer())
            {
                server.SendDataExceptClients(packet, client.id);
            }
        }

        public async static void HandleInitialLoad(IdTcpClient client, NetworkPacket packet) //Only executed by the server
        {
            //Called when a player connects
            if (IsServer())
            {
                //Go through each chunk and each block and send it to the client that requested it
                foreach (Chunk chunk in World.Get().totalChunkList)
                {
                    foreach (Block block in chunk.blockList.blocks)
                    {
                        await server.SendDataSingleClient(CreatePacket(MultiplayerPacketType.SET_BLOCK, block.id, chunk.index.ToString(), block.posX.ToString(), block.posY.ToString()), client.id);
                    }
                }

                //Send all moving entities to the connecting client
                World.Get().entityManager.SendInitLoadData(client.id);

                if (File.Exists(Path.Combine(World.Get().multiplayerDirectory, $"inventory_{client.id}.json")))
                {
                    await server.SendDataSingleClient(CreatePacket(MultiplayerPacketType.PLAYER_INFORMATION, File.ReadAllText(Path.Combine(World.Get().multiplayerDirectory, $"inventory_{client.id}.json"))), client.id);
                }
            }
        }
        public async static void HandleSetBlock(IdTcpClient client, NetworkPacket packet) //Handled by both server and clients
        {
            Block block = BlockRegister.Get(packet.content[0]);
            int cIndex = int.Parse(packet.content[1]);
            int posX = int.Parse(packet.content[2]);
            int posY = int.Parse(packet.content[3]);

            //Set the block using the multiplayer method (more info in method)
            //World.Get().SetBlock_Multiplayer(block, cIndex, posX, posY);

            //If the server receives the call, redirect it to the other clients
            if (IsServer())
            {
                server.SendDataExceptClients(packet, client.id);
            }
        }

        public async static void HandleCreateChunk(IdTcpClient client, NetworkPacket packet) //Handled by both server and clients
        {
            //Create the new chunk on current instance
            int index = int.Parse(packet.content[0]);
            World.Get().CreateChunk(index);

            //If the server receives the message, it should additionally send the chunk back to all clients
            if (IsServer())
            {
                foreach (Block block in World.Get().GetChunk(index).blockList.blocks)
                {
                    server.SendDataToAll(CreatePacket(MultiplayerPacketType.SET_BLOCK, block.id, index.ToString(), block.posX.ToString(), block.posY.ToString()));
                }
            }
        }

        public static async void HandleRequest(IdTcpClient client, NetworkPacket packet)
        {
            if (packet.content[0] == "ping")
            {
                DateTime sentDate = DateTime.Parse(packet.content[1]);
                double ping = (DateTime.Now - sentDate).TotalMilliseconds;

                if (IsServer())
                {
                    server.SendDataSingleClient(CreatePacket(MultiplayerPacketType.PING_RESPONSE, ping.ToString()), client.id);
                }
                else
                {
                    SendData(MultiplayerPacketType.PING_RESPONSE, ping.ToString());
                }
            }
            else if (packet.content[0] == "player_information")
            {
                if (IsClient())
                {
                    NetworkHandler.client.SendPlayerInformation();
                }
            }
        }

        public static async void HandlePingResponse(IdTcpClient client, NetworkPacket packet)
        {
            if (IsServer())
            {
                Log.Write($"Your ping to client #{client.id} is {Math.Round(double.Parse(packet.content[0]))}ms.", LogType.NETWORK, LogLevel.INFO);
            }
            else
            {
                NotificationManager.Get().Notify("sc:snowball_item", $"Your ping to the server is {Math.Round(double.Parse(packet.content[0]))}ms.");
                Log.Write($"Your ping to the server is {Math.Round(double.Parse(packet.content[0]))}ms.", LogType.NETWORK, LogLevel.INFO);
            }
        }

        public static async void HandleDisconnect(IdTcpClient client, NetworkPacket packet)
        {
            if (IsServer())
            {
                server.Disconnect(client, "Disconnected manually");
            }
        }

        public static async void HandleConnectionConfirmation(IdTcpClient client, NetworkPacket packet)
        {
            if (IsServer())
            {
                client.isConnected = true;
            }
        }

        public static async void HandlePlayerInformation(IdTcpClient client, NetworkPacket packet)
        {
            if (IsClient())
            {
                //If client receives the packet, load the inventory
                JObject invToken = JObject.Parse(packet.content[0]);
                Player.Get().inventory = Inventory.FromJson(invToken, true);
            }
            else if (IsServer())
            {
                if (!string.IsNullOrEmpty(packet.content[0])) //Player ID
                {
                    client.id = int.Parse(packet.content[0]);
                }

                if (!string.IsNullOrEmpty(packet.content[1])) //Player Name
                {
                    client.nickname = packet.content[1];
                }

                if (!string.IsNullOrEmpty(packet.content[2])) //Player Inventory
                {
                    File.WriteAllText(Path.Combine(World.Get().multiplayerDirectory, $"inventory_{client.id}.json"), packet.content[0]);
                }
            }
        }
    }
}