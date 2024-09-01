using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public class InventorySlot
    {
        //References
        public Inventory inventory;
        public Border bdrSlot = new Border();
        public Canvas cvsItem = new Canvas();
        public TextBlock tblItemAmount = new TextBlock();
        public ProgressBar pbDurability = new ProgressBar();
        public HotbarSlot hotbarSlot;

        //Variables
        public string itemId;
        public int xPos;
        public int yPos;
        public string itemTag;
        public bool isSelected;
        private int amount;

        public int Amount
        {
            set
            {
                //When setting the amount, also update the item amount textblock
                amount = value;
                tblItemAmount.Text = value.ToString();

                //If the amount is bigger than 0, show the textblock - if not, hide it
                if (amount > 0)
                {
                    tblItemAmount.Visibility = Visibility.Visible;
                }
                else
                {
                    tblItemAmount.Visibility = Visibility.Hidden;
                }
            }
            get
            {
                return amount;
            }
        }

        //-- Constructor --//

        public InventorySlot( Inventory inventory, int xPos, int yPos)
        {
            //Set the attributes
            this.inventory = inventory;
            this.xPos = xPos;
            this.yPos = yPos;

            //Setup the slot border
            bdrSlot.Width = 74;
            bdrSlot.Height = 74;
            bdrSlot.BorderThickness = new Thickness(3, 3, 3, 3);
            bdrSlot.Background = new SolidColorBrush(Colors.DarkGray);
            bdrSlot.MouseLeftButtonDown += bdrSlot_LeftMouseButtonDown;
            bdrSlot.MouseRightButtonDown += bdrSlot_RightMouseButtonDown;
            bdrSlot.Child = cvsItem;

            //Setup the slot textblock
            tblItemAmount.FontSize = 18;
            Canvas.SetLeft(tblItemAmount, 38);
            Canvas.SetTop(tblItemAmount, 34);

            //Setup progressbar
            pbDurability.Width = 36;
            pbDurability.Height = 7;
            Canvas.SetTop(pbDurability, 45);
            Canvas.SetLeft(pbDurability, -3);
            pbDurability.Visibility = Visibility.Hidden;

            //Setup canvas
            cvsItem.Width = 50;
            cvsItem.Height = 50;
            cvsItem.Children.Add(tblItemAmount);
            cvsItem.Children.Add(pbDurability);
            cvsItem.Margin = new Thickness(3, 3, 2.5, 2.5);
        }

        //-- Custom Methods --//

        public void Add(string id, int amount, string tag, out bool success)
        {
            success = true;

            //Check if the slot already has the specified id or is empty
            if (itemId == id || IsEmpty())
            {
                //If the slot is not empty and the item is not stackable
                if (Game.unstackableItems.Contains(id) && !IsEmpty())
                {
                    success = false;
                    return;
                }

                //Check if total amount would be above 64, which shouldn't be possible
                if (Amount + amount <= 64)
                {
                    //Update the slot
                    itemId = id;
                    Amount += amount;
                    itemTag = tag;
                    cvsItem.Background = ItemRegister.GenerateItem(id).image;

                    ToggleDurabilityDisplay();

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

        public void ToggleDurabilityDisplay()
        {
            if (GetDurability() != 0)
            {
                //Setup progressbar in slot
                ToolItem item = ItemRegister.GenerateItem(itemId) as ToolItem;
                pbDurability.Visibility = Visibility.Visible;
                pbDurability.Maximum = item.maxDurability;
                pbDurability.Value = GetDurability();

                //Setup progressbar in hotbar slot
                if (hotbarSlot != null)
                {
                    hotbarSlot.pbDurability.Visibility = Visibility.Visible;
                    hotbarSlot.pbDurability.Maximum = item.maxDurability;
                    hotbarSlot.pbDurability.Value = GetDurability();
                }
            }
            else
            {
                pbDurability.Visibility = Visibility.Hidden;
            }
        }

        public void Remove(int amount)
        {
            //Check if the total amount would be below 0, which shouldn't be possible
            if (Amount - amount >= 0)
            {
                //Update the slot and clear it if the amount is 0
                Amount -= amount;

                if (Amount == 0)
                {
                    cvsItem.Background = new SolidColorBrush(Colors.Transparent);
                    itemId = "";
                    pbDurability.Visibility = Visibility.Hidden;
                    if(hotbarSlot != null) hotbarSlot.pbDurability.Visibility= Visibility.Hidden;
                    inventory.UpdateHotbar();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Total item amount cannot be below 0");
            }
        }

        public int GetAvailableSpace()
        {
            //Return the available space, obviously
            return 64 - Amount;
        }

        public void Select()
        {
            //Select the slot and make it follow the mouse by selecting it in the game window
            Game.world.wndGame.ShowInvItem(this);
            cvsItem.Visibility = Visibility.Hidden;
            isSelected = true;
        }

        public void Unselect()
        {
            //Unselect the slot and make it no longer follow the mouse in the game window
            Game.world.wndGame.HideInvItem();
            cvsItem.Visibility = Visibility.Visible;
            isSelected = false;
        }

        public void MoveItem(InventorySlot newSlot, int amount) //Move item from this slot to another one
        {
            //Check if enough space is in the new slot available, if not, only add the amount of possible space
            if (newSlot.GetAvailableSpace() < amount)
            {
                amount = newSlot.GetAvailableSpace();
            }

            //Add to the new slot and remove from the old one
            newSlot.Add(itemId, amount, itemTag, out bool success);
            if (success)
            {
                Remove(amount);
            }
        }

        public bool IsEmpty()
        {
            //Check if no string is given and the amount is 0
            return string.IsNullOrEmpty(itemId) && Amount == 0;
        }

        public Inventory GetOtherInventory()
        {
            //Get the first other inventory that's in the list and is currently open. Since there should in most cases only be
            //One other inventory besides the main one, this works. It might cause issues with multiple issues, would need some
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
                int durability = GetDurability();
                itemTag = itemTag.Replace(durability.ToString(), (durability - 1).ToString());
                pbDurability.Value--;
                if(hotbarSlot != null) hotbarSlot.pbDurability.Value--;

                if (durability - 1 <= 0)
                {
                    Remove(1);
                }
            }
        }

        public int GetDurability()
        {
            //Only do on tools
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

        private void bdrSlot_LeftMouseButtonDown(object sender, EventArgs e)
        {
            InventorySlot selectedSlot = Game.world.GetSelectedInvSlot();

            //If no other slot is currently selected, select this slot
            if (selectedSlot == null && !IsEmpty())
            {
                Select();

                //If shift is pressed, try to move the items to another inventory 
                if (Game.world.wndGame.pressedKeys.Contains(Key.LeftShift))
                {
                    Inventory otherInv = GetOtherInventory();

                    if (otherInv != null)
                    {
                        otherInv.AddItem(itemId, amount, itemTag, out int remainingAmount);
                        Remove(Amount - remainingAmount);
                        Unselect();
                    }
                }
            }

            //If this slot is already selected and clicked again, simply unselect it to paste the items back in it
            else if (selectedSlot == this)
            {
                Unselect();
            }
            //If a different slot from this one is selected
            else if (selectedSlot != null && selectedSlot != this)
            {
                //Check if it's empty or contains the same item - May need a check for space
                if (itemId == selectedSlot.itemId || IsEmpty())
                {
                    //Try to move the items there
                    selectedSlot.MoveItem(this, selectedSlot.amount);
                    selectedSlot.Unselect();
                }
                else //Switch two slots
                {
                    //Save the old slot values and clear the slot
                    (string oldId, int oldAmount, string oldTag) = (itemId, amount, itemTag); //No idea why I used a tuple here, but it works I guess
                    itemId = "";
                    amount = 0;

                    //Try to move the items here
                    selectedSlot.MoveItem(this, selectedSlot.amount);
                    selectedSlot.Unselect();

                    //Move the items from this slot to the other one
                    selectedSlot.Add(oldId, oldAmount, oldTag, out bool success);
                    selectedSlot.Select();
                }
            }
        }

        private void bdrSlot_RightMouseButtonDown(object sender, EventArgs e)
        {
            InventorySlot selectedSlot = Game.world.GetSelectedInvSlot();

            if (selectedSlot != null)
            {
                //If a slot is selected and the slot has more than 1 item, move one singular item to that slot
                if ((itemId == selectedSlot.itemId || IsEmpty()) && selectedSlot.Amount > 1)
                {
                    selectedSlot.MoveItem(this, 1);
                    Game.world.wndGame.tblInvItem.Text = selectedSlot.Amount.ToString();
                }
            }else if(selectedSlot == null && Amount > 1)
            {
                //If no slot is selected and this slot has more than one item
                foreach(InventorySlot slot in inventory.slotList)
                {
                    //Search for an empty slot and move half of this slots items there, then select it.
                    //This will look like you halfed the stack at hand
                    if(slot.IsEmpty())
                    {
                        MoveItem(slot, Amount / 2);
                        slot.Select();
                        break;
                    }
                }
            }

        }
    }
}
