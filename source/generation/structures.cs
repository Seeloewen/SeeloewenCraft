using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    //-- Structures --//

    public class ContinuationStructure : Structure //This is not a normal structure. Its component list is made up of components that originally belonged to another structure but were cut off. This serves as a continuation.
    {
        public ContinuationStructure(List<StructureComponent> structureComponentList, World world, int x, int y, int index, bool isNew, Chunk chunk, int remainingWidth, bool canFloat, bool canReplaceSolidBlocks, string name) : base(world, chunk, canFloat)
        {
            totalWidth = remainingWidth;
            this.canReplaceSolidBlocks = canReplaceSolidBlocks;
            id = "sc:continuation_structure";
            this.name = name;

            //Add all structure components
            foreach (StructureComponent structureComponent in structureComponentList)
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
        public AlphaStructure(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(world, chunk, canFloat)
        {
            id = "sc:legacy_alpha_structure";
            name = "Legacy Alpha Structure";

            //Set the total width of the structure
            totalWidth = 3;
            canReplaceSolidBlocks = true;

            //Add all structure components - It's meant to look like a bedrock pyramid
            structureComponents.Add(new StructureComponent(world, 0, 0, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 1, 0, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 0, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 0, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 4, 0, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 1, 1, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 1, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 1, new BedrockBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 2, new BedrockBlock(world, false)));

            //Begin generating the alpha structure - was only meant for development purposes and is no longer in the game
            BeginGeneration(x, y, index, isNew);
        }
    }


    public class Lake : Structure
    {
        public Lake(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat, int floorHeight) : base(world, chunk, canFloat)
        {
            id = "sc:lake";
            name = "Lake";

            //Set the total width of the structure
            canReplaceSolidBlocks = true;

            //Add all structure components
            int xPos = 0;
            int yPos = 0;

            bool goDown = true;

            yPos = floorHeight;

            do
            {
                //Generate from bottom to floorheight
                for (int i = 0; i < floorHeight; i++)
                {
                    //Add dirt when below the currently observed ypos, else add water
                    if (i < yPos && i >= yPos - 2)
                    {
                        structureComponents.Add(new StructureComponent(world, xPos, i, new DirtBlock(world, false)));
                    }
                    else if (i >= yPos)
                    {
                        structureComponents.Add(new StructureComponent(world, xPos, i, new WaterBlock_6(world, false)));
                    }
                }

                //Generate a mirror of the lake above, but with air to clear potential blocks above
                for (int i = floorHeight; i <= floorHeight + floorHeight - yPos; i++)
                {
                    structureComponents.Add(new StructureComponent(world, xPos, i, new AirBlock(world, false)));
                }


                //Determine whether to go down or up
                if (yPos == 1)
                {
                    goDown = false;
                }

                //Roll for a 50% chance to go down/up or stay on the height
                if (xPos == 0)
                {
                    yPos--;
                }
                else if (goDown)
                {
                    yPos = (rnd.Next(0, 2) == 0) ? yPos - 1 : yPos;
                }
                else
                {
                    yPos = (rnd.Next(0, 2) == 0) ? yPos + 1 : yPos;
                }

                xPos++;
            }

            while (yPos <= floorHeight);

            totalWidth = GetTotalWidth();

            //Begin generation
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class OakTreeStructure : Structure
    {
        public OakTreeStructure(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(world, chunk, canFloat)
        {
            id = "sc:oak_tree_structure";
            name = "Oak Tree";

            canReplaceSolidBlocks = true;

            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(world, 2, 0, new OakLogBlock(world, true)));

            //Layer 2
            structureComponents.Add(new StructureComponent(world, 2, 1, new OakLogBlock(world, true)));

            //Layer 3
            structureComponents.Add(new StructureComponent(world, 2, 2, new OakLogBlock(world, true)));

            //Layer 4
            structureComponents.Add(new StructureComponent(world, 1, 3, new OakLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 3, new OakLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 3, new OakLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 4, 3, new OakLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 0, 3, new OakLeavesBlock(world, false)));

            //Layer 5
            structureComponents.Add(new StructureComponent(world, 1, 4, new OakLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 4, new OakLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 4, new OakLeavesBlock(world, false)));

            //Layer 6
            structureComponents.Add(new StructureComponent(world, 2, 5, new OakLeavesBlock(world, false)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class SpruceTreeStructure : Structure
    {
        public SpruceTreeStructure(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(world, chunk, canFloat)
        {
            id = "sc:spruce_tree_structure";
            name = "Spruce Tree";

            canReplaceSolidBlocks = true;

            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(world, 2, 0, new SpruceLogBlock(world, true)));

            //Layer 2
            structureComponents.Add(new StructureComponent(world, 2, 1, new SpruceLogBlock(world, true)));

            //Layer 3
            structureComponents.Add(new StructureComponent(world, 2, 2, new SpruceLogBlock(world, true)));

            //Layer 4
            structureComponents.Add(new StructureComponent(world, 1, 3, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 3, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 3, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 4, 3, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 0, 3, new SpruceLeavesBlock(world, false)));

            //Layer 5
            structureComponents.Add(new StructureComponent(world, 1, 4, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 4, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 4, new SpruceLeavesBlock(world, false)));

            //Layer 6
            structureComponents.Add(new StructureComponent(world, 1, 5, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 2, 5, new SpruceLeavesBlock(world, false)));
            structureComponents.Add(new StructureComponent(world, 3, 5, new SpruceLeavesBlock(world, false)));

            //Layer 7
            structureComponents.Add(new StructureComponent(world, 2, 6, new SpruceLeavesBlock(world, false)));

            //Layer 8
            structureComponents.Add(new StructureComponent(world, 2, 7, new SpruceLeavesBlock(world, false)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class OreStructure : Structure
    {
        public OreStructure(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(world, chunk, canFloat)
        {
            id = "sc:legacy_ore_structure";
            name = "Legacy Ore Structure";

            canReplaceSolidBlocks = false;

            //Generate a random number between 0 and 29 to get the ore type
            //WIP - Split into seperate ore structures for getting appropriate heights
            int random1 = rnd.Next(0, 31);

            if (random1 >= 0 && random1 <= 15) //Coal Vein
            {
                //Set the total width
                totalWidth = 4;

                //Generate ores
                for (int i = 0; i < 3; i++) //Y
                {
                    for (int j = 0; j < 4; j++) //X
                    {
                        if (i == 0 && j == 0)
                        {
                            structureComponents.Add(new StructureComponent(world, 0, 0, new CoalOreBlock(world, false)));
                        }
                        else
                        {
                            GetCoalOre(world, j, i);
                        }
                    }
                }
            }
            else if (random1 > 15 && random1 <= 25) //Iron Vein
            {
                //Set the total width
                totalWidth = 3;

                //Generate ores
                for (int i = 0; i < 3; i++) //Y
                {
                    for (int j = 0; j < 3; j++) //X
                    {
                        if (i == 0 && j == 0)
                        {
                            structureComponents.Add(new StructureComponent(world, 0, 0, new IronOreBlock(world, false)));
                        }
                        else
                        {
                            GetIronOre(world, j, i);
                        }
                    }
                }
            }
            else if (random1 > 25 && random1 <= 30) //Diamond Vein
            {
                //Set the total width
                totalWidth = 2;

                //Generate ores
                for (int i = 0; i < 2; i++) //Y
                {
                    for (int j = 0; j < 2; j++) //X
                    {
                        if (i == 0 && j == 0)
                        {
                            structureComponents.Add(new StructureComponent(world, 0, 0, new DiamondOreBlock(world, false)));
                        }
                        else
                        {
                            GetDiamondOre(world, j, i);
                        }
                    }
                }
            }

            BeginGeneration(x, y, index, isNew);

        }

        public void GetCoalOre(World world, int x, int y)
        {
            //50% chance to add a new ore block
            if (rnd.Next(0, 2) == 0)
            {
                structureComponents.Add(new StructureComponent(world, x, y, new CoalOreBlock(world, false)));
            }
        }

        public void GetIronOre(World world, int x, int y)
        {
            //50% chance to add a new ore block
            if (rnd.Next(0, 2) == 0)
            {
                structureComponents.Add(new StructureComponent(world, x, y, new IronOreBlock(world, false)));
            }
        }

        public void GetDiamondOre(World world, int x, int y)
        {
            //50% chance to add a new ore block
            if (rnd.Next(0, 2) == 0)
            {
                structureComponents.Add(new StructureComponent(world, x, y, new DiamondOreBlock(world, false)));
            }
        }
    }

    public class AlphaCave : Structure //This was a test implementation of caves. It works partially, but has many issues and doesn't look good. Not used anymore.
    {
        public AlphaCave(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(world, chunk, canFloat)
        {

            id = "sc:legacy_alpha_cave_structure";
            name = "Legacy Alpha Cave";

            //Generate first air block (base of cave)
            List<StructureComponent> generatedComponents = new List<StructureComponent>();
            structureComponents.Add(new StructureComponent(world, 0, 0, new AirBlock(world, false)));

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
                        StructureComponent newComponent = new StructureComponent(world, structureComponent.xOffset, structureComponent.yOffset - 1, new AirBlock(world, false));
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
                        StructureComponent newComponent = new StructureComponent(world, structureComponent.xOffset + 1, structureComponent.yOffset, new AirBlock(world, false));
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
                        StructureComponent newComponent = new StructureComponent(world, structureComponent.xOffset, structureComponent.yOffset + 1, new AirBlock(world, false));
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
                        StructureComponent newComponent = new StructureComponent(world, structureComponent.xOffset - 1, structureComponent.yOffset, new AirBlock(world, false));
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

            //Get total width
            totalWidth = GetTotalWidth();

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
}
