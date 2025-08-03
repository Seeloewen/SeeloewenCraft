using SeeloewenCraft.game.ui.ui_lib;
using System;

namespace SeeloewenCraft.game.ui
{
    internal class CInvSlot : CSlot
    {
        public bool isMouseFollower = false;

        private bool isHovered;
        private bool isPressed;

        private static readonly Color color = new Color(0.66f, 0.66f, 0.66f);
        private static readonly Color hoveredColor = new Color(0.73f, 0.73f, 0.73f);
        private static readonly Color pressedColor = new Color(0.6f, 0.6f, 0.6f);

        public int x;
        public int y;

        internal CInvSlot(int x, int y) : base(CalcBounds(x, y))
        {
            this.x = x;
            this.y = y;
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
            SetColor(isPressed ? pressedColor : (isHovered ? hoveredColor : color));

            base.OnRender();
        }

        private static Rectangle CalcBounds(int x, int y)
        {
            int startX = InvSizes.mx - (9 * InvSizes.slotSize + 8 * InvSizes.edgeSize) / 2;
            int startY = InvSizes.my - (4 * InvSizes.slotSize + 4 * InvSizes.edgeSize) / 2;

            int x1 = startX + (InvSizes.edgeSize + InvSizes.slotSize) * x;
            int y1 = startY + (InvSizes.edgeSize + InvSizes.slotSize) * y;
            if (y == 3) y1 += InvSizes.edgeSize; //Move last row down a little further to highlight hotbar
            int x2 = x1 + InvSizes.slotSize;
            int y2 = y1 + InvSizes.slotSize;

            return new Rectangle(x1, y1, x2, y2);
        }
    }
}
