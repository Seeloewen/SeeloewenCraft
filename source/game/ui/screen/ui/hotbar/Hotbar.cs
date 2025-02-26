using System;
using System.Collections.Generic;
using SeeloewenCraft.game.ui.ui_lib;

namespace SeeloewenCraft.game.ui
{
    internal class Hotbar : CRectangle
    {
        private CRectangle slotBorder;
        private CSlot[] slots = new CSlot[9];

        private const int startPos = 20;
        private const int edgeSize = 5;
        private const int slotSize = 70;

        public Hotbar() : base(new Color(0.6f, 0.6f, 0.6f), new Rectangle(startPos, startPos, startPos + 10 * edgeSize + 9 * slotSize, startPos + 2 * edgeSize + slotSize))
        {
            List<HotbarSlot> slots = Game.world.player.inventory.hotbarSlotList;

            //Backplate
            (int x1, int y1) = (startPos, startPos);
            (int x2, int y2) = (startPos + 10 * edgeSize + 9 * slotSize, startPos + 2 * edgeSize + slotSize);

            //Slot Border
            (x2, y2) = (startPos + slotSize + edgeSize * 2, startPos + slotSize + edgeSize * 2);
            slotBorder = new CRectangle(new Color(0.3f, 0.3f, 0.3f), new Rectangle(x1, y1, x2, y2));
            AddChild(slotBorder);

            //Slots
            foreach (HotbarSlot slot in slots)
            {
                int i = slot.xPos;
                (x1, y1) = (startPos + edgeSize + (slotSize + edgeSize) * i, startPos + edgeSize);
                (x2, y2) = (startPos + edgeSize + (slotSize + edgeSize) * i + slotSize, startPos + edgeSize + slotSize);

                CSlot cSlot = new CSlot(new Rectangle(x1, y1, x2, y2));
                this.slots[i] = cSlot;
                AddChild(cSlot);
            }
        }

        protected override void OnUpdate()
        {
            int s = -InputHandler.HandleScrollOffset();
            int c = Game.world.player.inventory.GetSelectedHotbarIndex();
            int n = (((c + s) % 9) + 9) % 9;

            foreach (HotbarSlot slot in Game.world.player.inventory.hotbarSlotList)
            {
                slots[slot.xPos].SetItem(slot.slot.itemId);
                slots[slot.xPos].SetAmount(slot.slot.Amount);

                if (slot.slot.itemTag != null && slot.slot.itemTag.Contains("durability="))
                {
                    ToolItem item = (ToolItem)ItemRegister.GenerateItem(slot.slot.itemId);
                    float d2 = item.maxDurability;
                    float d1 = slot.slot.GetDurability();

                    float d = d1 / d2;
                    d = Math.Min(1f, d);

                    slots[slot.xPos].SetDurability(d);
                }
                else
                {
                    slots[slot.xPos].SetDurability(0);
                }

                if (slot.xPos == n)
                {
                    slot.Select();
                    (int x1, int y1) = (startPos + edgeSize * slot.xPos + slotSize * slot.xPos, startPos);
                    (int x2, int y2) = (x1 + slotSize + edgeSize * 2, y1 + slotSize + edgeSize * 2);

                    slotBorder.SetBounds(new Rectangle(x1, y1, x2, y2));
                }
            }
        }
    }
}
