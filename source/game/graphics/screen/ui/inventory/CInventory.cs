using SeeloewenCraft.game.graphics.ui_lib;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    internal class CInventory : CGui
    {
        internal Inventory invData;
        internal List<CInvSlot> slots = new List<CInvSlot>();

        internal CInvSlot cMouseFollower;
        internal CBorder cBorder;
        internal CText cHeader;

        internal readonly int slotsX;
        internal readonly int slotsY;
        internal readonly bool hasHotbar = true;

        internal CInventory(IGuiData invData) : base(invData, new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            //Setup data
            this.invData = (Inventory)invData;

            slotsX = invData.GetTag<int>("slotsX");
            slotsY = invData.GetTag<int>("slotsY");

            int width = slotsX * GuiSizes.slotSize + 12 * GuiSizes.edgeSize + 15;
            int height = slotsY * GuiSizes.slotSize + 8 * GuiSizes.edgeSize + 40;

            //Setup gui
            SetBounds(new Rectangle(GuiSizes.mx - width / 2, GuiSizes.my - height / 2, GuiSizes.mx + width / 2, GuiSizes.my + height / 2));
            
            cHeader = new CText(invData.GetTag<string>("header"), 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));
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

        internal override void PostInit()
        {
            cMouseFollower = new CInvSlot();
            cMouseFollower.SetColor(new Color(0, 0, 0, 0));
            cMouseFollower.isMouseFollower = true;
            parent.AddChild(cMouseFollower);
        }

        protected override void OnUpdate()
        {
            InventorySlot selectedSlot = null;

            foreach (CInvSlot cSlot in slots)
            {
                //Update all the slots and check whether one is selected
                InventorySlot slot = invData.GetSlot(cSlot.x, cSlot.y);
                if (slot.isSelected) selectedSlot = slot;

                cSlot.Update(slot.itemId, slot.amount, slot.GetRelativeDurability(), slot.isSelected);
            }

            //If a slot is currently selected, it should follow the mouse
            SetMouseFollower(selectedSlot);

            if(cMouseFollower.visible) cMouseFollower.MoveTo(InputHandler.mouseXPixel - cMouseFollower.width + 10, InputHandler.mouseYPixel - cMouseFollower.height + 10);
        }

        private void SetMouseFollower(InventorySlot slot)
        {
            if (slot == null)
            {
                cMouseFollower.visible = false;
                return;
            }

            cMouseFollower.visible = true;
            cMouseFollower.Update(slot.itemId, slot.amount, 0);
        }
    }
}
