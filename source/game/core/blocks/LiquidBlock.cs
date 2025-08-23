using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.blocks
{
    public abstract class LiquidBlock : Block
    {
        public int liquidLevel;
        public string liquidTag; //Used for identification in LiquidHandler
        public int liquidSourceX { get; private set; } = 0;
        public int liquidSourceY { get; private set; } = 0;
        public int liquidSourceCIndex { get; private set; } = 0;

        protected LiquidBlock(bool isBackground) : base(isBackground)
        {
            isSolid = false;
            WriteTag(BlockTags.REPLACEABLE);
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
        }

        protected override void DoSpecificUpdate()
        {
            LiquidHandler.DoUpdate(this);
        }

        public abstract LiquidBlock GetLiquid(int level, Direction dir, LiquidBlock source);

        public static void SetSource(LiquidBlock target, LiquidBlock source)
        {
            target.liquidSourceX = source.xPos;
            target.liquidSourceY = source.yPos;
            target.liquidSourceCIndex = source.chunk.index;
        }

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"liquidLevel", $"{liquidLevel}");
        }
    }
}
