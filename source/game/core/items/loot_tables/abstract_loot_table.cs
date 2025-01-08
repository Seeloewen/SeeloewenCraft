using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    public abstract class LootTable
    {
        static int offset;
        public List<LootTableEntry> lootTableEntries = new List<LootTableEntry>();
        public Random rnd;

        //-- Custom Methods --//

        public LootTableEntry RollEntry()
        {
            offset++;
            return RollEntry(new Random(DateTime.Now.Millisecond + offset));
        }

        public LootTableEntry RollEntry(Random rnd)
        {
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
}
