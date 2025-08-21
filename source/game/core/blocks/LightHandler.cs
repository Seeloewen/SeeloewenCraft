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


        /*public static void UpdateAirLightsources(Block block)
        {
            //Update Air Lightsources
            for (int y = block.yPos + 1; y < 76; y++)
            {
                //Go through each block below the currently placed one
                if (block.chunk.GetBlock(xPos, y) != null && chunk.GetBlock(xPos, y).id == "sc:air_block")
                {
                    //If the block at that position is air, update it accordingly
                    AirBlock newBlock = new AirBlock(false);
                    newBlock.rangeToNearestLightSource = chunk.GetBlock(xPos, y).rangeToNearestLightSource;

                    //If the placed block is air, the blocks below should be a lightsource, if not, then no light source
                    if (block.id == "sc:air_block" && block.isLightSource)
                    {
                        newBlock.isLightSource = true;
                    }
                    else
                    {
                        newBlock.isLightSource = false;
                    }

                    chunk.SetBlock(newBlock, xPos, y);
                }
                else
                {
                    //If it's not air, the other blocks below don't matter since that block blocks it.
                    break;
                }
            }
        }*/

        public static bool IsAirLightSource(Block block) //This check may later also include transparent blocks like glass
        {
            //Go through every block above this block
            for (int y = block.yPos - 1; y >= 0; y--)
            {
                //If a block above that is found that is not air (some other solid block), it is not an air light source 
                if (block.chunk.GetBlock(block.xPos, y).id != "sc:air_block")
                {
                    block.isAirLightSource = false;
                    return false;
                }
            }

            //If all blocks above the specified block are air blocks, it is an air lightsource
            return true;
        }
    }
}
