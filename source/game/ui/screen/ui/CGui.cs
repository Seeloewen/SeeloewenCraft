using SeeloewenCraft.game.ui.ui_lib;

namespace SeeloewenCraft.game.ui
{
    abstract internal class CGui : CRectangle
    {
        public IGuiData data;

        protected CGui(IGuiData d, Color c, Rectangle b) : base(c, b)
        {
            data = d;
        }
    }
}
