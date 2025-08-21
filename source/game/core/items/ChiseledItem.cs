using System.Collections.Generic;

namespace SeeloewenCraft.game.core.items
{
    public abstract class ChiseledItem : Item
    {
        public bool isChiseled;
        public List<Item> unchiselItems = new List<Item>();

        public ChiseledItem() : base()
        {
            isChiseled = true;
        }

        public virtual List<Item> Unchisel()
        {
            return unchiselItems;
        }
    }
}
