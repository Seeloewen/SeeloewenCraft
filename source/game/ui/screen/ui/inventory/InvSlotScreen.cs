using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.ui
{
    internal class InvSlotScreen
    {
        


        int xIndex;
        int yIndex;

        int xPixel;
        int yPixel;

        internal string itemID;
        internal int amount;

        internal bool hovered;
        internal bool pressed;


        internal InvSlotScreen(int x, int y)
        {
            this.xIndex = x;
            this.yIndex = y;

            xPixel = IS.mx - (IS.slotSize * 4 + IS.slotSize / 2 + 4 * IS.edgeSize) + xIndex * (IS.slotSize + IS.edgeSize);
            yPixel = IS.my - (IS.yOffset) + yIndex * (IS.slotSize + IS.edgeSize);
            if (yIndex == 3) yPixel += IS.edgeSize;
        }


        internal bool IsInBounds(int x, int y)
        {
            return x >= xPixel && y >= yPixel && x < xPixel + IS.slotSize && y < yPixel + IS.slotSize;
        }


        internal void RenderBack()
        {

            Color color = pressed 
                ? new Color(0.9f, 0.9f, 0.9f)
                : hovered
                ? new Color(0.8f, 0.8f, 0.8f)
                : new Color(0.7f, 0.7f, 0.7f);

            PrimitiveRenderer.DrawRectangle(
                new Rectangle(xPixel, yPixel, xPixel+IS.slotSize, yPixel+IS.slotSize),
                color);
        }

        internal void RenderMid()
        {
            var slot = Game.world.player.inventory.GetSlot(xIndex, yIndex);
            string itemID = slot.itemId;
            int amount = slot.Amount;

            if(itemID != null) ItemRenderer.Draw(itemID,
                xPixel+IS.edgeSize, 
                yPixel + IS.edgeSize, 
                xPixel + IS.slotSize - IS.edgeSize,
                yPixel + IS.slotSize - IS.edgeSize);
        }

    }
}
