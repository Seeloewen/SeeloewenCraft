using SeeloewenCraft.game.graphics;

namespace SeeloewenCraft.game.core.entities.inventory
{
    public class CreativeSlot : InventorySlot
    {
        public CreativeSlot(Inventory parentInv, int x, int y) : base(parentInv, x, y) { }

        public override void OnLeftClick()
        {
            InventorySlot selectedSlot = Inventory.GetGlobalSelectedInvSlot();

            //If no other slot is currently selected, add items to the other inventory
            if (selectedSlot == null && !IsEmpty())
            {
                int amount = 1;
                if (KeyBinds.pressed[KeyBinds.SNEAK]) amount = 64; //If shift is pressed, give player 64 items

                //Add items to the other inventory without removing from this one
                Inventory otherInv = GetOtherInventory();
                if (otherInv != null) otherInv.Add(id, amount, tag);
            }
            //If another slot is selected
            else if (selectedSlot != null)
            {
                selectedSlot.Remove(64);
                selectedSlot.Unselect();
            }
        }

        public override void OnRightClick()
        {
            InventorySlot otherSlot = Inventory.GetGlobalSelectedInvSlot();

            if (otherSlot != null) otherSlot.Remove(64);
        }
    }
}
