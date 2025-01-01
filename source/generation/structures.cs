using System.Collections.Generic;

namespace SeeloewenCraft
{
    //-- Structures --//

    public class ContinuationStructure : Structure //This is not a normal structure. Its component list is made up of components that originally belonged to another structure but were cut off. This serves as a continuation.
    {
        public ContinuationStructure(List<StructureComponent> structureComponentList, int x, int y, int index, bool isNew, Chunk chunk, int remainingWidth, bool canFloat, bool canReplaceSolidBlocks, string name) : base(chunk, canFloat)
        {
            this.canReplaceSolidBlocks = canReplaceSolidBlocks;
            this.name = name;
            id = "sc:continuation_structure";

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
            canReplaceSolidBlocks = true;

            //Add all structure components - It's meant to look like a bedrock pyramid
            AddBlock(new BedrockBlock(false), 0, 0);
            AddBlock(new BedrockBlock(false), 1, 0);
            AddBlock(new BedrockBlock(false), 2, 0);
            AddBlock(new BedrockBlock(false), 3, 0);
            AddBlock(new BedrockBlock(false), 4, 0);
            AddBlock(new BedrockBlock(false), 1, 1);
            AddBlock(new BedrockBlock(false), 2, 1);
            AddBlock(new BedrockBlock(false), 3, 1);
            AddBlock(new BedrockBlock(false), 2, 2);

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
            canReplaceSolidBlocks = true;

            Dungeon dung = new Dungeon();
            dung.CreateDungeon(chunk.seededRnd, 100, 50, DungeonType.Plains);
            structureComponents.AddRange(dung.GenerateDungeon(0, 0));

            //Begin generation
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class FossilStructure : Structure
    {
        public FossilStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:fossil_structure";
            name = "Fossil";
            canReplaceSolidBlocks = true;

            AddBlock(new BoneBlock(false), 0, 0);
            AddBlock(new BoneBlock(false), 0, 1);
            AddBlock(new BoneBlock(false), 0, 2);
            AddBlock(new BoneBlock(false), 1, 3);
            AddBlock(new BoneBlock(false), 2, 0);
            AddBlock(new BoneBlock(false), 2, 1);
            AddBlock(new BoneBlock(false), 2, 2);
            AddBlock(new BoneBlock(false), 3, 3);
            AddBlock(new BoneBlock(false), 4, 0);
            AddBlock(new BoneBlock(false), 4, 1);
            AddBlock(new BoneBlock(false), 4, 2);

            //Begin generation
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class AbandonedFarm : Structure
    {
        public AbandonedFarm(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:abandoned_farm";
            name = "Abandoned Farm";
            canReplaceSolidBlocks = true;

            //Frame
            #region Frame
            AddBlock(new OakPlanksBlock(false), 2, 0);
            AddBlock(new OakPlanksBlock(false), 3, 0);
            AddBlock(new OakPlanksBlock(false), 4, 0);
            AddBlock(new OakPlanksBlock(false), 5, 0);
            AddBlock(new OakPlanksBlock(false), 6, 0);
            AddBlock(new OakPlanksBlock(false), 7, 0);
            AddBlock(new OakLogBlock(false), 1, 0);
            AddBlock(new OakLogBlock(false), 7, 0);
            AddBlock(new OakLogBlock(false), 1, 1);
            AddBlock(new OakLogBlock(false), 1, 2);
            AddBlock(new OakLogBlock(false), 1, 3);
            AddBlock(new OakLogBlock(false), 1, 4);
            AddBlock(new OakLogBlock(false), 1, 5);
            AddBlock(new OakLogBlock(false), 7, 3);
            AddBlock(new OakLogBlock(false), 7, 4);
            AddBlock(new OakLogBlock(false), 7, 5);
            AddBlock(new OakLogBlock(false), 2, 5);
            AddBlock(new OakLogBlock(false), 3, 5);
            AddBlock(new OakLogBlock(false), 4, 5);
            AddBlock(new OakLogBlock(false), 5, 5);
            AddBlock(new OakLogBlock(false), 6, 5);
            AddBlock(new OakLogBlock(false), 7, 5);

            //OakDoor_Base door = new DoorBlock(false);
            AddBackgroundBlock(new OakLogBlock(false), 7, 2, new OakDoor_Top(false) { baseBlock = (0, 1) });
            AddBackgroundBlock(new OakLogBlock(false), 7, 1, new OakDoor_Base(false) { baseBlock = (0, -1) });
            #endregion

            //Background
            #region Background
            for (int i = 2; i < 7; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    if (i == 3 && (j == 2 || j == 3) || i == 5 && (j == 2 || j == 3)) AddBackgroundBlock(new GlassBlock(false), i, j, null); //Windows
                    else if ((i == 2 || i == 4 || i == 6) && j == 2) AddBackgroundBlock(new OakPlanksBlock(false), i, j, new TorchBlock(false)); //Torch
                    else if (i == 3 && j == 1) AddBackgroundBlock(new OakPlanksBlock(false), i, j, new CraftingTableBlock(false)); //Crafting Table
                    else AddBackgroundBlock(new OakPlanksBlock(false), i, j, null); //Wood Background
                }
            }
            #endregion

            //Roof
            #region Roof
            AddBlock(new CobblestoneBlock_StairBottomRight(false), 0, 5);
            AddBlock(new CobblestoneBlock_StairBottomRight(false), 1, 6);
            AddBlock(new CobblestoneBlock_StairBottomRight(false), 2, 7);
            AddBlock(new CobblestoneBlock_StairBottomRight(false), 3, 8);
            AddBlock(new CobblestoneBlock_SlabBottom(false), 4, 9);
            AddBlock(new CobblestoneBlock_StairBottomLeft(false), 5, 8);
            AddBlock(new CobblestoneBlock_StairBottomLeft(false), 6, 7);
            AddBlock(new CobblestoneBlock_StairBottomLeft(false), 7, 6);
            AddBlock(new CobblestoneBlock_StairBottomLeft(false), 8, 5);
            AddBlock(new CobblestoneBlock(false), 2, 6);
            AddBlock(new CobblestoneBlock(false), 3, 6);
            AddBlock(new CobblestoneBlock(false), 3, 7);
            AddBlock(new CobblestoneBlock(false), 4, 6);
            AddBlock(new CobblestoneBlock(false), 4, 7);
            AddBlock(new CobblestoneBlock(false), 4, 8);
            AddBlock(new CobblestoneBlock(false), 5, 6);
            AddBlock(new CobblestoneBlock(false), 5, 7);
            AddBlock(new CobblestoneBlock(false), 6, 6);
            #endregion

            //Field
            #region Field
            for (int i = 8; i < 15; i++)
            {
                if (i != 11)
                {
                    AddBlock(new DirtBlock(false), i, -1);
                    AddBlock(new DirtBlock(false), i, -2);
                    AddBlock(new FarmlandBlock(false), i, 0);
                    AddBlock(new AirBlock(false), i, 2);
                    AddBlock(new AirBlock(false), i, 3);
                    AddBlock(GetRandomCrop(), i, 1);
                }
                else
                {
                    AddBlock(new DirtBlock(false), i, -1);
                    AddBlock(new DirtBlock(false), i, -2);
                    AddBlock(new AirBlock(false), i, 1);
                    AddBlock(new AirBlock(false), i, 2);
                    AddBlock(new AirBlock(false), i, 3);
                    AddBlock(new WaterBlock_6(false), i, 0);
                }
            }
            AddBlock(new OakLogBlock(false), 15, 0);
            AddBackgroundBlock(new OakLogBlock(false), 15, 1, null);
            AddBackgroundBlock(new OakLogBlock(false), 15, 2, null);
            AddBlock(new LanternBlock(false), 15, 3);
            #endregion

            //Begin generation
            BeginGeneration(x, y, index, isNew);
        }

        public Block GetRandomCrop()
        {
            return structRnd.Next(0, 7) switch
            {
                0 => new PotatoCropBlock(false) { progress = 10000000 },
                1 => new CarrotCropBlock(false) { progress = 10000000 },
                2 => new WheatCropBlock(false) { progress = 10000000 },
                3 => new PumpkinCropBlock(false) { progress = 10000000 },
                4 => new CabbageCropBlock(false) { progress = 10000000 },
                5 => new TomatoCropBlock(false) { progress = 10000000 },
                6 => new CucumberCropBlock(false) { progress = 10000000 },
                _ => new Grass(false)
            };
        }
    }

    public class CottonFieldStructure : Structure
    {
        public CottonFieldStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:cotton_field_structure";
            name = "Cotton Field";
            canReplaceSolidBlocks = true;

            int appends = structRnd.Next(2, 6);

            //Starter pillar
            AddBlock(new SpruceLogBlock(false), 0, -1);
            AddBlock(new SpruceLogBlock(false), 0, 0);
            AddBlock(new SpruceLogBlock(true), 0, 1);
            AddBlock(new SpruceLogBlock(true), 0, 2);
            AddBlock(new LanternBlock(true), 0, 3);

            //Starter pillar
            AddBlock(new SpruceLogBlock(false), 0, 0);
            AddBlock(new SpruceLogBlock(false), 0, 1);
            AddBlock(new SpruceLogBlock(true), 0, 2);
            AddBlock(new SpruceLogBlock(true), 0, 3);
            AddBlock(new LanternBlock(true), 0, 4);

            //First field
            for (int i = 0; i < 3; i++)
            {
                AddBlock(new SpruceLogBlock(false), i + 1, 0);
                AddBlock(new FarmlandBlock(false), i + 1, 1);
                AddBlock(new CottonCropBlock(false) { progress = 10000000 }, i + 1, 2);
                AddBlock(new AirBlock(false), i + 1, 3);
                AddBlock(new AirBlock(false), i + 1, 4);
            }

            //Appends 
            int offset = 4;
            for (int i = 0; i < appends; i++)
            {
                //Field seperator
                AddBlock(new SpruceLogBlock(false), offset, 0);
                AddBlock(new SpruceLogBlock(false), offset + 1, 0);
                AddBlock(new SpruceLogBlock(false), offset + 2, 0);
                AddBlock(new SpruceLogBlock(false), offset, 1);
                AddBlock(new SpruceLogBlock(true), offset, 2);
                AddBlock(new SpruceLogBlock(true), offset, 3);
                AddBlock(new LanternBlock(true), offset, 4);
                AddBlock(new WaterBlock_6(false), offset + 1, 1);
                AddBlock(new SprucePlanksBlock_SlabBottom(false), offset + 1, 2);
                AddBlock(new AirBlock(false), offset + 1, 3);
                AddBlock(new AirBlock(false), offset + 1, 4);
                AddBlock(new SpruceLogBlock(false), offset + 2, 1);
                AddBlock(new SpruceLogBlock(true), offset + 2, 2);
                AddBlock(new SpruceLogBlock(true), offset + 2, 3);
                AddBlock(new LanternBlock(true), offset + 2, 4);

                //Field
                for (int j = 0; j < 4; j++)
                {
                    AddBlock(new SpruceLogBlock(false), j + offset + 3, 0);
                    AddBlock(new FarmlandBlock(false), j + offset + 3, 1);
                    AddBlock(new CottonCropBlock(false) { progress = 10000000 }, j + offset + 3, 2);
                    AddBlock(new AirBlock(false), j + offset + 3, 3);
                    AddBlock(new AirBlock(false), j + offset + 3, 4);
                }

                offset += 7;
            }

            //End pillar
            AddBlock(new SpruceLogBlock(false), offset - 1, 0);
            AddBlock(new SpruceLogBlock(false), offset - 1, 1);
            AddBlock(new SpruceLogBlock(true), offset - 1, 2);
            AddBlock(new SpruceLogBlock(true), offset - 1, 3);
            AddBlock(new LanternBlock(true), offset - 1, 4);

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
                        AddBlock(new DirtBlock(false), xPos, i);
                    }
                    else if (i >= yPos)
                    {
                        AddBlock(new WaterBlock_6(false), xPos, i);
                    }
                }

                //Generate a mirror of the lake above, but with air to clear potential blocks above
                for (int i = floorHeight; i <= floorHeight + floorHeight - yPos + 2; i++)
                {
                    AddBlock(new AirBlock(false), xPos, i);
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
                    yPos = (structRnd.Next(0, 2) == 0) ? yPos - 1 : yPos;
                }
                else
                {
                    yPos = (structRnd.Next(0, 2) == 0) ? yPos + 1 : yPos;
                }

                xPos++;
            }

            while (yPos <= floorHeight);

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

            //Layer 1
            AddBlock(new OakLogBlock(true), 2, 0);

            //Layer 2
            AddBlock(new OakLogBlock(true), 2, 1);

            //Layer 3
            AddBlock(new OakLogBlock(true), 2, 2);

            //Layer 4
            AddBlock(new OakLeavesBlock(false), 1, 3);
            AddBlock(new OakLeavesBlock(false), 2, 3);
            AddBlock(new OakLeavesBlock(false), 3, 3);
            AddBlock(new OakLeavesBlock(false), 4, 3);
            AddBlock(new OakLeavesBlock(false), 0, 3);

            //Layer 5
            AddBlock(new OakLeavesBlock(false), 1, 4);
            AddBlock(new OakLeavesBlock(false), 2, 4);
            AddBlock(new OakLeavesBlock(false), 3, 4);

            //Layer 6
            AddBlock(new OakLeavesBlock(false), 2, 5);

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);
        }
    }

    public class CactusStructure : Structure
    {
        public CactusStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:cactus_structure";
            name = "Cactus";
            canReplaceSolidBlocks = true;

            int height = structRnd.Next(2, 6);
            bool leftBranchStarted = false;
            bool rightBranchStarted = false;

            for (int i = 0; i < height; i++)
            {
                if (i > 0 && i != height)
                {
                    //Potentially generate a new branch
                    switch (structRnd.Next(0, 6))
                    {
                        case 0:
                            if (!leftBranchStarted)
                            {
                                leftBranchStarted = true;

                                //Generate left branch
                                GenerateLeftBranch(i);
                            }
                            else
                            {
                                AddBlock(new Cactus_Vertical(false), 1, i);
                            }
                            break;
                        case 1:
                            if (!rightBranchStarted)
                            {
                                rightBranchStarted = true;

                                //Generate right branch
                                GenerateRightBranch(i);
                            }
                            else
                            {
                                AddBlock(new Cactus_Vertical(false), 1, i);
                            }
                            break;
                        case 2:
                            if (!rightBranchStarted && !leftBranchStarted)
                            {
                                rightBranchStarted = true;
                                leftBranchStarted = true;

                                //Generate right and left branch
                                GenerateRightBranch(i);
                                GenerateLeftBranch(i);
                                AddBlock(new Cactus_Cross(false), 1, i);
                            }
                            else
                            {
                                AddBlock(new Cactus_Vertical(false), 1, i);
                            }
                            break;
                        case > 2:
                            AddBlock(new Cactus_Vertical(false), 1, i);
                            break;
                    }
                }
                else
                {
                    AddBlock(new Cactus_Vertical(false), 1, i);
                }

                AddBlock(structRnd.Next(1, 4) == 1 ? new Cactus_TopFruit(false) : new Cactus_Top(false), 1, height);

                //Begin generating
                BeginGeneration(x, y, index, isNew);
            }

        }

        public void GenerateRightBranch(int startHeight)
        {
            //Generate start of branch
            AddBlock(new Cactus_TopLeft(false), 2, startHeight);
            AddBlock(new Cactus_Right(false), 1, startHeight);

            //Generate the branch upwards
            int rightHeight = structRnd.Next(1, 4);
            for (int j = startHeight + 1; j < startHeight + rightHeight; j++)
            {
                AddBlock(new Cactus_Vertical(false), 2, j);
            }

            AddBlock(structRnd.Next(1, 4) == 1 ? new Cactus_TopFruit(false) : new Cactus_Top(false), 2, startHeight + rightHeight);
        }

        public void GenerateLeftBranch(int startHeight)
        {
            //Generate start of branch
            AddBlock(new Cactus_TopRight(false), 0, startHeight);
            AddBlock(new Cactus_Left(false), 1, startHeight);

            //Generate the branch upwards
            int leftHeight = structRnd.Next(1, 4);
            for (int j = startHeight + 1; j < startHeight + leftHeight; j++)
            {
                AddBlock(new Cactus_Vertical(false), 0, j);
            }

            AddBlock(structRnd.Next(1, 4) == 1 ? new Cactus_TopFruit(false) : new Cactus_Top(false), 0, startHeight + leftHeight);
        }
    }

    public class PyramidStructure : Structure
    {
        public PyramidStructure(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            id = "sc:pyramid_structure";
            name = "Pyramid";
            canReplaceSolidBlocks = true;

            //Layer 1
            int height = 8;
            int width = height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    AddBlock(new SandStoneBricksBlock(false), width + i, j);
                    AddBlock(new SandStoneBricksBlock(false), width - i, j);
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
            AddBackgroundBlock(new SandStoneBlock(true), 6, 3, null);
            AddBackgroundBlock(new SandStoneBlock(true), 7, 1, null);
            AddBackgroundBlock(new SandStoneBlock(true), 7, 2, null);
            AddBackgroundBlock(new SandStoneBlock(true), 7, 3, null);
            AddBackgroundBlock(new SandStoneBlock(true), 8, 2, null);
            AddBackgroundBlock(new SandStoneBlock(true), 9, 1, null);
            AddBackgroundBlock(new SandStoneBlock(true), 9, 2, null);
            AddBackgroundBlock(new SandStoneBlock(true), 9, 3, null);
            AddBackgroundBlock(new SandStoneBlock(true), 10, 3, null);

            //Begin generating
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

            //Layer 1
            AddBlock(new SpruceLogBlock(true), 2, 0);

            //Layer 2
            AddBlock(new SpruceLogBlock(true), 2, 1);

            //Layer 3
            AddBlock(new SpruceLogBlock(true), 2, 2);

            //Layer 4
            AddBlock(new SpruceLeavesBlock(false), 1, 3);
            AddBlock(new SpruceLeavesBlock(false), 2, 3);
            AddBlock(new SpruceLeavesBlock(false), 3, 3);
            AddBlock(new SpruceLeavesBlock(false), 4, 3);
            AddBlock(new SpruceLeavesBlock(false), 0, 3);

            //Layer 5
            AddBlock(new SpruceLeavesBlock(false), 1, 4);
            AddBlock(new SpruceLeavesBlock(false), 2, 4);
            AddBlock(new SpruceLeavesBlock(false), 3, 4);

            //Layer 6
            AddBlock(new SpruceLeavesBlock(false), 1, 5);
            AddBlock(new SpruceLeavesBlock(false), 2, 5);
            AddBlock(new SpruceLeavesBlock(false), 3, 5);

            //Layer 7
            AddBlock(new SpruceLeavesBlock(false), 2, 6);

            //Layer 8
            AddBlock(new SpruceLeavesBlock(false), 2, 7);

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
            int random1 = structRnd.Next(0, 31);

            if (random1 >= 0 && random1 <= 15) //Coal Vein
            {
                foreach (StructureComponent com in shapeCreator.GetCustomCircle(3, 2))
                {
                    com.block = new CoalOreBlock(false);
                    structureComponents.Add(com);
                }
                if (structRnd.Next(1, 11) > 2)
                {
                    StructureComponent ranCom = structureComponents[structRnd.Next(0, structureComponents.Count)];
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
                if (structRnd.Next(1, 11) > 4)
                {
                    StructureComponent ranCom = structureComponents[structRnd.Next(0, structureComponents.Count)];
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
                if (structRnd.Next(1, 11) > 9)
                {
                    StructureComponent ranCom = structureComponents[structRnd.Next(0, structureComponents.Count)];
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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new CoalOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new IronOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new DiamondOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 5) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new AmethystOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new CopperOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new GoldOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new TinOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = new TungstenOreBlock(false);
                    structureComponents.Add(structComp);
                }
            }

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
                    //North
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset, structureComponent.yOffset - 1, new AirBlock(false));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    //East
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset + 1, structureComponent.yOffset, new AirBlock(false));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    //South
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset, structureComponent.yOffset + 1, new AirBlock(false));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    //West
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
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
