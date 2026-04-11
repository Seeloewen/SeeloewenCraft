using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.items;
using System.Collections.Generic;

namespace SeeloewenCraft.game.core.world.generation
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
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 0, 0);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 1, 0);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 2, 0);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 3, 0);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 4, 0);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 1, 1);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 2, 1);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 3, 1);
            AddBlock(BlockRegister.Get("sc:bedrock_block"), 2, 2);

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
            dung.CreateDungeon(chunk.chunkRnd, 100, 50, DungeonType.Plains);
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

            AddBlock(BlockRegister.Get("sc:bone_block"), 0, 0);
            AddBlock(BlockRegister.Get("sc:bone_block"), 0, 1);
            AddBlock(BlockRegister.Get("sc:bone_block"), 0, 2);
            AddBlock(BlockRegister.Get("sc:bone_block"), 1, 3);
            AddBlock(BlockRegister.Get("sc:bone_block"), 2, 0);
            AddBlock(BlockRegister.Get("sc:bone_block"), 2, 1);
            AddBlock(BlockRegister.Get("sc:bone_block"), 2, 2);
            AddBlock(BlockRegister.Get("sc:bone_block"), 3, 3);
            AddBlock(BlockRegister.Get("sc:bone_block"), 4, 0);
            AddBlock(BlockRegister.Get("sc:bone_block"), 4, 1);
            AddBlock(BlockRegister.Get("sc:bone_block"), 4, 2);

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
            AddBlock(BlockRegister.Get("sc:oak_planks_block"), 2, 0);
            AddBlock(BlockRegister.Get("sc:oak_planks_block"), 3, 0);
            AddBlock(BlockRegister.Get("sc:oak_planks_block"), 4, 0);
            AddBlock(BlockRegister.Get("sc:oak_planks_block"), 5, 0);
            AddBlock(BlockRegister.Get("sc:oak_planks_block"), 6, 0);
            AddBlock(BlockRegister.Get("sc:oak_planks_block"), 7, 0);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 1, 0);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 7, 0);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 1, 1);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 1, 2);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 1, 3);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 1, 4);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 1, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 7, 3);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 7, 4);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 7, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 2, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 3, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 4, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 5, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 6, 5);
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 7, 5);

            AddBackgroundBlock(BlockRegister.Get("sc:oak_log_block"), 7, 2, BlockRegister.Get("sc:oak_door_top"));
            AddBackgroundBlock(BlockRegister.Get("sc:oak_log_block"), 7, 1, BlockRegister.Get("sc:oak_door_base"));
            #endregion

            //Background
            #region Background
            for (int i = 2; i < 7; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    if (i == 3 && (j == 2 || j == 3) || i == 5 && (j == 2 || j == 3)) AddBackgroundBlock(BlockRegister.Get("sc:glass_block"), i, j, null); //Windows
                    else if ((i == 2 || i == 4 || i == 6) && j == 2) AddBackgroundBlock(BlockRegister.Get("sc:oak_planks_block"), i, j, BlockRegister.Get("sc:torch_block")); //Torch
                    else if (i == 4 && j == 1) AddBackgroundBlock(BlockRegister.Get("sc:oak_planks_block"), i, j, BlockRegister.Get("sc:crafting_table_block")); //Crafting Table
                    else AddBackgroundBlock(BlockRegister.Get("sc:oak_planks_block"), i, j, null); //Wood Background
                }
            }
            #endregion

            //Roof
            #region Roof
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomright"), 0, 5);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomright"), 1, 6);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomright"), 2, 7);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomright"), 3, 8);
            AddBlock(BlockRegister.Get("sc:cobblestone_slabbottom"), 4, 9);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomleft"), 5, 8);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomleft"), 6, 7);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomleft"), 7, 6);
            AddBlock(BlockRegister.Get("sc:cobblestone_stairbottomleft"), 8, 5);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 2, 6);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 3, 6);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 3, 7);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 4, 6);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 4, 7);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 4, 8);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 5, 6);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 5, 7);
            AddBlock(BlockRegister.Get("sc:cobblestone_block"), 6, 6);
            #endregion

            //Field
            #region Field
            for (int i = 8; i < 15; i++)
            {
                if (i != 11)
                {
                    AddBlock(BlockRegister.Get("sc:dirt_block"), i, -1);
                    AddBlock(BlockRegister.Get("sc:dirt_block"), i, -2);
                    AddBlock(BlockRegister.Get("sc:farmland_block"), i, 0);
                    AddBlock(BlockRegister.Get("sc:air_block"), i, 2);
                    AddBlock(BlockRegister.Get("sc:air_block"), i, 3);
                    AddBlock(GetRandomCrop(), i, 1);
                }
                else
                {
                    AddBlock(BlockRegister.Get("sc:dirt_block"), i, -1);
                    AddBlock(BlockRegister.Get("sc:dirt_block"), i, -2);
                    AddBlock(BlockRegister.Get("sc:air_block"), i, 1);
                    AddBlock(BlockRegister.Get("sc:air_block"), i, 2);
                    AddBlock(BlockRegister.Get("sc:air_block"), i, 3);
                    AddBlock(BlockRegister.Get("sc:water_6_block"), i, 0);
                }
            }
            AddBlock(BlockRegister.Get("sc:oak_log_block"), 15, 0);
            AddBackgroundBlock(BlockRegister.Get("sc:oak_log_block"), 15, 1, null);
            AddBackgroundBlock(BlockRegister.Get("sc:oak_log_block"), 15, 2, null);
            AddBlock(BlockRegister.Get("sc:lantern_block"), 15, 3);
            #endregion

            //Begin generation
            BeginGeneration(x, y, index, isNew);
        }

        public Block GetRandomCrop()
        {
            CropBlock b = (CropBlock)(structRnd.Next(0, 7) switch
            {
                0 => BlockRegister.Get("sc:potato_crop_block"),
                1 => BlockRegister.Get("sc:carrot_crop_block"),
                2 => BlockRegister.Get("sc:wheat_crop_block"),
                3 => BlockRegister.Get("sc:pumpkin_crop_block"),
                4 => BlockRegister.Get("sc:cabbage_crop_block"),
                5 => BlockRegister.Get("sc:tomato_crop_block"),
                6 => BlockRegister.Get("sc:cucumber_crop_block"),
                _ => BlockRegister.Get("sc:grass_crop_block")
            });

            b.progress = int.MaxValue;
            return b;
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
            AddBlock(BlockRegister.Get("sc:spruce_log_block"), 0, 0);
            AddBlock(BlockRegister.Get("sc:spruce_log_block"), 0, 1);
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), 0, 2);
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), 0, 3);
            AddBlock(BlockRegister.Get("sc:lantern_block").MoveToBackground(), 0, 4);

            //First field
            for (int i = 0; i < 3; i++)
            {
                AddBlock(BlockRegister.Get("sc:spruce_log_block"), i + 1, 0);
                AddBlock(BlockRegister.Get("sc:farmland_block"), i + 1, 1);
                CropBlock b = (CropBlock)BlockRegister.Get("sc:cotton_crop_block");
                b.progress = int.MaxValue;
                AddBlock(b, i + 1, 2);
                AddBlock(BlockRegister.Get("sc:air_block"), i + 1, 3);
                AddBlock(BlockRegister.Get("sc:air_block"), i + 1, 4);
            }

            //Appends 
            int offset = 4;
            for (int i = 0; i < appends; i++)
            {
                //Field seperator
                AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset, 0);
                AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset + 1, 0);
                AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset + 2, 0);
                AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset, 1);
                AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), offset, 2);
                AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), offset, 3);
                AddBlock(BlockRegister.Get("sc:lantern_block").MoveToBackground(), offset, 4);
                AddBlock(BlockRegister.Get("sc:water_6_block"), offset + 1, 1);
                AddBlock(BlockRegister.Get("sc:spruce_planks_slabbottom"), offset + 1, 2);
                AddBlock(BlockRegister.Get("sc:air_block"), offset + 1, 3);
                AddBlock(BlockRegister.Get("sc:air_block"), offset + 1, 4);
                AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset + 2, 1);
                AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), offset + 2, 2);
                AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), offset + 2, 3);
                AddBlock(BlockRegister.Get("sc:lantern_block").MoveToBackground(), offset + 2, 4);

                //Field
                for (int j = 0; j < 4; j++)
                {
                    AddBlock(BlockRegister.Get("sc:spruce_log_block"), j + offset + 3, 0);
                    AddBlock(BlockRegister.Get("sc:farmland_block"), j + offset + 3, 1);
                    CropBlock b = (CropBlock)BlockRegister.Get("sc:cotton_crop_block");
                    b.progress = int.MaxValue;
                    AddBlock(b, j + offset + 3, 2);
                    AddBlock(BlockRegister.Get("sc:air_block"), j + offset + 3, 3);
                    AddBlock(BlockRegister.Get("sc:air_block"), j + offset + 3, 4);
                }

                offset += 7;
            }

            //End pillar
            AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset - 1, 0);
            AddBlock(BlockRegister.Get("sc:spruce_log_block"), offset - 1, 1);
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), offset - 1, 2);
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), offset - 1, 3);
            AddBlock(BlockRegister.Get("sc:lantern_block").MoveToBackground(), offset - 1, 4);

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
            int posX = 0;
            int posY = 0;

            bool goDown = true;
            posY = floorHeight;

            do
            {
                //Generate from bottom to floorheight
                for (int i = 0; i < floorHeight; i++)
                {
                    //Add dirt when below the currently observed posY, else add water
                    if (i < posY && i >= posY - 2)
                    {
                        AddBlock(BlockRegister.Get("sc:dirt_block"), posX, i);
                    }
                    else if (i >= posY)
                    {
                        AddBlock(BlockRegister.Get("sc:water_6_block"), posX, i);
                    }
                }

                //Generate a mirror of the lake above, but with air to clear potential blocks above
                for (int i = floorHeight; i <= floorHeight + floorHeight - posY + 2; i++)
                {
                    AddBlock(BlockRegister.Get("sc:air_block"), posX, i);
                }

                //Determine whether to go down or up
                if (posY == 1)
                {
                    goDown = false;
                }

                //Roll for a 50% chance to go down/up or stay on the height
                if (posX == 0)
                {
                    posY--;
                }
                else if (goDown)
                {
                    posY = (structRnd.Next(0, 2) == 0) ? posY - 1 : posY;
                }
                else
                {
                    posY = (structRnd.Next(0, 2) == 0) ? posY + 1 : posY;
                }

                posX++;
            }

            while (posY <= floorHeight);


            if (structRnd.Next(0, 4) == 0) //Chance to generate sugar cane
            {
                CropBlock sugarCane = (CropBlock)BlockRegister.Get("sc:sugar_cane_crop_block");
                sugarCane.progress = int.MaxValue;
                if (structRnd.Next(0, 2) == 0) AddBlock(sugarCane, 0, floorHeight);
                else AddBlock(sugarCane, posX, floorHeight);
            }
            else if (structRnd.Next(0, 7) == 0) //Chance to generate rice
            {
                CropBlock riceTop = (CropBlock)BlockRegister.Get("sc:rice_top_crop_block");
                riceTop.progress = int.MaxValue;
                if (structRnd.Next(0, 2) == 0)
                {
                    AddBlock(BlockRegister.Get("sc:rice_base_block"), 0, floorHeight);
                    AddBlock(riceTop, 0, floorHeight + 1);
                }
                else
                {
                    AddBlock(BlockRegister.Get("sc:rice_base_block"), posX, floorHeight);
                    AddBlock(riceTop, posX, floorHeight + 1);
                }
            }


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
            AddBlock(BlockRegister.Get("sc:oak_log_block").MoveToBackground(), 2, 0);

            //Layer 2
            AddBlock(BlockRegister.Get("sc:oak_log_block").MoveToBackground(), 2, 1);

            //Layer 3
            AddBlock(BlockRegister.Get("sc:oak_log_block").MoveToBackground(), 2, 2);

            //Layer 4
            AddBlock(GetLeaves(), 1, 3);
            AddBlock(GetLeaves(), 2, 3);
            AddBlock(GetLeaves(), 3, 3);
            AddBlock(GetLeaves(), 4, 3);
            AddBlock(GetLeaves(), 0, 3);

            //Layer 5
            AddBlock(GetLeaves(), 1, 4);
            AddBlock(GetLeaves(), 2, 4);
            AddBlock(GetLeaves(), 3, 4);

            //Layer 6
            AddBlock(GetLeaves(), 2, 5);

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);
        }

        public Block GetLeaves()
        {
            Block block = BlockRegister.Get("sc:oak_leaves_block");
            block.WriteTag(BlockTags.STRUCTURE_LEAF);
            return block;
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
                                AddBlock(BlockRegister.Get("sc:cactus_vertical"), 1, i);
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
                                AddBlock(BlockRegister.Get("sc:cactus_vertical"), 1, i);
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
                                AddBlock(BlockRegister.Get("sc:cactus_cross"), 1, i);
                            }
                            else
                            {
                                AddBlock(BlockRegister.Get("sc:cactus_vertical"), 1, i);
                            }
                            break;
                        case > 2:
                            AddBlock(BlockRegister.Get("sc:cactus_vertical"), 1, i);
                            break;
                    }
                }
                else
                {
                    AddBlock(BlockRegister.Get("sc:cactus_vertical"), 1, i);
                }

                AddBlock(BlockRegister.Get(structRnd.Next(1, 4) == 1 ? "sc:cactus_top_fruit" : "sc:cactus_top"), 1, height);

                //Begin generating
                BeginGeneration(x, y, index, isNew);
            }

        }

        public void GenerateRightBranch(int startHeight)
        {
            //Generate start of branch
            AddBlock(BlockRegister.Get("sc:cactus_top_left"), 2, startHeight);
            AddBlock(BlockRegister.Get("sc:cactus_right"), 1, startHeight);

            //Generate the branch upwards
            int rightHeight = structRnd.Next(1, 4);
            for (int j = startHeight + 1; j < startHeight + rightHeight; j++)
            {
                AddBlock(BlockRegister.Get("sc:cactus_vertical"), 2, j);
            }

            AddBlock(BlockRegister.Get(structRnd.Next(1, 4) == 1 ? "sc:cactus_top_fruit" : "sc:cactus_top"), 2, startHeight + rightHeight);
        }

        public void GenerateLeftBranch(int startHeight)
        {
            //Generate start of branch
            AddBlock(BlockRegister.Get("sc:cactus_top_right"), 0, startHeight);
            AddBlock(BlockRegister.Get("sc:cactus_left"), 1, startHeight);

            //Generate the branch upwards
            int leftHeight = structRnd.Next(1, 4);
            for (int j = startHeight + 1; j < startHeight + leftHeight; j++)
            {
                AddBlock(BlockRegister.Get("sc:cactus_vertical"), 0, j);
            }

            AddBlock(BlockRegister.Get(structRnd.Next(1, 4) == 1 ? "sc:cactus_top_fruit" : "sc:cactus_top"), 0, startHeight + leftHeight);
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
                    AddBlock(BlockRegister.Get("sc:sand_stone_bricks_block"), width + i, j);
                    AddBlock(BlockRegister.Get("sc:sand_stone_bricks_block"), width - i, j);
                }
                height--;
            }

            //Left pot
            Block potBase = BlockRegister.Get("sc:archeology_pot_base");
            Block potTop = BlockRegister.Get("sc:archeology_pot_top");
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 6, 1, potBase, LootTables.potLootTable, 1);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 6, 2, potTop);

            //Chest
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 8, 1, BlockRegister.Get("sc:chest_block"), LootTables.pyramidLootTable, 3);

            //Torch
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 8, 3, BlockRegister.Get("sc:torch_block"));

            //Right pot
            Block potBase2 = BlockRegister.Get("sc:archeology_pot_base");
            Block potTop2 = BlockRegister.Get("sc:archeology_pot_top");
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 10, 1, potBase2, LootTables.potLootTable, 1);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 10, 2, potTop2);

            //Other background blocks
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 6, 3, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 7, 1, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 7, 2, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 7, 3, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 8, 2, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 9, 1, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 9, 2, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 9, 3, null);
            AddBackgroundBlock(BlockRegister.Get("sc:sand_stone_block").MoveToBackground(), 10, 3, null);

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
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), 2, 0);

            //Layer 2
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), 2, 1);

            //Layer 3
            AddBlock(BlockRegister.Get("sc:spruce_log_block").MoveToBackground(), 2, 2);

            //Layer 4
            AddBlock(GetLeaves(), 1, 3);
            AddBlock(GetLeaves(), 2, 3);
            AddBlock(GetLeaves(), 3, 3);
            AddBlock(GetLeaves(), 4, 3);
            AddBlock(GetLeaves(), 0, 3);

            //Layer 5
            AddBlock(GetLeaves(), 1, 4);
            AddBlock(GetLeaves(), 2, 4);
            AddBlock(GetLeaves(), 3, 4);

            //Layer 6
            AddBlock(GetLeaves(), 1, 5);
            AddBlock(GetLeaves(), 2, 5);
            AddBlock(GetLeaves(), 3, 5);

            //Layer 7
            AddBlock(GetLeaves(), 2, 6);

            //Layer 8
            AddBlock(GetLeaves(), 2, 7);

            //Begin generating the trees
            BeginGeneration(x, y, index, isNew);
        }

        public Block GetLeaves()
        {
            Block block = BlockRegister.Get("sc:spruce_leaves_block");
            block.WriteTag(BlockTags.STRUCTURE_LEAF);
            return block;
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
                    com.block = BlockRegister.Get("sc:coal_ore_block");
                    structureComponents.Add(com);
                }
                if (structRnd.Next(1, 11) > 2)
                {
                    StructureComponent ranCom = structureComponents[structRnd.Next(0, structureComponents.Count)];
                    foreach (StructureComponent com in shapeCreator.GetCustomCircle(3, 2))
                    {
                        com.xOffset += ranCom.xOffset;
                        com.yOffset += ranCom.yOffset;
                        com.block = BlockRegister.Get("sc:coal_ore_block");
                        structureComponents.Add(com);
                    }
                }
            }
            else if (random1 > 15 && random1 <= 25) //Iron Vein
            {
                foreach (StructureComponent com in shapeCreator.GetCustomCircle(2, 1))
                {
                    com.block = BlockRegister.Get("sc:iron_ore_block");
                    structureComponents.Add(com);
                }
                if (structRnd.Next(1, 11) > 4)
                {
                    StructureComponent ranCom = structureComponents[structRnd.Next(0, structureComponents.Count)];
                    foreach (StructureComponent com in shapeCreator.GetCustomCircle(2, 1))
                    {
                        com.xOffset += ranCom.xOffset;
                        com.yOffset += ranCom.yOffset;
                        com.block = BlockRegister.Get("sc:iron_ore_block");
                        structureComponents.Add(com);
                    }
                }
            }
            else if (random1 > 25 && random1 <= 30) //Diamond Vein
            {
                foreach (StructureComponent com in shapeCreator.GetCustomCircle(1, 1))
                {
                    com.block = BlockRegister.Get("sc:diamond_ore_block");
                    structureComponents.Add(com);
                }
                if (structRnd.Next(1, 11) > 9)
                {
                    StructureComponent ranCom = structureComponents[structRnd.Next(0, structureComponents.Count)];
                    foreach (StructureComponent com in shapeCreator.GetCustomCircle(2, 1))
                    {
                        com.xOffset += ranCom.xOffset;
                        com.yOffset += ranCom.yOffset;
                        com.block = BlockRegister.Get("sc:diamond_ore_block");
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
                structComp.block = BlockRegister.Get("sc:coal_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:coal_ore_block");
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
                structComp.block = BlockRegister.Get("sc:iron_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:iron_ore_block");
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
                structComp.block = BlockRegister.Get("sc:diamond_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:diamond_ore_block" );
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
                structComp.block = BlockRegister.Get("sc:amethyst_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 5) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:amethyst_ore_block");
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
                structComp.block = BlockRegister.Get("sc:copper_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(3, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:copper_ore_block");
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
                structComp.block = BlockRegister.Get("sc:gold_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:gold_ore_block");
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
                structComp.block = BlockRegister.Get("sc:emerald_ore_block");
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
                structComp.block = BlockRegister.Get("sc:tin_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 2))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:tin_ore_block");
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
                structComp.block = BlockRegister.Get("sc:tungsten_ore_block");
                structureComponents.Add(structComp);
            }
            if (structRnd.Next(1, 11) > 2) //Potentially attach a second part to the vein
            {
                StructureComponent structRndStructComp = structureComponents[structRnd.Next(0, structureComponents.Count)];
                foreach (StructureComponent structComp in shapeCreator.GetCustomCircle(2, 1))
                {
                    structComp.xOffset += structRndStructComp.xOffset;
                    structComp.yOffset += structRndStructComp.yOffset;
                    structComp.block = BlockRegister.Get("sc:tungsten_ore_block");
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
            structureComponents.Add(new StructureComponent(0, 0, BlockRegister.Get("sc:air_block")));

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
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset, structureComponent.yOffset - 1, BlockRegister.Get("sc:air_block"));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    //East
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset + 1, structureComponent.yOffset, BlockRegister.Get("sc:air_block"));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    //South
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset, structureComponent.yOffset + 1, BlockRegister.Get("sc:air_block"));
                        if (!StructureComponentsListContainsStructureComponent(structureComponents, newComponent) && !StructureComponentsListContainsStructureComponent(temporaryComponentList, newComponent))
                        {
                            temporaryComponentList.Add(newComponent);
                        }
                    }

                    //West
                    if (structRnd.Next(1, 3) == 1 && !StructureComponentsListContainsStructureComponent(generatedComponents, structureComponent))
                    {
                        //Generate the new component and check if it's already in some list. If not, add it.
                        StructureComponent newComponent = new StructureComponent(structureComponent.xOffset - 1, structureComponent.yOffset, BlockRegister.Get("sc:air_block"));
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
