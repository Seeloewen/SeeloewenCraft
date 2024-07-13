using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}