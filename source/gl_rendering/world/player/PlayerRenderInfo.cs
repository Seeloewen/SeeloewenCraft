
using System.Diagnostics;

namespace SeeloewenCraft.gl_rendering
{

    
    public class PlayerRenderInfo
    {
        internal int posX;
        internal int posY;
        internal Direction direction;
        internal float armBack;
        internal float armFront;
        internal float legBack;
        internal float legFront;

        public PlayerRenderInfo(int posX, int posY, Direction dir, float armBack, float armFront, float legBack, float legFront)
        {
            Debug.Assert(dir == Direction.LEFT || dir == Direction.RIGHT);
            this.posX = posX;
            this.posY = posY;
            direction = dir;
            this.armFront = armFront;
            this.armBack = armBack;
            this.legBack = legBack;
            this.legFront = legFront;
        }

    }
}
