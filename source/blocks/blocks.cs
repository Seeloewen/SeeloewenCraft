using SeeloewenCraft.entity;

namespace SeeloewenCraft
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Grass Block", "sc:grass_block", 150, "sc:grass_block_item", Tool.Shovel, Images.GrassBlock);
            tags.Add("CanBeFloor");
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(bool isInBackground) : base(isInBackground)
        {
            //lootTable = Game.world.lootTables.stoneLootTable;
            Init("Stone Block", "sc:stone_block", 1250, "sc:stone_block_item", Tool.Pickaxe, Images.StoneBlock);
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
            Init("Coal Ore", "sc:coal_ore_block", 1500, "sc:coal_ore_item", Tool.Pickaxe, Images.CoalOre);
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Diamond Ore", "sc:diamond_ore_block", 2000, "sc:diamond_ore_item", Tool.Pickaxe, Images.DiamondOre);
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Iron Ore", "sc:iron_ore_block", 1750, "sc:iron_ore_item", Tool.Pickaxe, Images.IronOre);
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
            Init("Spruce Log", "sc:spruce_log_block", 300, "sc:spruce_log_item", Tool.Axe, Images.SpruceLog);
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

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            touchingStatus[Entity.TOUCHING_CACTUS] =
                (250 < endX) && (750 > startX)
                && (0 < endY) && (500 > startY);
            return touchingStatus;
        }
    }


    public class PottedCactus_Top : Block
    {
        public PottedCactus_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Potted Cactus Top", "sc:potted_cactus_top", 0, null, Tool.None, Images.PottedCactus_Top);
            collision = new RectangleCollision(251, 749, 188, 999);
        }

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            touchingStatus[Entity.TOUCHING_CACTUS] =
                (250 <= endX) && (750 >= startX)
                && (187 <= endY) && (1000 >= startY);
            return touchingStatus;
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
            blockInventory = gui.inventory;
            hasInventory = true;

            craftingHandler = new CraftingHandler(this);
            gui = new UnchiselerGui(225, 225, 420, 530, "sc:unchiseler");
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

    public class Cactus_TopFruit : Block
    {
        public Cactus_TopFruit(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Fruit", "sc:cactus_top_fruit", 250, "sc:cactus_fruit_item", Tool.Axe, Images.Cactus_Top_Fruit);
        }
    }

    public class Cactus_Vertical : Block
    {
        public Cactus_Vertical(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Vertical", "sc:cactus_vertical", 250, null, Tool.Axe, Images.Cactus_Vertical);
        }
    }

    public class Cactus_Top : Block
    {
        public Cactus_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top", "sc:cactus_top", 250, null, Tool.Axe, Images.Cactus_Top);
        }
    }

    public class Cactus_TopLeft : Block
    {
        public Cactus_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Left", "sc:cactus_top_left", 250, null, Tool.Axe, Images.Cactus_TopLeft);
        }
    }

    public class Cactus_TopRight : Block
    {
        public Cactus_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Top Right", "sc:cactus_top_right", 250, null, Tool.Axe, Images.Cactus_TopRight);
        }
    }

    public class Cactus_Cross : Block
    {
        public Cactus_Cross(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Cross", "sc:cactus_cross", 250, null, Tool.Axe, Images.Cactus_Cross);
        }
    }

    public class Cactus_Horizontal : Block
    {
        public Cactus_Horizontal(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Horizontal", "sc:cactus_horizontal", 250, null, Tool.Axe, Images.Cactus_Horizontal);
        }
    }

    public class Cactus_BottomLeft : Block
    {
        public Cactus_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Bottom Left", "sc:cactus_bottom_left", 250, null, Tool.Axe, Images.Cactus_BottomLeft);
        }
    }

    public class Cactus_BottomRight : Block
    {
        public Cactus_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Cactus Bottom Right", "sc:cactus_bottom_right", 250, null, Tool.Axe, Images.Cactus_BottomRight);
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
            Init("Oak Trapdoor Base", "sc:oak_trapdoor_base", 500, "sc:oak_trapdoor_item", Tool.Axe, Images.OakTrapdoor_Closed);
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 720, 1000);

            imgClose = Images.OakTrapdoor_Closed.GetTexture();
            imgOpen = Images.OakTrapdoor_Open.GetTexture();
        }
    }

    public class SpruceTrapDoor : DoorBlock
    {
        public SpruceTrapDoor(bool isInBackground) : base(isInBackground)
        {
            Init("Spruce Trapdoor Base", "sc:spruce_trapdoor_base", 500, "sc:spruce_trapdoor_item", Tool.Axe, Images.SpruceTrapdoor_Closed);
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 720, 1000);

            imgClose = Images.SpruceTrapdoor_Closed.GetTexture();
            imgOpen = Images.SpruceTrapdoor_Open.GetTexture();
        }
    }

    public class Furnace : Block
    {
        public Furnace(bool isInBackground) : base(isInBackground)
        {
            Init("Furnace", "sc:furnace_block", 500, "sc:furnace_item", Tool.Pickaxe, Images.Furnace_Idle);
            tags.Add("workstation");
            hasRightClickAction = true;

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
}
