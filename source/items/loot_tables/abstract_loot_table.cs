using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace SeeloewenCraft
{
    public abstract class LootTable
    {
        World world;
        static int offset;
        public List<LootTableEntry> lootTableEntries = new List<LootTableEntry>();
        Random rnd;

        //-- Constructor --//

        public LootTable(World world)
        {
            //Create link to game window
            this.world = world;
        }

        //-- Custom Methods --//

        public LootTableEntry RollEntry()
        {     
            rnd = new Random(DateTime.Now.Millisecond + offset);
            offset++;

            //Get all entries into the pool
            int poolNumber = 0;
            foreach (LootTableEntry entry in lootTableEntries)
            {
                entry.numbersInPool.Clear();

                //Add the numbers into the loot tables pool
                
                for (int i = poolNumber; i < (poolNumber + entry.weight); i++)
                {
                    entry.numbersInPool.Add(i);
                }
                poolNumber += entry.weight;
            }

            //Roll a number and check for each entry if the entry matches the number
            int random = rnd.Next(0, poolNumber);
            foreach (LootTableEntry entry in lootTableEntries)
            {
                if (entry.numbersInPool.Contains(random))
                {
                    return entry;
                } 
            }

            return null;
        }
    }

    //-- Loot Tables --//

    public class StoneLootTable : LootTable
    {
        public StoneLootTable(World world) : base(world)
        {
            lootTableEntries.Add(new LootTableEntry(new StoneItem(world, null), 1, 3, 5, world));
            lootTableEntries.Add(new LootTableEntry(new GrassItem(world, null), 1, 3, 1, world));
        }
    }
}
