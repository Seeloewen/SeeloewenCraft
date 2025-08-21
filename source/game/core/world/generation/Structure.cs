using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.core.world.generation
{
    public class Structure
    {
        //References
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<StructureComponent> cutOffComponents = new List<StructureComponent>();
        public BlockList blockList;
        public Chunk chunk;
        protected Random structRnd;
        public StructureShapeCreator shapeCreator;

        //Constants
        public string name;
        public string id;
        public Direction direction;
        public int totalWidth;
        public int xBase;
        public int yBase;
        public bool isNew;
        public bool canFloat;
        public bool canReplaceSolidBlocks;

        //Variables
        public bool isCutOff;
        public int widthRemaining;

        //-- Constructor --//

        public Structure(Chunk chunk, bool canFloat)
        {
            //Set the attributes
            blockList = new BlockList(chunk);
            shapeCreator = new StructureShapeCreator();
            this.chunk = chunk;
            this.canFloat = canFloat;

            chunk.structureNum++;
            structRnd = new Random(chunk.chunkSeed + chunk.structureNum);
        }

        //-- Custom Methods --//

        public void SetupStructure(int xBase, int yBase, Direction direction)
        {
            //Set the base coordinates and direction
            this.xBase = xBase;
            this.yBase = yBase;
            this.direction = direction;

            //Check if the structure will be cut off
            if (isNew == true)
            {
                isCutOff = CheckForCutoff();

                if (isCutOff == true)
                {
                    widthRemaining = direction.IsLeft()
                            ? Math.Abs(xBase - totalWidth)
                            : xBase + totalWidth - 8;
                }
            }

            //Go through all the structure components
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (direction.IsLeft())
                {
                    if (xBase - structureComponent.xOffset < 0)
                    {
                        //If the component would be offscreen, move it to the cut off list and change the offset
                        structureComponent.xOffset = structureComponent.xOffset - xBase - 1;
                        cutOffComponents.Add(structureComponent);
                    }
                    else
                    {
                        //Place the block
                        AddBlock(xBase, yBase, structureComponent.xOffset, structureComponent.yOffset, structureComponent.block);
                    }
                }
                else if (direction.IsRight())
                {
                    if (xBase + structureComponent.xOffset > 7)
                    {
                        //If the component would be offscreen, move it to the cut off list and change the offset
                        structureComponent.xOffset = xBase + structureComponent.xOffset - 8;
                        cutOffComponents.Add(structureComponent);
                    }
                    else
                    {
                        //Place the block
                        AddBlock(xBase, yBase, structureComponent.xOffset, structureComponent.yOffset, structureComponent.block);
                    }
                }
            }
        }

        public void AddBlock(Block block, int xOffset, int yOffset)
        {
            //Create the structure component
            structureComponents.Add(new StructureComponent(xOffset, yOffset, block));
        }

        public void AddBlock(Block block, int xOffset, int yOffset, LootTable lootTable, int insertAmount)
        {
            //Insert the loot table in the block
            if (block.hasInventory)
            {
                block.InsertLootTable(lootTable, insertAmount, structRnd);
            }

            //Create the structure component
            structureComponents.Add(new StructureComponent(xOffset, yOffset, block));
        }

        public void AddBackgroundBlock(Block block, int xOffset, int yOffset, Block foregroundBlock)
        {
            //Move the block to background and set the foregroundblock
            block.MoveToBackground();
            if (foregroundBlock != null)
            {
                block.SetForegroundBlock(foregroundBlock);
            }

            //Create the structure component
            structureComponents.Add(new StructureComponent(xOffset, yOffset, block));
        }

        public void AddBackgroundBlock(Block block, int xOffset, int yOffset, Block foregroundBlock, LootTable lootTable, int insertAmount)
        {
            //Either add the loot table to the foreground block or the background block
            if (foregroundBlock != null && foregroundBlock.hasInventory)
            {
                foregroundBlock.InsertLootTable(lootTable, insertAmount, structRnd);
            }
            else if (block.hasInventory)
            {
                block.InsertLootTable(lootTable, insertAmount, structRnd);
            }

            //Set the block to background, add the foregroundblock and create the structure component
            block.MoveToBackground();
            if (foregroundBlock != null)
            {
                block.SetForegroundBlock(foregroundBlock);
            }
            structureComponents.Add(new StructureComponent(xOffset, yOffset, block));
        }


        public void GenerateStructure()
        {
            //Add all blocks from the structure to the blocklist
            foreach (Block block in blockList.blocks)
            {
                if (block != null && block.xPos >= 0 && block.xPos <= 7 && block.yPos >= 0 && block.yPos <= 74)
                {
                    //Compare each block in the structure list to the block that's already at that position; if it has a fixed solid state, don't replace it
                    if (canReplaceSolidBlocks)
                    {
                        chunk.SetBlock(block, block.xPos, block.yPos);

                        if (canFloat == false && block.yPos == yBase)
                        {
                            MakeBaseSolid(block);
                        }
                    }
                    else
                    {
                        if (chunk.blockList.Get(block.xPos, block.yPos).isBreakable)
                        {
                            chunk.SetBlock(block, block.xPos, block.yPos);

                            if (canFloat == false && block.yPos == yBase)
                            {
                                MakeBaseSolid(block);
                            }
                        }
                    }
                }
            }
            Log.Write($"Generated structure {id} at x{xBase} y{yBase} with width {totalWidth}, name {name}, direction {direction}, isCutOff = {isCutOff}, widthRemaining = {widthRemaining}", LogType.STRUCTURE_GENERATION, LogLevel.INFO);

        }

        public void MakeBaseSolid(Block block)
        {
            //Check if the block is solid and on structure ground level and the block below is not solid
            if (chunk.GetBlock(block.xPos, block.yPos + 1).isSolid == false && block.isSolid == true)
            {
                //Create a new block of the same type and place it below the original block
                Type blockType = block.GetType();
                Block newBlock = (Block)Activator.CreateInstance(blockType, args: block.isBackground);
                chunk.SetBlock(newBlock, block.xPos, block.yPos + 1);
                //Repeat until floor is reached
                MakeBaseSolid(chunk.GetBlock(block.xPos, block.yPos + 1));
            }
        }

        public void AddBlock(int xBase, int yBase, int xOffset, int yOffset, Block block)
        {
            block.xPos = xBase + (direction.IsRight() ? xOffset : -xOffset);
            block.yPos = yBase - yOffset;
            blockList.Add(block, block.xPos, block.yPos);
        }

        public void BeginGeneration(int x, int y, int index, bool isNew)
        {
            if (chunk != null)
            {
                //Check which direction it's going to be built in
                this.isNew = isNew;
                totalWidth = GetTotalWidth();
                SetupStructure(x, y, index > 0 ? Direction.RIGHT : Direction.LEFT);
                GenerateStructure();
            }
        }

        public bool CheckForCutoff()
        {
            //Check if the structure is cut off on left or right side
            if (direction.IsLeft())
            {
                return xBase - totalWidth <= 0;

            }
            else if (direction.IsRight())
            {
                return xBase + totalWidth > 7;

            }
            else
            {
                return false;
            }
        }

        public int GetTotalWidth() //TODO: This should not be called by the structures, but by the actual generator itself
        {
            //Get the total width by checking the amount of different X coordinates
            List<int> handledX = new List<int>();
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (!handledX.Contains(structureComponent.xOffset))
                {
                    handledX.Add(structureComponent.xOffset);
                }
            }
            return handledX.Count;
        }

    }

    //Components that tell the structure which blocks to place at which position
    public class StructureComponent
    {
        public Block block;
        public int xOffset;
        public int yOffset;

        public StructureComponent(int xOffset, int yOffset, Block block)
        {
            //Set the attributes of the structure component
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.block = block;
        }
    }
}
