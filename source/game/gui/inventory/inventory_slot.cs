using System;
using System.Windows;
using System.Windows.Input;
using SeeloewenCraft.game.graphics;

namespace SeeloewenCraft
{
    public class InventorySlot
    {
        public Inventory inventory;
        public HotbarSlot hotbarSlot;

        public string itemId;
        public int xPos;
        public int yPos;
        public string itemTag;
        public bool isSelected;
        public int amount;

        public InventorySlot(Inventory inventory, int xPos, int yPos)
        {
            this.inventory = inventory;
            this.xPos = xPos;
            this.yPos = yPos;
        }

        public bool Add(string id, int amount, string tag) //Returns whether the items were successfully added
        {
            //Check if the slot already has the specified id or is empty
            if (itemId == id || IsEmpty())
            {
                //If the slot is not empty and the item is not stackable
                if (Game.unstackableItems.Contains(id) && !IsEmpty()) return false;

                if (this.amount + amount <= 64)
                {
                    //Update the slot
                    itemId = id;
                    this.amount += amount;
                    itemTag = tag;

                    if (inventory.block != null && inventory.block.chunk != null)
                    {
                        NetworkHandler.SendData(MultiplayerPacketType.ADD_TO_INV, inventory.block.xPos.ToString(), inventory.block.yPos.ToString(), inventory.block.chunk.index.ToString(), id, amount.ToString(), xPos.ToString(), yPos.ToString(), tag ??= "");
                    }

                    return true;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Total item amount cannot be above 64");
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot add item of different id to the slot");
            }
        }

        public void Add_Multiplayer(string id, string tag, int amount)
        {
            //Update the slot
            itemId = id;
            this.amount += amount;
            itemTag = tag;
        }

        public void Remove(int amount)
        {
            //Check if the total amount would be below 0, which shouldn't be possible
            if (this.amount - amount >= 0)
            {
                this.amount -= amount;

                if (this.amount == 0)
                {
                    //Clear the slot
                    itemId = null;
                    itemTag = null;
                    if (hotbarSlot != null) hotbarSlot.pbDurability.Visibility = Visibility.Hidden;
                }

                if (inventory.block != null && inventory.block.chunk != null)
                {
                    NetworkHandler.SendData(MultiplayerPacketType.REMOVE_FROM_INV, inventory.block.xPos.ToString(), inventory.block.yPos.ToString(), inventory.block.chunk.index.ToString(), amount.ToString(), xPos.ToString(), yPos.ToString());
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Total item amount cannot be below 0");
            }
        }

        public void Remove_Multiplayer(int amount)
        {
            //Update the slot and clear it if the amount is 0
            this.amount -= amount;

            if (this.amount == 0)
            {
                itemId = "";
            }
        }

        public int GetAvailableSpace() => 64 - amount;

        public void Select()
        {
            //Select the slot and unselect all other slots
            foreach (InventorySlot slot in inventory.slotList)
            {
                slot.Unselect();
            }

            isSelected = true;
        }

        public void Unselect() => isSelected = false;

        public void MoveItemTo(InventorySlot newSlot, int amount) //Move item from this slot to another one
        {
            //Check if enough space is in the new slot available, if not, only add the amount of possible space
            if (newSlot.GetAvailableSpace() < amount)
            {
                amount = newSlot.GetAvailableSpace();
            }

            //Add to the new slot and remove from the old one        
            if (newSlot.Add(itemId, amount, itemTag))
            {
                Remove(amount);
            }
        }

        public bool IsEmpty() => string.IsNullOrEmpty(itemId) && this.amount == 0;

        public Inventory GetOtherInventory()
        {
            //Get the first other inventory that's in the list and is currently open. Since there should in most cases only be
            //One other inventory besides the main one, this works. It might cause issues with multiple inventories, would need some
            //Improvements then to search for specific inventories.
            foreach (Inventory inventory in Game.world.inventoryList)
            {
                if (inventory != this.inventory && inventory.inventoryGui.isOpen)
                {
                    return inventory;
                }
            }
            return null;
        }

        public void RemoveDurablity()
        {
            if (ItemRegister.GenerateItem(itemId) is ToolItem && Game.world.gamemode == Gamemode.Survival)
            {
                //Update durability of the held tool item
                int durability = GetDurability();
                itemTag = itemTag.Replace(durability.ToString(), (durability - 1).ToString());

                if (durability - 1 <= 0) Remove(1); //Remove the item if it "breaks"
            }
        }

        public int GetDurability() //TODO: Rework with new tag system
        {
            if (ItemRegister.GenerateItem(itemId) is ToolItem)
            {
                //Split the tags and the durability tag to the durability
                string[] tagSplit = itemTag.Split(';');
                string[] durabilitySplit = tagSplit[0].Split('=');

                return Convert.ToInt32(durabilitySplit[1]);
            }

            return 0;
        }

        //-- Event Handlers --//

        public void OnLeftClick()
        {
            InventorySlot selectedSlot = Game.world.GetSelectedInvSlot();

            //If no other slot is currently selected, select this slot
            if (selectedSlot == null && !IsEmpty())
            {
                Select();

                //If shift is pressed, try to move the items to another inventory 
                if (InputHandler.pressedKeys.Contains(Key.LeftShift))
                {
                    Inventory otherInv = GetOtherInventory();

                    if (otherInv != null)
                    {
                        otherInv.AddItem(itemId, amount, itemTag, out int remainingAmount);
                        Remove(amount - remainingAmount);
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
                if (itemId == selectedSlot.itemId || IsEmpty())
                {
                    selectedSlot.MoveItemTo(this, selectedSlot.amount);
                    selectedSlot.Unselect();
                }
                else //Switch two slots
                {
                    //Save the old slot values and clear the slot
                    (string oldId, int oldAmount, string oldTag) = (itemId, amount, itemTag); //No idea why I used a tuple here, but it works I guess
                    itemId = "";
                    amount = 0;

                    selectedSlot.MoveItemTo(this, selectedSlot.amount); //Move content from other slot here
                    selectedSlot.Unselect();
   
                    selectedSlot.Add(oldId, oldAmount, oldTag); //Move the saved content from this slot to the other one
                    selectedSlot.Select();
                }
            }
        }

        public void OnRightClick()
        {
            InventorySlot selectedSlot = Game.world.GetSelectedInvSlot();

            if (selectedSlot != null)
            {
                //If a slot is selected and the slot has more than 1 item, move one singular item to that slot
                if ((itemId == selectedSlot.itemId || IsEmpty()) && selectedSlot.amount > 1)
                {
                    selectedSlot.MoveItemTo(this, 1);
                }
            }
            else if (selectedSlot == null && amount > 1)
            {
                //If no slot is selected and this slot has more than one item
                foreach (InventorySlot slot in inventory.slotList)
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
            if (itemTag != null && itemTag.Contains("durability="))
            {
                //Get current durability relative to max durability of the tool
                ToolItem item = (ToolItem)ItemRegister.GenerateItem(itemId);
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
