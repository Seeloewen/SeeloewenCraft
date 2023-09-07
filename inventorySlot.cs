using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class InventorySlot
    {
        public Border bdrSlot = new Border();
        public TextBlock tblItemAmount = new TextBlock();
        public List<Item> items = new List<Item>();
        wndGame wndGame;
        public int xPos;
        public int yPos;
        public int itemAmount;
        public bool isSelected;

        public InventorySlot(wndGame wndGame, int xPos, int yPos)
        {
            //Set the attributes
            this.wndGame = wndGame;
            this.xPos = xPos;
            this.yPos = yPos;

            //Setup the slot border
            bdrSlot.Width = 100;
            bdrSlot.Height = 100;
            bdrSlot.BorderThickness = new Thickness(3, 3, 3, 3);
            bdrSlot.Background = new SolidColorBrush(Colors.DarkGray);
            bdrSlot.MouseDown += bdrSlot_MouseDown;

            //Setup the slot textblock
            tblItemAmount.FontSize = 20;
            Canvas.SetLeft(tblItemAmount, 50);
            Canvas.SetTop(tblItemAmount, 45);
        }

        public void SetItem(Item item)
        {
            //Remove the item's canvas and the current slots textblock from previous parents
            wndGame.RemoveFromParent(item.cvsItem);
            wndGame.RemoveFromParent(tblItemAmount);

            //Add the item to the current slot and set the amount to 1 since it's the first item in the slot
            items.Add(item);
            bdrSlot.Child = item.cvsItem;
            item.cvsItem.Children.Add(tblItemAmount);
            itemAmount = 1;
            tblItemAmount.Text = "1";
        }

        public void AddToSlot(Item item)
        {
            //If the slot already contains items of the same type
            if (item.itemName == items[0].itemName)
            {
                //Add the item to the slot and increase the amount
                items.Add(item);
                itemAmount++;
                tblItemAmount.Text = itemAmount.ToString();
            }
        }
        public void ClearSlot()
        {
            //Remove all items from the slot and reset the border & textblock
            bdrSlot.Child = null;
            items.Clear();
            itemAmount = 0;
            tblItemAmount.Text = "";
        }

        private void bdrSlot_MouseDown(object sender, EventArgs e)
        {
            bool wasJustSelected = false;
            foreach (Inventory inventory in wndGame.inventoryList)
            {
                foreach (InventorySlot slot in inventory.slotList)
                {
                    //Go through every slot in the inventory and check if it's selected, not the currently clicked one and doesn't contain items
                    if (slot.isSelected && slot != this && items.Count == 0)
                    {
                        //Add all items from the other slot to this slot
                        foreach (Item item in slot.items)
                        {
                            items.Add(item);
                        }
                        itemAmount = slot.itemAmount;

                        foreach (Item item in items)
                        {
                            //Change the attributes of the items to the current slot
                            item.xPos = xPos;
                            item.yPos = yPos;

                            //Remove the item canvas from it's old parent border
                            wndGame.RemoveFromParent(item.cvsItem);

                            //Remove the item textblock from it's old parent object and update it
                            wndGame.RemoveFromParent(tblItemAmount);
                            tblItemAmount.Text = itemAmount.ToString();
                        }

                        //Add the canvas of item in the slot to the current slots border
                        bdrSlot.Child = items[0].cvsItem;
                        items[0].cvsItem.Children.Clear();
                        items[0].cvsItem.Children.Add(tblItemAmount);

                        //Unselect and clear the old slot
                        slot.isSelected = false;
                        slot.ClearSlot();

                        //Make sure that the slot that was currently clicked to place an item doesn't pick up the item instantly again
                        wasJustSelected = true;
                    }
                    else if (slot.isSelected && slot == this)
                    {
                        //If the slot is selected and it's also the currently clicked slot
                        foreach (Item item in items)
                        {
                            //Remove all items from the previous parent
                            wndGame.RemoveFromParent(item.cvsItem);
                        }
                        //Add the canvas back to this border
                        bdrSlot.Child = items[0].cvsItem;

                        //Unselect the slot
                        slot.isSelected = false;

                        //Make sure it doesn't select the slot instantly again
                        wasJustSelected = true;
                    }
                }
            }

            bool canBeSelected = true;
            foreach (Inventory inventory2 in wndGame.inventoryList)
            {
                foreach (InventorySlot slot in inventory2.slotList)
                {
                    //Check if any other slots are currently selected
                    if (slot.isSelected == true)
                    {
                        canBeSelected = false;
                        break;
                    }
                }
            }

            //If the slot wasn't just selected and no other slot is currently selected
            if (wasJustSelected == false && canBeSelected == true)
            {
                if (items.Count > 0)
                {
                    //If there's actually items in the slot, select it
                    isSelected = true;

                    //Remove the item from the slot and add it to the main window to make it follow the mouse while being selected
                    wndGame.RemoveFromParent(items[0].cvsItem);
                    wndGame.cvsGame.Children.Add(items[0].cvsItem);
                    Panel.SetZIndex(items[0].cvsItem, 5);
                    Canvas.SetLeft(items[0].cvsItem, wndGame.mousePosition.X + 5);
                    Canvas.SetTop(items[0].cvsItem, wndGame.mousePosition.Y + 5);
                }

            }

        }
    }
}
