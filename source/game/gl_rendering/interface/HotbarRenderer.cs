using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.gl_rendering
{
    public static class HotbarRenderer
    {

        const float startPos = 20f;
        const float edgeSize = 5f;
        const float slotSize = 70f;


        internal static void Render(PrimitiveRenderer primitiveRenderer, ItemRenderer itemRenderer, TextRenderer textRenderer)
        {
            List<HotbarSlot> slots = Game.world.player.inventory.hotbarSlotList;

            //backplate

            primitiveRenderer.Begin();
            (float x1, float y1) = Resolution.PixelToScreen(startPos + 0.5f, startPos + 0.5f);
            (float x2, float y2) = Resolution.PixelToScreen(startPos + 0.5f + 10 * edgeSize + 9 * slotSize, startPos + 0.5f + 2 * edgeSize + slotSize);

            primitiveRenderer.DrawRectangle(x1, y1, x2, y2, 0.4f, 0.4f, 0.4f);


            for (int i = 0; i < 9; i++)
            {
                (x1, y1) = Resolution.PixelToScreen(startPos + edgeSize + 0.5f + (slotSize + edgeSize) * i, startPos + edgeSize + 0.5f);
                (x2, y2) = Resolution.PixelToScreen(startPos + edgeSize + 0.5f + (slotSize + edgeSize) * i + slotSize, startPos + edgeSize + 0.5f + slotSize);

                primitiveRenderer.DrawRectangle(x1, y1, x2, y2, 0.8f, 0.8f, 0.8f);
            }
            primitiveRenderer.End();

            itemRenderer.Begin();
            foreach (HotbarSlot slot in slots)
            {
                string itemID = slot.slot.itemId;
                if (itemID == null) continue;
                int i = slot.xPos;

                (x1, y1) = Resolution.PixelToScreen(startPos + edgeSize * 2 + 0.5f + (slotSize + edgeSize) * i, startPos + edgeSize * 2 + 0.5f);
                (x2, y2) = Resolution.PixelToScreen(startPos + 0.5f + (slotSize + edgeSize) * i + slotSize, startPos + 0.5f + slotSize);

                itemRenderer.DrawItem(itemID, x1, y1, x2, y2);





            }
            itemRenderer.End();
            textRenderer.Begin();
            foreach (HotbarSlot slot in slots)
            {
                int amount = slot.slot.amount;

                int i = slot.xPos;

                if (amount > 10)
                {
                    /*(x1, y1) = Resolution.PixelToScreen(startPos + edgeSize * 2 + 0.5f + (slotSize + edgeSize) * i + 50f, startPos + edgeSize * 2 + 0.5f + 50f);
                    (x2, y2) = Resolution.PixelToScreen(startPos + 0.5f + (slotSize + edgeSize) * i + slotSize + 55f, startPos + 0.5f + slotSize + 58f);
                    */

                    int t = TextRenderer.GetWidth($"{amount}", 2);

                    int x = (int)(startPos + edgeSize + (slotSize + edgeSize) * i + 65 - t);
                    int y = (int)(startPos + edgeSize + 66 - 7*2);

                    textRenderer.Draw($"{amount}", x, y, 2);

                }
                else if (amount > 1)
                {
                    
                }
                else
                {

                }
            }
            textRenderer.End();



        }



    }
}
