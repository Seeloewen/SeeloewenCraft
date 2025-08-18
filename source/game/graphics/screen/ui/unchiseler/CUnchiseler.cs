using SeeloewenCraft.game.core;
using SeeloewenCraft.game.graphics.ui_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.graphics
{
    internal class CUnchiseler : CGui
    {
        internal Inventory invData;

        private CText cHeader;
        private CUnchiselerSlot cUnchiselerSlot;
        private CButton cButton;
        private CBorder cBorder;
        internal CUnchiseler(IGuiData data) : base(data, new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            //Display
            SetBounds(new Rectangle(GuiSizes.mx - 100, GuiSizes.my - 100, GuiSizes.mx + 100, GuiSizes.my + 100));

            cHeader = new CText("Unchiseler", 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));
            cBorder = new CBorder(5, new Color(0.5f));
            cUnchiselerSlot = new CUnchiselerSlot(bounds.x1P + 65, bounds.y1P + 47);
            cButton = new CButton(cButton_OnClick, "Break", "sc:button_1", GeneralTextureMap.Get(), new Rectangle(bounds.x1P + 50, bounds.y1P + 135, bounds.x2P - 50, bounds.y1P + 175));

            AddChild(cUnchiselerSlot);
            AddChild(cBorder);
            AddChild(cHeader);
            AddChild(cButton);

            //Data
            invData = ((UnchiselHandler)data).inv;
        }

        protected override void OnUpdate()
        {
            //Update the slot based on the inventory connected with the block
            InventorySlot slot = invData.slotList[0];
            cUnchiselerSlot.Update(slot.itemId, slot.amount, slot.GetRelativeDurability(), slot.isSelected);
            if (slot.isSelected) Screen.guiHandler.SetMouseFollower(slot);
        }

        private void cButton_OnClick()
        {
            ((UnchiselHandler)data).Unchisel();
        }
    }
}
