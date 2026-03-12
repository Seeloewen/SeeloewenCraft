using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.core.blocks
{
    public struct PositionData
    {
        public readonly int x;
        public readonly int y;
        public readonly int ci;

        public PositionData(int x, int y, int ci)
        {
            this.x = x;
            this.y = y;
            this.ci = ci;
        }
    }
}
