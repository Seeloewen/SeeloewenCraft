using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CHotbarSlot : CSlot
    {
        internal CHotbarSlot(Rectangle bounds, Inventory invData) : base(bounds, invData) => color = new Color(0.8f);
        protected override void OnClickEvent(ClickEvent mouseClickEvent) { }
        protected override void OnMouseEnter() { }
        protected override void OnMouseLeave() { }
    }
}
