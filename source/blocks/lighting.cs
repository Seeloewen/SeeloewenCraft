using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public abstract partial class Block
    {
        public bool IsLightSource(bool ignoreAir)
        {
            //Check if the block is a light source

            //If the block is a light source (possibly excluding air)
            if (isLightSource && (id != "sc:air_block" || !ignoreAir))
            {
                return true;
            }
            else
            {
                //If the foreground block is a lightsource (possibly excluding air)
                if (foregroundBlock != null && foregroundBlock.isLightSource && (foregroundBlock.id != "sc:air_block" || !ignoreAir))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateNearbyBlocks()
        {
            List<Block> blocksInRange = GetBlocksInRange(Game.world.lightRange);

            foreach (Block block in blocksInRange)
            {
                if (block != null)
                {
                    //For all blocks in range, check if either the block or foreground block are a lightsource
                    if (isLightSource || (foregroundBlock != null && foregroundBlock.isLightSource))
                    {
                        //Get the range to this block and compare it to the blocks current range to nearest light source
                        int range = GetRangeToBlock(block);
                        if (range < block.rangeToNearestLightSource)
                        {
                            //If the range is smaller, update the block
                            block.rangeToNearestLightSource = range;
                            block.SetLightLevel(range);

                            if (block.blockContainer != null)
                            {
                                block.blockContainer.SetLightOpacity();
                            }
                        }
                    }
                    //If it's not a lightsource
                    else if (!isLightSource && (foregroundBlock == null || (foregroundBlock != null && !foregroundBlock.isLightSource)))
                    {
                        if (blockContainer.previousBlockWasLightSource || blockContainer.previousForegroundBlockWasLightSource)
                        {
                            if (blockContainer != null && block.blockContainer != null)
                            {
                                //If the previous block or foregroundblock was a lightsource, update the blocks nearby
                                block.rangeToNearestLightSource = block.RangeToLightSource();
                                block.SetLightLevel(block.RangeToLightSource());
                                block.blockContainer.SetLightOpacity();
                            }
                        }
                    }
                }
            }
        }

        public int RangeToLightSource()
        {
            List<Block> blocksInRange = GetBlocksInRange(Game.world.lightRange);

            int minRange = Game.world.lightRange + 1;

            //For all blocks in range
            foreach (Block block in blocksInRange)
            {
                if (block != null)
                {
                    //Check if foreground block or block is a lightsource
                    if (block.isLightSource || (block.foregroundBlock != null && block.foregroundBlock.isLightSource))
                    {
                        //Get the range to that block and set it as nearest lightsource
                        int range = GetRangeToBlock(block);
                        SetAsNearestLightSource(range);

                        //Check if it's actually the smallest range
                        minRange = Math.Min(minRange, range);
                    }
                }
            }

            //Return the range to the smallest light source
            return minRange;
        }

        public void SetLightLevel(int range)
        {
            //Set light level based on range and it being a light source
            int rangeToLightSource = range;
            if (isLightSource || (foregroundBlock != null && foregroundBlock.isLightSource) || rangeToLightSource == 1 || rangeToLightSource == 2)
            {
                lightLevel = 0;
            }
            else if (rangeToLightSource < Game.world.lightRange)
            {
                lightLevel = 1.0 / (Game.world.lightRange - 3) * rangeToLightSource - 0.75;
            }
            else if (rangeToLightSource == Game.world.lightRange)
            {
                lightLevel = 0.9;
            }
            else
            {
                lightLevel = 1;
            }
        }

        private void SetAsNearestLightSource(int range)
        {
            if (!isLightSource && (foregroundBlock != null && !foregroundBlock.isLightSource))
            {
                //If no nearest lightsource is detected, add block as lightsource
                if (rangeToNearestLightSource == 100000) //Any random giant number that a range can never be
                {
                    rangeToNearestLightSource = range;
                }
                //If a block with a lower range to nearest lightsource is found, delete all current nearest ones and add new one
                else if (range < rangeToNearestLightSource)
                {
                    rangeToNearestLightSource = range;
                }
            }
        }

        private Block GetBlockFromOffset(int xOffset, int yOffset)
        {
            //Get the new block based from the offset from this block
            
            //If total y is above 74 or below 0, there isn't any available block
            if (yOffset + yPos < 0 || yOffset + yPos > 74)
            {
                return null;
            }

            //If total x is below 0, get the chunk to the left
            if (xPos + xOffset < 0)
            {
                Chunk chunk = Game.world.GetLoadedChunk(this.chunk.index - 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(xPos + xOffset + 8, yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            //If total x is above 7, get the chunk to the right
            else if (xPos + xOffset > 7)
            {
                Chunk chunk = Game.world.GetLoadedChunk(this.chunk.index + 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(xPos + xOffset - 8, yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //Else just get the block from the current chunk
                return chunk.blockList.Get(xPos + xOffset, yPos + yOffset);
            }
        }

        public void UpdateAirLightsources(Block block)
        {
            //Update Air Lightsources
            for (int y = yPos + 1; y < 76; y++)
            {
                //Go through each block below the currently placed one
                if (chunk.GetBlock(xPos, y).id == "sc:air_block")
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
        }

        public bool IsAirLightSource(Block block) //This check may later also include transparent blocks like glass
        {
            //Go through every block above this block
            for (int y = yPos - 1; y >= 0; y--)
            {
                //If a block above that is found that is not air (some other solid block), it is not an air light source 
                if (chunk.GetBlock(xPos, y).id != "sc:air_block")
                {
                    block.isLightSource = false;
                    return false;
                }
            }

            //If all blocks above the specified block are air blocks, it is an air lightsource
            return true;
        }
    }
}
