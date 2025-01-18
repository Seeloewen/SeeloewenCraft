using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using static System.Reflection.Metadata.BlobBuilder;

namespace SeeloewenCraft.game.ui
{
    public static class HotbarScreen
    {

        const int startPos = 20;
        const int edgeSize = 5;
        const int slotSize = 70;


        internal static void Update()
        {
            int s = -InputHandler.HandleScrollOffset();
            int c = Game.world.player.inventory.GetSelectedHotbarIndex();
            int n = (((c + s) % 9) + 9) % 9;
            foreach (HotbarSlot slot in Game.world.player.inventory.hotbarSlotList)
            {
                if (slot.xPos == n) slot.Select();
            }
        }


        internal static void Render()
        {
            List<HotbarSlot> slots = Game.world.player.inventory.hotbarSlotList;

            //backplate

            PrimitiveRenderer.Begin();
            (int x1, int y1) = (startPos, startPos);
            (int x2, int y2) = (startPos + 10 * edgeSize + 9 * slotSize, startPos + 2 * edgeSize + slotSize);

            PrimitiveRenderer.DrawRectangle(x1, y1, x2, y2, 0.4f, 0.4f, 0.4f);


            foreach (HotbarSlot slot in slots)
            {
                int i = slot.xPos;
                (x1, y1) = (startPos + edgeSize + (slotSize + edgeSize) * i, startPos + edgeSize);
                (x2, y2) = (startPos + edgeSize + (slotSize + edgeSize) * i + slotSize, startPos + edgeSize + slotSize);

                if (slot.isSelected) PrimitiveRenderer.DrawRectangle(x1 - edgeSize, y1 - edgeSize, x2 + edgeSize, y2 + edgeSize, 0.1f, 0.1f, 0.1f);

                PrimitiveRenderer.DrawRectangle(x1, y1, x2, y2, 0.8f, 0.8f, 0.8f);


            }
            PrimitiveRenderer.End();

            ItemRenderer.SetTexture();
            TextureRenderer.Begin();
            foreach (HotbarSlot slot in slots)
            {
                string itemID = slot.slot.itemId;
                if (itemID == null) continue;
                int i = slot.xPos;

                (x1, y1) = (startPos + edgeSize * 2 + (slotSize + edgeSize) * i, startPos + edgeSize * 2);
                (x2, y2) = (startPos + (slotSize + edgeSize) * i + slotSize, startPos + slotSize);

                ItemRenderer.Draw(itemID, x1, y1, x2, y2);


            }
            TextureRenderer.End();


            TextRenderer.Begin();
            foreach (HotbarSlot slot in slots)
            {
                int amount = slot.slot.Amount;

                int i = slot.xPos;

                if (amount > 1)
                {

                    int t = TextRenderer.GetWidth($"{amount}", 2);

                    int x = (int)(startPos + edgeSize + (slotSize + edgeSize) * i + 65 - t);
                    int y = (int)(startPos + edgeSize + 66 - 7 * 2);

                    TextRenderer.Draw($"{amount}", x, y, 2);

                }
            }
            TextRenderer.End();

            PrimitiveRenderer.Begin();
            foreach (HotbarSlot slot in slots)
            {
                if(slot.slot.Amount == 0)
                {
                    continue;
                }

                if (slot.slot.itemTag.Contains("durability="))
                {
                    ToolItem item = (ToolItem)ItemRegister.GenerateItem(slot.slot.itemId);
                    float d2 = item.maxDurability;
                    float d1 = slot.slot.GetDurability();

                    float d = d1/d2;
                    d = Math.Min(1f, d);

                    int i = slot.xPos;



                    x1 = (startPos + edgeSize + (slotSize + edgeSize) * i + 7);
                    y1 = (startPos + edgeSize + 60);
                    x2 = x1 + 56;
                    y2 = y1 + 7;
                    PrimitiveRenderer.DrawRectangle(x1, y1, x2, y2, 0f, 0f, 0f);

                    float g = Math.Min(1f, 2 * d);
                    float r = Math.Min(1f, 2 - 2 * d);
                    x2 = (int)(x1 + (x2 - x1) * d);
                    PrimitiveRenderer.DrawRectangle(x1, y1, x2, y2, r, g, 0f);

                }
            }
            PrimitiveRenderer.End();


        }



    }
}
