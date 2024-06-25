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
        static int o = 0;
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<StructureComponent> cutOffComponents = new List<StructureComponent>();
        public BlockList blockList = new BlockList();
        public Random rnd;
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
        public bool canReplaceSolidBlocks;

        //-- Constructor --//

        public Structure(wndGame wndGame, Chunk chunk, bool canFloat)
        {
            //Set the attributes
            this.chunk = chunk;
            this.wndGame = wndGame;
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
            //Add all blocks from the structure to the blocklist
            foreach (Block block in blockList.blocks)
            {
                if (block != null && block.xPos > 0 && block.xPos < 9 && block.yPos > 0 && block.yPos < 76)
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
            wndGame.log.Write($"Generated structure {GetType()} at x{xBase} y{yBase} with width {totalWidth}, direction {direction}, isCutOff = {isCutOff}, widthRemaining = {widthRemaining}", "Info");

        }

        public void MakeBaseSolid(Block block)
        {
            //Check if the block is solid and on structure ground level and the block below is not solid
            if (chunk.GetBlock(block.xPos, block.yPos + 1).isSolid == false && block.isSolid == true)
            {
                //Create a new block of the same type and place it below the original block
                Type blockType = block.GetType();
                Block newBlock = (Block)Activator.CreateInstance(blockType, wndGame, block.xPos, block.yPos + 1, chunk, null, block.isBackground);
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
            wndGame.log.Write($"Beginning to generate structure {GetType()} at x{xBase} y{yBase}", "Info");
            if (y != 0) //The game somehow tries to generate structures on y0 at some points. This is to prevent that. An actual fix may follow later.
            {
                this.isNew = isNew;
                //Check which direction it's going to be built in
                if (index > 0)
                {
                    wndGame.log.Write("Determined structure generation direction 'right'", "Info");
                    SetupStructure(x, y, "right");
                    GenerateStructure();
                }
                else if (index < 0)
                {
                    wndGame.log.Write("Determined structure generation direction 'left'", "Info");
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
                    wndGame.log.Write("Detected cutoff in structure generation!", "Info");
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
                    wndGame.log.Write("Detected cutoff in structure generation!", "Info");
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

    //-- Structures --//

    public class ContinuationStructure : Structure //This is not a normal structure. Its component list is made up of components that originally belonged to another structure but were cut off. This serves as a continuation.
    {
        public ContinuationStructure(List<StructureComponent> structureList, wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, int remainingWidth, bool canFloat, bool canReplaceSolidBlocks) : base(wndGame, chunk, canFloat)
        {
            totalWidth = remainingWidth;
            this.canReplaceSolidBlocks = canReplaceSolidBlocks;

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

    public class AlphaStructure : Structure //Not currently used, was only in the game for debugging
    {
        public AlphaStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            //Set the total width of the structure
            totalWidth = 3;
            canReplaceSolidBlocks = true;

            //Add all structure components - It's meant to look like a bedrock pyramid
            structureComponents.Add(new StructureComponent(wndGame, 0, 0, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 1, 0, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 0, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 0, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 4, 0, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 1, 1, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 1, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 1, new BedrockBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 2, new BedrockBlock(wndGame, x, y, chunk, null, false)));

            //Begin generating the alpha structure - was only meant for development purposes and is no longer in the game
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class OakTreeStructure : Structure
    {
        public OakTreeStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            canReplaceSolidBlocks = true;

            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(wndGame, 2, 0, new OakLogBlock(wndGame, x, y, chunk, null, true)));

            //Layer 2
            structureComponents.Add(new StructureComponent(wndGame, 2, 1, new OakLogBlock(wndGame, x, y, chunk, null, true)));

            //Layer 3
            structureComponents.Add(new StructureComponent(wndGame, 2, 2, new OakLogBlock(wndGame, x, y, chunk, null, true)));

            //Layer 4
            structureComponents.Add(new StructureComponent(wndGame, 1, 3, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 3, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 3, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 4, 3, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 0, 3, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Layer 5
            structureComponents.Add(new StructureComponent(wndGame, 1, 4, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 4, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 4, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Layer 6
            structureComponents.Add(new StructureComponent(wndGame, 2, 5, new OakLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class SpruceTreeStructure : Structure
    {
        public SpruceTreeStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            canReplaceSolidBlocks = true;

            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(wndGame, 2, 0, new SpruceLogBlock(wndGame, x, y, chunk, null, true)));

            //Layer 2
            structureComponents.Add(new StructureComponent(wndGame, 2, 1, new SpruceLogBlock(wndGame, x, y, chunk, null, true)));

            //Layer 3
            structureComponents.Add(new StructureComponent(wndGame, 2, 2, new SpruceLogBlock(wndGame, x, y, chunk, null, true)));

            //Layer 4
            structureComponents.Add(new StructureComponent(wndGame, 1, 3, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 3, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 3, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 4, 3, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 0, 3, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Layer 5
            structureComponents.Add(new StructureComponent(wndGame, 1, 4, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 4, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 4, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Layer 6
            structureComponents.Add(new StructureComponent(wndGame, 1, 5, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 2, 5, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));
            structureComponents.Add(new StructureComponent(wndGame, 3, 5, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Layer 7
            structureComponents.Add(new StructureComponent(wndGame, 2, 6, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Layer 8
            structureComponents.Add(new StructureComponent(wndGame, 2, 7, new SpruceLeavesBlock(wndGame, x, y, chunk, null, false)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class OreStructure : Structure
    {
        public OreStructure(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            canReplaceSolidBlocks = false;

            //Generate a random number between 0 and 29 to get the ore type
            //WIP - Split into seperate ore structures for getting appropriate heights
            int random1 = rnd.Next(0, 30);

            if (random1 >= 0 && random1 <= 15) //Coal Vein
            {
                //Set the total width
                totalWidth = 4;
                int random2;

                //Layer 1
                structureComponents.Add(new StructureComponent(wndGame, 0, 0, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 0, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 0, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 3, 0, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }

                //Layer 2
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 1, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 1, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 1, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 3, 1, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }

                //Layer 3
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 2, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 2, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 2, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 3, 2, new CoalOreBlock(wndGame, x, y, chunk, null, false)));
                }
            }
            else if (random1 > 15 && random1 <= 25) //Iron Vein
            {
                //Set the total width
                totalWidth = 3;
                int random2;

                //Layer 1
                structureComponents.Add(new StructureComponent(wndGame, 0, 0, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 0, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 0, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }

                //Layer 2
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 1, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 1, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 1, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }

                //Layer 3
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 2, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 2, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 2, 2, new IronOreBlock(wndGame, x, y, chunk, null, false)));
                }
            }
            else if (random1 > 25 && random1 <= 30) //Diamond Vein
            {
                //Set the total width
                totalWidth = 2;
                int random2;

                //Layer 1
                structureComponents.Add(new StructureComponent(wndGame, 0, 0, new DiamondOreBlock(wndGame, x, y, chunk, null, false)));
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 0, new DiamondOreBlock(wndGame, x, y, chunk, null, false)));
                }

                //Layer 2
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 0, 1, new DiamondOreBlock(wndGame, x, y, chunk, null, false)));
                }
                random2 = rnd.Next(0, 2);
                if (random2 == 0)
                {
                    structureComponents.Add(new StructureComponent(wndGame, 1, 1, new DiamondOreBlock(wndGame, x, y, chunk, null, false)));
                }
            }

            BeginGeneration(x, y, index, isNew);

        }
    }

    public class AlphaCave : Structure //This was a test implementation of caves. It works partially, but has many issues and doesn't look good. Not used anymore.
    {
        public AlphaCave(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            //Generate first air block (base of cave)
            List<StructureComponent> generatedComponents = new List<StructureComponent>();
            structureComponents.Add(new StructureComponent(wndGame, 0, 0, new AirBlock(wndGame, x, y, chunk, null, false)));


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
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset, structureComponent.yOffset - 1, new AirBlock(wndGame, x, y, chunk, null, false));
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
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset + 1, structureComponent.yOffset, new AirBlock(wndGame, x, y, chunk, null, false));
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
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset, structureComponent.yOffset + 1, new AirBlock(wndGame, x, y, chunk, null, false));
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
                        StructureComponent newComponent = new StructureComponent(wndGame, structureComponent.xOffset - 1, structureComponent.yOffset, new AirBlock(wndGame, x, y, chunk, null, false));
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
                }
                temporaryComponentList.Clear();
            }

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

            //Begin generating the cave
            BeginGeneration(x, y, index, isNew);
        }

        //I have no idea how I thought this is a good name for a method
        public bool StructureComponentsListContainsStructureComponent(List<StructureComponent> structureComponentList, StructureComponent structureComponent)
        {
            //Check if the structure component list already contains a structure component with the same coordinates
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

    //Components that tell the structure which blocks to place at which position
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
