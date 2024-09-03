using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    public abstract class LootTable
    {
        static int offset;
        public List<LootTableEntry> lootTableEntries = new List<LootTableEntry>();
        Random rnd;

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

    public class PlainsDungeonChest : LootTable
    {
        public PlainsDungeonChest() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new WaxItem(), 1, 3, 15));
            lootTableEntries.Add(new LootTableEntry(new IronBarItem(), 1, 2, 15));
            lootTableEntries.Add(new LootTableEntry(new GoldBarItem(), 1, 2, 15));
            lootTableEntries.Add(new LootTableEntry(new BoneItem(), 1, 6, 20));
            lootTableEntries.Add(new LootTableEntry(new DiamondItem(), 1, 2, 1));
            lootTableEntries.Add(new LootTableEntry(new IronSwordItem(), 1, 1, 2));
            lootTableEntries.Add(new LootTableEntry(new TinPickaxeItem(), 1, 1, 2));
            lootTableEntries.Add(new LootTableEntry(new AppleItem(), 1, 6, 16));
            lootTableEntries.Add(new LootTableEntry(new BucketEmptyItem(), 1, 2, 5));
        }
    }

    public class PlainsDungeonBarrel : LootTable
    {
        public PlainsDungeonBarrel() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new CroissantItem(), 1, 3, 5));
            lootTableEntries.Add(new LootTableEntry(new TinBarItem(), 1, 2, 5));
            lootTableEntries.Add(new LootTableEntry(new BreadItem(), 1, 3, 3));
            lootTableEntries.Add(new LootTableEntry(new BoneItem(), 1, 6, 10));
            lootTableEntries.Add(new LootTableEntry(new EmeraldItem(), 1, 3, 1));
            lootTableEntries.Add(new LootTableEntry(new CroissantItem(), 1, 3, 2));
            lootTableEntries.Add(new LootTableEntry(new RockItem(), 1, 3, 5));
            lootTableEntries.Add(new LootTableEntry(new IronScytheItem(), 1, 1, 1));
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
