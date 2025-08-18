namespace SeeloewenCraft.game.graphics
{
    internal class CInvSlot : CSlot
    {

        internal CInvSlot(int x, int y, int slotsX, int slotsY, bool hasHotbar = false) : base(CalcBounds(x, y, slotsX, slotsY, hasHotbar))
        {
            this.x = x;
            this.y = y;
        }

        internal CInvSlot() : base(CalcBounds(0, 0, 0, 0, false))
        {
            x = 0;
            y = 0;
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
