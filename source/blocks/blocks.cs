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
            image = world.images.GrassBlock;
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
            image = world.images.StoneBlock;
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
            image = world.images.DirtBlock;
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
            image = world.images.AirBlock;
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
            image = world.images.BedrockBlock;
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
            image = world.images.CoalOreBlock;
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
            image = world.images.DiamondOreBlock;
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
            image = world.images.IronOreBlock;
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
            image = world.images.OakLogBlock;
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
            image = world.images.OakLeavesBlock;
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
            image = world.images.SpruceLogBlock;
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
            image = world.images.SpruceLeavesBlock;
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(world, false, 9, 4);
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
            image = world.images.ChestBlock;
        }
        public override void RightClickAction(object sender)
        {
            //If the block is solid and has inventory
            if (IsInRange() && isSolid && hasInventory)
            {
                //If the block has an inventory, open it as well as the players inventory
                world.player.inventory.inventoryGui.SetTop(0);
                world.player.inventory.ShowInventory();
                blockInventory.inventoryGui.SetTop(400);
                blockInventory.inventoryGui.tblHeader.Text = "Chest";
                blockInventory.ShowInventory();
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
            image = world.images.MagmaBlock;
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
            image = world.images.Torch;
        }
    }
    public class Plant2Block_Base : Block
    {
        public Plant2Block_Base(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            isBase = true;
            SetTexture();
            name = "Cactus Plant Base";
            id = "sc:cactus_plant_base_block";
            connectedBlocks.Add(new Plant2Block_Top(world, isInBackground));
            connectedBlocks[0].yOffset = -1;
            connectedBlocks[0].baseBlock = this;
            breakTime = 0;
        }

        override public void GenerateItem(World world)
        {
            item = new Plant2Item(world);
        }

        public override void SetTexture()
        {
            image = world.images.Plant2_Base;
        }
    }

    public class Plant2Block_Top : Block
    {
        public Plant2Block_Top(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            SetTexture();
            name = "Cactus Plant Top";
            id = "sc:cactus_plant_top_block";
            breakTime = 0;
        }

        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Plant2_Top;
        }
    }

    public class AlphaCrafterBlock : Block
    {
        public AlphaCrafterBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Alpha Crafter";
            id = "sc:alpha_crafter_block";
            tags.Add("workstation");
            hasRightClickAction = true;
            breakTime = 500;

            craftingHandler = new CraftingHandler(world, this);
            gui = new AlphaCrafterGui(world, 535, 720, 120, 200, "sc:alpha_crafter", null, this);
        }

        override public void GenerateItem(World world)
        {
            item = new AlphaCrafterItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.AlphaCrafter;
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
            image = world.images.Chiseler;
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
        }

        override public void GenerateItem(World world)
        {
            item = new UnchiselerItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.Unchiseler;
        }

        public override void RightClickAction(object sender)
        {
            if (IsInRange())
            {
                world.player.inventory.inventoryGui.SetTop(25);
                world.player.inventory.ShowInventory();
                gui.Show();
            }
        }
    }

    public class CobbleStoneBlock : Block
    {
        public CobbleStoneBlock(World world, bool isInBackground) : base(world, isInBackground)
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
            image = world.images.CobbleStoneBlock;
        }
    }
}
