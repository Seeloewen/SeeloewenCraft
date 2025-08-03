using SeeloewenCraft.game.ui.ui_lib;
using System.Collections.Generic;

namespace SeeloewenCraft.game.ui
{
    internal class CInventory : CGui
    {
        internal Inventory invData;
        internal List<CInvSlot> slots = new List<CInvSlot>();

        internal CInvSlot cMouseFollower;

        private const int invWidth = 9; //TODO: should be changeable
        private const int invHeight = 4;

        internal CInventory(IGuiData invData) : base(invData, new Color(0.9f), new Rectangle(0, 0, 0, 0))
        {
            this.invData = (Inventory)invData;

            int width = 9 * InvSizes.slotSize + 12 * InvSizes.edgeSize;
            int height = 4 * InvSizes.slotSize + 8 * InvSizes.edgeSize;

            SetBounds(new Rectangle(InvSizes.mx - width / 2, InvSizes.my - height / 2, InvSizes.mx + width / 2, InvSizes.my + height / 2));
            AddChild(new CBorder(3, new Color(.0f)));

            for (int y = invHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < invWidth; x++)
                {
                    CInvSlot slot = new CInvSlot(x, y);
                    slots.Add(slot);
                    AddChild(slot);
                }
            }

            cMouseFollower = new CInvSlot(2, 0);
            cMouseFollower.SetColor(new Color(0, 0, 0, 0));
            cMouseFollower.isMouseFollower = true;
            AddChild(cMouseFollower);
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
