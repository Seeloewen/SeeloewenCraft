using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class DungeonBlock
    {
        public int x;
        public int y;
        public bool isDoor = false;
        public bool isOccupied = false;
        public Direction doorDirection;
        public Block block;

        //-- Constructor --//

        public DungeonBlock(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
