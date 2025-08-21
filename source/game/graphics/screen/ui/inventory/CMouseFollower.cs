using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CMouseFollower : CSlot
    {
        internal CMouseFollower() : base(new Rectangle(0, 0, GuiSizes.slotSize, GuiSizes.slotSize))
        {
            visible = false;
            hasBackground = false;
            SetColor(new Color(0f, 0f, 0f, 0f));
        }

        internal void SetSlot(InventorySlot slot)
        {
            if (slot == null)
            {
                visible = false;
                return;
            }

            visible = true;
            Update(slot.itemId, slot.amount, 0);
        }

        protected override void OnClickEvent(ClickEvent mouseClickEvent) { }

        internal void Reset() //Should be called BEFORE updating the guis
        {
            SetSlot(null);
        }

        protected override void OnRender()
        {
            MoveTo(InputHandler.mouseXPixel - width + 10, InputHandler.mouseYPixel - height + 10);

            base.OnRender();
        }
    }
}
