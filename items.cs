using System;
using System.Collections.Generic;
using System.Linq;
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
        public ImageBrush imageBrush = new ImageBrush();
        Random random = new Random(DateTime.Now.Millisecond);
        public wndGame wndGame;
        public InventorySlot slot;
        public Block block;
        public string itemName;
        public int id;
        public int xPos;
        public int yPos;
        public bool isPlacable = false;



        public Item(wndGame wndGame, int id)
        {
            //Set the attributes
            this.wndGame = wndGame;

            //Setup the item canvas
            cvsItem.Width = 75;
            cvsItem.Height = 75;
            cvsItem.Background = imageBrush;

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


        public ImageSource GetImageSource(string assemblyName, string resourceName)
        {
            //Get an image source from a Uri
            Uri imageUri = new Uri(string.Format("pack://application:,,,/{0};component/Resources/{1}", assemblyName, resourceName), UriKind.Absolute);
            return BitmapFrame.Create(imageUri);
        }

        public abstract Block GenerateBlock(int x, int y, Chunk chunk);
    }

    public class GrassItem : Item
    {
        public GrassItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Grass";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "GrassBlock.png");
        }
        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new GrassBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class StoneItem : Item
    {
        public StoneItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Stone";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "StoneBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new StoneBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class DirtItem : Item
    {
        public DirtItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Dirt";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "DirtBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new DirtBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Coal Ore";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "CoalOreBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new CoalOreBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Diamond Ore";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "DiamondOreBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new DiamondOreBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Iron Ore";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "IronOreBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new IronOreBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Oak Log";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "OakLogBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new OakLogBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Oak Leaves";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "OakLeavesBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new OakLeavesBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Bedrock";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "BedrockBlock.png");
        }

        override  public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new BedrockBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class AirItem : Item
    {
        public AirItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Air";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "AirBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new AirBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }

    public class ChestItem : Item
    {
        public ChestItem(wndGame wndGame, int id) : base(wndGame, id)
        {
            isPlacable = true;
            itemName = "Chest";
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "ChestBlock.png");
        }

        override public Block GenerateBlock(int x, int y, Chunk chunk)
        {
            block = new ChestBlock(wndGame, x, y, chunk, this);
            return block;
        }
    }
}
