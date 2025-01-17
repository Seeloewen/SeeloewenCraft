
namespace SeeloewenCraft.game.ui
{
    public class BlockRenderInfo
    {

        internal int x;
        internal int y;

        string id;
        string state;

        internal bool isBackground;

        internal int breakAnimation;

        internal bool hasForegroundBlock;
        string foregroundID;
        string foregroundState;

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

        public BlockRenderInfo(int x, int y, string id, string state, bool isBackground, int breakAnimation)
        {
            this.x = x;
            this.y = y;
            this.id = id;
            this.state = state;
            this.isBackground = isBackground;
            this.breakAnimation = breakAnimation;
        }

        public void AddForegroundBlock(string id, string state)
        {
            hasForegroundBlock = true;
            this.foregroundID = id;
            this.foregroundState = state;
        }



    }
}
