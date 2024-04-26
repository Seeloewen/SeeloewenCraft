using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SeeloewenCraft
{
    public class Structure
    {
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<StructureComponent> cutOffComponents = new List<StructureComponent>();
        public List<Block> blockList = new List<Block>();
        public Random rnd = new Random(DateTime.Now.Millisecond);
        public Chunk chunk;
        wndGame wndGame;
        public string direction = "";
        public int totalWidth;
        public int widthRemaining;
        public int xBase;
        public int yBase;
        public bool isCutOff;
        public bool isNew;

        public Structure(wndGame wndGame, Chunk chunk)
        {
            //Set the attributes
            this.chunk = chunk;
            this.wndGame = wndGame;
        }

        public void SetupStructure(int xBase, int yBase, string direction)
        {
            this.direction = direction;

            //Set the base coordinates
            this.xBase = xBase;
            this.yBase = yBase;

            //Check if the structure will be cut off
            if (isNew == true)
            {
                isCutOff = checkForCutoff();

                if (isCutOff == true)
                {
                    if (direction == "left")
                    {
                        widthRemaining = Math.Abs(xBase - totalWidth);
                    }
                    else if (direction == "right")
                    {
                        widthRemaining = xBase + totalWidth - 8;
                    }
                }
            }

            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (direction == "left")
                {
                    if (xBase - structureComponent.xOffset < 1)
                    {
                        structureComponent.xOffset = structureComponent.xOffset - xBase;
                        cutOffComponents.Add(structureComponent);
                    }
                    else
                    {
                        AddBlock(xBase, yBase, structureComponent.xOffset, structureComponent.yOffset, structureComponent.block);
                    }
                }
                else if (direction == "right")
                {
                    if (xBase + structureComponent.xOffset > 8)
                    {
                        structureComponent.xOffset = xBase + structureComponent.xOffset - 9;
                        cutOffComponents.Add(structureComponent);
                    }
                    else
                    {
                        AddBlock(xBase, yBase, structureComponent.xOffset, structureComponent.yOffset, structureComponent.block);
                    }
                }
            }
        }

        public void GenerateStructure()
        {

            //Create a list of block that will need to be replaced
            List<Block> removeBlocks = new List<Block>();

            foreach (Block block in blockList)
            {
                foreach (Block blockMain in chunk.blockList)
                {
                    if (block.xPos == blockMain.xPos && block.yPos == blockMain.yPos)
                    {
                        removeBlocks.Add(blockMain);
                    }
                }
            }

            //Remove the blocks from the list
            foreach (Block block in removeBlocks)
            {
                chunk.blockList.Remove(block);
            }

            //Add all blocks from the structure to the blocklist
            foreach (Block block in blockList)
            {
                if (block.xPos > 0 && block.xPos < 9)
                {
                    chunk.blockList.Add(block);
                }
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
            if (index >= 0)
            {              
                SetupStructure(x, y, "right");
                GenerateStructure();
            }
            else
            {                
                SetupStructure(x, y, "left");
                GenerateStructure();
            }

        }

        public bool checkForCutoff()
        {
            if (direction == "left")
            {
                if (xBase - totalWidth < 0)
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
                if (xBase + totalWidth > 9)
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

    public class ContinuationStructure : Structure
    {
        public ContinuationStructure(List<StructureComponent> structureList, wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, int remainingWidth) : base(wndGame, chunk)
        {
            totalWidth = remainingWidth;

            //Add all structure components
            foreach (StructureComponent structureComponent in structureList)
            {
                structureComponent.block.chunk = chunk;
                structureComponents.Add(structureComponent);
            }

            //Begin generating structure
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class AlphaStructure : Structure
    {
        public AlphaStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk) : base(wndGame, chunk)
        {
            //Set the total width of the structure
            totalWidth = 3;

            //Add all structure components - It's meant to look like a bedrock pyramid
            structureComponents.Add(new StructureComponent(wndGame, 0, 0, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 1, 0, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 0, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 0, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 4, 0, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 1, 1, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 1, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 1, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 2, new BedrockItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Begin generating the alpha structure - was only meant for development purposes and is no longer in the game
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class TreeStructure : Structure
    {
        public TreeStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk) : base(wndGame, chunk)
        {
            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(wndGame, 2, 0, new OakLogItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Layer 2
            structureComponents.Add(new StructureComponent(wndGame, 2, 1, new OakLogItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Layer 3
            structureComponents.Add(new StructureComponent(wndGame, 2, 2, new OakLogItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Layer 4
            structureComponents.Add(new StructureComponent(wndGame, 1, 3, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 3, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 3, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 4, 3, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 0, 3, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Layer 5
            structureComponents.Add(new StructureComponent(wndGame, 1, 4, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 4, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 4, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Layer 6
            structureComponents.Add(new StructureComponent(wndGame, 2, 5, new OakLeavesItem(wndGame, 0).GenerateBlock(x, y, chunk)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class OreStructure : Structure
    {
        public OreStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk) : base(wndGame, chunk)
        {
            //Generate a random number between 0 and 29 to get the ore type
            //WIP - Split into seperate ore structures for getting appropriate heights
            int random1 = rnd.Next(0, 30);

            if (random1 >= 0 && random1 <= 15) //Coal Vein
            {
                //Set the total width
                totalWidth = 4;
                int random2;

                //Layer 1
                structureComponents.Add(new StructureComponent(wndGame, 0, 0, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 0, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 0, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 3, 0, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }

                //Layer 2
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 1, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 1, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 1, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 3, 1, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }

                //Layer 3
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 2, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 2, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 2, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 3, 2, new CoalOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
            }
            else if (random1 > 15 && random1 <= 25) //Iron Vein
            {
                //Set the total width
                totalWidth = 3;
                int random2;

                //Layer 1
                structureComponents.Add(new StructureComponent(wndGame, 0, 0, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 0, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 0, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }

                //Layer 2
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 1, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 1, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 1, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }

                //Layer 3
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 2, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 2, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 2, new IronOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
            }
            else if (random1 > 25 && random1 <= 30) //Diamond Vein
            {
                //Set the total width
                totalWidth = 2;
                int random2;

                //Layer 1
                structureComponents.Add(new StructureComponent(wndGame, 0, 0, new DiamondOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 0, new DiamondOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }

                //Layer 2
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 1, new DiamondOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 1, new DiamondOreItem(wndGame, 0).GenerateBlock(x, y, chunk)));
                }
            }

            BeginGeneration(x, y, index, isNew);

        }
    }

    public class StructureComponent
    {
        public Block block;
        public wndGame wndGame;
        public int xOffset;
        public int yOffset;

        public StructureComponent(wndGame wndGame, int xOffset, int yOffset, Block block)
        {
            //Set the attributes of the structure component
            this.wndGame = wndGame;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.block = block;
        }
    }
}
