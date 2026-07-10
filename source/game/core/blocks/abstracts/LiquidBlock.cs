using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.blocks
{ 
    public abstract class LiquidBlock : Block
    {
        public int liquidLevel;
        public string liquidTag; //Used for identification in LiquidHandler
        internal PositionData liquidSource { private set; get; }

        protected LiquidBlock(string name, string id) : base(name, id, 0, null, Tool.None)
        {
            isSolid = false;
            WriteTag(BlockTags.REPLACEABLE);
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.LIQUID_SOURCE);

            liquidSource = new PositionData();
        }

        protected override void AppendJson(JObject obj)
        {
            obj.Add("liquid_level", liquidLevel);
            obj.Add("liquid_source_x", liquidSource.x);
            obj.Add("liquid_source_y", liquidSource.y);
            obj.Add("liquid_source_c", liquidSource.ci);
        }

        protected override void LoadAdditionalData(JObject obj)
        {
            liquidLevel = obj.Get<int>("liquid_level");

            int x = obj.Get<int>("liquid_source_x");
            int y = obj.Get<int>("liquid_source_y");
            int c = obj.Get<int>("liquid_source_c");

            liquidSource = new PositionData(x,y,c);
        }

        protected override void DoSpecificUpdate(double dt)
        {
            LiquidHandler.DoUpdate(this);
        }

        internal abstract LiquidBlock GetLiquid(int level, Direction dir, PositionData source);

        internal static void SetSource(LiquidBlock target, PositionData source)
        {
            target.liquidSource = source;
        }

        public override void OnSetBlock()
        {
            base.OnSetBlock();

            if(liquidSource.y == -1) //If no source block is set, use this as source
            {
                liquidSource = new PositionData(posX, posY, chunk.index);
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
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"sourceC", $"{liquidSource.ci}");
        }
    }
}
