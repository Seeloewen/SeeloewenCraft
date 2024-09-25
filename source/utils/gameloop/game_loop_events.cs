using SeeloewenCraft.entity;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class WaterUpdateEvent : GameLoopEvent
    {
        public WaterUpdateEvent(GameLoop gameLoop) : base(gameLoop)
        {
            maxTick = 1000;
        }

        public override void DoEvent()
        {
            Game.world.waterHandler.DoUpdate();
        }
    }

    public class EntitySyncEvent : GameLoopEvent
    {
        public EntitySyncEvent(GameLoop gameLoop) : base(gameLoop)
        {
            maxTick = 400;
        }

        public override void DoEvent()
        {
            //Game.world.player.SendSyncData();
            /*foreach (Entity entity in Game.world.entities)
            {
                if (entity is MovingEntity movEntity)
                {
                    movEntity.SendSyncData();
                }
            }*/ 

            //TODO: Needs rework for new entity system
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