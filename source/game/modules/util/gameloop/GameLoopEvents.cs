using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.networking;
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

    public class BlockUpdateEvent : GameLoopEvent
    {
        public BlockUpdateEvent(GameEventHandlerLegacy gameLoop) : base(gameLoop)
        {
            maxTick = 1000;
        }

        public override void DoEvent()
        {
            List<Block> leaves = new List<Block>();

            //Go through all blocks and update them accordingly
            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    //Update blocks that aren't allowed to float
                    UpdateFloating(block);

                    if (block is FarmlandBlock farmland)
                    {
                        UpdateFarmland(farmland);
                    }

                    if (block is GrassBlock grass)
                    {
                        UpdateGrass(grass);
                    }

                    //Update leaves that might need to decay
                    if (block.HasTag(BlockTags.TYPES_LEAF) && !block.HasTag(BlockTags.PLACED_MANUALLY))
                    {
                        leaves.Add(block);
                    }
                }
            }

            UpdateLeaves(leaves);
        }

        public void UpdateGrass(Block block)
        {
            //Roll whether to grow grass to adjacent dirt
            if (Game.rnd.Next(0, 40) == 0)
            {
                Block blockRight = block.GetBlockRight();
                Block blockLeft = block.GetBlockLeft();

                //Skip check for blocks at chunk borders, would only cause issues
                if (blockRight == null || blockLeft == null)
                {
                    return;
                }

                List<Block> candidates = new List<Block>
                {
                    blockRight,
                    blockLeft,
                    blockRight.GetBlockAbove(),
                    blockRight.GetBlockBelow(),
                    block.GetBlockLeft(),
                    blockLeft.GetBlockAbove(),
                    blockLeft.GetBlockBelow(),
                };

                //Evaluate possible candidates
                foreach (Block candidate in candidates)
                {
                    //Confirms that the candidate is actually a dirt block that has either nothing above (top world border), or a non-solid block above (except water)
                    if (candidate != null && candidate is DirtBlock dirt && (candidate.GetBlockAbove == null || (!candidate.GetBlockAbove().isSolid) && !candidate.GetBlockAbove().HasTag(BlockTags.LIQUIDS_WATER)))
                    {
                        candidate.SetBlock(BlockRegister.GenerateBlock("sc:grass_block"));
                    }
                }
            }


            //Roll whether to grow a random plant
            if (Game.rnd.Next(0, 100000) == 0)
            {
                Block blockAbove = block.GetBlockAbove();

                if (blockAbove != null && blockAbove.id == "sc:air_block")
                {
                    //Roll the crop
                    string cropId = Game.rnd.Next(0, 9) switch
                    {
                        0 => "sc:potato_crop_block",
                        1 => "sc:berry_bush_crop_block",
                        2 => "sc:carrots_crop_block",
                        3 => "sc:pumpkin_crop_block",
                        4 => "sc:cotton_crop_block",
                        5 => "sc:cucumber_crop_block",
                        6 => "sc:yellow_flower_block",
                        7 => "sc:blue_flower_block",
                        _ => "sc:grass"
                    };

                    blockAbove.SetBlock(BlockRegister.GenerateBlock(cropId));
                }
            }
        }

        public void UpdateFloating(Block block)
        {
            //Check if the block is floating even though it's not allowed to
            Block blockBelow = block.chunk.GetBlock(block.xPos, block.yPos + 1);

            if (block.needsGround.doesNeed && !block.CanStayOnBlockBelow(block, blockBelow))
            {
                block.BreakBlock(true, false, true);
            }
        }

        public void UpdateFarmland(Block block)
        {
            if (!HasWaterNearby(block) || block.lightLevel >= 0.9)
            {
                if (Game.rnd.Next(0, 9) == 0)
                {
                    block.SetBlock(BlockRegister.GenerateBlock("sc:dirt_block"));
                }
            }
        }


        public void UpdateLeaves(List<Block> leafList)
        {
            List<Block> decayingLeaves = new List<Block>();

            //Check for each leaf recursively if it has a connection to a log
            foreach (Block block in leafList)
            {
                if (!HasAdjacentLog(block, new List<Block>()))
                {
                    if (!block.HasTag(BlockTags.STRUCTURE_LEAF))
                    {
                        decayingLeaves.Add(block);
                    }
                }
                else if (block.HasTag(BlockTags.STRUCTURE_LEAF))
                {
                    block.RemoveTag(BlockTags.STRUCTURE_LEAF);
                }
            }

            //All leaves that should decay have a 33% chance to do so
            foreach (Block block in decayingLeaves)
            {
                if (Game.rnd.Next(0, 3) == 0)
                {
                    block.BreakBlock(true, false, true);
                }
            }
        }

        public bool HasAdjacentLog(Block block, List<Block> visitedBlocks) //Check recursively if a connection to a log is found
        {
            //If the block is null or it was already visited, don't check it
            if (block == null || visitedBlocks.Contains(block))
            {
                return false;
            }

            visitedBlocks.Add(block);

            //If it's a log, stop the search
            if (block.HasTag(BlockTags.TYPES_LOG))
            {
                return true;
            }

            //If it's not a leaf, stop searching on this branch
            if (!block.HasTag(BlockTags.TYPES_LEAF))
            {
                return false;
            }

            Block blockBelow = block.chunk.GetBlock(block.xPos, block.yPos + 1);
            Block blockAbove = block.chunk.GetBlock(block.xPos, block.yPos - 1);
            Block blockRight = block.chunk.GetBlock(block.xPos + 1, block.yPos);
            Block blockLeft = block.chunk.GetBlock(block.xPos - 1, block.yPos);

            //If it's a leaf, check if the adjacent blocks are connected to a log
            return HasAdjacentLog(blockBelow, visitedBlocks) || HasAdjacentLog(blockAbove, visitedBlocks) || HasAdjacentLog(blockRight, visitedBlocks) || HasAdjacentLog(blockLeft, visitedBlocks);

        }

        public bool HasWaterNearby(Block block)
        {
            //Checks 4 blocks to the left and right on the same y level whether they are a water block
            for (int i = 1; i < 5; i++)
            {
                Block blockRight = block.chunk.GetBlock(block.xPos + i, block.yPos);
                Block blockLeft = block.chunk.GetBlock(block.xPos - i, block.yPos);

                if (blockRight != null && blockRight.HasTag(BlockTags.LIQUIDS_WATER) //Blocks to the right
                    || blockLeft != null && blockLeft.HasTag(BlockTags.LIQUIDS_WATER)) //Blocks to the left
                {
                    return true;
                }
            }

            return false;
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
                NotificationHandler.ShowNotification("Successfully Auto-Saved the world!", 3000);
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