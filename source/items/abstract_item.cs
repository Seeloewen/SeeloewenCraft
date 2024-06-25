using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
}
