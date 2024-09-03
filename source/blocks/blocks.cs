using SeeloewenCraft.entity;

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

namespace SeeloewenCraft
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Grass Block", "sc:grass_block", 150, "sc:dirt_item", Tool.Shovel, Images.GrassBlock);
            tags.Add("CanBeFloor");
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(bool isInBackground) : base(isInBackground)
        {
            //lootTable = Game.world.lootTables.stoneLootTable;
            Init("Stone Block", "sc:stone_block", 1250, "sc:stone_block_item", Tool.Pickaxe, Images.StoneBlock);
            lootTable = LootTables.stoneLootTable;
            tags.Add("CanBeFloor");
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Dirt", "sc:dirt_block", 150, "sc:dirt_item", Tool.Shovel, Images.Dirt);
            tags.Add("CanBeFloor");
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Air", "sc:air_block", 150, "sc:air_item", Tool.None, Images.Air);
            isBreakable = false;
            isSolid = false;
            isReplacable = true;
            canBeMovedToBackground = false;
            isLightSource = true;
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Bedrock", "sc:bedrock_block", 150, "sc:bedrock_item", Tool.None, Images.Bedrock);
            isBreakable = false;
            canBeMovedToBackground = false;
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Coal Ore", "sc:coal_ore_block", 1750, "sc:coal_ore_item", Tool.Pickaxe, Images.CoalOre);
            lootTable = LootTables.coalLootTable; 
            dropsOnWrongTool = false;
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Diamond Ore", "sc:diamond_ore_block", 1750, "sc:diamond_item", Tool.Pickaxe, Images.DiamondOre);
            effectiveMaterial = Material.Iron;
            dropsOnWrongTool = false;
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Iron Ore", "sc:iron_ore_block", 1750, "sc:iron_ore_item", Tool.Pickaxe, Images.IronOre);
            effectiveMaterial = Material.Stone;
            dropsOnWrongTool = false;
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Log", "sc:oak_log_block", 350, "sc:oak_log_item", Tool.Axe, Images.OakLog);
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Leaves", "sc:oak_leaves_block", 125, "sc:oak_leaves_item", Tool.None, Images.OakLeaves);
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Log", "sc:spruce_log_block", 350, "sc:spruce_log_item", Tool.Axe, Images.SpruceLog);
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Leaves", "sc:spruce_leaves_block", 125, "sc:spruce_leaves_item", Tool.None, Images.SpruceLeaves);
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Chest", "sc:chest_block", 500, "sc:chest_item", Tool.Axe, Images.Chest);
            hasInventory = true;
            blockInventory = new Inventory(9, 4);
            Game.world.inventoryList.Add(blockInventory);
            hasRightClickAction = true;
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange() && isSolid && hasInventory)
            {
                blockInventory.inventoryGui.SetTop(355);
                blockInventory.inventoryGui.tblHeader.Text = "Chest";
                blockInventory.Show();
                Game.world.player.inventory.inventoryGui.SetTop(20);
                Game.world.player.inventory.Show();
            }
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Magma Block", "sc:magma_block", 750, "sc:magma_block_item", Tool.Pickaxe, Images.MagmaBlock);
            collision = new RectangleCollision(0, 1000, 1, 1000);
        }

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            if(isBackground) base.CheckTouch(startX, startY, endX, endY);
            bool[] touching = new bool[Entity.TOUCHING_STATUS_COUNT];
            touching[Entity.TOUCHING_MAGMA] = true;
            return touching;
        }
    }

    public class TorchBlock : Block
    {
        public TorchBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Torch", "sc:torch_block", 0, "sc:torch_item", Tool.None, Images.Torch);
            isSolid = false;
            canBeMovedToBackground = false;
            isLightSource = true;
        }
    }

    public class PottedCactus_Base : Block
    {
        public PottedCactus_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Potted Cactus Base", "sc:potted_cactus_base", 0, "sc:potted_cactus_item", Tool.None, Images.PottedCactus_Base);
            isBase = true;
            connectedBlocks.Add((0, -1, "sc:potted_cactus_top"));
            collision = new MultipleRectangleCollision([125, 251], [875, 749], [375, 1], [1000, 375]);
        }

        
    }

    public class PottedCactus_Top : Block
    {
        public PottedCactus_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Potted Cactus Top", "sc:potted_cactus_top", 0, null, Tool.None, Images.PottedCactus_Top);
            collision = new RectangleCollision(251, 749, 188, 999);
        }

    }

    public class CraftingTableBlock : Block
    {
        public CraftingTableBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Crafting Table", "sc:crafting_table_block", 500, "sc:crafting_table_item", Tool.Axe, Images.CraftingTable);
            tags.Add("workstation");
            hasRightClickAction = true;

            craftingHandler = new CraftingHandler(this);
            gui = new CraftingTableGui(535, 720, 120, 285, "sc:crafting_table", null, this);
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                gui.Show();
            }
        }

        public override void ShowAdditionalDebugInfo()
        {
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class ChiselerBlock : Block
    {
        public ChiselerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseler", "sc:chiseler_block", 500, "sc:chiseler_item", Tool.Axe, Images.Chiseler);
            tags.Add("workstation");
            hasRightClickAction = true;

            craftingHandler = new CraftingHandler(this);
            gui = new ChiselerGui(535, 720, 120, 285, "sc:chiseler", null, this);
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                gui.Show();
            }
        }

        public override void ShowAdditionalDebugInfo()
        {
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class UnchiselerBlock : Block
    {
        public UnchiselerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Unchiseler", "sc:unchiseler_block", 500, "sc:unchiseler_item", Tool.Axe, Images.Unchiseler);
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 565, 1000);
            gui = new UnchiselerGui(225, 225, 420, 530, "sc:unchiseler");
            blockInventory = gui.inventory;
            hasInventory = true;
            craftingHandler = new CraftingHandler(this);
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                Game.world.player.inventory.inventoryGui.SetTop(25);
                Game.world.player.inventory.Show();
                gui.Show();
            }
        }
    }

    public class CobblestoneBlock : Block
    {
        public CobblestoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cobblestone", "sc:cobblestone_block", 1250, "sc:cobblestone_item", Tool.Pickaxe, Images.CobbleStoneBlock);
            dropsOnWrongTool = false;
        }
    }

    public class SpruceDoor_Base : DoorBlock
    {
        public SpruceDoor_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Door Base", "sc:spruce_door_base", 500, "sc:spruce_door_item", Tool.Axe, Images.SpruceDoor_Closed_Base);
            isBase = true;
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);

            connectedBlocks.Add((0, -1, "sc:spruce_door_top"));
            imgClose = Images.SpruceDoor_Closed_Base.GetTexture();
            imgOpen = Images.SpruceDoor_Open_Base.GetTexture();
        }
    }

    public class SpruceDoor_Top : DoorBlock
    {
        public SpruceDoor_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Door Top", "sc:spruce_door_top", 500, null, Tool.Axe, Images.SpruceDoor_Closed_Top);
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);

            imgClose = Images.SpruceDoor_Closed_Top.GetTexture();
            imgOpen = Images.SpruceDoor_Open_Top.GetTexture();
        }

        public override void RightClickAction(object sender)
        {
            if (!isForeground)
            {
                if (GetBaseBlock() is DoorBlock block)
                {
                    block.RightClickAction(sender);
                }
            }
            else
            {
                if (GetBaseBlock().GetForegroundBlock() is DoorBlock block)
                {
                    block.RightClickAction(sender);
                }
            }
        }
    }

    public class SandStoneBlock : Block
    {
        public SandStoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sand Stone", "sc:sand_stone_block", 1250, "sc:sand_stone_item", Tool.Pickaxe, Images.SandStone);
            dropsOnWrongTool = false;
        }
    }

    public class OakPlanksBlock : Block
    {
        public OakPlanksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Plank", "sc:oak_planks_block", 500, "sc:oak_planks_item", Tool.Axe, Images.OakPlanks);
        }
    }

    public class SprucePlanksBlock : Block
    {
        public SprucePlanksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Planks", "sc:spruce_planks_block", 500, "sc:spruce_planks_item", Tool.Axe, Images.SprucePlanks);
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
                = cactusCollision.CheckCollision(Direction.RIGHT, startX, endX+1, startY, endY+1);
            return touchingStatus;
        }

        protected CactusBlock(bool isInBackground) : base(isInBackground) { }


    }

    public class Cactus_TopFruit : CactusBlock
    {
        public Cactus_TopFruit(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Fruit", "sc:cactus_top_fruit", 250, "sc:cactus_top_fruit_item", Tool.Axe, Images.Cactus_Top_Fruit);
            collision = new RectangleCollision(191, 809, 631, 999);
            cactusCollision = new RectangleCollision(190, 810, 630, 1000);
        }
    }

    public class Cactus_Vertical : CactusBlock
    {
        public Cactus_Vertical(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Vertical", "sc:cactus_vertical", 250, null, Tool.Axe, Images.Cactus_Vertical);
            collision = new RectangleCollision(191, 809, 1, 999);
            cactusCollision = new RectangleCollision(190, 810, 0, 1000);
        }
    }

    public class Cactus_Top : CactusBlock
    {
        public Cactus_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top", "sc:cactus_top", 250, null, Tool.Axe, Images.Cactus_Top);
            collision = new RectangleCollision(191, 809, 631, 999);
            cactusCollision = new RectangleCollision(190, 810, 630, 1000);
        }
    }

    public class Cactus_TopLeft : CactusBlock
    {
        public Cactus_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Left", "sc:cactus_top_left", 250, null, Tool.Axe, Images.Cactus_TopLeft);
            collision = new MultipleRectangleCollision([191, 1], [809, 189], [1, 191], [809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [0, 190], [810, 810]);
        }
    }

    public class Cactus_TopRight : CactusBlock
    {
        public Cactus_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Right", "sc:cactus_top_right", 250, null, Tool.Axe, Images.Cactus_TopRight);
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [1, 191], [809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [0, 190], [810, 810]);
        }
    }

    public class Cactus_Cross : CactusBlock
    {
        public Cactus_Cross(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Cross", "sc:cactus_cross", 250, null, Tool.Axe, Images.Cactus_Cross);
            collision = new MultipleRectangleCollision([191, 811, 1], [809, 999, 189], [1, 191, 191], [999, 809, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810, 0], [810, 1000, 190], [0, 190, 190], [1000, 810, 810]);
        }
    }

    public class Cactus_Horizontal : CactusBlock
    {
        public Cactus_Horizontal(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Horizontal", "sc:cactus_horizontal", 250, null, Tool.Axe, Images.Cactus_Horizontal);
            collision = new RectangleCollision(1, 999, 189, 811);
            cactusCollision = new RectangleCollision(0, 1000, 190, 810);
        }
    }

    public class Cactus_BottomLeft : CactusBlock
    {
        public Cactus_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Bottom Left", "sc:cactus_bottom_left", 250, null, Tool.Axe, Images.Cactus_BottomLeft);
            collision = new MultipleRectangleCollision([191, 1], [809, 191], [191, 191], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 0], [810, 190], [190, 190], [1000, 810]);
        }
    }

    public class Cactus_BottomRight : CactusBlock
    {
        public Cactus_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Bottom Right", "sc:cactus_bottom_right", 250, null, Tool.Axe, Images.Cactus_BottomRight);
            collision = new MultipleRectangleCollision([191, 811], [809, 999], [189, 189], [999, 809]);
            cactusCollision = new MultipleRectangleCollision([190, 810], [810, 1000], [190, 190], [1000, 810]);
        }
    }

    public class OakDoor_Base : DoorBlock
    {
        public OakDoor_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Door Base", "sc:oak_door_base", 500, "sc:oak_door_item", Tool.Axe, Images.OakDoor_Closed_Base);
            isBase = true;
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);

            connectedBlocks.Add((0, -1, "sc:oak_door_top"));
            imgClose = Images.OakDoor_Closed_Base.GetTexture();
            imgOpen = Images.OakDoor_Open_Base.GetTexture();
        }
    }

    public class OakDoor_Top : DoorBlock
    {
        public OakDoor_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Door Top", "sc:oak_door_top", 500, null, Tool.Axe, Images.OakDoor_Closed_Top);
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);

            imgClose = Images.OakDoor_Closed_Top.GetTexture();
            imgOpen = Images.OakDoor_Open_Top.GetTexture();
        }

        public override void RightClickAction(object sender)
        {
            if (!isForeground)
            {
                if (GetBaseBlock() is DoorBlock block)
                {
                    block.RightClickAction(sender);
                }
            }
            else
            {
                if (GetBaseBlock().GetForegroundBlock() is DoorBlock block)
                {
                    block.RightClickAction(sender);
                }
            }
        }
    }

    public class OakTrapDoor : DoorBlock
    {
        public OakTrapDoor(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Trapdoor Base", "sc:oak_trapdoor", 500, "sc:oak_trapdoor_item", Tool.Axe, Images.OakTrapdoor_Closed);
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 0, 150);

            imgClose = Images.OakTrapdoor_Closed.GetTexture();
            imgOpen = Images.OakTrapdoor_Open.GetTexture();
        }
    }

    public class SpruceTrapDoor : DoorBlock
    {
        public SpruceTrapDoor(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Trapdoor Base", "sc:spruce_trapdoor", 500, "sc:spruce_trapdoor_item", Tool.Axe, Images.SpruceTrapdoor_Closed);
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 0, 150);

            imgClose = Images.SpruceTrapdoor_Closed.GetTexture();
            imgOpen = Images.SpruceTrapdoor_Open.GetTexture();
        }
    }

    public class FurnaceBlock : Block
    {
        public FurnaceBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Furnace", "sc:furnace_block", 500, "sc:furnace_item", Tool.Pickaxe, Images.Furnace_Idle);
            tags.Add("workstation");
            hasRightClickAction = true;
            dropsOnWrongTool = false;

            craftingHandler = new CraftingHandler(this);
            gui = new FurnaceGui(535, 720, 120, 285, "sc:furnace", null, this);
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                gui.Show();
            }
        }

        public override void ShowAdditionalDebugInfo()
        {
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class OakChair_Base : DoorBlock
    {
        public OakChair_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Chair Base", "sc:oak_chair_base", 0, "sc:oak_chair_item", Tool.Axe, Images.OakChair_Bottom);
            isBase = true;
            isSolid = false;

            connectedBlocks.Add((0, -1, "sc:oak_chair_top"));
        }
    }

    public class OakChair_Top : DoorBlock
    {
        public OakChair_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Chair Top", "sc:oak_chair_top", 0, null, Tool.Axe, Images.OakChair_Top);
            isSolid = false;
        }
    }

    public class SpruceChair_Base : DoorBlock
    {
        public SpruceChair_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Chair Base", "sc:spruce_chair_base", 0, "sc:spruce_chair_item", Tool.Axe, Images.SpruceChair_Bottom);
            isBase = true;
            isSolid = false;

            connectedBlocks.Add((0, -1, "sc:spruce_chair_top"));
        }
    }

    public class SpruceChair_Top : DoorBlock
    {
        public SpruceChair_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Chair Top", "sc:spruce_chair_top", 0, null, Tool.Axe, Images.SpruceChair_Top);
            isSolid = false;
        }
    }

    public class ArcheologyPot_Base : Block
    {
        public ArcheologyPot_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Archeology Pot Base", "sc:archeology_pot_base", 100, "sc:archeology_pot_item", Tool.Pickaxe, Images.ArcheologyPot_Base);
            isBase = true;
            isSolid = false;

            connectedBlocks.Add((0, -1, "sc:archeology_pot_top"));
        }
    }

    public class ArcheologyPot_Top : Block
    {
        public ArcheologyPot_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Archeology Pot Top", "sc:archeology_pot_top", 100, null, Tool.Pickaxe, Images.ArcheologyPot_Top);
            isSolid = false;
        }
    }

    public class AmethystOreBlock : Block
    {
        public AmethystOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Amethyst Ore", "sc:amethyst_ore_block", 1750, "sc:amethyst_item", Tool.Pickaxe, Images.AmethystOre);
            effectiveMaterial = Material.Diamond;
            dropsOnWrongTool = false;
        }
    }

    public class AnvilBlock : Block
    {
        public AnvilBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Anvil", "sc:anvil_block", 2000, "sc:anvil_item", Tool.Pickaxe, Images.Anvil);
            tags.Add("workstation");
            hasRightClickAction = true;
            dropsOnWrongTool = false;

            craftingHandler = new CraftingHandler(this);
            gui = new AnvilGui(535, 720, 120, 285, "sc:anvil", null, this);
            collision = new RectangleCollision(0, 1000, 190, 1000);
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                gui.Show();
            }
        }

        public override void ShowAdditionalDebugInfo()
        {
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class BarrelBlock : Block
    {
        public BarrelBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Barrel", "sc:barrel_block", 500, "sc:barrel_item", Tool.Axe, Images.Barrel);
            hasInventory = true;
            blockInventory = new Inventory(9, 2);
            Game.world.inventoryList.Add(blockInventory);
            hasRightClickAction = true;
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange() && isSolid && hasInventory)
            {
                blockInventory.inventoryGui.SetTop(355);
                blockInventory.inventoryGui.tblHeader.Text = "Barrel";
                blockInventory.Show();
                Game.world.player.inventory.inventoryGui.SetTop(20);
                Game.world.player.inventory.Show();
            }
        }
    }

    public class BlueFlowerBlock : Block
    {
        public BlueFlowerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Blue Flower", "sc:blue_flower_block", 0, "sc:blue_flower_item", Tool.None, Images.BlueFlower);
            isSolid = false;
        }
    }

    public class BoneBlock : Block
    {
        public BoneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Bone Block", "sc:bone_block", 750, "sc:bone_block_item", Tool.None, Images.BoneBlock);
        }
    }

    public class CactusFruitBlock : Block
    {
        public CactusFruitBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Fruit", "sc:cactus_fruit_block", 0, "sc:cactus_fruit_item", Tool.None, Images.CactusFruit);
            isSolid = false;
        }
    }

    public class CandleBlock : Block
    {
        public CandleBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Candle", "sc:candle_block", 0, "sc:candle_item", Tool.None, Images.Candle);
            isSolid = false;
        }
    }

    public class CopperOreBlock : Block
    {
        public CopperOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Copper Ore", "sc:copper_ore_block", 1750, "sc:copper_ore_block", Tool.Pickaxe, Images.CopperOre);
            effectiveMaterial = Material.Tin;
            dropsOnWrongTool = false;
        }
    }

    public class DeadBushBlock : Block
    {
        public DeadBushBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Dead Bush", "sc:dead_bush_block", 0, "sc:dead_bush_item", Tool.None, Images.DeadBush);
            isSolid = false;
        }
    }

    public class EmeraldOreBlock : Block
    {
        public EmeraldOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Emerald Ore", "sc:emerald_ore_block", 1750, "sc:emerald_item", Tool.Pickaxe, Images.Emerald);
            effectiveMaterial = Material.Diamond;
            dropsOnWrongTool = false;
        }
    }

    public class FlowerPotBlock : Block
    {
        public FlowerPotBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Flower Pot", "sc:flower_pot_block", 200, "sc:flower_pot_item", Tool.Pickaxe, Images.FlowerPot);
            isSolid = false;
        }
    }

    public class GoldOreBlock : Block
    {
        public GoldOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Gold Ore", "sc:gold_ore_block", 1750, "sc:gold_ore_item", Tool.Pickaxe, Images.GoldOre);
            effectiveMaterial = Material.Tin;
            dropsOnWrongTool = false;
        }
    }

    public class Grass : Block
    {
        public Grass(bool isInBackground) : base(isInBackground)
        {
            Init("Grass", "sc:grass", 0, "sc:grass_item", Tool.None, Images.Grass);
            isSolid = false;
        }
    }

    public class IronGatesBlock : Block
    {
        public IronGatesBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Iron Gates", "sc:iron_gates_block", 2000, "sc:iron_gates_item", Tool.Pickaxe, Images.IronGates);
            effectiveMaterial = Material.Stone;
            dropsOnWrongTool = false;
        }
    }

    public class LadderBlock : Block
    {
        public LadderBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Ladder", "sc:ladder_block", 250, "sc:ladder_item", Tool.Axe, Images.Ladder);
            isSolid = false;
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
            Init("Mossy Cobblestone", "sc:mossy_cobblestone_block", 1250, "sc:mossy_cobblestone_item", Tool.Pickaxe, Images.MossyCobblestone);
            dropsOnWrongTool = false;
        }
    }

    public class OakSaplingBlock : Block
    {
        public OakSaplingBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Sapling", "sc:oak_sapling_block", 0, "sc:oak_sapling_item", Tool.None, Images.OakSapling);
            isSolid = false;
        }
    }

    public class SpruceSaplingBlock : Block
    {
        public SpruceSaplingBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Sapling", "sc:spruce_sapling_block", 0, "sc:spruce_sapling_item", Tool.None, Images.SpruceSapling);
            isSolid = false;
        }
    }

    public class OakTableBlock : Block
    {
        public OakTableBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Oak Table", "sc:oak_table_block", 350, "sc:oak_table_item", Tool.Axe, Images.OakTable);
            isSolid = false;
        }
    }

    public class SpruceTableBlock : Block
    {
        public SpruceTableBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Table", "sc:spruce_table_block", 350, "sc:spruce_table_item", Tool.Axe, Images.SpruceTable);
            isSolid = false;
        }
    }

    public class SandBlock : Block
    {
        public SandBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sand", "sc:sand_block", 150, "sc:sand_item", Tool.Shovel, Images.Sand);
        }
    }

    public class SandStoneBricksBlock : Block
    {
        public SandStoneBricksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sand Stone Bricks", "sc:sand_stone_bricks_block", 1250, "sc:sand_stone_bricks_item", Tool.Pickaxe, Images.SandStoneBricks);
            dropsOnWrongTool = false;
        }
    }

    public class StoneBricksBlock : Block
    {
        public StoneBricksBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Stone Bricks", "sc:stone_bricks_block", 1250, "sc:stone_bricks_item", Tool.Pickaxe, Images.StoneBricks);
            dropsOnWrongTool = false;
        }
    }

    public class TinOreBlock : Block
    {
        public TinOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Tin Ore", "sc:tin_ore_block", 1750, "sc:tin_ore_item", Tool.Pickaxe, Images.TinOre);
            dropsOnWrongTool = false;
        }
    }

    public class TungstenOreBlock : Block
    {
        public TungstenOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Tungsten Ore", "sc:tungsten_ore_block", 1750, "sc:tungsten_ore_item", Tool.Pickaxe, Images.TungstenOre);
            dropsOnWrongTool = false;
        }
    }

    public class YellowFlowerBlock : Block
    {
        public YellowFlowerBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Yellow Flower", "sc:yellow_flower_block", 0, "sc:yellow_flower_item", Tool.None, Images.YellowFlower);
            isSolid = false;
        }
    }

    public class GlassBlock : Block
    {
        public GlassBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Glass", "sc:glass_block", 250, "sc:glass_item", Tool.None, Images.Glass);
            dropsOnWrongTool = false;
        }
    }
}
