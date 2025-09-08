using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.notifications;
using System.Collections.Generic;

namespace SeeloewenCraft.game.core.legacy
{

    public class SendConnectionStateEvent : GameLoopEvent
    {
        public SendConnectionStateEvent(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            maxTick = 10000;
        }

        public override void DoEvent()
        {
            //Sends a packet to the server that confirms that the connection is still standing
            if (NetworkHandler.IsClient())
            {
                NetworkHandler.SendData(MultiplayerPacketType.CONNECTION_CONFIRMATION, "");
            }
        }
    }

    public class ClientConnectedCheckEvent : GameLoopEvent
    {
        public ClientConnectedCheckEvent(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            maxTick = 20000;
        }

        public override void DoEvent()
        {
            //Checks for each client if it is listed as connected. If not, disconnect it properly
            if (NetworkHandler.IsServer())
            {
                for (int i = 0; i < NetworkHandler.server.clients.Count; i++)
                {
                    IdTcpClient client = NetworkHandler.server.clients[i];

                    if (!client.isConnected)
                    {
                        NetworkHandler.server.Disconnect(client, "Timed out");
                    }
                    else
                    {
                        client.isConnected = false;
                    }
                }
            }
        }
    }
    public class AutoSaveEvent : GameLoopEvent
    {
        public AutoSaveEvent(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            UpdateMaxTick();
        }

        public override void DoEvent()
        {
            Game.world.Save();

            if (Settings.showAutoSaveNotification)
            {
                NotificationHandler.Notify("sc:grass_block_item", "Successfully Auto-Saved the world!");
            }
        }

        public void UpdateMaxTick()
        {
            maxTick = Settings.autoSaveInterval * 60000;
        }
    }

    public class EntitySyncEvent : GameLoopEvent //Temporarily reverted to legacy system
    {
        public EntitySyncEvent(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            maxTick = 400;
        }

        public override void DoEvent()
        {
            Game.world.player.SendSyncData();
            foreach (Entity entity in Game.world.entityManager.entities)
            {
                if (entity is MovingEntity movEntity)
                {
                    movEntity.SendSyncData();
                }
            }
        }

    }

    public class DayNightCycle : GameLoopEvent
    {
        public DayNightCycle(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            singleEvent = false;
            maxTick = 1200000;
        }

        public override void DoEvent()
        {
            
        }
    }

    public class CropTimerEvent : GameLoopEvent
    {
        public CropTimerEvent(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            maxTick = 250;
        }

        public override void DoEvent()
        {
            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    if (block is CropBlock crop && crop.progress < 200000000)
                    {
                        crop.UpdateProgress(maxTick);
                    }

                    Block foregroundBlock = block.GetForegroundBlock();
                    if (block.isBackground && foregroundBlock != null && foregroundBlock is CropBlock foreCrop && foreCrop.progress < 200000000)
                    {
                        foreCrop.UpdateProgress(maxTick);
                    }
                }
            }
        }
    }
}