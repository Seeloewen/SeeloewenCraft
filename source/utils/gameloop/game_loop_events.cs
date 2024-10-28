using System.Collections.Generic;
using System.Windows.Media;
using SeeloewenCraft.entity;

namespace SeeloewenCraft
{
    public class BlockUpdateEvent : GameLoopEvent
    {
        public BlockUpdateEvent(GameLoop gameLoop) : base(gameLoop)
        {
            maxTick = 1000;
        }

        public override void DoEvent()
        {
            Game.world.waterHandler.DoUpdate();

            List<Block> leaves = new List<Block>();

            //Go through all blocks and update them accordingly
            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    //Update blocks that aren't allowed to float
                    UpdateFloating(block);

                    //Update leaves that might need to decay
                    if (block.tags.Contains("type/leaf") && !block.tags.Contains("placedManually"))
                    {
                        leaves.Add(block);
                    }
                }
            }

            UpdateLeaves(leaves);
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

        public void UpdateLeaves(List<Block> leafList)
        {
            List<Block> decayingLeaves = new List<Block>();

            //Check for each leaf recursively if it has a connection to a log
            foreach (Block block in leafList)
            {
                if (!HasAdjacentLog(block, new List<Block>()))
                {
                    decayingLeaves.Add(block);
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
            if (block.tags.Contains("type/log"))
            {
                return true;
            }

            //If it's not a leaf, stop searching on this branch
            if (!block.tags.Contains("type/leaf"))
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
    }

    public class AutoSaveEvent : GameLoopEvent
    {
        public AutoSaveEvent(GameLoop gameLoop) : base(gameLoop)
        {
            UpdateMaxTick();
        }

        public override void DoEvent()
        {
            Game.world.Save();

            if (Settings.showAutoSaveNotification)
            {
                NotificationHandler.ShowNotification("Successfully Auto-Saved the world!", 3000, Images.Diamond.GetTexture());
            }
        }

        public void UpdateMaxTick()
        {
            maxTick = Settings.autoSaveInterval * 60000;
        }
    }

    public class EntitySyncEvent : GameLoopEvent //Temporarily reverted to legacy system
    {
        public EntitySyncEvent(GameLoop gameLoop) : base(gameLoop)
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
        public DayNightCycle(GameLoop gameLoop) : base(gameLoop)
        {
            singleEvent = false;
            maxTick = 1200000;
        }

        public override bool IsReady()
        {
            if (tick == 480000
                || tick == 510000
                || tick == 540000
                || tick == 570000
                || tick == 600000
                || tick == 1080000
                || tick == 1110000
                || tick == 1140000
                || tick == 1170000
                || tick == 1200000)
            {
                return true;
            }
            return false;
        }

        public override void DoEvent()
        {
            //Set night state based on tick
            switch (tick)
            {
                case 480000://State 1: Sun setting slightly
                case 1170000: //State 1: Sun almost up
                    Game.world.SetNight(1);
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 150, 195, 198));
                    break;
                case 510000: //State 2: Sun setting more
                case 1140000: //State 2: Sun rising even more
                    Game.world.SetNight(2);
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 113, 146, 148));
                    break;
                case 540000: //State 3: Sun setting even more
                case 1110000: //State 3: Sun rising some more
                    Game.world.SetNight(3);
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 75, 98, 99));
                    break;
                case 570000: //State 4: Sun almost down
                case 1080000: //State 4: Sun rising slightly
                    Game.world.SetNight(4);
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 38, 49, 49));
                    break;
                case 600000: //State 5: Sun completely down
                    Game.world.SetNight(5);
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 10, 12, 13));
                    break;
                case 1200000: //State 0: Sun completely up
                    Game.world.SetNight(0);
                    Game.world.wndGame.cvsGame.Background = new SolidColorBrush(Color.FromArgb(255, 188, 244, 247));
                    Reset();
                    break;
            }
        }
    }

    public class CropTimerEvent : GameLoopEvent
    {
        public CropTimerEvent(GameLoop gameLoop) : base(gameLoop)
        {
            maxTick = 250;
        }

        public override void DoEvent()
        {
            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    if (block is CropBlock crop)
                    {
                        crop.UpdateProgress(maxTick);
                    }
                }
            }
        }
    }
}