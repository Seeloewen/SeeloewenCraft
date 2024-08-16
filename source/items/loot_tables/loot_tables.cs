
namespace SeeloewenCraft
{
    public class LootTables
    {
        

        //-- Constructor --//

        public LootTables()
        {
            //Create link to game window
            

            //Generate the loot tables
            GenerateLootTables();
        }

        public void GenerateLootTables()
        {
            //Generate all the loot tables
            stoneLootTable = new StoneLootTable();
            plainsDungeonChest1 = new PlainsDungeonChest1();
        }

        //-- Loot Tables --//

        public StoneLootTable stoneLootTable;
        public PlainsDungeonChest1 plainsDungeonChest1;
    }
}
