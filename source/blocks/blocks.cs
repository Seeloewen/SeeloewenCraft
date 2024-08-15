using System;

namespace SeeloewenCraft
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Grass Block";
            id = "sc:grass_block";
            tags.Add("CanBeFloor");
        }

        override public void GenerateItem(World world)
        {
            item = new GrassItem(world);
        }

        public override void SetTexture()
        {
            image = Images.GrassBlock.GetTexture();
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            //lootTable = world.lootTables.stoneLootTable;
            SetTexture();
            name = "Stone Block";
            id = "sc:stone_block";
            breakTime = 1250;
            tags.Add("CanBeFloor");
        }

        override public void GenerateItem(World world)
        {
            item = new StoneItem(world);
        }

        public override void SetTexture()
        {
            image = Images.StoneBlock.GetTexture();
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Dirt";
            id = "sc:dirt_block";
            tags.Add("CanBeFloor");
        }

        override public void GenerateItem(World world)
        {
            item = new DirtItem(world);
        }

        public override void SetTexture()
        {
            image = Images.Dirt.GetTexture();
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(World world, bool isInBackground) : base(world, isInBackground)
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

        override public void GenerateItem(World world)
        {
            item = new AirItem(world);
        }

        public override void SetTexture()
        {
            image = Images.Air.GetTexture();
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            isBreakable = false;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Bedrock";
            id = "sc:bedrock_block";
        }

        override public void GenerateItem(World world)
        {
            item = new BedrockItem(world);
        }

        public override void SetTexture()
        {
            image = Images.Bedrock.GetTexture();
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Coal Ore";
            breakTime = 1500;
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(World world)
        {
            item = new CoalOreItem(world);
        }

        public override void SetTexture()
        {
            image = Images.CoalOre.GetTexture();
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Diamond Ore";
            breakTime = 2000;
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(World world)
        {
            item = new DiamondOreItem(world);
        }

        public override void SetTexture()
        {
            image = Images.DiamondOre.GetTexture();
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Iron Ore";
            breakTime = 1750;
            id = "sc:iron_ore_block";
        }

        override public void GenerateItem(World world)
        {
            item = new IronOreItem(world);
        }

        public override void SetTexture()
        {
            image = Images.IronOre.GetTexture();
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Oak Log";
            breakTime = 350;
            id = "sc:oak_log_block";
        }

        override public void GenerateItem(World world)
        {
            item = new OakLogItem(world);
        }

        public override void SetTexture()
        {
            image = Images.OakLog.GetTexture();
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Oak Leaves";
            breakTime = 125;
            id = "sc:oak_leaves_block";
        }

        override public void GenerateItem(World world)
        {
            item = new OakLeavesItem(world);
        }

        public override void SetTexture()
        {
            image = Images.OakLeaves.GetTexture();
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Spruce Log";
            breakTime = 300;
            id = "sc:spruce_log_block";
        }

        override public void GenerateItem(World world)
        {
            item = new SpruceLogItem(world);
        }

        public override void SetTexture()
        {
            image = Images.SpruceLog.GetTexture();
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_block";
            breakTime = 125;
        }

        override public void GenerateItem(World world)
        {
            item = new SpruceLeavesItem(world);
        }

        public override void SetTexture()
        {
            image = Images.SpruceLeaves.GetTexture();
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(world, 9, 4);
            world.inventoryList.Add(blockInventory);
            SetTexture();
            name = "Chest";
            id = "sc:chest_block";
            hasRightClickAction = true;
            breakTime = 500;
        }

        override public void GenerateItem(World world)
        {
            item = new ChestItem(world);
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
                world.player.inventory.inventoryGui.SetTop(20);
                world.player.inventory.Show();
            }
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Magma Block";
            id = "sc:magma_block";
            breakTime = 750;
        }

        override public void GenerateItem(World world)
        {
            item = new MagmaBlockItem(world);
        }

        public override void SetTexture()
        {
            image = Images.MagmaBlock.GetTexture();
        }

    }

    public class TorchBlock : Block
    {
        public TorchBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            canBeMovedToBackground = false;
            isLightSource = true;
            SetTexture();
            name = "Torch";
            id = "sc:torch_block";
            breakTime = 0;
        }

        override public void GenerateItem(World world)
        {
            item = new TorchItem(world);
        }

        public override void SetTexture()
        {
            image = Images.Torch.GetTexture();
        }
    }
    public class PottedCactus_Base : Block
    {
        public PottedCactus_Base(World world, bool isInBackground) : base(world, isInBackground)
        {
            isBase = true;
            SetTexture();
            name = "Potted Cactus Base";
            id = "sc:potted_cactus_base";
            connectedBlocks.Add(new PottedCactus_Top(world, isInBackground));
            connectedBlocks[0].yOffset = -1;
            connectedBlocks[0].baseBlock = this;
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

        override public void GenerateItem(World world)
        {
            item = new PottedCactusItem(world);
        }

        public override void SetTexture()
        {
            image = Images.PottedCactus_Base.GetTexture();
        }
    }

    public class PottedCactus_Top : Block
    {
        public PottedCactus_Top(World world, bool isInBackground) : base(world, isInBackground)
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

        override public void GenerateItem(World world)
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
        public CraftingTableBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Crafting Table";
            id = "sc:crafting_table_block";
            tags.Add("workstation");
            hasRightClickAction = true;
            breakTime = 500;

            craftingHandler = new CraftingHandler(world, this);
            gui = new CraftingTableGui(world, 535, 720, 120, 200, "sc:crafting_table", null, this);
        }

        override public void GenerateItem(World world)
        {
            item = new CraftingTable(world);
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
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class ChiselerBlock : Block
    {
        public ChiselerBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseler";
            id = "sc:chiseler_block";
            tags.Add("workstation");
            breakTime = 500;
            hasRightClickAction = true;

            craftingHandler = new CraftingHandler(world, this);
            gui = new ChiselerGui(world, 535, 720, 120, 200, "sc:chiseler", null, this);
        }

        override public void GenerateItem(World world)
        {
            item = new ChiselerItem(world);
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
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class UnchiselerBlock : Block
    {
        public UnchiselerBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Unchiseler";
            id = "sc:unchiseler_block";
            hasRightClickAction = true;
            collision = new RectangleCollision(0, 1000, 565, 1000);
            breakTime = 500;

            craftingHandler = new CraftingHandler(world, this);
            gui = new UnchiselerGui(world, 225, 225, 465, 475, "sc:unchiseler");
            blockInventory = gui.inventory;
            hasInventory = true;
        }

        override public void GenerateItem(World world)
        {
            item = new UnchiselerItem(world);
        }

        public override void SetTexture()
        {
            image = Images.Unchiseler.GetTexture();
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                world.player.inventory.inventoryGui.SetTop(25);
                world.player.inventory.Show();
                gui.Show();
            }
        }
    }

    public class CobblestoneBlock : Block
    {
        public CobblestoneBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Cobblestone";
            id = "sc:cobblestone_block";
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock.GetTexture();
        }
    }


    public class SpruceDoor_Base : DoorBlock
    {
        public SpruceDoor_Base(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            isBase = true;
            name = "Spruce Door Base";
            id = "sc:spruce_door_base";
            hasRightClickAction = true;
            collision = new RectangleCollision(720, 1000, 0, 1000);
            breakTime = 500;

            connectedBlocks.Add(new SpruceDoor_Top(world, isInBackground));
            connectedBlocks[0].yOffset = -1;
            connectedBlocks[0].baseBlock = this;

            imgClose = Images.SpruceDoor_Closed_Base.GetTexture();
            imgOpen = Images.SpruceDoor_Open_Base.GetTexture();
        }

        override public void GenerateItem(World world)
        {
            item = new SpruceDoorItem(world);
        }

        public override void SetTexture()
        {
            image = Images.SpruceDoor_Closed_Base.GetTexture();
        }   
    }


    public class SpruceDoor_Top : DoorBlock
    {
        public SpruceDoor_Top(World world, bool isInBackground) : base(world, isInBackground)
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

        override public void GenerateItem(World world)
        {
            item = new SpruceDoorItem(world);
        }

        public override void SetTexture()
        {
            image = Images.SpruceDoor_Closed_Top.GetTexture();
        }

        public override void RightClickAction(object sender)
        {
            if (baseBlock is DoorBlock block)
            {
                block.RightClickAction(sender);
            }
        }
    }
}
