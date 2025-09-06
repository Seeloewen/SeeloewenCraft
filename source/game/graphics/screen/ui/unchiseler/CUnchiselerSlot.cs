using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CUnchiselerSlot : CSlot
    {
        internal CUnchiselerSlot(int x, int y) : base(new Rectangle(x, y, x + GuiSizes.slotSize, y + GuiSizes.slotSize), null) { }

        protected override void OnClickEvent(ClickEvent mouseClickEvent)
        {
            isPressed = mouseClickEvent.pressed;

            if (mouseClickEvent.button == ButtonType.LEFT)
            {
                if (isPressed && parent is CUnchiseler cInv) cInv.invData.GetSlot(x, y).OnLeftClick();
            }
            else if (mouseClickEvent.button == ButtonType.RIGHT)
            {
                if (isPressed && parent is CUnchiseler cInv) cInv.invData.GetSlot(x, y).OnRightClick();
            }
        }
    }
}
