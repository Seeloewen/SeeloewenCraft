
namespace SeeloewenCraft
{
    public static class LootTables
    {
        //-- Loot Tables --//

        public static PlainsDungeonChest plainsDungeonChest = new PlainsDungeonChest();
        public static PlainsDungeonBarrel plainsDungeonBarrel = new PlainsDungeonBarrel();
        public static PotLootTable potLootTable = new PotLootTable();
        public static PyramidLootTable pyramidLootTable = new PyramidLootTable();
        public static SpruceTreeLootTable spruceTreeLootTable = new SpruceTreeLootTable();
        public static OakTreeLootTable oakTreeLootTable = new OakTreeLootTable();
        public static GrassLootTable grassLootTable = new GrassLootTable();
    }

    //-- Loot Tables --//

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


    public class OakTreeLootTable : LootTable
    {
        public OakTreeLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new OakSaplingItem(), 1, 1, 1));
            lootTableEntries.Add(new LootTableEntry(new OakLeavesItem(), 1, 1, 6));
            lootTableEntries.Add(new LootTableEntry(new AppleItem(), 1, 1, 1));
        }
    }

    public class SpruceTreeLootTable : LootTable
    {
        public SpruceTreeLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new SpruceSaplingItem(), 1, 1, 1));
            lootTableEntries.Add(new LootTableEntry(new SpruceLeavesItem(), 1, 1, 6));
        }
    }

    public class PyramidLootTable : LootTable
    {
        public PyramidLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new FossilFragmentItem(), 1, 3, 3));
            lootTableEntries.Add(new LootTableEntry(new PotShardItem(), 1, 2, 5));
            lootTableEntries.Add(new LootTableEntry(new CroissantItem(), 1, 3, 5));
            lootTableEntries.Add(new LootTableEntry(new GoldBarItem(), 1, 2, 3));
            lootTableEntries.Add(new LootTableEntry(new BoneItem(), 1, 4, 7));
            lootTableEntries.Add(new LootTableEntry(new SandItem(), 1, 7, 7));
            lootTableEntries.Add(new LootTableEntry(new SandStoneItem(), 1, 4, 7));
            lootTableEntries.Add(new LootTableEntry(new AmethystItem(), 1, 1, 1));
        }
    }

    public class PotLootTable : LootTable
    {
        public PotLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new FossilFragmentItem(), 1, 3, 3));
            lootTableEntries.Add(new LootTableEntry(new TinBarItem(), 1, 6, 3));
            lootTableEntries.Add(new LootTableEntry(new BoneItem(), 1, 3, 3));
            lootTableEntries.Add(new LootTableEntry(new ArrowItem(), 1, 3, 3));
            lootTableEntries.Add(new LootTableEntry(new SandStoneItem(), 1, 3, 5));
            lootTableEntries.Add(new LootTableEntry(new PaperItem(), 1, 1, 4));
            lootTableEntries.Add(new LootTableEntry(new RockItem(), 1, 1, 2));
        }
    }

    public class GrassLootTable : LootTable
    {
        public GrassLootTable() : base()
        {
            lootTableEntries.Add(new LootTableEntry(new SeedsItem(), 1, 1, 1));
            lootTableEntries.Add(new LootTableEntry(new GrassItem(), 1, 1, 7));
        }
    }
}
