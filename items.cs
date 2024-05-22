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

        public void SetTexture(ImageBrush texture)
        {
            image = texture;
            cvsItem.Background = image;
        }

        public abstract Block GenerateBlock(int x, int y, Chunk chunk);

    }

    public class GrassItem : Item
    {
        public GrassItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Grass";
            SetTexture(wndGame.images.GrassBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new GrassBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class StoneItem : Item
    {
        public StoneItem(wndGame wndGame, int id,Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Stone";
            SetTexture(wndGame.images.StoneBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new StoneBlock(wndGame, x, y, chunk, this);
            return block;
        }

    }

    public class DirtItem : Item
    {
        public DirtItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Dirt";
            SetTexture(wndGame.images.DirtBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new DirtBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Coal Ore";
            SetTexture(wndGame.images.CoalOreBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new CoalOreBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Diamond Ore";
            SetTexture(wndGame.images.DiamondOreBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new DiamondOreBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block  )
        {
            isPlacable = true;
            itemName = "Iron Ore";
            SetTexture(wndGame.images.IronOreBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new IronOreBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Oak Log";
            SetTexture(wndGame.images.OakLogBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new OakLogBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Oak Leaves";
            SetTexture(wndGame.images.OakLeavesBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new OakLeavesBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Bedrock";
            SetTexture(wndGame.images.BedrockBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new BedrockBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class AirItem : Item
    {
        public AirItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Air";
            SetTexture(wndGame.images.AirBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new AirBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class ChestItem : Item
    {
        public ChestItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Chest";
            SetTexture(wndGame.images.ChestBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new ChestBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class MagmaBlockItem : Item
    {
        public MagmaBlockItem(wndGame wndGame, int id, Block block) : base(wndGame, id, block)
        {
            isPlacable = true;
            itemName = "Magma Block";
            SetTexture(wndGame.images.MagmaBlock);
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new MagmaBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }
}
