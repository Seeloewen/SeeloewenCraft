
using SeeloewenCraft.game.core.blocks;

namespace SeeloewenCraft.game.graphics
{
    public class BlockRenderInfo
    {

        internal int x;
        internal int y;

        string id;
        string state = "";

        internal bool isBackground;

        internal float lighting;

        internal int breakAnimation;
        internal bool hammering;

        internal bool hasForegroundBlock;
        string foregroundID;
        string foregroundState = "";

        internal string GetTextureID()
        {
            if (state.Length > 0)
            {
                return $"{id}/{state}";
            }
            else
            {
                return $"{id}";
            }
        }
        internal string GetForegroundTextureID()
        {
            if (foregroundState.Length > 0)
            {
                return $"{foregroundID}/{foregroundState}";
            }
            else
            {
                return $"{foregroundID}";
            }
        }

        private float ParseLightLevel(int level) => level switch
        {
            4 => 0.25f,
            3 => 0.5f,
            2 => 0.75f,
            1 => 0.9f,
            0 => 1f,
            _ => 0f,
        };

        public BlockRenderInfo(int x, int y, string id, BlockState state, bool isBackground, int breakAnimation, bool hammering, int lightLevel)
        {
            this.x = x;
            this.y = y;
            this.id = id;
            if(state != BlockState.DEFAULT) this.state = state.ToString();
            this.isBackground = isBackground;
            this.breakAnimation = breakAnimation;
            this.hammering = hammering;
            lighting = ParseLightLevel(lightLevel);
        }

        public void AddForegroundBlock(string id, BlockState state)
        {
            hasForegroundBlock = true;
            foregroundID = id;
            if(state != BlockState.DEFAULT) foregroundState = state.ToString();
        }



    }
}
