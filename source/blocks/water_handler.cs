using System.Collections.Generic;

namespace SeeloewenCraft
{
    public class WaterHandler
    {
        

        //-- Custom Methods --//

        public void DoUpdate()
        {
            List<Block> newBlocks = new List<Block>();

            //Water timer, ticks at a rate of 1 second
            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    if (block.name == "Water")
                    {
                        //For each water block, check first if it can expand
                        ExpandWater(chunk, block, newBlocks);

                        //Check if the block has water source and if it still exists
                        if (block.hasWaterSource)
                        {
                            if (!SourceBlockExists(block, "normal"))
                            {
                                Block newBlock = new AirBlock( false);
                                newBlock.SetCoords(block.xPos, block.yPos, block.chunk);
                                newBlocks.Add(newBlock);
                            }
                        }
                    }
                    else if (block.GetForegroundBlock() != null && block.GetForegroundBlock().tags.Contains("liquids/water"))
                    {
                        //For each water block, check first if it can expand
                        ExpandWater(chunk, block.GetForegroundBlock(), newBlocks);

                        if (block.GetForegroundBlock().hasWaterSource)
                        {
                            if (!SourceBlockExists(block, "foreground"))
                            {
                                Block newBlock = new AirBlock( false);
                                newBlock.SetCoords(block.xPos, block.yPos, block.chunk);
                                newBlocks.Add(newBlock);
                            }
                        }

                    }
                }
            }

            //Apply the block updates in the newBlocks list
            UpdateBlocks(newBlocks);
        }

        private bool SourceBlockExists(Block block, string blockState)
        {
            Chunk sourceChunk;
            Block sourceBlock;

            if (blockState == "foreground")
            {
                sourceChunk = Game.world.GetLoadedChunk(block.GetForegroundBlock().waterSourceChunkIndex);
                if (sourceChunk != null)
                {
                    sourceBlock = sourceChunk.GetBlock(block.GetForegroundBlock().waterSourceXPos, block.GetForegroundBlock().waterSourceYPos);

                    if (sourceBlock != null && sourceBlock.GetForegroundBlock() == null && !sourceBlock.isWaterSource)
                    {
                        return false;
                    }
                    else if (sourceBlock != null && sourceBlock.GetForegroundBlock() != null && !sourceBlock.GetForegroundBlock().isWaterSource)
                    {
                        return false;
                    }
                }
            }
            else if (blockState == "normal")
            {
                sourceChunk = Game.world.GetLoadedChunk(block.waterSourceChunkIndex);
                if (sourceChunk != null)
                {
                    sourceBlock = sourceChunk.GetBlock(block.waterSourceXPos, block.waterSourceYPos);

                    if (sourceBlock != null && sourceBlock.GetForegroundBlock() == null && !sourceBlock.isWaterSource)
                    {
                        return false;
                    }
                    else if (sourceBlock != null && sourceBlock.GetForegroundBlock() != null && !sourceBlock.GetForegroundBlock().isWaterSource)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ExpandWater(Chunk chunk, Block block, List<Block> newBlocks)
        {
            //Check if the block below is free for expansion, and expand downwards if true
            if (block != null)
            {
                Block blockBelow = chunk.GetBlock(block.xPos, block.yPos + 1);

                if (blockBelow != null && (blockBelow.isReplacable && (!blockBelow.tags.Contains("liquids/water") || blockBelow.waterLevel < 6)) || (blockBelow.isBackground && (blockBelow.GetForegroundBlock() == null || (block.GetForegroundBlock()   != null && block.GetForegroundBlock().waterLevel < 6))))
                {
                    Block newWater = new WaterBlock_6( false);
                    newWater.SetCoords(block.xPos, block.yPos + 1, block.chunk);
                    SetSourceBlock(block, newWater);
                    newBlocks.Add(newWater);
                }
                //If not expansion downwards was possible, try to expand to the sides
                else if (CantFlowDownwards(blockBelow))
                {
                    //Only expand if the water hasn't reached the end of the stream
                    if (block.waterLevel != 1 && chunk != null)
                    {
                        Block blockRight = chunk.GetBlock(block.xPos + 1, block.yPos);
                        Block blockLeft = chunk.GetBlock(block.xPos - 1, block.yPos);

                        if (blockRight != null && blockLeft != null)
                        {
                            if ((blockRight.isReplacable && (!blockRight.tags.Contains("liquids/water") || blockRight.waterLevel < block.waterLevel - 1)) || (blockRight.isBackground && blockRight.GetForegroundBlock() == null))
                            {
                                //Expand to the right
                                Block newWater = CreateWaterBlock(block.waterLevel - 1, "right");
                                newWater.SetCoords(block.xPos + 1, block.yPos, block.chunk);
                                SetSourceBlock(block, newWater);
                                newBlocks.Add(newWater);
                            }
                            if ((blockLeft.isReplacable && (!blockLeft.tags.Contains("liquids/water") || blockLeft.waterLevel < block.waterLevel - 1)) || (blockLeft.isBackground && blockLeft.GetForegroundBlock() == null))
                            {
                                //Expand to the left
                                Block newWater = CreateWaterBlock(block.waterLevel - 1, "left");
                                newWater.SetCoords(block.xPos - 1, block.yPos, block.chunk);
                                SetSourceBlock(block, newWater);
                                newBlocks.Add(newWater);
                            }
                        }
                    }
                }
            }
        }

        public bool CantFlowDownwards(Block blockBelow)
        {
            if (blockBelow != null)
            {
                if (!blockBelow.isBackground)
                {
                    if (!blockBelow.isReplacable && !blockBelow.tags.Contains("liquids/water"))
                    {
                        return true;
                    }
                }
                else
                {
                    if (blockBelow.GetForegroundBlock() != null && !blockBelow.GetForegroundBlock().tags.Contains("liquids/water"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateBlocks(List<Block> newBlocks)
        {
            foreach (Block block in newBlocks)
            {
                if (block != null && block.yPos > 0 && block.yPos < 76)
                {
                    //Check if a new water blocks needs to be placed or a block needs to be removed
                    if (block.tags.Contains("liquids/water"))
                    {
                        if (block.chunk.GetBlock(block.xPos, block.yPos).isBackground && block.chunk.GetBlock(block.xPos, block.yPos).GetForegroundBlock() == null)
                        {
                            //If the block is in the background, place the block in the foreground
                            block.chunk.GetBlock(block.xPos, block.yPos).SetForegroundBlock(block);
                        }
                        else if (!block.chunk.GetBlock(block.xPos, block.yPos).isBackground)
                        {
                            //If it's not in the background, just place it in the foreground
                            block.chunk.GetBlock(block.xPos, block.yPos).PlaceNewBlock(block);
                        }
                    }
                    else
                    {
                        block.chunk.GetBlock(block.xPos, block.yPos).BreakBlock(true, true);
                    }
                }
            }
        }

        private Block CreateWaterBlock(int level, string direction)
        {
            //Check direction and level and return the corresponding water block
            if (direction == "right")
            {
                switch (level)
                {
                    case 1:
                        return new WaterBlock_1_Right( false);
                    case 2:
                        return new WaterBlock_2_Right( false);
                    case 3:
                        return new WaterBlock_3_Right( false);
                    case 4:
                        return new WaterBlock_4_Right( false);
                    case 5:
                        return new WaterBlock_5_Right( false);
                    case 6:
                        return new WaterBlock_6( false);
                    default:
                        return null;
                }
            }
            else if (direction == "left")
            {
                switch (level)
                {
                    case 1:
                        return new WaterBlock_1_Left( false);
                    case 2:
                        return new WaterBlock_2_Left( false);
                    case 3:
                        return new WaterBlock_3_Left( false);
                    case 4:
                        return new WaterBlock_4_Left( false);
                    case 5:
                        return new WaterBlock_5_Left( false);
                    case 6:
                        return new WaterBlock_6( false);
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }

        }

        private void SetSourceBlock(Block currentBlock, Block newBlock)
        {
            //If the block at the location is a background block
            if (currentBlock.isBackground && currentBlock.GetForegroundBlock() != null)
            {
                //If the current block is not a source block, set its source block as the source
                currentBlock.GetForegroundBlock().isWaterSource = true;

            }
            else //If it's just a normal non-solid replacable block
            {
                //If the current block is not a source block, set its source block as the source
                currentBlock.isWaterSource = true;
            }

            //Add the source block attributes for the new block
            newBlock.hasWaterSource = true;
            newBlock.waterSourceXPos = currentBlock.xPos;
            newBlock.waterSourceYPos = currentBlock.yPos;
            newBlock.waterSourceChunkIndex = currentBlock.chunk.index;
        }
    }
}
