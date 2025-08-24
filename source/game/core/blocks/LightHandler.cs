using System;

namespace SeeloewenCraft.game.core.blocks
{
    internal class LightHandler
    {
        internal static readonly int lightRange = 7;

        internal static void UpdateLighting(Block block)
        {
            //Set light level based on range and it being a light source
            int rangeTS = GetRangeToLS(block);
            block.lightLevel = Math.Abs(Math.Max(0, Math.Min(Game.world.lightRange, rangeTS)) - Game.world.lightRange);

            if(block.HasTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE)) block.isAirLightSource = IsAirLightSource(block);
        }

        private static int GetRangeToLS(Block block)
        {
            if (block.IsLightSource() || (block.GetForegroundBlock() != null && block.GetForegroundBlock().IsLightSource())) return 0;

            int minRange = Game.world.lightRange + 1; //Start off with a value that is higher than possible

            //Go through all blocks in range and check them and their foreground block
            foreach (Block b in Block.GetBlocksInRange(block, Game.world.lightRange))
            {
                if (b == null) continue;

                //Check if foreground block or block is a lightsource
                if (b.IsLightSource() || (b.GetForegroundBlock() != null && b.GetForegroundBlock().IsLightSource()))
                {
                    //Check if the range to this block is actually the smallest range
                    minRange = Math.Min(minRange, block.GetRangeToBlock(b));
                }
            }

            return minRange;
        }

        //Check block above - if there is no block above it is the top block, meaning it should be air light source
        //Similarly, if there is an air lightsource above, this one is also air lightsource
        public static bool IsAirLightSource(Block block)
        {
            if (Game.world.dayTime == world.DayTime.NIGHT) return false;

            Block blockAbove = block.GetBlockAbove();

            if (blockAbove == null) return true;

            if(blockAbove != null && blockAbove.isAirLightSource) return true;

            return false;
        }
    }
}
