using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace SeeloewenCraft
{
    public abstract class Item
    {
        public List<string> tags = new List<string>();
        public Canvas cvsItem = new Canvas();
        public ImageBrush image;
        public World world;
        public InventorySlot slot;
        public Block block;
        public string name;
        public string id;
        public int xPos;
        public int yPos;
        public bool isPlacable = false;
        public bool hasRightClickAction = false;


        //-- Constructor --//

        public Item(World world)
        {
            //Set the attributes
            this.world = world;

            //Setup the item canvas
            cvsItem.Width = 67;
            cvsItem.Height = 67;
            cvsItem.Background = image;
        }

        //-- Custom Methods --//

        public virtual void SetTexture()
        {
            throw new Exception("No texture for item was set.");
        }

        //This is currently required, but may be changed in the future if items that don't have blocks are added
        public abstract Block GenerateBlock(bool isInBackground);

        public virtual void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }

    }
}
