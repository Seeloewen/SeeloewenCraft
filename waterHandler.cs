using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class WaterHandler
    {
        wndGame wndGame;

        //-- Constructor --//

        public WaterHandler(wndGame wndGame)
        {
            this.wndGame = wndGame;
        }

        //-- Custom Methods --//

        public void DoUpdate()
        {
            List<Block> newBlocks = new List<Block>();

            //Water timer, ticks at a rate of 1 second
            foreach (Chunk chunk in wndGame.currentChunkList)
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
                            Chunk sourceChunk = wndGame.GetFromCurrentChunks(block.waterSourceChunkIndex);
                            if (sourceChunk != null)
                            {
                                Block sourceBlock = sourceChunk.GetBlock(block.waterSourceXPos, block.waterSourceYPos);
                                if (sourceBlock != null && !sourceBlock.isWaterSource && sourceBlock.foregroundBlock != null && !sourceBlock.foregroundBlock.isWaterSource)
                                {
                                    newBlocks.Add(new AirBlock(wndGame, block.xPos, block.yPos, chunk, null, false));
                                }
                            }
                        }
                    }
                    else if (block.foregroundBlock != null && block.foregroundBlock.name == "Water")
                    {
                        if (block.foregroundBlock.hasWaterSource)
                        {
                            //For each water block, check first if it can expand
                            ExpandWater(chunk, block.foregroundBlock, newBlocks);
                            Chunk sourceChunk = wndGame.GetFromCurrentChunks(block.foregroundBlock.waterSourceChunkIndex);
                            if (sourceChunk != null)
                            {
                                Block sourceBlock = sourceChunk.GetBlock(block.foregroundBlock.waterSourceXPos, block.foregroundBlock.waterSourceYPos);
                                if (sourceBlock != null && !sourceBlock.isWaterSource && sourceBlock.foregroundBlock != null && !sourceBlock.foregroundBlock.isWaterSource)
                                {
                                    newBlocks.Add(new AirBlock(wndGame, block.xPos, block.yPos, chunk, null, false));
                                }
                            }
                        }
                    }
                }

                //Apply the block updates in the newBlocks list
                UpdateBlocks(newBlocks);
            }
        }

        private void ExpandWater(Chunk chunk, Block block, List<Block> newBlocks)
        {
            //Check if the block below is free for expansion, and expand downwards if true
            if (block != null)
            {
                Block blockBelow = chunk.GetBlock(block.xPos, block.yPos + 1);

                if (blockBelow != null && (!blockBelow.isSolid && (blockBelow.name != "Water" || blockBelow.waterLevel < 6)) || (blockBelow.isBackground && (blockBelow.foregroundBlock == null || (block.foregroundBlock != null && block.foregroundBlock.waterLevel < 6))))
                {
                    Block newWater = new WaterBlock_6(wndGame, block.xPos, block.yPos + 1, chunk, null, false);
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
                            if ((!blockRight.isSolid && (blockRight.name != "Water" || blockRight.waterLevel < block.waterLevel - 1)) || (blockRight.isBackground && blockRight.foregroundBlock == null))
                            {
                                //Expand to the right
                                Block newWater = CreateWaterBlock(block.waterLevel - 1, "right", block.xPos + 1, block.yPos, chunk);
                                SetSourceBlock(block, newWater);
                                newBlocks.Add(newWater);
                            }
                            if ((!blockLeft.isSolid && (blockLeft.name != "Water" || blockLeft.waterLevel < block.waterLevel - 1)) || (blockLeft.isBackground && blockLeft.foregroundBlock == null))
                            {
                                //Expand to the left
                                Block newWater = CreateWaterBlock(block.waterLevel - 1, "left", block.xPos - 1, block.yPos, chunk);
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
                    if (blockBelow.isSolid && blockBelow.name != "Water")
                    {
                        return true;
                    }
                }
                else
                {
                    if (blockBelow.foregroundBlock != null && blockBelow.foregroundBlock.name != "Water")
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
                    if (block.name == "Water")
                    {
                        if (block.chunk.GetBlock(block.xPos, block.yPos).isBackground && block.chunk.GetBlock(block.xPos, block.yPos).foregroundBlock == null)
                        {
                            //If the block is in the background, place the block in the foreground
                            block.chunk.GetBlock(block.xPos, block.yPos).PlaceInForeground(block);
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

        private Block CreateWaterBlock(int level, string direction, int x, int y, Chunk chunk)
        {
            //Check direction and level and return the corresponding water block
            if (direction == "right")
            {
                switch (level)
                {
                    case 1:
                        return new WaterBlock_1_Right(wndGame, x, y, chunk, null, false);
                    case 2:
                        return new WaterBlock_2_Right(wndGame, x, y, chunk, null, false);
                    case 3:
                        return new WaterBlock_3_Right(wndGame, x, y, chunk, null, false);
                    case 4:
                        return new WaterBlock_4_Right(wndGame, x, y, chunk, null, false);
                    case 5:
                        return new WaterBlock_5_Right(wndGame, x, y, chunk, null, false);
                    case 6:
                        return new WaterBlock_6(wndGame, x, y, chunk, null, false);
                    default:
                        return null;
                }
            }
            else if (direction == "left")
            {
                switch (level)
                {
                    case 1:
                        return new WaterBlock_1_Left(wndGame, x, y, chunk, null, false);
                    case 2:
                        return new WaterBlock_2_Left(wndGame, x, y, chunk, null, false);
                    case 3:
                        return new WaterBlock_3_Left(wndGame, x, y, chunk, null, false);
                    case 4:
                        return new WaterBlock_4_Left(wndGame, x, y, chunk, null, false);
                    case 5:
                        return new WaterBlock_5_Left(wndGame, x, y, chunk, null, false);
                    case 6:
                        return new WaterBlock_6(wndGame, x, y, chunk, null, false);
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
            if (currentBlock.isBackground && currentBlock.foregroundBlock != null)
            {
                //If the current block is not a source block, set its source block as the source
                currentBlock.foregroundBlock.isWaterSource = true;

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
