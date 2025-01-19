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



        internal InvSlotScreen(int x, int y)
        {
            this.xIndex = x;
            this.yIndex = y;

            xPixel = InvSizes.mx - (InvSizes.slotSize * 4 + InvSizes.slotSize / 2 + 4 * InvSizes.edgeSize) + xIndex * (InvSizes.slotSize + InvSizes.edgeSize);
            yPixel = InvSizes.my - (InvSizes.yOffset) + yIndex * (InvSizes.slotSize + InvSizes.edgeSize);
            if (yIndex == 3) yPixel += InvSizes.edgeSize;
        }


        internal bool IsInBounds(int x, int y)
        {
            return x >= xPixel && y >= yPixel && x < xPixel + InvSizes.slotSize && y < yPixel + InvSizes.slotSize;
        }


        internal void RenderBack()
        {
            /*int x1 = InvSizes.mx - (InvSizes.slotSize*4+InvSizes.slotSize/2+4*InvSizes.edgeSize) + xIndex*(InvSizes.slotSize + InvSizes.edgeSize);
            int y1 = InvSizes.my - (InvSizes.yOffset) +yIndex* (InvSizes.slotSize + InvSizes.edgeSize);
            if (yIndex == 3) y1 += InvSizes.edgeSize;
            int x2 = x1 + InvSizes.slotSize;
            int y2 = y1 + InvSizes.slotSize;
            */

            ColorI color = hovered
                ? new ColorI(0.8f, 0.8f, 0.8f)
                : new ColorI(0.9f, 0.9f, 0.9f);

            PrimitiveRenderer.DrawRectangle(
                new Rectangle(xPixel, yPixel, xPixel+InvSizes.slotSize, yPixel+InvSizes.slotSize),
                color);
        }



    }
}
