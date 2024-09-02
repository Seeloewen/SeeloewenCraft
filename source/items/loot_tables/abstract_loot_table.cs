using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    public abstract class LootTable
    {
        static int offset;
        public List<LootTableEntry> lootTableEntries = new List<LootTableEntry>();
        Random rnd;

        //-- Constructor --//

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
        public StoneLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new RockItem(), 1, 4, 1));
        }
    }

    public class PlainsDungeonChest1 : LootTable
    {
        public PlainsDungeonChest1() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new WaxItem(), 1, 3, 3));
            lootTableEntries.Add(new LootTableEntry(new IronBarItem(), 1, 2, 2));
            lootTableEntries.Add(new LootTableEntry(new BoneItem(), 1, 6, 5));
            lootTableEntries.Add(new LootTableEntry(new BreadItem(), 1, 2, 2));
            lootTableEntries.Add(new LootTableEntry(new RockItem(), 1, 4, 5));
        }
    }

    public class CoalLootTable : LootTable
    {
        public CoalLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new CoalItem(), 1, 3, 1));
        }
    }
}
