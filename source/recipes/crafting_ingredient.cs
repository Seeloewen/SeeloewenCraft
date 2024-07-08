using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SeeloewenCraft
{
    public class craftingIngredient
    {
        public Item item;
        public int amount;

        public craftingIngredient(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
