using SeeloewenCraft.game.graphics.ui_lib;
using System.Windows.Forms.Design;

namespace SeeloewenCraft.game.graphics
{
    internal class CInvSlot : CSlot
    {
        public bool isMouseFollower = false;

        private bool isHovered;
        private bool isPressed;

        private static readonly Color color = new Color(0.66f);
        private static readonly Color hoveredColor = new Color(0.71f);
        private static readonly Color pressedColor = new Color(0.6f);

        public int x;
        public int y;

        internal CInvSlot(int x, int y, int slotsX, int slotsY, bool hasHotbar = false) : base(CalcBounds(x, y, slotsX, slotsY, hasHotbar))
        {
            this.x = x;
            this.y = y;
        }

        internal CInvSlot() : base(CalcBounds(0,0,0,0,false))
        {
            x = 0;
            y = 0;
        }

        protected override void OnMouseEnter()
        {
            isHovered = true;
            isPressed = false;
        }

        protected override void OnMouseLeave()
        {
            isHovered = false;
            isPressed = false;
        }

        protected override void OnClickEvent(ClickEvent mouseClickEvent)
        {
            if (isMouseFollower) return;

            isPressed = mouseClickEvent.pressed;

            if (mouseClickEvent.button == ButtonType.LEFT)
            {
                if(isPressed) ((CInventory)parent).invData.GetSlot(x, y).OnLeftClick();
            }
            else if (mouseClickEvent.button == ButtonType.RIGHT)
            {
                if (isPressed) ((CInventory)parent).invData.GetSlot(x, y).OnRightClick();
            }
        }

        protected override void OnRender()
        {
            //Set color based on the pressstate (if it isn't following the mouse)
            if(!isMouseFollower) SetColor(isPressed ? pressedColor : (isHovered ? hoveredColor : color));

            base.OnRender();
        }

        private static Rectangle CalcBounds(int x, int y, int slotsX, int slotsY, bool hasHotbar)
        {
            int startX = GuiSizes.mx - (slotsX * GuiSizes.slotSize + 8 * GuiSizes.edgeSize) / 2;
            int startY = GuiSizes.my - (slotsY * GuiSizes.slotSize + 4 * GuiSizes.edgeSize) / 2 + 15;

            int x1 = startX + (GuiSizes.edgeSize + GuiSizes.slotSize) * x;
            int y1 = startY + (GuiSizes.edgeSize + GuiSizes.slotSize) * y;
            if (y == 3 && hasHotbar) y1 += GuiSizes.edgeSize; //Move last row down a little further to highlight hotbar
            int x2 = x1 + GuiSizes.slotSize;
            int y2 = y1 + GuiSizes.slotSize;

            return new Rectangle(x1, y1, x2, y2);
        }
    }
}
