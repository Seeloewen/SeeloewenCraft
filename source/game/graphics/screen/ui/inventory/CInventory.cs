using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.graphics.ui_lib;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    internal class CInventory : CGui
    {
        internal Inventory invData;
        internal List<CInvSlot> slots = new List<CInvSlot>();

        internal CBorder cBorder;
        internal CText cHeader;

        internal readonly int slotsX;
        internal readonly int slotsY;
        internal readonly bool hasHotbar = true;

        internal CInventory(IGuiData data) : base(data, new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            //Setup data
            invData = (Inventory)data;

            slotsX = data.GetTag<int>("slotsX");
            slotsY = data.GetTag<int>("slotsY");

            int width = slotsX * GuiSizes.slotSize + 12 * GuiSizes.edgeSize + 15;
            int height = slotsY * GuiSizes.slotSize + 8 * GuiSizes.edgeSize + 40;

            //Setup gui
            SetBounds(new Rectangle(GuiSizes.mx - width / 2, GuiSizes.my - height / 2, GuiSizes.mx + width / 2, GuiSizes.my + height / 2));

            cHeader = new CText(data.GetTag<string>("header"), 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));
            AddChild(cHeader);

            cBorder = new CBorder(5, new Color(0.5f));
            AddChild(cBorder);

            //Add all the slots
            for (int y = slotsY - 1; y >= 0; y--)
            {
                for (int x = 0; x < slotsX; x++)
                {
                    CInvSlot slot = new CInvSlot(x, y, slotsX, slotsY, hasHotbar);
                    slots.Add(slot);
                    AddChild(slot);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (CInvSlot cSlot in slots)
            {
                //Update all the slots and check whether one is selected
                InventorySlot slot = invData.GetSlot(cSlot.x, cSlot.y);
                if (slot.isSelected)
                {
                    Screen.guiHandler.SetMouseFollower(slot); //If a slot is currently selected, it should follow the mouse
                }
                cSlot.Update(slot.itemId, slot.amount, slot.GetRelativeDurability(), slot.isSelected);
            }

        }
    }
}
