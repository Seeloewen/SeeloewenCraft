using System;
using System.Collections.Generic;
using System.Linq;
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
        public Canvas cvsItem = new Canvas();
        public ImageBrush image;
        Random random = new Random(DateTime.Now.Millisecond);
        public wndGame wndGame;
        public InventorySlot slot;
        public Block block;
        public string itemName;
        public int id;
        public int xPos;
        public int yPos;
        public bool isPlacable = false;


        //-- Constructor --//

        public Item(wndGame wndGame, int id, Block block)
        {
            //Set the attributes
            this.wndGame = wndGame;
            if(block != null )
            {
                this.block = block;
            }

            //Setup the item canvas
            cvsItem.Width = 75;
            cvsItem.Height = 75;
            cvsItem.Background = image;

            //Set the ID
            if(id == 0)
            {
                this.id = random.Next(1, 99999);
            }
            else
            {
                this.id = id;
            }
        }

        //-- Custom Methods --//

        public abstract void SetTexture();

        //This is currently required, but may be changed in the future if items that don't have blocks are added
        public abstract Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground);
    }

    //-- Items --//

    public class GrassItem : Item
    {
        public GrassItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Grass";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new GrassBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.GrassBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class StoneItem : Item
    {
        public StoneItem(wndGame wndGame, int id,Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Stone";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new StoneBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.StoneBlock;
                cvsItem.Background = image;
            }
        }

    }

    public class DirtItem : Item
    {
        public DirtItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Dirt";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new DirtBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.DirtBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Coal Ore";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new CoalOreBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.CoalOreBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Diamond Ore";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk,  bool isInBackground)
        {
            block = new DiamondOreBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.DiamondOreBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block  )
        {
            isPlacable = true;
            itemName = "Iron Ore";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new IronOreBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.IronOreBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Oak Log";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new OakLogBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.OakLogBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Oak Leaves";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new OakLeavesBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.OakLeavesBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class SpruceLogItem : Item
    {
        public SpruceLogItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Spruce Log";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new SpruceLogBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.SpruceLogBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class SpruceLeavesItem : Item
    {
        public SpruceLeavesItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Spruce Leaves";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new SpruceLeavesBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.SpruceLeavesBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Bedrock";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new BedrockBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.BedrockBlock;
                cvsItem.Background = image;
            }
        }

    }

    public class AirItem : Item
    {
        public AirItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Air";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new AirBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.AirBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class ChestItem : Item
    {
        public ChestItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Chest";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new ChestBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.ChestBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class MagmaBlockItem : Item
    {
        public MagmaBlockItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Magma Block";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            block = new MagmaBlock(wndGame, x, y, chunk, this, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.MagmaBlock;
                cvsItem.Background = image;
            }
        }
    }

    public class HammerItem : Item
    {
        public HammerItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = false;
            itemName = "Hammer";
            SetTexture();
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk, bool isInBackground)
        {
            return null;
        }

        override public void SetTexture()
        {
            {
                //Set the texture of the block on the canvas
                image = wndGame.images.Hammer;
                cvsItem.Background = image;
            }
        }
    }
}
