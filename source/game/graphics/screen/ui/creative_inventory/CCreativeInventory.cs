using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.graphics.ui_lib;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    internal class CCreativeInventory : CGui
    {
        internal Inventory invData;

        private readonly int slotsX;
        private readonly int slotsY;

        private CText cHeader;
        private CBorder cBorder;
        private CScrollPane scrollPane;
        private List<CInvSlot> invSlots = new List<CInvSlot>();

        internal CCreativeInventory(IGuiData data) : base(data, new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            //Setup data
            invData = (Inventory)data;

            slotsX = data.GetTag<int>("slotsX");
            slotsY = data.GetTag<int>("slotsY");

            int width = slotsX * GuiSizes.slotSize + 12 * GuiSizes.edgeSize + 15;
            int height = 345;

            SetBounds(new Rectangle(GuiSizes.mx - width / 2, GuiSizes.my - height / 2, GuiSizes.mx + width / 2, GuiSizes.my + height / 2));

            cHeader = new CText(data.GetTag<string>("header"), 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));
            AddChild(cHeader);

            scrollPane = new CScrollPane(GetColor(), new Rectangle(bounds.x1P,bounds.y1P + 40, bounds.x1P + width - 1, bounds.y1P + height - 15), slotsY * (GuiSizes.slotSize + GuiSizes.edgeSize) - height + 55);
            AddChild(scrollPane);

            cBorder = new CBorder(5, new Color(0.5f));
            AddChild(cBorder);

            //Add all the slots
            for(int  y = 0; y < slotsY; y++)
            {
                for (int x = 0; x < slotsX; x++)
                {
                    CInvSlot slot = new CInvSlot(x, y, invData,new Rectangle(
                        bounds.x1P + 17 + (GuiSizes.slotSize + GuiSizes.edgeSize) * x,
                        bounds.y1P + 40 + (GuiSizes.slotSize + GuiSizes.edgeSize) * y,
                        bounds.x1P + 17 + (GuiSizes.slotSize + GuiSizes.edgeSize) * (x + 1) - 5,
                        bounds.y1P + 40 + (GuiSizes.slotSize + GuiSizes.edgeSize) * (y + 1) - 5));
                    invSlots.Add(slot);
                    scrollPane.AddScrollable(slot);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (CInvSlot cSlot in invSlots)
            {
                //Update all the slots and check whether one is selected
                InventorySlot slot = invData.GetSlot(cSlot.x, cSlot.y);
                if (slot.isSelected)
                {
                    Screen.guiHandler.SetMouseFollower(slot); //If a slot is currently selected, it should follow the mouse
                }
                cSlot.Update(slot.id, slot.GetItemName(), slot.amount, slot.GetRelativeDurability(), slot.isSelected);
            }

        }
    }
}
