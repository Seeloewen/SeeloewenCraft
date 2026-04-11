using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics;
using System;
using System.Windows.Input;

namespace SeeloewenCraft.game.core.entities.inventory
{
    public class InventorySlot
    {
        internal const int MAX_AMOUNT = 64;

        private Inventory parentInv;
        public readonly int posX;
        public readonly int posY;

        public string id = "";
        public string tag = "";
        public int amount;

        public bool isSelected { get; private set; }

        internal bool isHotbar;
        internal bool isHotbarSelected { get; private set; }

        public InventorySlot(Inventory parentInv, int posX, int posY)
        {
            this.parentInv = parentInv;
            this.posX = posX;
            this.posY = posY;
        }

        public int Add(string id, int amount, string tag = "") //Returns the amount of items that weren't added
        {
            //Check if the slot already has the specified id or is empty
            if (this.id == id || IsEmpty())
            {
                bool isUnstackable = Item.unstackableItems.Contains(id);
                //If the slot is not empty and the item is not stackable, don't add anything
                if (isUnstackable && !IsEmpty()) return amount;

                this.id = id;
                this.tag = tag;

                int a = Math.Min(amount, GetAvailableSpace()); //The amount of items to add to this slot
                if (isUnstackable) a = 1; //Only add one item if the item is unstackable
                this.amount += a;
                return amount - a; //Return the remaining item amount

                //TODO: Networking Rework
                /*if (inventory.block != null && inventory.block.chunk != null)
                {
                    NetworkHandler.SendData(MultiplayerPacketType.ADD_TO_INV, inventory.block.posX.ToString(), inventory.block.posY.ToString(), inventory.block.chunk.index.ToString(), id, amount.ToString(), posX.ToString(), posY.ToString(), tag ??= "");
                }*/
            }

            return amount;
        }

        //TODO: Multiplayer rewrite
        /*public void Add_Multiplayer(string id, string tag, int amount)
        {
            //Update the slot
            itemId = id;
            this.amount += amount;
            itemTag = tag;
        }*/

        public int Remove(int amount)
        {
            //Check if the total amount would be below 0, which shouldn't be possible
            int r = Math.Min(this.amount, amount); //Remove at most the amount of items that is available
            this.amount -= r;

            if (this.amount == 0)
            {
                tag = "";
                id = "";
            }

            return amount - r;

            //TODO: Multiplayer rewrite
            /*if (inventory.block != null && inventory.block.chunk != null)
            {
                NetworkHandler.SendData(MultiplayerPacketType.REMOVE_FROM_INV, inventory.block.posX.ToString(), inventory.block.posY.ToString(), inventory.block.chunk.index.ToString(), amount.ToString(), posX.ToString(), posY.ToString());
            }*/
        }

        //TODO: Multiplayer rewrite
        /*public void Remove_Multiplayer(int amount)
        {
            //Update the slot and clear it if the amount is 0
            this.amount -= amount;

            if (this.amount == 0)
            {
                itemId = "";
            }
        }*/

        public int GetAvailableSpace() => MAX_AMOUNT - amount;

        public void Select()
        {
            //Select the slot and unselect all other slots
            foreach (InventorySlot slot in parentInv.slots)
            {
                slot.Unselect();
            }

            isSelected = true;
        }

        public void SelectInHotbar()
        {
            //Select the slot and unselect all other slots
            for(int x = 0; x < parentInv.slotsX; x++)
            {
                parentInv.GetSlot(x, parentInv.slotsY - 1).UnselectInHotbar();
            }

            isHotbarSelected = true;
        }

        public void Unselect() => isSelected = false;

        public void UnselectInHotbar() => isHotbarSelected = false;

        public virtual void MoveItemTo(InventorySlot newSlot, int amount) //Move item from this slot to another one
        {
            //Check if enough space is in the new slot available, if not, only add the amount of possible space
            amount = Math.Min(newSlot.GetAvailableSpace(), amount);

            //Add to the new slot and remove from the old one        
            newSlot.Add(id, amount, tag);
            Remove(amount);
        }

        public bool IsEmpty() => string.IsNullOrEmpty(id) && this.amount == 0;

        public string GetItemName()
        {
            if (IsEmpty()) return "";

            return ItemRegister.Get(id).name;
        }

        public Inventory GetOtherInventory()
        {
            //Get the first other inventory that's in the list and is currently open. Since there should in most cases only be
            //One other inventory besides the main one, this works. It might cause issues with multiple inventories, would need some
            //Improvements then to search for specific inventories.
            foreach (Inventory inventory in Inventory.globalInventories)
            {
                if (inventory != parentInv && inventory.isOpen)
                {
                    return inventory;
                }
            }
            return null;
        }

        public void RemoveDurablity()
        {
            if (ItemRegister.Get(id) is ToolItem && World.Get().gamemode == Gamemode.Survival)
            {
                //Update durability of the held tool item
                int durability = GetDurability();
                tag = tag.Replace(durability.ToString(), (durability - 1).ToString());

                if (durability - 1 <= 0) Remove(1); //Remove the item if it "breaks"
            }
        }

        public int GetDurability() //TODO: Rework with new tag system
        {
            if (ItemRegister.Get(id) is ToolItem)
            {
                //Split the tags and the durability tag to the durability
                string[] tagSplit = tag.Split(';');
                string[] durabilitySplit = tagSplit[0].Split('=');

                return Convert.ToInt32(durabilitySplit[1]);
            }

            return 0;
        }

        //-- Event Handlers --//

        public virtual void OnLeftClick()
        {
            InventorySlot selectedSlot = Inventory.GetGlobalSelectedInvSlot();

            //If no other slot is currently selected, select this slot
            if (selectedSlot == null && !IsEmpty())
            {
                Select();

                //If shift is pressed, try to move the items to another inventory 
                if (KeyBinds.pressed[KeyBinds.SNEAK])
                {
                    Inventory otherInv = GetOtherInventory();

                    if (otherInv != null)
                    {
                        int remaining = otherInv.Add(id, amount, tag);
                        Remove(amount - remaining);
                        Unselect();
                    }
                }
            }

            else if (selectedSlot == this)
            {
                Unselect(); //Unselect and paste the items back here
            }
            //If a different slot from this one is selected
            else if (selectedSlot != null && selectedSlot != this)
            {
                //Check if it's empty or contains the same item - May need a check for space
                if (id == selectedSlot.id || IsEmpty())
                {
                    selectedSlot.MoveItemTo(this, selectedSlot.amount);
                    selectedSlot.Unselect();
                }
                else //Switch two slots
                {
                    //Save the old slot values and clear the slot
                    (string oldId, int oldAmount, string oldTag) = (id, amount, tag); //No idea why I used a tuple here, but it works I guess
                    id = "";
                    amount = 0;

                    selectedSlot.MoveItemTo(this, selectedSlot.amount); //Move content from other slot here
                    selectedSlot.Unselect();

                    selectedSlot.Add(oldId, oldAmount, oldTag); //Move the saved content from this slot to the other one
                    selectedSlot.Select();
                }
            }
        }

        public virtual void OnRightClick()
        {
            InventorySlot selectedSlot = Inventory.GetGlobalSelectedInvSlot();

            if (selectedSlot != null)
            {
                //If a slot is selected and the slot has more than 1 item, move one singular item to that slot
                if ((id == selectedSlot.id || IsEmpty()) && selectedSlot.amount > 1)
                {
                    selectedSlot.MoveItemTo(this, 1);
                }
            }
            else if (selectedSlot == null && amount > 1)
            {
                //If no slot is selected and this slot has more than one item
                foreach (InventorySlot slot in parentInv.slots)
                {
                    //Search for an empty slot and move half of this slots items there, then select it.
                    //This will look like you halfed the stack at hand
                    if (slot.IsEmpty())
                    {
                        MoveItemTo(slot, amount / 2);
                        slot.Select();
                        break;
                    }
                }
            }
        }

        public float GetRelativeDurability()
        {
            if (tag != null && tag.Contains("durability="))
            {
                //Get current durability relative to max durability of the tool
                ToolItem item = (ToolItem)ItemRegister.Get(id);
                float d2 = item.maxDurability;
                float d1 = GetDurability();

                float d = d1 / d2;
                d = Math.Min(1f, d);

                return d;
            }

            return 0;
        }
    }
}
