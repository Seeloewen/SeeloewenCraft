
using SeeloewenCraft.entity;

namespace SeeloewenCraft
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Grass Block";
            id = "sc:grass_block";
            tags.Add("CanBeFloor");
        }

        override public void GenerateItem()
        {
            item = new GrassItem();
        }

        public override void SetTexture()
        {
            image = Images.GrassBlock.GetTexture();
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(bool isInBackground) : base(isInBackground)
        {
            //lootTable = Game.world.lootTables.stoneLootTable;
            SetTexture();
            name = "Stone Block";
            id = "sc:stone_block";
            breakTime = 1250;
            tags.Add("CanBeFloor");
        }

        override public void GenerateItem()
        {
            item = new StoneItem();
        }

        public override void SetTexture()
        {
            image = Images.StoneBlock.GetTexture();
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Dirt";
            id = "sc:dirt_block";
            tags.Add("CanBeFloor");
        }

        override public void GenerateItem()
        {
            item = new DirtItem();
        }

        public override void SetTexture()
        {
            image = Images.Dirt.GetTexture();
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(bool isInBackground) : base(isInBackground)
        {
            isBreakable = false;
            isSolid = false;
            isReplacable = true;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Air";
            id = "sc:air_block";
            isLightSource = true;
        }

        override public void GenerateItem()
        {
            item = new AirItem();
        }

        public override void SetTexture()
        {
            image = Images.Air.GetTexture();
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(bool isInBackground) : base(isInBackground)
        {
            isBreakable = false;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Bedrock";
            id = "sc:bedrock_block";
        }

        override public void GenerateItem()
        {
            item = new BedrockItem();
        }

        public override void SetTexture()
        {
            image = Images.Bedrock.GetTexture();
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Coal Ore";
            breakTime = 1500;
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem()
        {
            item = new CoalOreItem();
        }

        public override void SetTexture()
        {
            image = Images.CoalOre.GetTexture();
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Diamond Ore";
            breakTime = 2000;
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem()
        {
            item = new DiamondOreItem();
        }

        public override void SetTexture()
        {
            image = Images.DiamondOre.GetTexture();
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Iron Ore";
            breakTime = 1750;
            id = "sc:iron_ore_block";
        }

        override public void GenerateItem()
        {
            item = new IronOreItem();
        }

        public override void SetTexture()
        {
            image = Images.IronOre.GetTexture();
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Oak Log";
            breakTime = 350;
            id = "sc:oak_log_block";
        }

        override public void GenerateItem()
        {
            item = new OakLogItem();
        }

        public override void SetTexture()
        {
            image = Images.OakLog.GetTexture();
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Oak Leaves";
            breakTime = 125;
            id = "sc:oak_leaves_block";
        }

        override public void GenerateItem()
        {
            item = new OakLeavesItem();
        }

        public override void SetTexture()
        {
            image = Images.OakLeaves.GetTexture();
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Spruce Log";
            breakTime = 300;
            id = "sc:spruce_log_block";
        }

        override public void GenerateItem()
        {
            item = new SpruceLogItem();
        }

        public override void SetTexture()
        {
            image = Images.SpruceLog.GetTexture();
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_block";
            breakTime = 125;
        }

        override public void GenerateItem()
        {
            item = new SpruceLeavesItem();
        }

        public override void SetTexture()
        {
            image = Images.SpruceLeaves.GetTexture();
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(bool isInBackground) : base(isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(9, 4);
            Game.world.inventoryList.Add(blockInventory);
            SetTexture();
            name = "Chest";
            id = "sc:chest_block";
            hasRightClickAction = true;
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new ChestItem();
        }

        public override void SetTexture()
        {
            image = Images.Chest.GetTexture();
        }
        public override void RightClickAction(object sender)
        {
            //If the block is solid and has inventory
            if (IsInRange() && isSolid && hasInventory)
            {
                //If the block has an inventory, open it as well as the players inventory
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
            SetTexture();
            name = "Magma Block";
            id = "sc:magma_block";
            breakTime = 750;
        }

        override public void GenerateItem()
        {
            item = new MagmaBlockItem();
        }

        public override void SetTexture()
        {
            image = Images.MagmaBlock.GetTexture();
        }

    }

    public class TorchBlock : Block
    {
        public TorchBlock(bool isInBackground) : base(isInBackground)
        {
            isSolid = false;
            canBeMovedToBackground = false;
            isLightSource = true;
            SetTexture();
            name = "Torch";
            id = "sc:torch_block";
            breakTime = 0;
        }

        override public void GenerateItem()
        {
            item = new TorchItem();
        }

        public override void SetTexture()
        {
            image = Images.Torch.GetTexture();
        }
    }
    public class PottedCactus_Base : Block
    {
        public PottedCactus_Base(bool isInBackground) : base(isInBackground)
        {
            isBase = true;
            SetTexture();
            name = "Potted Cactus Base";
            id = "sc:potted_cactus_base";
            connectedBlocks.Add((0, -1, "sc:potted_cactus_top"));
            breakTime = 0;
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

        override public void GenerateItem()
        {
            item = new PottedCactusItem();
        }

        public override void SetTexture()
        {
            image = Images.PottedCactus_Base.GetTexture();
        }
    }

    public class PottedCactus_Top : Block
    {
        public PottedCactus_Top(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Potted Cactus Top";
            id = "sc:potted_cactus_top";
            breakTime = 0;
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

        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.PottedCactus_Top.GetTexture();
        }
    }

    public class CraftingTableBlock : Block
    {
        public CraftingTableBlock(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Crafting Table";
            id = "sc:crafting_table_block";
            tags.Add("workstation");
            hasRightClickAction = true;
            breakTime = 500;

            craftingHandler = new CraftingHandler(this);
            gui = new CraftingTableGui(535, 720, 120, 285, "sc:crafting_table", null, this);
        }

        override public void GenerateItem()
        {
            item = new CraftingTable();
        }

        public override void SetTexture()
        {
            image = Images.CraftingTable.GetTexture();
        }

        public override void RightClickAction(object sender)
        {
            //If the block is in range
            if (IsInRange())
            {
                //If the block has an inventory, open it as well as the players inventory
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
            SetTexture();
            name = "Chiseler";
            id = "sc:chiseler_block";
            tags.Add("workstation");
            breakTime = 500;
            hasRightClickAction = true;

            craftingHandler = new CraftingHandler(this);
            gui = new ChiselerGui(535, 720, 120, 285, "sc:chiseler", null, this);
        }

        override public void GenerateItem()
        {
            item = new ChiselerItem();
        }

        public override void SetTexture()
        {
            image = Images.Chiseler.GetTexture();
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
            SetTexture();
            name = "Unchiseler";
            id = "sc:unchiseler_block";
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 565, 1000);
            breakTime = 500;

            craftingHandler = new CraftingHandler(this);
            gui = new UnchiselerGui(225, 225, 420, 530, "sc:unchiseler");
            blockInventory = gui.inventory;
            hasInventory = true;
        }

        override public void GenerateItem()
        {
            item = new UnchiselerItem();
        }

        public override void SetTexture()
        {
            image = Images.Unchiseler.GetTexture();
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
            SetTexture();
            name = "Cobblestone";
            id = "sc:cobblestone_block";
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock.GetTexture();
        }
    }


    public class SpruceDoor_Base : DoorBlock
    {
        public SpruceDoor_Base(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            isBase = true;
            name = "Spruce Door Base";
            id = "sc:spruce_door_base";
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);
            breakTime = 500;

            connectedBlocks.Add((0, -1, "sc:spruce_door_top"));

            imgClose = Images.SpruceDoor_Closed_Base.GetTexture();
            imgOpen = Images.SpruceDoor_Open_Base.GetTexture();
        }

        override public void GenerateItem()
        {
            item = new SpruceDoorItem();
        }

        public override void SetTexture()
        {
            image = Images.SpruceDoor_Closed_Base.GetTexture();
        }
    }


    public class SpruceDoor_Top : DoorBlock
    {
        public SpruceDoor_Top(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Spruce Door Top";
            id = "sc:spruce_door_top";
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);
            breakTime = 500;

            imgClose = Images.SpruceDoor_Closed_Top.GetTexture();
            imgOpen = Images.SpruceDoor_Open_Top.GetTexture();
        }

        override public void GenerateItem()
        {
            item = new SpruceDoorItem();
        }

        public override void SetTexture()
        {
            image = Images.SpruceDoor_Closed_Top.GetTexture();
        }

        public override void RightClickAction(object sender)
        {
            if (GetBaseBlock() is DoorBlock block)
            {
                block.RightClickAction(sender);
            }
        }
    }
}
