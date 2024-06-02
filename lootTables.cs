using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class LootTables
    {
        wndGame wndGame;

        //-- Constructor --//

        public LootTables(wndGame wndGame)
        {
            //Create link to game window
            this.wndGame = wndGame;

            //Generate the loot tables
            GenerateLootTables();
        }

        public void GenerateLootTables()
        {
            //Generate all the loot tables
            stoneLootTable = new StoneLootTable(wndGame);
        }

        //-- Loot Tables --//

        public StoneLootTable stoneLootTable;
    }
}
