using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System;

using SeeloewenCraft.entity;

namespace SeeloewenCraft
{
    public class Inventory
    {
        //References
        private Random rnd;
        public InventoryGui inventoryGui;
        public List<InventorySlot> slotList = new List<InventorySlot>();
        public List<HotbarSlot> hotbarSlotList = new List<HotbarSlot>();
        public Grid grdInventory = new Grid();
        public Grid grdHotbar = new Grid();

        //Variables
        private static int rndOffset;
        public bool hasHotbar = false;
        private int slotsX;
        private int slotsY;

        //-- Constructor --//

        public Inventory(int slotsX, int slotsY)
        {
            //Set the attributes
            this.slotsX = slotsX;
            this.slotsY = slotsY;
            rnd = new Random(DateTime.Now.Millisecond + rndOffset);

            inventoryGui = new InventoryGui(80 * slotsY + 30, 695, 175, 290, "sc:inventory", this);

            //Create the inventory grid
            grdInventory.Width = 72 * slotsX;
            grdInventory.Height = 72 * slotsY;
            Canvas.SetLeft(grdInventory, 22);
            Canvas.SetTop(grdInventory, 40);
            inventoryGui.cvsGui.Children.Add(grdInventory);

            //Add colums and rows to the grid
            for (int i = 0; i < slotsX; i++)
            {
                grdInventory.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < slotsY; i++)
            {
                grdInventory.RowDefinitions.Add(new RowDefinition());
            }

            //Create inventory slots
            for (int y = slotsY - 1; y >= 0; y--)
            {
                for (int x = 0; x < slotsX; x++)
                {
                    InventorySlot slot = new InventorySlot(this, x, y);

                    //Add the slot to the grid and slotlist
                    grdInventory.Children.Add(slot.bdrSlot);
                    Grid.SetRow(slot.bdrSlot, slot.yPos);
                    Grid.SetColumn(slot.bdrSlot, slot.xPos);
                    slotList.Add(slot);
                }
            }
        }

        //-- Custom Methods --//

        public void InitHotbar()
        {
            hasHotbar = true;

            //Create the hotbar grid
            grdHotbar.Width = 75 * slotsX;
            grdHotbar.Height = 75;
            grdHotbar.Visibility = Visibility.Visible;
            grdHotbar.Background = new SolidColorBrush(Colors.Gray);
            Canvas.SetLeft(grdHotbar, 10);
            Canvas.SetTop(grdHotbar, 10);
            Panel.SetZIndex(grdHotbar, 4);
            Game.world.wndGame.cvsGame.Children.Add(grdHotbar);

            //Create the hotbar rows and colums
            for (int i = 0; i < slotsX; i++)
            {
                grdHotbar.ColumnDefinitions.Add(new ColumnDefinition());
            }
            grdHotbar.RowDefinitions.Add(new RowDefinition());

            //Create the hotbar slots
            for (int x = 0; x < slotsX; x++)
            {
                foreach (InventorySlot slot in slotList)
                {
                    if (slot.yPos == 3 && slot.xPos == x)
                    {
                        HotbarSlot hSlot = new HotbarSlot(x, slot);

                        //Add the hotbar slot to the grid and slotlist
                        grdHotbar.Children.Add(hSlot.bdrSlot);
                        Grid.SetRow(hSlot.bdrSlot, 0);
                        Grid.SetColumn(hSlot.bdrSlot, hSlot.xPos);
                        hotbarSlotList.Add(hSlot);
                    }
                }

            }
        }

        public int GetSelectedHotbarIndex()
        {
            if (hasHotbar)
            {
                int index = 0;

                //Go through the hotbar slots and get the index of the selected slot
                foreach (HotbarSlot slot in hotbarSlotList)
                {
                    if (slot.isSelected)
                    {
                        return index;
                    }
                    index++;
                }
            }

            return 0;
        }

        public HotbarSlot GetSelectedHotbarSlot()
        {
            if (hasHotbar)
            {
                //Get the currently selected hotbar slot, not the inventory slot!
                foreach (HotbarSlot slot in hotbarSlotList)
                {
                    if (slot.isSelected)
                    {
                        return slot;
                    }
                }
            }

            return null;
        }

        public InventorySlot GetSelectedInvSlot()
        {
            //Get the currently selected inventory slot, NOT the hotbar slot
            foreach (InventorySlot slot in slotList)
            {
                if (slot.isSelected)
                {
                    return slot;
                }
            }

            return null;
        }

        public int GetItemAmount(string id)
        {
            //Gets the total amount of an item in your inventory
            int amount = 0;

            foreach (InventorySlot slot in slotList)
            {
                //Sums up all the amounts of the different slots
                if (slot.itemId == id)
                {
                    amount += slot.Amount;
                }
            }

            return amount;
        }

        public int GetAvailableSpace()
        {
            int space = 0;

            //Get the space that's available in total in the inventory by summing up all possible space in the slots
            foreach (InventorySlot slot in slotList)
            {
                space += slot.GetAvailableSpace();
            }

            return space;
        }

        public Item GetSelectedItem()
        {
            //Get the item from the currently selected Hotbar slot
            if (GetSelectedHotbarSlot() != null)
            {
                return ItemRegister.GenerateItem(GetSelectedHotbarSlot().slot.itemId);
            }

            return null;
        }

        public void AddItem(string id, int amount, out int remainingAmount)
        {
            //Add the item by first checking if a slot already has the item, otherwise add it to a new slot. Also update the hotbar.
            AddToExistingSlot(id, ref amount);
            AddToNewSlot(id, ref amount);
            UpdateHotbar();

            //Output the remaining amount of items that couldn't be added
            remainingAmount = amount;
        }

        public void AddItem(string id, int amount)
        {
            //Add the item by first checking if a slot already has the item, otherwise add it to a new slot. Also update the hotbar.
            AddToExistingSlot(id, ref amount);
            AddToNewSlot(id, ref amount);
            UpdateHotbar();
        }

        private void AddToExistingSlot(string id, ref int amount)
        {
            //Go through all inventory slots and check if the slot has the specified id
            foreach (InventorySlot slot in slotList)
            {
                if (slot.itemId == id)
                {
                    //Check if the slot has enough space available
                    if (slot.GetAvailableSpace() >= amount)
                    {
                        slot.Add(id, amount);
                        amount = 0;
                    }
                    else
                    {
                        //If not, only add the amount of possible space to the slot and edit amount to continue afterwards
                        amount -= slot.GetAvailableSpace();
                        slot.Add(id, slot.GetAvailableSpace());
                    }

                }

                //If amount is 0, all items have been added and the process is done
                if (amount == 0)
                {
                    break;
                }
            }
        }

        private void AddToNewSlot(string id, ref int amount)
        {
            //Go through all inventory slots and check if the slot is empty
            foreach (InventorySlot slot in slotList)
            {
                if (string.IsNullOrEmpty(slot.itemId))
                {
                    //Check if the slot has enough space available
                    if (slot.GetAvailableSpace() >= amount)
                    {
                        slot.Add(id, amount);
                        amount = 0;
                    }
                    else
                    {
                        //If not, only add the amount of possible space to the slot and edit amount to continue afterwards
                        amount -= slot.GetAvailableSpace();
                        slot.Add(id, slot.GetAvailableSpace());
                    }

                }

                //If amount is 0, all items have been added and the process is done
                if (amount == 0)
                {
                    UpdateHotbar();
                    break;
                }
            }
        }


        public void RemoveItem(string id, int amount)
        {
            //Go through all slots and check if the slot has the specified id
            foreach (InventorySlot slot in slotList)
            {
                if (slot.itemId == id)
                {
                    //If the amount in the slot is bigger than the amount that should be removed, simply remove it
                    if (slot.Amount >= amount)
                    {
                        slot.Remove(amount);
                        amount = 0;
                    }
                    else
                    {
                        //If not, only remove the possible amount from the slot and subtract amount so the process can continue
                        slot.Remove(slot.Amount);
                        amount -= slot.Amount;
                    }

                }

                //If the amount is 0, all items have been removed and the process is done
                if (amount == 0)
                {
                    break;
                }
            }

            UpdateHotbar();
        }

        public bool HasItem(string id)
        {
            //Go through each slot and check for the id to see if the inventory contains the item at all
            foreach (InventorySlot slot in slotList)
            {
                if (slot.itemId.Contains(id))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasEmptySlot()
        {
            //Go through each slot and check if it's empty to see if the inventory contains at least one empty slot
            foreach (InventorySlot slot in slotList)
            {
                if (slot.IsEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        public void Show()
        {
            //Show the inventory and hide the hotbar
            if (Game.world.wndGame.bdrMenu.Visibility != Visibility.Visible)
            {
                inventoryGui.Show();
                HideHotbar();
            }
        }

        public void Hide()
        {
            //Hide the inventoy, update and show the hotbar
            inventoryGui.Hide();
            UpdateHotbar();
            ShowHotbar();
        }

        public void ShowHotbar()
        {
            if (hasHotbar)
            {
                //Show the hotbar
                grdHotbar.Visibility = Visibility.Visible;
            }
        }

        public void HideHotbar()
        {
            if (hasHotbar)
            {
                //Hide the hotbar
                grdHotbar.Visibility = Visibility.Hidden;
            }
        }
        public void UpdateHotbar()
        {
            //Check if the inventory has a hotbar in the first place
            if (hasHotbar)
            {
                //Go through each hotbar slot
                foreach (HotbarSlot hotbarSlot in hotbarSlotList)
                {
                    if (!string.IsNullOrEmpty(hotbarSlot.slot.itemId))
                    {
                        //If there's an item in the inventory slot bound to the hotbar slot, show it in the hotbar
                        hotbarSlot.cvsSlot.Background = hotbarSlot.slot.cvsItem.Background;
                        hotbarSlot.tblItemAmount.Text = hotbarSlot.slot.Amount.ToString();
                    }
                    else
                    {
                        //If not, just clear the hotbar slot
                        hotbarSlot.cvsSlot.Background = null;
                        hotbarSlot.tblItemAmount.Text = "";
                    }
                }
            }
        }

        public void Drop(int x, int y)
        {
            //Get the selected slot and selected item
            foreach (InventorySlot slot in slotList)
            {
                Item item = null;
                if (!slot.IsEmpty())
                {
                    for (int i = 0; i < slot.Amount; i++)
                    {
                        item = ItemRegister.GenerateItem(slot.itemId);

                        //If the selected item is not null, drop it
                        if (item != null)
                        {
                            Game.world.AddEntity(new ItemEntity(item, //item type
                                x + 500 - ItemEntity.itemSizeX / 2, //posX
                                y + 500 - ItemEntity.itemSizeY / 2, //posY
                                rnd.Next(-6000, 6000), rnd.Next(-15000, -10000))); //velX and velY 

                            slot.inventory.UpdateHotbar();
                        }
                    }
                }
            }
        }

        public void SaveToJson(JsonWriter writer)
        {
            //Write all properties to json
            writer.WriteStartObject();

            writer.WritePropertyName("size_x");
            writer.WriteValue(slotsX);

            writer.WritePropertyName("size_y");
            writer.WriteValue(slotsY);

            writer.WritePropertyName("has_hotbar");
            writer.WriteValue(hasHotbar);

            writer.WritePropertyName("slots");
            writer.WriteStartArray();

            foreach (InventorySlot slot in slotList)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("item");

                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(slot.itemId);

                writer.WritePropertyName("amount");
                writer.WriteValue(slot.Amount);

                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        public static Inventory LoadFromJson(JsonToken token)
        {

            //Get the inventory size
            int slotsX = token.GetInt("/size_x");
            int slotsY = token.GetInt("/size_y");

            Inventory inventory = new Inventory(slotsX, slotsY);

            //Get a possible hotbar
            bool hasHotbar = token.GetBool("/has_hotbar");
            if (hasHotbar)
            {
                inventory.InitHotbar();
            }

            JsonToken slotArrayToken = token.GetToken("/slots");
            int slotNum = 0;

            foreach (InventorySlot slot in inventory.slotList)
            {
                //Go trough each slot and load the item and amount
                JsonToken slotToken = token.GetToken($"/slots/{slotNum}/item");

                string id = slotToken.GetString("/id");
                int amount = slotToken.GetInt("/amount");

                if (!string.IsNullOrEmpty(id))
                {
                    slot.Add(id, amount);
                }

                slotNum++;
            }

            return inventory;
        }
    }
}
