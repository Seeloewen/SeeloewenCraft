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
        private World world;
        public Inventory inventory;
        public Border bdrSlot = new Border();
        public Canvas cvsItem = new Canvas();
        public TextBlock tblItemAmount = new TextBlock();

        //Variables
        public string itemId;
        public int xPos;
        public int yPos;
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

        public InventorySlot(World world, Inventory inventory, int xPos, int yPos)
        {
            //Set the attributes
            this.world = world;
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

            //Setup canvas
            cvsItem.Width = 50;
            cvsItem.Height = 50;
            cvsItem.Children.Add(tblItemAmount);
            cvsItem.Margin = new Thickness(3, 3, 2.5, 2.5);
        }

        //-- Custom Methods --//

        public void Add(string id, int amount)
        {

            //Check if the slot already has the specified id or is empty
            if (itemId == id || IsEmpty())
            {
                //Check if total amount would be above 64, which shouldn't be possible
                if (Amount + amount <= 64)
                {
                    //Update the slot
                    itemId = id;
                    Amount += amount;
                    cvsItem.Background = ItemRegister.GenerateItem(id, world).image;
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
            world.wndGame.ShowInvItem(this);
            cvsItem.Visibility = Visibility.Hidden;
            isSelected = true;
        }

        public void Unselect()
        {
            //Unselect the slot and make it no longer follow the mouse in the game window
            world.wndGame.HideInvItem();
            cvsItem.Visibility = Visibility.Visible;
            isSelected = false;
        }

        public void MoveItem(InventorySlot newSlot, int amount)
        {
            //Check if enough space is in the new slot available, if not, only add the amount of possible space
            if (newSlot.GetAvailableSpace() < amount)
            {
                amount = newSlot.GetAvailableSpace();
            }

            //Add to the new slot and remove from the old one
            newSlot.Add(itemId, amount);
            Remove(amount);
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

            foreach (Inventory inventory in world.inventoryList)
            {
                if (inventory != this.inventory && inventory.inventoryGui.isOpen)
                {
                    return inventory;
                }
            }
            return null;
        }
        //-- Event Handlers --//

        private void bdrSlot_LeftMouseButtonDown(object sender, EventArgs e)
        {
            InventorySlot selectedSlot = world.GetSelectedInvSlot();

            //If no other slot is currently selected, select this slot
            if (selectedSlot == null && !IsEmpty())
            {
                Select();

                //If shift is pressed, try to move the items to another inventory 
                if (world.wndGame.pressedKeys.Contains(Key.LeftShift))
                {
                    Inventory otherInv = GetOtherInventory();

                    if (otherInv != null)
                    {
                        otherInv.AddItem(itemId, amount, out int remainingAmount);
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
            }
        }

        private void bdrSlot_RightMouseButtonDown(object sender, EventArgs e)
        {
            InventorySlot selectedSlot = world.GetSelectedInvSlot();

            if (selectedSlot != null)
            {
                //If a slot is selected and the slot has more than 1 item, move one singular item to that slot
                if ((itemId == selectedSlot.itemId || IsEmpty()) && selectedSlot.Amount > 0)
                {
                    selectedSlot.MoveItem(this, 1);
                    world.wndGame.tblInvItem.Text = selectedSlot.Amount.ToString();
                }
            }
        }
    }
}
