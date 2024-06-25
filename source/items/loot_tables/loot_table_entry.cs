using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class LootTableEntry
    {
        World world;
        public Item item;
        public int minAmount;
        public int maxAmount;
        public int weight;
        public List<int> numbersInPool = new List<int>();
        static int offset;
        Random rnd;

        //-- Constructor --//

        public LootTableEntry(Item item, int minAmount, int maxAmount, int weight, World world)
        {
            //Generate the random object
            rnd = new Random(DateTime.Now.Millisecond + offset);
            offset++;

            //Create links
            this.world = world;
            this.item = item;
            this.minAmount = minAmount;
            this.maxAmount = maxAmount;
            this.weight = weight;
            this.world = world;
        }

        //-- Custom Methods --//

        public List<Item> RollItems()
        {
            //Create the list and roll the amount
            List<Item> items = new List<Item>();
            int amount = rnd.Next(minAmount, maxAmount + 1);

            //Add all the items to the list
            for(int i = 0; i < amount; i++)
            {
                Type itemType = item.GetType();
                Item newItem = (Item)Activator.CreateInstance(itemType, world, 0, null);
                items.Add(newItem);
            }

            return items;
        }

        /*public int GetNumber(double d)
        {
            double c = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                if (d < weights[i] + c)
                {
                    return i;
                }
                c += weights[i];
            }
            return weights.Length - 1;
        }*/
    }
}
