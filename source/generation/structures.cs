using System.Collections.Generic;

namespace SeeloewenCraft
{
    //-- Structures --//

    public class ContinuationStructure : Structure //This is not a normal structure. Its component list is made up of components that originally belonged to another structure but were cut off. This serves as a continuation.
    {
        public ContinuationStructure(List<StructureComponent> structureComponentList, int x, int y, int index, bool isNew, Chunk chunk, int remainingWidth, bool canFloat, bool canReplaceSolidBlocks, string name) : base(chunk, canFloat)
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
        public AlphaStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:legacy_alpha_structure";
            name = "Legacy Alpha Structure";

            //Set the total width of the structure
            totalWidth = 3;
            canReplaceSolidBlocks = true;

            //Add all structure components - It's meant to look like a bedrock pyramid
            structureComponents.Add(new StructureComponent(0, 0, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(1, 0, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(2, 0, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(3, 0, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(4, 0, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(1, 1, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(2, 1, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(3, 1, new BedrockBlock(false)));
            structureComponents.Add(new StructureComponent(2, 2, new BedrockBlock(false)));

            //Begin generating the alpha structure - was only meant for development purposes and is no longer in the game
            BeginGeneration(x, y, index, isNew);
        }
    }


    public class PlainsDungeon : Structure
    {
        public PlainsDungeon(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:plains_dungeon";
            name = "Plains Dungeon";

            //Set the total width of the structure
            canReplaceSolidBlocks = true;

            Dungeon dung = new Dungeon();
            dung.CreateDungeon(100, 50, DungeonType.Plains);
            structureComponents.AddRange(dung.GenerateDungeon(0, 0));

            totalWidth = GetTotalWidth();

            //Begin generation
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class Lake : Structure
    {
        public Lake(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat, int floorHeight) : base(chunk, canFloat)
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
                        structureComponents.Add(new StructureComponent(xPos, i, new DirtBlock(false)));
                    }
                    else if (i >= yPos)
                    {
                        structureComponents.Add(new StructureComponent(xPos, i, new WaterBlock_6(false)));
                    }
                }

                //Generate a mirror of the lake above, but with air to clear potential blocks above
                for (int i = floorHeight; i <= floorHeight + floorHeight - yPos + 2; i++)
                {
                    structureComponents.Add(new StructureComponent(xPos, i, new AirBlock(false)));
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
        public OakTreeStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:oak_tree_structure";
            name = "Oak Tree";

            canReplaceSolidBlocks = true;

            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(2, 0, new OakLogBlock(true)));

            //Layer 2
            structureComponents.Add(new StructureComponent(2, 1, new OakLogBlock(true)));

            //Layer 3
            structureComponents.Add(new StructureComponent(2, 2, new OakLogBlock(true)));

            //Layer 4
            structureComponents.Add(new StructureComponent(1, 3, new OakLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(2, 3, new OakLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(3, 3, new OakLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(4, 3, new OakLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(0, 3, new OakLeavesBlock(false)));

            //Layer 5
            structureComponents.Add(new StructureComponent(1, 4, new OakLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(2, 4, new OakLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(3, 4, new OakLeavesBlock(false)));

            //Layer 6
            structureComponents.Add(new StructureComponent(2, 5, new OakLeavesBlock(false)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class PyramidStructure : Structure
    {
        public PyramidStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:pyramid_structure";
            name = "Pyramid";

            canReplaceSolidBlocks = true;

            //Set total width of the structure

            //Layer 1
            int height = 8;
            int width = height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    structureComponents.Add(new StructureComponent(width + i, j, new SandStoneBricksBlock(false)));
                    structureComponents.Add(new StructureComponent(width - i, j, new SandStoneBricksBlock(false)));
                }
                height--;
            }

            //Left pot
            ArcheologyPot_Base potBase = new ArcheologyPot_Base(false);
            ArcheologyPot_Top potTop = new ArcheologyPot_Top(false);
            potTop.baseBlock = (0, 1);
            AddBackgroundBlock(new SandStoneBlock(true), 6, 1, potBase, LootTables.potLootTable, 1);
            AddBackgroundBlock(new SandStoneBlock(true), 6, 2, potTop);

            //Chest
            AddBackgroundBlock(new SandStoneBlock(true), 8, 1, new ChestBlock(false), LootTables.pyramidLootTable, 3);

            //Torch
            AddBackgroundBlock(new SandStoneBlock(true), 8, 3, new TorchBlock(false));

            //Right pot
            ArcheologyPot_Base potBase2 = new ArcheologyPot_Base(false);
            ArcheologyPot_Top potTop2 = new ArcheologyPot_Top(false);
            potTop2.baseBlock = (0, 1);
            AddBackgroundBlock(new SandStoneBlock(true), 10, 1, potBase2, LootTables.potLootTable, 1);
            AddBackgroundBlock(new SandStoneBlock(true), 10, 2, potTop2);

            //Other background blocks
            structureComponents.Add(new StructureComponent(7, 1, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(7, 2, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(6, 3, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(7, 3, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(8, 2, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(9, 3, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(9, 1, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(9, 2, new SandStoneBlock(true)));
            structureComponents.Add(new StructureComponent(10, 3, new SandStoneBlock(true)));


            //Begin generating
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class SpruceTreeStructure : Structure
    {
        public SpruceTreeStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:spruce_tree_structure";
            name = "Spruce Tree";

            canReplaceSolidBlocks = true;

            //Set total width of the structure
            totalWidth = 5;

            //Layer 1
            structureComponents.Add(new StructureComponent(2, 0, new SpruceLogBlock(true)));

            //Layer 2
            structureComponents.Add(new StructureComponent(2, 1, new SpruceLogBlock(true)));

            //Layer 3
            structureComponents.Add(new StructureComponent(2, 2, new SpruceLogBlock(true)));

            //Layer 4
            structureComponents.Add(new StructureComponent(1, 3, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(2, 3, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(3, 3, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(4, 3, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(0, 3, new SpruceLeavesBlock(false)));

            //Layer 5
            structureComponents.Add(new StructureComponent(1, 4, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(2, 4, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(3, 4, new SpruceLeavesBlock(false)));

            //Layer 6
            structureComponents.Add(new StructureComponent(1, 5, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(2, 5, new SpruceLeavesBlock(false)));
            structureComponents.Add(new StructureComponent(3, 5, new SpruceLeavesBlock(false)));

            //Layer 7
            structureComponents.Add(new StructureComponent(2, 6, new SpruceLeavesBlock(false)));

            //Layer 8
            structureComponents.Add(new StructureComponent(2, 7, new SpruceLeavesBlock(false)));

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class LegacyOreStructure : Structure //Legacy ore structure used before Alpha 1.2.0
    {
        public LegacyOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:legacy_ore_structure";
            name = "Legacy Ore Structure";

            canReplaceSolidBlocks = false;

            //Generate a random number between 0 and 29 to get the ore type
            int random1 = rnd.Next(0, 31);

            if (random1 >= 0 && random1 <= 15) //Coal Vein
            {
                foreach (StructureComponent com in shapeCreator.GetCustomCircle(3, 2))
                {
                    com.block = new CoalOreBlock(false);
                    structureComponents.Add(com);
                }
                if (rnd.Next(1, 11) > 2)
                {
                    StructureComponent ranCom = structureComponents[rnd.Next(0, structureComponents.Count)];
                    foreach (StructureComponent com in shapeCreator.GetCustomCircle(3, 2))
                    {
                        com.xOffset += ranCom.xOffset;
                        com.yOffset += ranCom.yOffset;
                        com.block = new CoalOreBlock(false);
                        structureComponents.Add(com);
                    }
                }
            }
            else if (random1 > 15 && random1 <= 25) //Iron Vein
            {
                foreach (StructureComponent com in shapeCreator.GetCustomCircle(2, 1))
                {
                    com.block = new IronOreBlock(false);
                    structureComponents.Add(com);
                }
                if (rnd.Next(1, 11) > 4)
                {
                    StructureComponent ranCom = structureComponents[rnd.Next(0, structureComponents.Count)];
                    foreach (StructureComponent com in shapeCreator.GetCustomCircle(2, 1))
                    {
                        com.xOffset += ranCom.xOffset;
                        com.yOffset += ranCom.yOffset;
                        com.block = new IronOreBlock(false);
                        structureComponents.Add(com);
                    }
                }
            }
            else if (random1 > 25 && random1 <= 30) //Diamond Vein
            {
                foreach (StructureComponent com in shapeCreator.GetCustomCircle(1, 1))
                {
                    com.block = new DiamondOreBlock(false);
                    structureComponents.Add(com);
                }
                if (rnd.Next(1, 11) > 9)
                {
                    StructureComponent ranCom = structureComponents[rnd.Next(0, structureComponents.Count)];
                    foreach (StructureComponent com in shapeCreator.GetCustomCircle(2, 1))
                    {
                        com.xOffset += ranCom.xOffset;
                        com.yOffset += ranCom.yOffset;
                        com.block = new DiamondOreBlock(false);
                        structureComponents.Add(com);
                    }
                }
            }
            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);

        }
    }

    public class CoalOreStructure : Structure
    {
        public CoalOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:coal_ore_structure";
            name = "Coal Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
            {
                structComp.block = new CoalOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new CoalOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class IronOreStructure : Structure
    {
        public IronOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:iron_ore_structure";
            name = "Iron Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
            {
                structComp.block = new IronOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 2))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new IronOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class DiamondOreStructure : Structure
    {
        public DiamondOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:diamond_ore_structure";
            name = "Diamond Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(1, 1))
            {
                structComp.block = new DiamondOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new DiamondOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class AmethystOreStructure : Structure
    {
        public AmethystOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:amethyst_ore_structure";
            name = "Amethyst Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(1, 1))
            {
                structComp.block = new AmethystOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 5) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new AmethystOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class CopperOreStructure : Structure
    {
        public CopperOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:copper_ore_structure";
            name = "Copper Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
            {
                structComp.block = new CopperOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new CopperOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class GoldOreStructure : Structure
    {
        public GoldOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:gold_ore_structure";
            name = "Gold Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
            {
                structComp.block = new GoldOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new GoldOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class EmeraldOreStructure : Structure
    {
        public EmeraldOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:emerald_ore_structure";
            name = "Emerald Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(1, 2))
            {
                structComp.block = new EmeraldOreBlock(false);
                structureComponents.Add(structComp);
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class TinOreStructure : Structure
    {
        public TinOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:tin_ore_structure";
            name = "Tin Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
            {
                structComp.block = new TinOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 2))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new TinOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class TungstenOreStructure : Structure
    {
        public TungstenOreStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:tungsten_ore_structure";
            name = "Tungsten Ore Vein";
            canReplaceSolidBlocks = false;

            //Generate the first part of the vein
            foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
            {
                structComp.block = new TungstenOreBlock(false);
                structureComponents.Add(structComp);
            }
            if (rnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent rndStructComp = structureComponents[rnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += rndStructComp.xOffset;
                    structComp.yOffset += rndStructComp.yOffset;
                    structComp.block = new TungstenOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

            //Get total width
            totalWidth = GetTotalWidth();
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class AlphaCave : Structure //This was a test implementation of caves. It works partially, but has many issues and doesn't look good. Not used anymore.
    {
        public AlphaCave(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {

            id = "sc:legacy_alpha_cave_structure";
            name = "Legacy Alpha Cave";

            //Generate first air block (base of cave)
            List<StructureComponent> generatedComponents = new List<StructureComponent>();
            structureComponents.Add(new StructureComponent(0, 0, new AirBlock(false)));

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
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset, structureComponent.yOffset - 1, new AirBlock(false));
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
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset + 1, structureComponent.yOffset, new AirBlock(false));
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
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset, structureComponent.yOffset + 1, new AirBlock(false));
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
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset - 1, structureComponent.yOffset, new AirBlock(false));
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
