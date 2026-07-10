using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.core.blocks
{
    public struct PositionData
    {
        public readonly int x;
        public readonly int y;
        public readonly int ci;

        public PositionData(int x, int y, int ci)
        {
            //Try to normalize pos data by calculating chunk offset from x pos
            while (x < 0)
            {
                ci--;
                x += 8;
            }

            while (x > 7)
            {
                ci++;
                x -= 8;
            }

            this.x = x;
            this.y = y;
            this.ci = ci;
        }

        public PositionData()
        {
            //Returns invalid values
            x = -1;
            y = -1;
            ci = -1;
        }

        public bool ChunkExists() => World.Get().GetChunk(ci) != null;

        public PositionData Offset(int xo, int yo)
        {
            return new PositionData(x + xo, y + yo, ci);
        }
        public static PositionData FromGlobalX(int globalX, int y)
        {
            return new PositionData(globalX, y, 0);
        } 
    }
}
