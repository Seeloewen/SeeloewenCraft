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
        public GrassBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Grass Block";
            id = "sc:grass_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new GrassItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.GrassBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            //lootTable = wndGame.lootTables.stoneLootTable;
            SetTexture();
            name = "Stone Block";
            id = "sc:stone_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new StoneItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.StoneBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Dirt";
            id = "sc:dirt_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new DirtItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.DirtBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isBreakable = false;
            isSolid = false;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Air";
            id = "sc:air_block";
            isLightSource = true;
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new AirItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.AirBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isBreakable = false;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Bedrock";
            id = "sc:bedrock_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new BedrockItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.BedrockBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Coal Ore";
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new CoalOreItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.CoalOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Diamond Ore";
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new DiamondOreItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.DiamondOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Iron Ore";
            id = "sc:iron_ore_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new IronOreItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.IronOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Oak Log";
            id = "sc:oak_log_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new OakLogItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.OakLogBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Oak Leaves";
            id = "sc:oak_leaves_block";
            isLightSource = true;
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new OakLeavesItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.OakLeavesBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Spruce Log";
            id = "sc:spruce_log_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new SpruceLogItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.SpruceLogBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isLightSource = true;
            SetTexture();
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new SpruceLeavesItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.SpruceLeavesBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(wndGame, false);
            SetTexture();
            name = "Chest";
            id = "sc:chest_block";
            hasRightClickAction = true;
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new ChestItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.ChestBlock;
        }
        public override void RightClickAction(object sender)
        {
            //If the block is solid and has inventory
            if (IsInRange() && isSolid && hasInventory)
            {
                //If the block has an inventory, open it as well as the players inventory
                Canvas.SetTop(wndGame.player.inventory.grdInventory, -10);
                wndGame.player.inventory.ShowInventory();
                blockInventory.ShowInventory();
            }
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Magma Block";
            id = "sc:magma_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new MagmaBlockItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.MagmaBlock;
        }
        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class TorchBlock : Block
    {
        public TorchBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            canBeMovedToBackground = false;
            isLightSource = true;
            SetTexture();
            name = "Torch";
            id = "sc:torch_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new TorchItem(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.Torch;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }
    public class Plant2Block_Base : Block
    {
        public Plant2Block_Base(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBase = true;
            SetTexture();
            name = "Cactus Plant Base";
            id = "sc:cactus_plant_base_block";
            connectedBlocks.Add(new Plant2Block_Top(wndGame, x, y, chunk, item, isInBackground));
            connectedBlocks[0].yOffset = -1;
            connectedBlocks[0].baseBlock = this;
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new Plant2Item(wndGame, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.Plant2_Base;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class Plant2Block_Top : Block
    {
        public Plant2Block_Top(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            SetTexture();
            name = "Cactus Plant Top";
            id = "sc:cactus_plant_top_block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = wndGame.images.Plant2_Top;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }
}
