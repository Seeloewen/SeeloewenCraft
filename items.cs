using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeeloewenCraft
{
    public abstract class Item
    {
        public List<string> tags = new List<string>();
        public Canvas cvsItem = new Canvas();
        public ImageBrush image;
        public wndGame wndGame;
        public InventorySlot slot;
        public Block block;
        public string name;
        public string id;
        public int xPos;
        public int yPos;
        public bool isPlacable = false;
        public bool canBeForeground = false;
        public bool hasRightClickAction = false;


        //-- Constructor --//

        public Item(wndGame wndGame, Block block)
        {
            //Set the attributes
            this.wndGame = wndGame;
            if (block != null)
            {
                this.block = block;
            }

            //Setup the item canvas
            cvsItem.Width = 75;
            cvsItem.Height = 75;
            cvsItem.Background = image;
        }

        //-- Custom Methods --//

        public abstract void SetTexture();

        //This is currently required, but may be changed in the future if items that don't have blocks are added
        public abstract Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground);

        public abstract void RightClickAction(Block block, object sender);

    }

    //-- Items --//

    public class GrassItem : Item
    {
        public GrassItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Grass";
            id = "sc:grass_block_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new GrassBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.GrassBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class StoneItem : Item
    {
        public StoneItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Stone";
            id = "sc:stone_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new StoneBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.StoneBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class DirtItem : Item
    {
        public DirtItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Dirt";
            id = "sc:dirt_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new DirtBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.DirtBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Coal Ore";
            id = "sc:coal_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new CoalOreBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.CoalOreBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Diamond Ore";
            id = "sc:diamond_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new DiamondOreBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.DiamondOreBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Iron Ore";
            id = "sc:iron_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new IronOreBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.IronOreBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Oak Log";
            id = "sc:oak_log_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new OakLogBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.OakLogBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Oak Leaves";
            id = "sc:oak_leaves_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new OakLeavesBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.OakLeavesBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLogItem : Item
    {
        public SpruceLogItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Spruce Log";
            id = "sc:spruce_log_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new SpruceLogBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.SpruceLogBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLeavesItem : Item
    {
        public SpruceLeavesItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new SpruceLeavesBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.SpruceLeavesBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Bedrock";
            id = "sc:bedrock_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new BedrockBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.BedrockBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }

    }

    public class AirItem : Item
    {
        public AirItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Air";
            id = "sc:air_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new AirBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.AirBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class ChestItem : Item
    {
        public ChestItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Chest";
            id = "chest_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new ChestBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.ChestBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class MagmaBlockItem : Item
    {
        public MagmaBlockItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Magma Block";
            id = "sc:magma_block_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new MagmaBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.MagmaBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class HammerItem : Item
    {
        public HammerItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = false;
            hasRightClickAction = true;
            name = "Hammer";
            id = "sc:hammer_item";
            SetTexture();
            tags.Add("tools/hammer");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            return null;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.Hammer;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            if (block.isBackground && block.canBeMovedToBackground && block.IsInRange())
            {
                block.MoveToForeground();
            }
            else if (!block.isBackground && block.IsInRange() && block.canBeMovedToBackground)
            {
                block.MoveToBackground();
            }
        }
    }

    public class TorchItem : Item
    {
        public TorchItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Torch";
            id = "sc:torch_item";
            canBeForeground = true;
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new TorchBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.Torch;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterItem : Item
    {
        public WaterItem(wndGame wndGame, Block block) : base(wndGame, block)
        {
            isPlacable = true;
            name = "Water";
            id = "sc:water_item";
            canBeForeground = false;
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new WaterBlock_6(wndGame, x, y, chunk, this, isInBackground);
            block.isWaterSource = true;
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.Water_6;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class Plant2Item : Item
    {
        public Plant2Item(wndGame wndGame, Block block) : base(wndGame, block)
        {
            canBeForeground = true;
            isPlacable = true;
            name = "Plant2";
            id = "sc:plant_2_item";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new Plant2Block_Base(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = wndGame.images.Plant2;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }
}
