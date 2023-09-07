using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeloewenCraft
{
    public class Structure
    {
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<Block> blockList = new List<Block>();
        public Random rnd = new Random(DateTime.Now.Millisecond);
        public Chunk chunk;
        wndGame wndGame;
        public int totalWidth;
        public int widthRemaining;
        public int xBase;
        public int yBase;
        public bool isCutOff;

        public Structure(wndGame wndGame, Chunk chunk)
        {
            //Set the attributes
            this.chunk = chunk;
            this.wndGame = wndGame;
        }

        public void SetupStructure(int xBase, int yBase, string direction, int alreadyDone)
        {
            //WIP - Continuation of structures doesn't work correctly yet

            //Set the base coordinates
            this.xBase = xBase;
            this.yBase = yBase;

            //Add blocks to the structure depending on the direction
            if (direction == "left")
            {
                //Check if the chunk would've been cut off
                if (xBase - 3 < 0)
                {
                    //Calculate the remaining blocks
                    isCutOff = true;
                    widthRemaining = Math.Abs(xBase - 3);
                }
                else
                {
                    //Set the remaining width to 0
                    isCutOff = false;
                    widthRemaining = 0;
                }

                //Add all the blocks to the structure
                foreach (StructureComponent structureComponent in structureComponents)
                {
                    AddBlock(xBase, yBase, structureComponent.xOffset + alreadyDone, structureComponent.yOffset, structureComponent.block);
                }
            }
            else if (direction == "right")
            {
                //Check if the chunk would've been cut off
                if (xBase + 3 > 8)
                {
                    //Calculate the remaining blocks
                    isCutOff = true;
                    widthRemaining = Math.Abs(xBase + 3 - 8);
                }
                else
                {
                    //Set the remaining width to 0
                    isCutOff = false;
                    widthRemaining = 0;
                }

                //Add all the blocks to the structure
                foreach (StructureComponent structureComponent in structureComponents)
                {
                    AddBlock(xBase, yBase, structureComponent.xOffset - alreadyDone, structureComponent.yOffset, structureComponent.block);
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
                chunk.blockList.Add(block);
            }
        }

        public void AddBlock(int xBase, int yBase, int xOffset, int yOffset, Block block)
        {
            //Add block to the structure blocklist
            if (xBase + xOffset > 0 && xBase + xOffset < 9)
            {
                block.xPos = xBase + xOffset;
                block.yPos = yBase - yOffset;
                blockList.Add(block);
            }
        }

        public void BeginGeneration(int x, int y, int index, bool isNew, int alreadyDone)
        {
            //Begin generating the structure based on whether it's new or a contination
            if (isNew == true)
            {

                //Check which direction it's going to be built in
                if (index >= 0)
                {
                    SetupStructure(x, y, "right", 0);
                    GenerateStructure();
                }
                else
                {
                    SetupStructure(x, y, "left", 0);
                    GenerateStructure();
                }
            }
            else
            {
                //Check which direction it's going to be built in
                if (index >= 0)
                {
                    SetupStructure(x, y, "right", alreadyDone);
                    GenerateStructure();
                }
                else
                {
                    SetupStructure(x, y, "left", alreadyDone);
                    GenerateStructure();
                }
            }
        }
    }

    public class AlphaStructure : Structure
    {
        public AlphaStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, int alreadyDone) : base(wndGame, chunk)
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
            BeginGeneration(x, y, index, isNew, alreadyDone);
        }
    }

    public class TreeStructure : Structure
    {
        public TreeStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, int alreadyDone) : base(wndGame, chunk)
        {
            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(wndGame, 2, 0, new OakLogItem(wndGame, 0 ).GenerateBlock(x, y, chunk)));

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
            BeginGeneration(x, y, index, isNew, alreadyDone);

        }
    }

    public class OreStructure : Structure
    {
        public OreStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, int alreadyDone) : base(wndGame, chunk)
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

            BeginGeneration(x, y, index, isNew, alreadyDone);

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
