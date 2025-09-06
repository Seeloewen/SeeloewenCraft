using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.graphics.ui_lib;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    internal class CHotbar : CRectangle
    {
        private Inventory invData;

        private CRectangle slotBorder;
        private List<CHotbarSlot> slots = new List<CHotbarSlot>();

        private const int slotAmount = 9; //TODO: should be changeable

        private const int startPos = 20;
        private const int edgeSize = 5;
        private const int slotSize = 70;

        public CHotbar(Inventory invData) : base(new Color(0.6f, 0.6f, 0.6f), new Rectangle(startPos, startPos, startPos + 10 * edgeSize + 9 * slotSize, startPos + 2 * edgeSize + slotSize))
        {
            this.invData = invData;

            //Backplate
            (int x1, int y1) = (startPos, startPos);
            (int x2, int y2) = (startPos + 10 * edgeSize + 9 * slotSize, startPos + 2 * edgeSize + slotSize);

            //Slot Border
            (x2, y2) = (startPos + slotSize + edgeSize * 2, startPos + slotSize + edgeSize * 2);
            slotBorder = new CRectangle(new Color(0.3f, 0.3f, 0.3f), new Rectangle(x1, y1, x2, y2));
            AddChild(slotBorder);

            //Slots
            for (int i = 0; i < slotAmount; i++)
            {
                (x1, y1) = (startPos + edgeSize + (slotSize + edgeSize) * i, startPos + edgeSize);
                (x2, y2) = (startPos + edgeSize + (slotSize + edgeSize) * i + slotSize, startPos + edgeSize + slotSize);

                CHotbarSlot slot = new CHotbarSlot(new Rectangle(x1, y1, x2, y2), invData);
                slots.Add(slot);
                AddChild(slot);
            }
        }

        protected override void OnUpdate()
        {
            if (Screen.allowIngameInputs)
            {
                //Calculate which hotbar slot is currently selected
                int scrollOffset = -InputHandler.scrollAmount;
                int currentIndex = Game.world.player.inventory.GetSelectedHotbarIndex();
                int newIndex = (((currentIndex + scrollOffset) % slotAmount) + slotAmount) % slotAmount;

                for (int i = 0; i < slotAmount; i++)
                {
                    InventorySlot slot = invData.GetHotbarSlot(i);
                    CHotbarSlot cHotbarSlot = slots[i];

                    cHotbarSlot.Update(slot.id, slot.GetItemName(), slot.amount, slot.GetRelativeDurability());

                    //Update the display of the currently selected hotbar slot
                    if (i == newIndex && scrollOffset != 0)
                    {
                        slot.SelectInHotbar();
                        (int x1, int y1) = (startPos + edgeSize * slot.xPos + slotSize * slot.xPos, startPos);
                        (int x2, int y2) = (x1 + slotSize + edgeSize * 2, y1 + slotSize + edgeSize * 2);

                        slotBorder.SetBounds(new Rectangle(x1, y1, x2, y2));
                    }
                }
            }
        }
    }
}
