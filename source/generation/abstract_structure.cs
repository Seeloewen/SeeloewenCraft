using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class Structure
    {
        //References
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<StructureComponent> cutOffComponents = new List<StructureComponent>();
        public BlockList blockList = new BlockList();
        public Random rnd;
        public Chunk chunk;
        World world;

        //Constants
        public string name;
        public string id;
        public string direction = "";
        public int totalWidth;
        public int xBase;
        public int yBase;
        public bool isNew;
        public bool canFloat;
        public bool canReplaceSolidBlocks;

        //Variables
        public bool isCutOff;
        public int widthRemaining;
        static int o = 0;

        //-- Constructor --//

        public Structure(World world, Chunk chunk, bool canFloat)
        {
            //Set the attributes
            this.chunk = chunk;
            this.world = world;
            this.canFloat = canFloat;
            rnd = new Random(DateTime.Now.Millisecond + o);
            o++;
        }

        //-- Custom Methods --//

        public void SetupStructure(int xBase, int yBase, string direction)
        {
            //Set the base coordinates and direction
            this.xBase = xBase;
            this.yBase = yBase;
            this.direction = direction;

            //Check if the structure will be cut off
            if (isNew == true)
            {
                isCutOff = checkForCutoff();

                if (isCutOff == true)
                {
                    //1
                    if (direction == "left")
                    {
                        widthRemaining = Math.Abs(xBase - totalWidth);
                    }
                    else if (direction == "right")
                    {
                        widthRemaining = xBase + totalWidth - 8;
                    }

                    //2
                    //widthRemaining = direction == "left" ? Math.Abs(xBase - totalWidth) : xBase + totalWidth - 8;
                }
            }

            //Go through all the structure components
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (direction == "left")
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
                else if (direction == "right")
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
                        chunk.blockList.Add(block);

                        if (canFloat == false && block.yPos == yBase)
                        {
                            MakeBaseSolid(block);
                        }
                    }
                    else
                    {
                        if (chunk.blockList.Get(block.xPos, block.yPos).isBreakable)
                        {
                            chunk.blockList.Add(block);

                            if (canFloat == false && block.yPos == yBase)
                            {
                                MakeBaseSolid(block);
                            }
                        }
                    }
                }
            }
            world.log.Write($"Generated structure {id} at x{xBase} y{yBase} with width {totalWidth}, name {name} direction {direction}, isCutOff = {isCutOff}, widthRemaining = {widthRemaining}", "Info");

        }

        public void MakeBaseSolid(Block block)
        {
            //Check if the block is solid and on structure ground level and the block below is not solid
            if (chunk.GetBlock(block.xPos, block.yPos + 1).isSolid == false && block.isSolid == true)
            {
                //Create a new block of the same type and place it below the original block
                Type blockType = block.GetType();
                Block newBlock = (Block)Activator.CreateInstance(blockType, world, block.xPos, block.yPos + 1, chunk, null, block.isBackground);
                chunk.SetBlock(newBlock, block.xPos, block.yPos + 1);
                //Repeat until floor is reached
                MakeBaseSolid(chunk.GetBlock(block.xPos, block.yPos + 1));
            }
        }

        public void AddBlock(int xBase, int yBase, int xOffset, int yOffset, Block block)
        {
            //Add block to the structure blocklist
            if (direction == "right")
            {
                block.xPos = xBase + xOffset;
            }
            else if (direction == "left")
            {
                block.xPos = xBase - xOffset;
            }

            block.yPos = yBase - yOffset;
            blockList.Add(block);
        }

        public void BeginGeneration(int x, int y, int index, bool isNew)
        {
            this.isNew = isNew;
            //Check which direction it's going to be built in
            if (index > 0)
            {
                SetupStructure(x, y, "right");
                GenerateStructure();
            }
            else if (index < 0)
            {
                SetupStructure(x, y, "left");
                GenerateStructure();
            }
        }

        public bool checkForCutoff()
        {
            //Check if the structure is cut off on left or right side
            if (direction == "left")
            {
                if (xBase - totalWidth <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (direction == "right")
            {
                if (xBase + totalWidth > 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    //Components that tell the structure which blocks to place at which position
    public class StructureComponent
    {
        public Block block;
        public World world;
        public int xOffset;
        public int yOffset;

        public StructureComponent(World world, int xOffset, int yOffset, Block block)
        {
            //Set the attributes of the structure component
            this.world = world;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.block = block;
        }
    }
}
