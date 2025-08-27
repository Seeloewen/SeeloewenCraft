using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world.generation;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using System;
using System.Collections.Generic;
using System.Windows;

/* 
 * General overview of block break times:
 * (Note that there may be deviations)
 * Dirt-Like Block: 150
 * Stone: 1250
 * Ore: 1750
 * Iron: 2000
 * Wood: 350
 * Leaves: 125
 * Non-Solid: 0
 */

namespace SeeloewenCraft.game.core.blocks
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Grass Block", "sc:grass_block", 150, "sc:grass_block_item", Tool.Shovel);
            drops.Add(("sc:dirt_item", 1, 1));

            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
            WriteTag(BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.SCYTHEABLE);
        }

        protected override void DoSpecificUpdate()
        {
            //Roll whether to grow grass to adjacent dirt
            if (Game.rnd.Next(0, 40) == 0)
            {
                Block blockRight = GetBlockRight();
                Block blockLeft = GetBlockLeft();

                if (blockRight == null || blockLeft == null) return; //Skip check for blocks at chunk borders, would only cause issues

                List<Block> candidates = new List<Block> { blockRight, blockLeft, blockRight.GetBlockAbove(), blockRight.GetBlockBelow(), blockLeft.GetBlockAbove(), blockLeft.GetBlockBelow() };
                foreach (Block candidate in candidates)
                {
                    //Confirms that the candidate is actually a dirt block that has either nothing above (top world border), or a non-solid block above (except water)
                    if (candidate != null && candidate is DirtBlock dirt && (candidate.GetBlockAbove == null || (!candidate.GetBlockAbove().isSolid) && !candidate.GetBlockAbove().HasTag(BlockTags.LIQUIDS_WATER)))
                    {
                        candidate.SetBlock(BlockRegister.GenerateBlock("sc:grass_block"));
                    }
                }
            }


            //Roll whether to grow a random plant
            if (Game.rnd.Next(0, 1000) == 0)
            {
                Block blockAbove = GetBlockAbove();

                if (blockAbove != null && blockAbove.id == "sc:air_block")
                {
                    string cropId = Game.rnd.Next(0, 9) switch
                    {
                        0 => "sc:potato_crop_block",
                        1 => "sc:berry_bush_crop_block",
                        2 => "sc:carrot_crop_block",
                        3 => "sc:pumpkin_crop_block",
                        4 => "sc:cotton_crop_block",
                        5 => "sc:cucumber_crop_block",
                        6 => "sc:yellow_flower_block",
                        7 => "sc:blue_flower_block",
                        _ => "sc:grass"
                    };

                    Block newBlock = BlockRegister.GenerateBlock(cropId);
                    newBlock.needsGround = (true, BlockTags.GROUNDS_DIRT);
                    blockAbove.SetBlock(newBlock);
                }
            }
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Stone Block", "sc:stone_block", 1250, "sc:stone_block_item", Tool.Pickaxe);
            drops.Add(("sc:rock_item", 1, 4));

            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Dirt", "sc:dirt_block", 150, "sc:dirt_item", Tool.Shovel);
            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
            WriteTag(BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.SCYTHEABLE);
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Air", "sc:air_block", 150, "sc:air_item", Tool.None);
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.REPLACEABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            isSolid = false;
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Bedrock", "sc:bedrock_block", 150, "sc:bedrock_item", Tool.None);
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Coal Ore", "sc:coal_ore_block", 1750, "sc:coal_ore_item", Tool.Pickaxe);
            drops.Add(("sc:coal_item", 1, 3));
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Diamond Ore", "sc:diamond_ore_block", 1750, "sc:diamond_ore_item", Tool.Pickaxe);
            drops.Add(("sc:diamond_item", 1, 1));
            effectiveMaterial = Material.Iron;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Iron Ore", "sc:iron_ore_block", 1750, "sc:iron_ore_item", Tool.Pickaxe);
            effectiveMaterial = Material.Stone;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Log", "sc:oak_log_block", 350, "sc:oak_log_item", Tool.Axe);
            WriteTag(BlockTags.TYPES_LOG);
        }
    }

    public class OakLeavesBlock : LeavesBlock
    {
        public OakLeavesBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Leaves", "sc:oak_leaves_block", 125, "sc:oak_leaves_item", Tool.None);
            lootTable = (LootTables.oakTreeLootTable, 1, 1);
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Log", "sc:spruce_log_block", 350, "sc:spruce_log_item", Tool.Axe);
            WriteTag(BlockTags.TYPES_LOG);
        }
    }

    public class SpruceLeavesBlock : LeavesBlock
    {
        public SpruceLeavesBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Leaves", "sc:spruce_leaves_block", 125, "sc:spruce_leaves_item", Tool.None);
            lootTable = (LootTables.spruceTreeLootTable, 1, 1);
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Chest", "sc:chest_block", 500, "sc:chest_item", Tool.Axe);
            WriteTag(BlockTags.HAS_INVENTORY);
            inventory = new Inventory(9, 4, false, "Chest");
            inventory.block = this;
            Game.world.inventoryList.Add(inventory);
            WriteTag(BlockTags.RIGHTCLICKABLE);
        }

        public override void RightClickAction()
        {
            if (IsInRange() && isSolid)
            {
                Game.world.player.inventory.ShowGui();
                inventory.ShowGui();
            }
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Magma Block", "sc:magma_block", 750, "sc:magma_block_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 1000, 1, 1000);
            WriteTag(BlockTags.LIGHTSOURCE);
        }

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            if (isBackground) base.CheckTouch(startX, startY, endX, endY);
            bool[] touching = new bool[Entity.TOUCHING_STATUS_COUNT];
            touching[Entity.TOUCHING_MAGMA] = true;
            return touching;
        }
    }

    public class TorchBlock : Block
    {
        public TorchBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Torch", "sc:torch_block", 0, "sc:torch_item", Tool.None);
            isSolid = false;
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.LIGHTSOURCE);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class PottedCactus_Base : Block
    {
        public PottedCactus_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Potted Cactus Base", "sc:potted_cactus_base", 0, "sc:potted_cactus_item", Tool.None);
            isBase = true;
            connectedBlocks.Add((0, -1, "sc:potted_cactus_top"));
            collision = new MultipleRectangleCollision([125, 251], [875, 749], [375, 1], [1000, 375]);
            needsGround = (true, "");
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class PottedCactus_Top : Block
    {
        public PottedCactus_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Potted Cactus Top", "sc:potted_cactus_top", 0, null, Tool.None);
            collision = new RectangleCollision(251, 749, 188, 999);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class CraftingTableBlock : Block
    {
        public CraftingTableBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Crafting Table", "sc:crafting_table_block", 500, "sc:crafting_table_item", Tool.Axe);
            WriteTag(BlockTags.WORKSTATION);
            WriteTag(BlockTags.RIGHTCLICKABLE);

            craftingHandler = new CraftingHandler(this, "Crafting_Table", "Crafting Table");
        }

        public override void RightClickAction()
        {
            if (IsInRange())
            {
                ((IGuiData)craftingHandler).Show();
            }
        }

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();

            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeClaimable={craftingHandler.recipeClaimable}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"selectedRecipe={craftingHandler.currentRecipe}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeProgress={craftingHandler.recipeProgress}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"amount={craftingHandler.recipeAmount}");
            }
        }
    }

    public class ChiselerBlock : Block
    {
        public ChiselerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseler", "sc:chiseler_block", 500, "sc:chiseler_item", Tool.Axe);
            WriteTag(BlockTags.WORKSTATION);
            WriteTag(BlockTags.RIGHTCLICKABLE);

            craftingHandler = new CraftingHandler(this, "Chiseler", "Chiseler");
        }

        public override void RightClickAction()
        {
            if (IsInRange())
            {
                ((IGuiData)craftingHandler).Show();
            }
        }

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeClaimable={craftingHandler.recipeClaimable}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"selectedRecipe={craftingHandler.currentRecipe}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeProgress={craftingHandler.recipeProgress}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"amount={craftingHandler.recipeAmount}");
            }
        }
    }

    public class UnchiselerBlock : Block
    {
        private UnchiselHandler unchiselHandler;

        public UnchiselerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Unchiseler", "sc:unchiseler_block", 500, "sc:unchiseler_item", Tool.Axe);
            WriteTag(BlockTags.WORKSTATION);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            WriteTag(BlockTags.HAS_INVENTORY);
            unchiselHandler = new UnchiselHandler();
            inventory = unchiselHandler.inv;
            unchiselHandler.inv.block = this;
            collision = new RectangleCollision(0, 1000, 565, 1000);
        }

        public override void RightClickAction()
        {
            if (IsInRange())
            {
                Game.world.player.inventory.ShowGui();
                ((IGuiData)unchiselHandler).Show();
            }
        }
    }

    public class CobblestoneBlock : Block
    {
        public CobblestoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cobblestone", "sc:cobblestone_block", 1250, "sc:cobblestone_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SpruceDoor_Base : DoorBlock
    {
        public SpruceDoor_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Door Base", "sc:spruce_door_base", 500, "sc:spruce_door_item", Tool.Axe);
            isBase = true;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, "");
            collision = new RectangleCollision(720, 1000, 0, 1000);

            connectedBlocks.Add((0, -1, "sc:spruce_door_top"));
        }
    }

    public class SpruceDoor_Top : DoorBlock
    {
        public SpruceDoor_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Door Top", "sc:spruce_door_top", 500, null, Tool.Axe);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(720, 1000, 0, 1000);
        }

        public override void RightClickAction()
        {
            if (!isForeground)
            {
                if (GetBaseBlock() is DoorBlock block)
                {
                    block.RightClickAction();
                }
            }
            else
            {
                if (GetBaseBlock().GetForegroundBlock() is DoorBlock block)
                {
                    block.RightClickAction();
                }
            }
        }
    }

    public class SandStoneBlock : Block
    {
        public SandStoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sand Stone", "sc:sand_stone_block", 1250, "sc:sand_stone_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class OakPlanksBlock : Block
    {
        public OakPlanksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Plank", "sc:oak_planks_block", 500, "sc:oak_planks_item", Tool.Axe);
        }
    }

    public class SprucePlanksBlock : Block
    {
        public SprucePlanksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Planks", "sc:spruce_planks_block", 500, "sc:spruce_planks_item", Tool.Axe);
        }
    }

    public abstract class CactusBlock : Block
    {
        protected Collision cactusCollision;

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            //idk why the +1 is needed but it works so i will leave it here until further review(which will probably never happen lol)
            (touchingStatus[Entity.TOUCHING_CACTUS], _)
                = cactusCollision.CheckCollision(Direction.RIGHT, startX, endX + 1, startY, endY + 1);
            return touchingStatus;
        }

        protected CactusBlock(bool isInBackground) : base(isInBackground)
        {
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class Cactus_TopFruit : CactusBlock
    {
        public Cactus_TopFruit(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Fruit", "sc:cactus_top_fruit", 250, "sc:cactus_top_fruit_item", Tool.Axe);
            drops.Add(("sc:cactus_fruit_item", 1, 1));
            collision = new RectangleCollision(191, 809, 631, 999);
            cactusCollision = new RectangleCollision(190, 810, 630, 1000);
        }
    }

    public class Cactus_Vertical : CactusBlock
    {
        public Cactus_Vertical(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Vertical", "sc:cactus_vertical", 250, null, Tool.Axe);
            collision = new RectangleCollision(191, 809, 1, 999);
            cactusCollision = new RectangleCollision(190, 810, 0, 1000);
        }
    }

    public class Cactus_Top : CactusBlock
    {
        public Cactus_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top", "sc:cactus_top", 250, null, Tool.Axe);
            collision = new RectangleCollision(191, 809, 631, 999);
            cactusCollision = new RectangleCollision(190, 810, 630, 1000);
        }
    }

    public class Cactus_TopLeft : CactusBlock
    {
        public Cactus_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Left", "sc:cactus_top_left", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 1], [809, 189], [1, 191], [809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [0, 190], [810, 810]);
        }
    }

    public class Cactus_TopRight : CactusBlock
    {
        public Cactus_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Right", "sc:cactus_top_right", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [1, 191], [809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [0, 190], [810, 810]);
        }
    }

    public class Cactus_Cross : CactusBlock
    {
        public Cactus_Cross(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Cross", "sc:cactus_cross", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 811, 1], [809, 999, 189], [1, 191, 191], [999, 809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810, 0], [810, 1000, 190], [0, 190, 190], [1000, 810, 810]);
        }
    }

    public class Cactus_Right : CactusBlock
    {
        public Cactus_Right(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Right", "sc:cactus_right", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [1, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [0, 190], [1000, 810]);
        }
    }

    public class Cactus_Left : CactusBlock
    {
        public Cactus_Left(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Left", "sc:cactus_left", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 1], [809, 189], [1, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [0, 190], [1000, 810]);
        }
    }

    public class Cactus_Horizontal : CactusBlock
    {
        public Cactus_Horizontal(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Horizontal", "sc:cactus_horizontal", 250, null, Tool.Axe);
            collision = new RectangleCollision(1, 999, 189, 811);
            cactusCollision = new RectangleCollision(0, 1000, 190, 810);
        }
    }

    public class Cactus_BottomLeft : CactusBlock
    {
        public Cactus_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Bottom Left", "sc:cactus_bottom_left", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 1], [809, 191], [191, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [190, 190], [1000, 810]);
        }
    }

    public class Cactus_BottomRight : CactusBlock
    {
        public Cactus_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Bottom Right", "sc:cactus_bottom_right", 250, null, Tool.Axe);
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [189, 189], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [190, 190], [1000, 810]);
        }
    }

    public class OakDoor_Base : DoorBlock
    {
        public OakDoor_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Door Base", "sc:oak_door_base", 500, "sc:oak_door_item", Tool.Axe);
            isBase = true;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(720, 1000, 0, 1000);

            connectedBlocks.Add((0, -1, "sc:oak_door_top"));
        }
    }

    public class OakDoor_Top : DoorBlock
    {
        public OakDoor_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Door Top", "sc:oak_door_top", 500, null, Tool.Axe);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(720, 1000, 0, 1000);
        }

        public override void RightClickAction()
        {
            if (!isForeground)
            {
                if (GetBaseBlock() is DoorBlock block)
                {
                    block.RightClickAction();
                }
            }
            else
            {
                if (GetBaseBlock().GetForegroundBlock() is DoorBlock block)
                {
                    block.RightClickAction();
                }
            }
        }
    }

    public class OakTrapDoor : DoorBlock
    {
        public OakTrapDoor(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Trapdoor Base", "sc:oak_trapdoor", 500, "sc:oak_trapdoor_item", Tool.Axe);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(0, 1000, 0, 150);
        }
    }

    public class SpruceTrapDoor : DoorBlock
    {
        public SpruceTrapDoor(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Trapdoor Base", "sc:spruce_trapdoor", 500, "sc:spruce_trapdoor_item", Tool.Axe);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(0, 1000, 0, 150);
        }
    }
    public class FurnaceBlock : Block
    {
        public FurnaceBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Furnace", "sc:furnace_block", 500, "sc:furnace_item", Tool.Pickaxe);
            WriteTag(BlockTags.WORKSTATION);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            WriteTag(BlockTags.TOOL_SPECIFIC);

            craftingHandler = new CraftingHandler(this, "Furnace", "Furnace");
        }

        public override void RightClickAction()
        {
            if (IsInRange())
            {
                ((IGuiData)craftingHandler).Show();
            }
        }

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeClaimable={craftingHandler.recipeClaimable}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"selectedRecipe={craftingHandler.currentRecipe}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeProgress={craftingHandler.recipeProgress}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"amount={craftingHandler.recipeAmount}");
            }
        }
    }

    public class OakChair_Base : Block
    {
        public OakChair_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Chair Base", "sc:oak_chair_base", 0, "sc:oak_chair_item", Tool.Axe);
            isBase = true;
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);

            connectedBlocks.Add((0, -1, "sc:oak_chair_top"));
        }
    }

    public class OakChair_Top : Block
    {
        public OakChair_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Chair Top", "sc:oak_chair_top", 0, null, Tool.Axe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class SpruceChair_Base : Block
    {
        public SpruceChair_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Chair Base", "sc:spruce_chair_base", 0, "sc:spruce_chair_item", Tool.Axe);
            isBase = true;
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);

            connectedBlocks.Add((0, -1, "sc:spruce_chair_top"));
        }
    }

    public class SpruceChair_Top : Block
    {
        public SpruceChair_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Chair Top", "sc:spruce_chair_top", 0, null, Tool.Axe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class ArcheologyPot_Base : Block
    {
        public ArcheologyPot_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Archeology Pot Base", "sc:archeology_pot_base", 100, "sc:archeology_pot_item", Tool.Pickaxe);
            isBase = true;
            isSolid = false;
            inventory = new Inventory(1, 1, false);
            WriteTag(BlockTags.HAS_INVENTORY);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);

            connectedBlocks.Add((0, -1, "sc:archeology_pot_top"));
        }
    }

    public class ArcheologyPot_Top : Block
    {
        public ArcheologyPot_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Archeology Pot Top", "sc:archeology_pot_top", 100, null, Tool.Pickaxe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class AmethystOreBlock : Block
    {
        public AmethystOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Amethyst Ore", "sc:amethyst_ore_block", 1750, "sc:amethyst_ore_item", Tool.Pickaxe);
            drops.Add(("sc:amethyst_item", 1, 1));
            effectiveMaterial = Material.Diamond;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class AnvilBlock : Block
    {
        public AnvilBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Anvil", "sc:anvil_block", 2000, "sc:anvil_item", Tool.Pickaxe);
            WriteTag(BlockTags.WORKSTATION);
            WriteTag(BlockTags.RIGHTCLICKABLE);
            WriteTag(BlockTags.TOOL_SPECIFIC);
            WriteTag(BlockTags.CAN_FALL);

            craftingHandler = new CraftingHandler(this, "Anvil", "Anvil");
            collision = new RectangleCollision(0, 1000, 190, 1000);
        }

        public override void RightClickAction()
        {
            if (IsInRange())
            {
                ((IGuiData)craftingHandler).Show();
            }
        }

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeClaimable={craftingHandler.recipeClaimable}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"selectedRecipe={craftingHandler.currentRecipe}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeProgress={craftingHandler.recipeProgress}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"amount={craftingHandler.recipeAmount}");
            }
        }
    }

    public class BarrelBlock : Block
    {
        public BarrelBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Barrel", "sc:barrel_block", 500, "sc:barrel_item", Tool.Axe);
            WriteTag(BlockTags.HAS_INVENTORY);
            inventory = new Inventory(7, 2, false, "Barrel");
            Game.world.inventoryList.Add(inventory);
            WriteTag(BlockTags.RIGHTCLICKABLE);
        }

        public override void RightClickAction()
        {
            if (IsInRange() && isSolid)
            {
                Game.world.player.inventory.ShowGui();
                inventory.ShowGui();
            }
        }
    }

    public class BlueFlowerBlock : Block
    {
        public BlueFlowerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Blue Flower", "sc:blue_flower_block", 0, "sc:blue_flower_item", Tool.None);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class BoneBlock : Block
    {
        public BoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Bone Block", "sc:bone_block", 750, "sc:bone_block_item", Tool.None);
        }
    }

    public class CactusFruitBlock : CropBlock
    {
        public CactusFruitBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Fruit", "sc:cactus_fruit_block", 0, "sc:cactus_fruit_item", Game.rnd.Next(180000, 480001), "sc:cactus_fruit_item", "sc:cactus_fruit_item", 0, 0, Tool.None);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_SAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                if (HasSpaceAbove(-1, 0, 3, 6))
                {
                    Structure cactus = new CactusStructure(xPos, yPos, chunk.index, true, null, true);
                    foreach (StructureComponent structComp in cactus.structureComponents)
                    {
                        chunk.SetBlock(structComp.block, xPos + structComp.xOffset - 1, yPos - structComp.yOffset);
                    }
                }
                else
                {
                    growthTime += 10000;
                }
            }
        }
    }

    public class CandleBlock : Block
    {
        public CandleBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Candle", "sc:candle_block", 0, "sc:candle_item", Tool.None);
            WriteTag(BlockTags.LIGHTSOURCE);
            isSolid = false;
            needsGround = (true, "");
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class CopperOreBlock : Block
    {
        public CopperOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Copper Ore", "sc:copper_ore_block", 1750, "sc:copper_ore_block", Tool.Pickaxe);
            effectiveMaterial = Material.Tin;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class DeadBushBlock : Block
    {
        public DeadBushBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Dead Bush", "sc:dead_bush_block", 0, "sc:dead_bush_item", Tool.None);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_SAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class EmeraldOreBlock : Block
    {
        public EmeraldOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Emerald Ore", "sc:emerald_ore_block", 1750, "sc:emerald_ore_item", Tool.Pickaxe);
            drops.Add(("sc:emerald_item", 1, 1));
            effectiveMaterial = Material.Diamond;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class FlowerPotBlock : Block
    {
        public FlowerPotBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Flower Pot", "sc:flower_pot_block", 200, "sc:flower_pot_item", Tool.Pickaxe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class GoldOreBlock : Block
    {
        public GoldOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Gold Ore", "sc:gold_ore_block", 1750, "sc:gold_ore_item", Tool.Pickaxe);
            effectiveMaterial = Material.Tin;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class Grass : Block
    {
        public Grass(bool isInBackground) : base(isInBackground)
        {
            Init("Grass", "sc:grass", 0, "sc:grass_item", Tool.None);
            lootTable = (LootTables.grassLootTable, 1, 1);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class IronGatesBlock : Block
    {
        public IronGatesBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Iron Gates", "sc:iron_gates_block", 2000, "sc:iron_gates_item", Tool.Pickaxe);
            effectiveMaterial = Material.Stone;
            WriteTag(BlockTags.TOOL_SPECIFIC);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class LadderBlock : Block
    {
        public LadderBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Ladder", "sc:ladder_block", 250, "sc:ladder_item", Tool.Axe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            bool[] touching = new bool[Entity.TOUCHING_STATUS_COUNT];
            touching[Entity.TOUCHING_LADDER] = true;
            return touching;
        }
    }

    public class MossyCobblestoneBlock : Block
    {
        public MossyCobblestoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Mossy Cobblestone", "sc:mossy_cobblestone_block", 1250, "sc:mossy_cobblestone_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class OakSaplingBlock : CropBlock
    {
        public OakSaplingBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Sapling", "sc:oak_sapling_block", 0, "sc:oak_sapling_item", Game.rnd.Next(600000, 1200001), "sc:oak_sapling_item", "sc:oak_sapling_item", 0, 0, Tool.None);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                if (HasSpaceAbove(-2, 0, 5, 5))
                {
                    Structure tree = new OakTreeStructure(xPos, yPos, chunk.index, true, null, true);

                    foreach (StructureComponent structComp in tree.structureComponents)
                    {
                        chunk.SetBlock(structComp.block, xPos + structComp.xOffset - 2, yPos - structComp.yOffset);
                    }
                }
                else
                {
                    growthTime += 10000;
                }
            }
        }
    }

    public class SpruceSaplingBlock : CropBlock
    {
        public SpruceSaplingBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Sapling", "sc:spruce_sapling_block", 0, "sc:spruce_sapling_item", Game.rnd.Next(600000, 1200001), "sc:tree_sapling_item", "sc:tree_sapling_item", 0, 0, Tool.None);
            isSolid = false;
            needsGround = (true, "ground/plant");
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                if (HasSpaceAbove(-2, 0, 5, 7))
                {

                    Structure tree = new SpruceTreeStructure(xPos, yPos, chunk.index, true, null, true);

                    foreach (StructureComponent structComp in tree.structureComponents)
                    {
                        chunk.SetBlock(structComp.block, xPos + structComp.xOffset - 2, yPos - structComp.yOffset);
                    }
                }
                else
                {
                    growthTime += 10000;
                }
            }
        }
    }

    public class OakTableBlock : Block
    {
        public OakTableBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Table", "sc:oak_table_block", 350, "sc:oak_table_item", Tool.Axe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

    }

    public class SpruceTableBlock : Block
    {
        public SpruceTableBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Table", "sc:spruce_table_block", 350, "sc:spruce_table_item", Tool.Axe);
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class SandBlock : Block
    {
        public SandBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sand", "sc:sand_block", 150, "sc:sand_item", Tool.Shovel);
            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
            WriteTag(BlockTags.GROUNDS_SAND);
            WriteTag(BlockTags.CAN_FALL);
        }
    }

    public class SandStoneBricksBlock : Block
    {
        public SandStoneBricksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sand Stone Bricks", "sc:sand_stone_bricks_block", 1250, "sc:sand_stone_bricks_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class StoneBricksBlock : Block
    {
        public StoneBricksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Stone Bricks", "sc:stone_bricks_block", 1250, "sc:stone_bricks_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class TinOreBlock : Block
    {
        public TinOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Tin Ore", "sc:tin_ore_block", 1750, "sc:tin_ore_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class TungstenOreBlock : Block
    {
        public TungstenOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Tungsten Ore", "sc:tungsten_ore_block", 1750, "sc:tungsten_ore_item", Tool.Pickaxe);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class YellowFlowerBlock : Block
    {
        public YellowFlowerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Yellow Flower", "sc:yellow_flower_block", 0, "sc:yellow_flower_item", Tool.None);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class GlassBlock : Block
    {
        public GlassBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Glass", "sc:glass_block", 250, "sc:glass_item", Tool.None);
            WriteTag(BlockTags.TOOL_SPECIFIC);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class FarmlandBlock : Block
    {
        public FarmlandBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Farmland", "sc:farmland_block", 150, "sc:farmland_item", Tool.Shovel);
            drops.Add(("sc:dirt_item", 1, 1));
            WriteTag(BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.GROUNDS_FARMLAND);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
        }

        protected override void DoSpecificUpdate()
        {
            if (!HasWaterNearby() || lightLevel == 0) //Farmland needs either water or light nearby
            {
                if (Game.rnd.Next(0, 9) == 0)
                {
                    Replace(BlockRegister.GenerateBlock("sc:dirt_block"));
                }
            }
        }

        public bool HasWaterNearby()
        {
            //Checks 4 blocks to the left and right on the same y level whether they are a water block
            for (int i = 1; i < 5; i++)
            {
                Block blockRight = chunk.GetBlock(xPos + i, yPos);
                Block blockLeft = chunk.GetBlock(xPos - i, yPos);

                if (blockRight != null && blockRight.HasTag(BlockTags.LIQUIDS_WATER) //Blocks to the right
                    || blockLeft != null && blockLeft.HasTag(BlockTags.LIQUIDS_WATER)) //Blocks to the left
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class WoolBlock : Block
    {
        public WoolBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Wool", "sc:wool_block", 150, "sc:wool_item", Tool.None);
        }
    }

    public class PumpkinBlock : Block
    {
        public PumpkinBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Pumpkin", "sc:pumpkin_block", 500, "sc:pumpkin_item", Tool.Axe);
            isSolid = false;
        }
    }

    public class LanternBlock : Block
    {
        public LanternBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Lantern", "sc:lantern_block", 500, "sc:lantern_item", Tool.Pickaxe);
            WriteTag(BlockTags.LIGHTSOURCE);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            isSolid = false;
        }
    }
}


