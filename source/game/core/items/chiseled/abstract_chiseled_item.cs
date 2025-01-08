using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
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
