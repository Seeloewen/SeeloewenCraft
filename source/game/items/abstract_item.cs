using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace SeeloewenCraft
{
    public abstract class Item
    {
        public Canvas cvsItem = new Canvas();
        public ImageBrush image;
        public string name;
        public string id;
        public string blockId;
        public string tag;
        public bool isPlacable = false;
        public bool hasRightClickAction = false;


        //-- Constructor --//

        public Item()
        {
            //Setup the item canvas
            cvsItem.Width = 67;
            cvsItem.Height = 67;
        }

        //-- Custom Methods --//
        public virtual void Init(string name, string id, string? blockId, bool isPlacable)
        {
            //Initialize the item
            this.isPlacable = isPlacable;
            this.name = name;
            this.id = id;
            this.blockId = blockId;
        }

        public Block GetBlock()
        {
            //If the item has a block id, generate the associated block
            if (!string.IsNullOrEmpty(blockId))
            {
                return BlockRegister.GenerateBlock(blockId);
            }

            return null;
        }

        public virtual void RightClickAction(Block block, InventorySlot invSlot)
        {
            throw new NotImplementedException();
        }

        public bool ContainsTag(string tag)
        {
            if (!string.IsNullOrEmpty(this.tag))
            {
                string[] splitted = this.tag.Split(';');
                foreach (string s in splitted)
                {
                    if (s == tag)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
