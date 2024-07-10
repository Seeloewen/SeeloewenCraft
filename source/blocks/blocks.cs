using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SeeloewenCraft
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Grass Block";
            id = "sc:grass_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new GrassItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.GrassBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            //lootTable = world.lootTables.stoneLootTable;
            SetTexture();
            name = "Stone Block";
            id = "sc:stone_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new StoneItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.StoneBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Dirt";
            id = "sc:dirt_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new DirtItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.DirtBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
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

        override public void GenerateItem(World world, int id)
        {
            item = new AirItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.AirBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isBreakable = false;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Bedrock";
            id = "sc:bedrock_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new BedrockItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.BedrockBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Coal Ore";
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new CoalOreItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.CoalOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Diamond Ore";
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new DiamondOreItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.DiamondOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Iron Ore";
            id = "sc:iron_ore_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new IronOreItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.IronOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Oak Log";
            id = "sc:oak_log_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new OakLogItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.OakLogBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Oak Leaves";
            id = "sc:oak_leaves_block";
            isLightSource = true;
        }

        override public void GenerateItem(World world, int id)
        {
            item = new OakLeavesItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.OakLeavesBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Spruce Log";
            id = "sc:spruce_log_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new SpruceLogItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.SpruceLogBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isLightSource = true;
            SetTexture();
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new SpruceLeavesItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.SpruceLeavesBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(world, false);
            world.inventoryList.Add(blockInventory);
            SetTexture();
            name = "Chest";
            id = "sc:chest_block";
            hasRightClickAction = true;
        }

        override public void GenerateItem(World world, int id)
        {
            item = new ChestItem(world, this);
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

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Magma Block";
            id = "sc:magma_block";
            tags.Add("workstation");
        }

        override public void GenerateItem(World world, int id)
        {
            item = new MagmaBlockItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.MagmaBlock;
        }
        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class TorchBlock : Block
    {
        public TorchBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            canBeMovedToBackground = false;
            isLightSource = true;
            SetTexture();
            name = "Torch";
            id = "sc:torch_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = new TorchItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.Torch;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }
    public class Plant2Block_Base : Block
    {
        public Plant2Block_Base(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBase = true;
            SetTexture();
            name = "Cactus Plant Base";
            id = "sc:cactus_plant_base_block";
            connectedBlocks.Add(new Plant2Block_Top(world, x, y, chunk, item, isInBackground));
            connectedBlocks[0].yOffset = -1;
            connectedBlocks[0].baseBlock = this;
        }

        override public void GenerateItem(World world, int id)
        {
            item = new Plant2Item(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.Plant2_Base;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class Plant2Block_Top : Block
    {
        public Plant2Block_Top(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            SetTexture();
            name = "Cactus Plant Top";
            id = "sc:cactus_plant_top_block";
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Plant2_Top;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class AlphaCrafterBlock : Block
    {
        public AlphaCrafterBlock(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Alpha Crafter";
            id = "sc:alpha_crafter_block";
            hasRightClickAction = true;

            craftingHandler = new CraftingHandler(world, this);
            gui = new AlphaCrafterGui(world, 535, 720, 120, 200, "sc:alpha_crafter", null, this);
        }

        override public void GenerateItem(World world, int id)
        {
            item = new AlphaCrafterItem(world, this);
        }

        public override void SetTexture()
        {
            image = world.images.AlphaCrafter;
        }

        public override void RightClickAction(object sender)
        {
            gui.Show();
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
}
