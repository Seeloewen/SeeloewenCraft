using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class WaterUpdateEvent : GameLoopEvent
    {
        public WaterUpdateEvent(World world, GameLoop gameLoop) : base(world, gameLoop)
        {
            maxTick = 800;
        }

        public override void DoEvent()
        {
            world.waterHandler.DoUpdate();
        }
    }

    public class DayNightCycle : GameLoopEvent
    {
        public DayNightCycle(World world, GameLoop gameLoop) : base(world, gameLoop)
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
            switch (tick)
            {
                case 480000://State 1: Sun setting slightly
                case 1170000: //State 1: Sun almost up
                    world.SetNight(1);
                    world.wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 150, 195, 198));
                    break;
                case 510000: //State 2: Sun setting more
                case 1140000: //State 2: Sun rising even more
                    world.SetNight(2);
                    world.wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 113, 146, 148));
                    break;
                case 540000: //State 3: Sun setting even more
                case 1110000: //State 3: Sun rising some more
                    world.SetNight(3);
                    world.wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 75, 98, 99));
                    break;
                case 570000: //State 4: Sun almost down
                case 1080000: //State 4: Sun rising slightly
                    world.SetNight(4);
                    world.wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 38, 49, 49));
                    break;
                case 600000: //State 5: Sun completely down
                    world.SetNight(5);
                    world.wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 10, 12, 13));
                    break;
                case 1200000: //State 0: Sun completely up
                    world.SetNight(0);
                    world.wndGame.cvsWorld.Background = new SolidColorBrush(Color.FromArgb(255, 188, 244, 247));
                    Reset();
                    break;
            }

            world.log.Write($"Ticked! {world.nightState}", "Info");
        }
    }
}