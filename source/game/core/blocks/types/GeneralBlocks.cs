using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world.generation;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using System.Collections.Generic;

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
        public GrassBlock() : base("Grass Block", "sc:grass_block", 150, "sc:grass_block_item", Tool.Shovel)
        {
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
                        candidate.SetBlock(BlockRegister.Get("sc:grass_block"));
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

                    Block newBlock = BlockRegister.Get(cropId);
                    newBlock.needsGround = (true, BlockTags.GROUNDS_DIRT);
                    blockAbove.SetBlock(newBlock);
                }
            }
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock() : base("Stone Block", "sc:stone_block", 1250, "sc:stone_block_item", Tool.Pickaxe)
        {
            drops.Add(("sc:rock_item", 1, 4));

            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock() : base("Dirt", "sc:dirt_block", 150, "sc:dirt_item", Tool.Shovel)
        {
            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
            WriteTag(BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.SCYTHEABLE);
        }
    }

    public class AirBlock : Block
    {
        public AirBlock() : base("Air", "sc:air_block", 150, "sc:air_item" )
        {
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.REPLACEABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            isSolid = false;
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock() : base("Bedrock", "sc:bedrock_block", 150, "sc:bedrock_item" )
        {
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock() : base("Coal Ore", "sc:coal_ore_block", 1750, "sc:coal_ore_item", Tool.Pickaxe)
        {
            drops.Add(("sc:coal_item", 1, 3));
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock() : base("Diamond Ore", "sc:diamond_ore_block", 1750, "sc:diamond_ore_item", Tool.Pickaxe)
        {
            drops.Add(("sc:diamond_item", 1, 1));
            effectiveMaterial = Material.Iron;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock() : base("Iron Ore", "sc:iron_ore_block", 1750, "sc:iron_ore_item", Tool.Pickaxe)
        {
            effectiveMaterial = Material.Stone;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock() : base("Oak Log", "sc:oak_log_block", 350, "sc:oak_log_item", Tool.Axe)
        {
            WriteTag(BlockTags.TYPES_LOG);
        }
    }

    public class OakLeavesBlock : LeavesBlock
    {
        public OakLeavesBlock() : base("Oak Leaves", "sc:oak_leaves_block", "sc:oak_leaves_item" )
        {
            lootTable = (LootTables.oakTreeLootTable, 1, 1);
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock() : base("Spruce Log", "sc:spruce_log_block", 350, "sc:spruce_log_item", Tool.Axe)
        {
            WriteTag(BlockTags.TYPES_LOG);
        }
    }

    public class SpruceLeavesBlock : LeavesBlock
    {
        public SpruceLeavesBlock() : base("Spruce Leaves", "sc:spruce_leaves_block", "sc:spruce_leaves_item" )
        {
            lootTable = (LootTables.spruceTreeLootTable, 1, 1);
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock() : base("Chest", "sc:chest_block", 500, "sc:chest_item", Tool.Axe)
        {
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
        public MagmaBlock() : base("Magma Block", "sc:magma_block", 750, "sc:magma_block_item", Tool.Pickaxe)
        {
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
        public TorchBlock() : base("Torch", "sc:torch_block", 0, "sc:torch_item" )
        {
            isSolid = false;
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.LIGHTSOURCE);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class PottedCactus_Base : Block
    {
        public PottedCactus_Base() : base("Potted Cactus Base", "sc:potted_cactus_base", 0, "sc:potted_cactus_item" )
        {
            isBase = true;
            connectedBlocks.Add((0, -1, "sc:potted_cactus_top"));
            collision = new MultipleRectangleCollision([125, 251], [875, 749], [375, 1], [1000, 375]);
            needsGround = (true, "");
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class PottedCactus_Top : Block
    {
        public PottedCactus_Top() : base("Potted Cactus Top", "sc:potted_cactus_top", 0 )
        {
            collision = new RectangleCollision(251, 749, 188, 999);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            baseBlock = (0, 1);
        }
    }

    public class CraftingTableBlock : Block
    {
        public CraftingTableBlock() : base("Crafting Table", "sc:crafting_table_block", 500, "sc:crafting_table_item", Tool.Axe)
        {
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
        public ChiselerBlock() : base("Chiseler", "sc:chiseler_block", 500, "sc:chiseler_item", Tool.Axe)
        {
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

        public UnchiselerBlock() : base("Unchiseler", "sc:unchiseler_block", 500, "sc:unchiseler_item", Tool.Axe)
        {
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
        public CobblestoneBlock() : base("Cobblestone", "sc:cobblestone_block", 1250, "sc:cobblestone_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SpruceDoor_Base : DoorBlock
    {
        public SpruceDoor_Base() : base("Spruce Door Base", "sc:spruce_door_base", "sc:spruce_door_item")
        {
            WriteTag(BlockTags.RIGHTCLICKABLE);
            isBase = true;
            needsGround = (true, "");
            collision = new RectangleCollision(720, 1000, 0, 1000);

            connectedBlocks.Add((0, -1, "sc:spruce_door_top"));
        }
    }

    public class SpruceDoor_Top : DoorBlock
    {
        public SpruceDoor_Top() : base("Spruce Door Top", "sc:spruce_door_top")
        {
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(720, 1000, 0, 1000);
            baseBlock = (0, 1);
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
        public SandStoneBlock() : base("Sand Stone", "sc:sand_stone_block", 1250, "sc:sand_stone_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class OakPlanksBlock : Block
    {
        public OakPlanksBlock() : base("Oak Plank", "sc:oak_planks_block", 500, "sc:oak_planks_item", Tool.Axe)
        {
        }
    }

    public class SprucePlanksBlock : Block
    {
        public SprucePlanksBlock() : base("Spruce Planks", "sc:spruce_planks_block", 500, "sc:spruce_planks_item", Tool.Axe)
        {
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

        protected CactusBlock(string name, string id, string itemId = null) : base(name, id, 250, itemId, Tool.Axe)
        {
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class Cactus_TopFruit : CactusBlock
    {
        public Cactus_TopFruit() : base("Cactus Top Fruit", "sc:cactus_top_fruit", "sc:cactus_top_fruit_item")
        {
            drops.Add(("sc:cactus_fruit_item", 1, 1));
            collision = new RectangleCollision(191, 809, 631, 999);
            cactusCollision = new RectangleCollision(190, 810, 630, 1000);
        }
    }

    public class Cactus_Vertical : CactusBlock
    {
        public Cactus_Vertical() : base("Cactus Vertical", "sc:cactus_vertical")
        {
            collision = new RectangleCollision(191, 809, 1, 999);
            cactusCollision = new RectangleCollision(190, 810, 0, 1000);
        }
    }

    public class Cactus_Top : CactusBlock
    {
        public Cactus_Top() : base("Cactus Top", "sc:cactus_top")
        {
            collision = new RectangleCollision(191, 809, 631, 999);
            cactusCollision = new RectangleCollision(190, 810, 630, 1000);
        }
    }

    public class Cactus_TopLeft : CactusBlock
    {
        public Cactus_TopLeft() : base("Cactus Top Left", "sc:cactus_top_left")
        {
            collision = new MultipleRectangleCollision([191, 1], [809, 189], [1, 191], [809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [0, 190], [810, 810]);
        }
    }

    public class Cactus_TopRight : CactusBlock
    {
        public Cactus_TopRight() : base("Cactus Top Right", "sc:cactus_top_right")
        {
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [1, 191], [809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [0, 190], [810, 810]);
        }
    }

    public class Cactus_Cross : CactusBlock
    {
        public Cactus_Cross() : base("Cactus Cross", "sc:cactus_cross")
        {
            collision = new MultipleRectangleCollision([191, 811, 1], [809, 999, 189], [1, 191, 191], [999, 809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810, 0], [810, 1000, 190], [0, 190, 190], [1000, 810, 810]);
        }
    }

    public class Cactus_Right : CactusBlock
    {
        public Cactus_Right() : base("Cactus Right", "sc:cactus_right")
        {
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [1, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [0, 190], [1000, 810]);
        }
    }

    public class Cactus_Left : CactusBlock
    {
        public Cactus_Left() : base("Cactus Left", "sc:cactus_left")
        {
            collision = new MultipleRectangleCollision([191, 1], [809, 189], [1, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [0, 190], [1000, 810]);
        }
    }

    public class Cactus_Horizontal : CactusBlock
    {
        public Cactus_Horizontal() : base("Cactus Horizontal", "sc:cactus_horizontal")
        {
            collision = new RectangleCollision(1, 999, 189, 811);
            cactusCollision = new RectangleCollision(0, 1000, 190, 810);
        }
    }

    public class Cactus_BottomLeft : CactusBlock
    {
        public Cactus_BottomLeft() : base("Cactus Bottom Left", "sc:cactus_bottom_left")
        {
            collision = new MultipleRectangleCollision([191, 1], [809, 191], [191, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [190, 190], [1000, 810]);
        }
    }

    public class Cactus_BottomRight : CactusBlock
    {
        public Cactus_BottomRight() : base("Cactus Bottom Right", "sc:cactus_bottom_right")
        {
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [189, 189], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [190, 190], [1000, 810]);
        }
    }

    public class OakDoor_Base : DoorBlock
    {
        public OakDoor_Base() : base("Oak Door Base", "sc:oak_door_base", "sc:oak_door_item")
        {
            isBase = true;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(720, 1000, 0, 1000);

            connectedBlocks.Add((0, -1, "sc:oak_door_top"));
        }
    }

    public class OakDoor_Top : DoorBlock
    {
        public OakDoor_Top() : base("Oak Door Top", "sc:oak_door_top")
        {
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(720, 1000, 0, 1000);
            baseBlock = (0, 1);
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
        public OakTrapDoor() : base("Oak Trapdoor Base", "sc:oak_trapdoor", "sc:oak_trapdoor_item")
        {
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(0, 1000, 0, 150);
        }
    }

    public class SpruceTrapDoor : DoorBlock
    {
        public SpruceTrapDoor() : base("Spruce Trapdoor Base", "sc:spruce_trapdoor", "sc:spruce_trapdoor_item")
        {
            WriteTag(BlockTags.RIGHTCLICKABLE);
            collision = new RectangleCollision(0, 1000, 0, 150);
        }
    }
    public class FurnaceBlock : Block
    {
        public FurnaceBlock() : base("Furnace", "sc:furnace_block", 500, "sc:furnace_item", Tool.Pickaxe)
        {
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

    public class OakChair_block : Block
    {
        public OakChair_block() : base("Oak Chair Base", "sc:oak_chair_block", 0, "sc:oak_chair_item", Tool.Axe)
        {
            isBase = true;
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);

            connectedBlocks.Add((0, -1, "sc:oak_chair_top"));
        }
    }

    public class OakChair_Top : Block
    {
        public OakChair_Top() : base("Oak Chair Top", "sc:oak_chair_top", 0, tool: Tool.Axe)
        {
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class SpruceChair_block : Block
    {
        public SpruceChair_block() : base("Spruce Chair Base", "sc:spruce_chair_block", 0, "sc:spruce_chair_item", Tool.Axe)
        {
            isBase = true;
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);

            connectedBlocks.Add((0, -1, "sc:spruce_chair_top"));
        }
    }

    public class SpruceChair_Top : Block
    {
        public SpruceChair_Top() : base("Spruce Chair Top", "sc:spruce_chair_top", 0, tool: Tool.Axe)
        {
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class ArcheologyPot_Base : Block
    {
        public ArcheologyPot_Base() : base("Archeology Pot Base", "sc:archeology_pot_base", 100, "sc:archeology_pot_item", Tool.Pickaxe)
        {
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
        public ArcheologyPot_Top() : base("Archeology Pot Top", "sc:archeology_pot_top", 100, tool: Tool.Pickaxe)
        {
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            baseBlock = (0, 1);
        }
    }

    public class AmethystOreBlock : Block
    {
        public AmethystOreBlock() : base("Amethyst Ore", "sc:amethyst_ore_block", 1750, "sc:amethyst_ore_item", Tool.Pickaxe)
        {
            drops.Add(("sc:amethyst_item", 1, 1));
            effectiveMaterial = Material.Diamond;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class AnvilBlock : Block
    {
        public AnvilBlock() : base("Anvil", "sc:anvil_block", 2000, "sc:anvil_item", Tool.Pickaxe)
        {
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
        public BarrelBlock() : base("Barrel", "sc:barrel_block", 500, "sc:barrel_item", Tool.Axe)
        {
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
        public BlueFlowerBlock() : base("Blue Flower", "sc:blue_flower_block", 0, "sc:blue_flower_item" )
        {
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class BoneBlock : Block
    {
        public BoneBlock() : base("Bone Block", "sc:bone_block", 750, "sc:bone_block_item" )
        {
        }
    }

    public class CactusFruitBlock : CropBlock
    {
        public CactusFruitBlock() : base("Cactus Fruit", "sc:cactus_fruit_block", 300000, 500000, "sc:cactus_fruit_item" )
        {
            needsGround = (true, BlockTags.GROUNDS_SAND);
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
        public CandleBlock() : base("Candle", "sc:candle_block", 0, "sc:candle_item" )
        {
            WriteTag(BlockTags.LIGHTSOURCE);
            isSolid = false;
            needsGround = (true, "");
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class CopperOreBlock : Block
    {
        public CopperOreBlock() : base("Copper Ore", "sc:copper_ore_block", 1750, "sc:copper_ore_block", Tool.Pickaxe)
        {
            effectiveMaterial = Material.Tin;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class DeadBushBlock : Block
    {
        public DeadBushBlock() : base("Dead Bush", "sc:dead_bush_block", 0, "sc:dead_bush_item" )
        {
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_SAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class EmeraldOreBlock : Block
    {
        public EmeraldOreBlock() : base("Emerald Ore", "sc:emerald_ore_block", 1750, "sc:emerald_ore_item", Tool.Pickaxe)
        {
            drops.Add(("sc:emerald_item", 1, 1));
            effectiveMaterial = Material.Diamond;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class FlowerPotBlock : Block
    {
        public FlowerPotBlock() : base("Flower Pot", "sc:flower_pot_block", 200, "sc:flower_pot_item", Tool.Pickaxe)
        {
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class GoldOreBlock : Block
    {
        public GoldOreBlock() : base("Gold Ore", "sc:gold_ore_block", 1750, "sc:gold_ore_item", Tool.Pickaxe)
        {
            effectiveMaterial = Material.Tin;
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class Grass : Block
    {
        public Grass() : base("Grass", "sc:grass", 0, "sc:grass_item" )
        {
            lootTable = (LootTables.grassLootTable, 1, 1);
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class IronGatesBlock : Block
    {
        public IronGatesBlock() : base("Iron Gates", "sc:iron_gates_block", 2000, "sc:iron_gates_item", Tool.Pickaxe)
        {
            effectiveMaterial = Material.Stone;
            WriteTag(BlockTags.TOOL_SPECIFIC);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class LadderBlock : Block
    {
        public LadderBlock() : base("Ladder", "sc:ladder_block", 250, "sc:ladder_item", Tool.Axe)
        {
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
        public MossyCobblestoneBlock() : base("Mossy Cobblestone", "sc:mossy_cobblestone_block", 1250, "sc:mossy_cobblestone_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class OakSaplingBlock : CropBlock
    {
        public OakSaplingBlock() : base("Oak Sapling", "sc:oak_sapling_block", 600000, 1200000, "sc:oak_sapling_item" )
        {
            needsGround = (true, BlockTags.GROUNDS_DIRT);
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
        public SpruceSaplingBlock() : base("Spruce Sapling", "sc:spruce_sapling_block", 600000, 1200000, "sc:tree_sapling_item" )
        {
            needsGround = (true, "ground/plant");
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
        public OakTableBlock() : base("Oak Table", "sc:oak_table_block", 350, "sc:oak_table_item", Tool.Axe)
        {
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

    }

    public class SpruceTableBlock : Block
    {
        public SpruceTableBlock() : base("Spruce Table", "sc:spruce_table_block", 350, "sc:spruce_table_item", Tool.Axe)
        {
            isSolid = false;
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class SandBlock : Block
    {
        public SandBlock() : base("Sand", "sc:sand_block", 150, "sc:sand_item", Tool.Shovel)
        {
            WriteTag(BlockTags.CAN_BE_FLOOR_SPAWNING);
            WriteTag(BlockTags.GROUNDS_SAND);
            WriteTag(BlockTags.CAN_FALL);
        }
    }

    public class SandStoneBricksBlock : Block
    {
        public SandStoneBricksBlock() : base("Sand Stone Bricks", "sc:sand_stone_bricks_block", 1250, "sc:sand_stone_bricks_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class StoneBricksBlock : Block
    {
        public StoneBricksBlock() : base("Stone Bricks", "sc:stone_bricks_block", 1250, "sc:stone_bricks_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class TinOreBlock : Block
    {
        public TinOreBlock() : base("Tin Ore", "sc:tin_ore_block", 1750, "sc:tin_ore_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class TungstenOreBlock : Block
    {
        public TungstenOreBlock() : base("Tungsten Ore", "sc:tungsten_ore_block", 1750, "sc:tungsten_ore_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class YellowFlowerBlock : Block
    {
        public YellowFlowerBlock() : base("Yellow Flower", "sc:yellow_flower_block", 0, "sc:yellow_flower_item" )
        {
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class GlassBlock : Block
    {
        public GlassBlock() : base("Glass", "sc:glass_block", 250, "sc:glass_item" )
        {
            WriteTag(BlockTags.TOOL_SPECIFIC);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class FarmlandBlock : Block
    {
        public FarmlandBlock() : base("Farmland", "sc:farmland_block", 150, "sc:farmland_item", Tool.Shovel)
        {
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
                    Replace(BlockRegister.Get("sc:dirt_block"));
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
        public WoolBlock() : base("Wool", "sc:wool_block", 150, "sc:wool_item" )
        {
        }
    }

    public class PumpkinBlock : Block
    {
        public PumpkinBlock() : base("Pumpkin", "sc:pumpkin_block", 500, "sc:pumpkin_item", Tool.Axe)
        {
            isSolid = false;
        }
    }

    public class LanternBlock : Block
    {
        public LanternBlock() : base("Lantern", "sc:lantern_block", 500, "sc:lantern_item", Tool.Pickaxe)
        {
            WriteTag(BlockTags.LIGHTSOURCE);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            isSolid = false;
        }
    }
}


