using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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
        public bool canFloat;

        public Structure(wndGame wndGame, Chunk chunk, bool canFloat)
        {
            //Set the attributes
            this.chunk = chunk;
            this.wndGame = wndGame;
            this.canFloat = canFloat;
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

            //Go through all the structure components
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (direction == "left")
                {
                    if (xBase - structureComponent.xOffset < 1)
                    {
                        //If the component would be offscreen, move it to the cut off list and change the offset
                        structureComponent.xOffset = structureComponent.xOffset - xBase;
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
                    if (xBase + structureComponent.xOffset > 8)
                    {
                        //If the component would be offscreen, move it to the cut off list and change the offset
                        structureComponent.xOffset = xBase + structureComponent.xOffset - 9;
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

                    if (canFloat == false && block.yPos == yBase)
                    {
                        MakeBaseSolid(block);
                    }
                }
            }
        }

        public void MakeBaseSolid(Block block)
        {
            //Check if the block is solid and on structure ground level and the block below is not solid
            if (chunk.GetBlock(block.xPos, block.yPos + 1).isSolid == false && block.isSolid == true)
            {
                //Create a new block of the same type and place it below the original block
                Type itemType = block.item.GetType();
                Item newItem = (Item)Activator.CreateInstance(itemType, wndGame, 0);
                chunk.SetBlock(block.xPos, block.yPos + 1, newItem.GenerateBlock(block.xPos, block.yPos + 1, chunk));
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
            if (y != 0) //The game somehow tries to generate structures on y0 at some points. This is to prevent that
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
        }

        public bool checkForCutoff()
        {
            //Check if the structure is cut off on left or right side
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
        public ContinuationStructure(List<StructureComponent> structureList, wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, int remainingWidth, bool canFloat) : base(wndGame, chunk, canFloat)
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
        public AlphaStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
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
        public TreeStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
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
        public OreStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
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

    public class AlphaCave : Structure
    {
        public AlphaCave(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            //Generate first air block (base of cave)
            List<StructureComponent> generatedComponents = new List<StructureComponent>();
            structureComponents.Add(new StructureComponent(wndGame, 0, 0, new AirItem(wndGame, 0).GenerateBlock(x, y, chunk)));


            //Go through all components
            for (int i = 0; i < 6; i++)
            {
                List<StructureComponent> temporaryComponentList = new List<StructureComponent>();

                foreach (StructureComponent structureComponent in structureComponents)
                {
                    int randomNorth = rnd.Next(1, 3);
                    //North
                    if (randomNorth == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset, structureComponent.yOffset - 1, new AirItem(wndGame, 0).GenerateBlock(x, y, chunk));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    int randomEast = rnd.Next(1, 3);
                    //East
                    if (randomEast == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset + 1, structureComponent.yOffset, new AirItem(wndGame, 0).GenerateBlock(x, y, chunk));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);

                        }
                    }

                    int randomSouth = rnd.Next(1, 3);
                    //South
                    if (randomSouth == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset, structureComponent.yOffset + 1, new AirItem(wndGame, 0).GenerateBlock(x, y, chunk));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);

                        }
                    }

                    int randomWest = rnd.Next(1, 3);
                    //West
                    if (randomWest == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset - 1, structureComponent.yOffset, new AirItem(wndGame, 0).GenerateBlock(x, y, chunk));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);

                        }
                    }

                    generatedComponents.Add(structureComponent);
                }

                //Add all the just generated components to the final list
                foreach (StructureComponent structureComponent in temporaryComponentList)
                {
                    structureComponents.Add(structureComponent);
                    Console.WriteLine("Added component " + structureComponent.xOffset + " " + structureComponent.yOffset);
                }
                temporaryComponentList.Clear();
                Console.WriteLine("Cleared List!");

            }

            Console.WriteLine("Structure Components Count: " + structureComponents.Count.ToString());

            //Get width
            List<int> handledX = new List<int>();
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (!handledX.Contains(structureComponent.xOffset))
                {
                    handledX.Add(structureComponent.xOffset);
                }
            }
            totalWidth = handledX.Count;
            Console.WriteLine("Structure total width: " + totalWidth);

            //Begin generating the alpha structure - was only meant for development purposes and is no longer in the game
            BeginGeneration(x, y, index, isNew);
        }

        //I have no idea how I thought this is a good name of a method
        public bool StructureComponentsListContainsStructureComponent(List<StructureComponent> structureComponentList, StructureComponent structureComponent)
        {
            foreach (StructureComponent entry in structureComponentList)
            {
                if (structureComponent.xOffset == entry.xOffset && structureComponent.yOffset == entry.yOffset)
                {
                    return true;
                }
            }
            return false;
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
