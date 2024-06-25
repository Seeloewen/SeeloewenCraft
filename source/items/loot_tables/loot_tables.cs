using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class LootTables
    {
        World world;

        //-- Constructor --//

        public LootTables(World world)
        {
            //Create link to game window
            this.world = world;

            //Generate the loot tables
            GenerateLootTables();
        }

        public void GenerateLootTables()
        {
            //Generate all the loot tables
            stoneLootTable = new StoneLootTable(world);
        }

        //-- Loot Tables --//

        public StoneLootTable stoneLootTable;
    }
}
