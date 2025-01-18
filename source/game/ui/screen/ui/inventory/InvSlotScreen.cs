using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.ui
{
    internal class InvSlotScreen
    {
        


        int x;
        int y;

        internal string itemID;
        internal int amount;



        internal InvSlotScreen(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        internal void RenderBack()
        {
            int x1 = InvSizes.mx - (InvSizes.slotSize*4+InvSizes.slotSize/2+4*InvSizes.edgeSize) + x*(InvSizes.slotSize + InvSizes.edgeSize);
            int y1 = InvSizes.my - (300) +y* (InvSizes.slotSize + InvSizes.edgeSize);
            if (y == 3) y1 += InvSizes.edgeSize;
            int x2 = x1 + InvSizes.slotSize;
            int y2 = y1 + InvSizes.slotSize;

            PrimitiveRenderer.DrawRectangle(x1, y1, x2, y2, 0f, 0f, 0f);
        }



    }
}
