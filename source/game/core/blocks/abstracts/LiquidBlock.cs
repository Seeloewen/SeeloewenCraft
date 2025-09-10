using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.blocks
{
    internal struct LiquidSource
    {
        internal int x;
        internal int y;
        internal int cIndex;

        internal LiquidSource(int x, int y, int cIndex)
        {
            this.x = x;
            this.y = y;
            this.cIndex = cIndex;
        }
    }

    public abstract class LiquidBlock : Block
    {
        public int liquidLevel;
        public string liquidTag; //Used for identification in LiquidHandler
        internal LiquidSource liquidSource { private set; get; }

        protected LiquidBlock(string name, string id) : base(name, id, 0, null, Tool.None)
        {
            isSolid = false;
            WriteTag(BlockTags.REPLACEABLE);
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.LIQUID_SOURCE);

            liquidSource = new LiquidSource(-1, -1, -1);
        }

        protected override void DoSpecificUpdate(double dt)
        {
            LiquidHandler.DoUpdate(this);
        }

        internal abstract LiquidBlock GetLiquid(int level, Direction dir, LiquidSource source);

        internal static void SetSource(LiquidBlock target, LiquidSource source)
        {
            target.liquidSource = source;
        }

        public override void OnSetBlock()
        {
            base.OnSetBlock();

            if(liquidSource.y == -1) //If no source block is set, use this as source
            {
                liquidSource = new LiquidSource(xPos, yPos, chunk.index);
            }
        }

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"liquidLevel", $"{liquidLevel}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"sourceX");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"sourceY");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"sourceC");
        }

        public override void UpdateDebugMenu()
        {
            base.UpdateDebugMenu();
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"sourceX", $"{liquidSource.x}");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"sourceY", $"{liquidSource.y}");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"sourceC", $"{liquidSource.cIndex}");
        }
    }
}
