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

        internal readonly int invWidth = 9; //TODO: should be changeable
        internal readonly int invHeight = 4;
        internal readonly bool hasHotbar = true;

        internal CInventory(IGuiData invData) : base(invData, new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            this.invData = (Inventory)invData;

            int width = 9 * GuiSizes.slotSize + 12 * GuiSizes.edgeSize + 15;
            int height = 4 * GuiSizes.slotSize + 8 * GuiSizes.edgeSize + 40;

            SetBounds(new Rectangle(GuiSizes.mx - width / 2, GuiSizes.my - height / 2, GuiSizes.mx + width / 2, GuiSizes.my + height / 2));
            
            cHeader = new CText("inventory_replace", 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));
            AddChild(cHeader);
            
            cBorder = new CBorder(5, new Color(0.5f));
            AddChild(cBorder);

            for (int y = invHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < invWidth; x++)
                {
                    CInvSlot slot = new CInvSlot(x, y, hasHotbar);
                    slots.Add(slot);
                    AddChild(slot);
                }
            }
        }

        internal override void PostInit()
        {
            cMouseFollower = new CInvSlot(2, 0);
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
