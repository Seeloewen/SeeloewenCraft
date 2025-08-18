using SeeloewenCraft.entity;
using SeeloewenCraft.game.graphics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace SeeloewenCraft
{
    public class Inventory : IGuiData
    {
        public string guiId { get; set; } = "inventory";
        public string tags { get; set; }

        public List<InventorySlot> slotList = new List<InventorySlot>();
        public List<HotbarSlot> hotbarSlotList = new List<HotbarSlot>();
        public Block block;

        public bool hasHotbar = false;
        private int slotsX;
        private int slotsY;
        public bool isPlayer;
        public bool isOpen;

        public Inventory(int slotsX, int slotsY, bool isPlayer, string name = "Inventory")
        {
            this.slotsX = slotsX;
            this.slotsY = slotsY;
            this.isPlayer = isPlayer;

            ((IGuiData)this).AddTag("header", name);
            ((IGuiData)this).AddTag("slotsX", slotsX);
            ((IGuiData)this).AddTag("slotsY", slotsY);

            //Create inventory slots
            for (int y = slotsY - 1; y >= 0; y--) //Start at the end to make sure items are being added to the hotbar first afterwards
            {
                for (int x = 0; x < slotsX; x++)
                {
                    slotList.Add(new InventorySlot(this, x, y));
                }
            }

            Game.world.inventoryList.Add(this);
        }

        public void ShowGui()
        {
            ((IGuiData)this).Show();
            isOpen = true;
        }

        public void HideGui()
        {
            ((IGuiData)this).Show();
            isOpen = false;
        }

        //-- Custom Methods --//

        public void InitHotbar()
        {
            hasHotbar = true;

            //Create the hotbar slots
            for (int x = 0; x < slotsX; x++)
            {
                foreach (InventorySlot slot in slotList)
                {
                    if (slot.yPos == 3 && slot.xPos == x)
                    {
                        hotbarSlotList.Add(new HotbarSlot(x, slot));
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
                    amount += slot.amount;
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

        public InventorySlot GetSlot(int x, int y)
        {
            //Go through all slots and check if the x and y pos matches
            foreach (InventorySlot slot in slotList)
            {
                if (slot.xPos == x && slot.yPos == y)
                {
                    return slot;
                }
            }

            return null;
        }

        public HotbarSlot GetHotbarSlot(int x)
        {
            //Go through all slots and check if the x and y pos matches
            foreach (HotbarSlot slot in hotbarSlotList)
            {
                if (slot.xPos == x)
                {
                    return slot;
                }
            }

            return null;
        }

        public void AddItem(string id, int amount, string tag, out int remainingAmount)
        {
            //Add the item by first checking if a slot already has the item, otherwise add it to a new slot. Also update the hotbar.
            if (!Game.unstackableItems.Contains(id))
            {
                //Only check existing slots if it's stackable
                AddToExistingSlot(id, ref amount, tag);
            }
            AddToNewSlot(id, ref amount, tag);

            //Output the remaining amount of items that couldn't be added
            remainingAmount = amount;
        }

        public void AddItem(string id, int amount, string tag)
        {
            //Add the item by first checking if a slot already has the item, otherwise add it to a new slot. Also update the hotbar.
            if (!Game.unstackableItems.Contains(id))
            {
                //Only check existing slots if it's stackable
                AddToExistingSlot(id, ref amount, tag);
            }
            AddToNewSlot(id, ref amount, tag);
        }

        private void AddToExistingSlot(string id, ref int amount, string tag)
        {
            //Go through all inventory slots and check if the slot has the specified id
            foreach (InventorySlot slot in slotList)
            {
                //If amount is 0, all items have been added and the process is done
                if (amount == 0)
                {
                    break;
                }

                if (slot.itemId == id)
                {
                    //Check if the slot has enough space available
                    if (slot.GetAvailableSpace() >= amount)
                    {
                        slot.Add(id, amount, tag);
                        amount = 0;
                    }
                    else
                    {
                        //If not, only add the amount of possible space to the slot and edit amount to continue afterwards
                        amount -= slot.GetAvailableSpace();
                        slot.Add(id, slot.GetAvailableSpace(), tag);
                    }

                }
            }
        }

        private void AddToNewSlot(string id, ref int amount, string tag)
        {
            //Go through all inventory slots and check if the slot is empty
            foreach (InventorySlot slot in slotList)
            {
                //If amount is 0, all items have been added and the process is done
                if (amount == 0) return;

                if (slot.IsEmpty())
                {
                    //If it's unstackable, only add one
                    if (Game.unstackableItems.Contains(id))
                    {
                        slot.Add(id, 1, tag);
                        amount--;
                    }
                    else //If it's stackable, add as many as possible
                    {
                        //Check if the slot has enough space available
                        if (slot.GetAvailableSpace() >= amount)
                        {
                            slot.Add(id, amount, tag);
                            amount = 0;
                        }
                        else
                        {
                            //If not, only add the amount of possible space to the slot and edit amount to continue afterwards
                            amount -= slot.GetAvailableSpace();
                            slot.Add(id, slot.GetAvailableSpace(), tag);
                        }
                    }
                }
            }
        }


        public void RemoveItem(string id, int amount)
        {
            //Go through all slots and check if the slot has the specified id
            foreach (InventorySlot slot in slotList)
            {
                //If the amount is 0, all items have been removed and the process is done
                if (amount == 0)
                {
                    break;
                }

                if (slot.itemId == id)
                {
                    //If the amount in the slot is bigger than the amount that should be removed, simply remove it
                    if (slot.amount >= amount)
                    {
                        slot.Remove(amount);
                        amount = 0;
                    }
                    else
                    {
                        //If not, only remove the possible amount from the slot and subtract amount so the process can continue
                        slot.Remove(slot.amount);
                        amount -= slot.amount;
                    }

                }
            }
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

        public void Drop(int x, int y)
        {
            //Get the selected slot and selected item
            foreach (InventorySlot slot in slotList)
            {
                Item item = null;
                if (!slot.IsEmpty())
                {
                    for (int i = 0; i < slot.amount; i++)
                    {
                        item = ItemRegister.GenerateItem(slot.itemId);

                        //If the selected item is not null, drop it
                        if (item != null)
                        {
                            Game.world.AddEntity(new ItemEntity(item, slot.itemTag, //item type
                                x + 500 - ItemEntity.itemSizeX / 2, //posX
                                y + 500 - ItemEntity.itemSizeY / 2, //posY
                                Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY 
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
                writer.WriteValue(slot.amount);

                writer.WritePropertyName("tag");
                writer.WriteValue(slot.itemTag);

                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        public static Inventory LoadFromJson(JsonToken token, bool isPlayer)
        {
            //Get the inventory size
            int slotsX = token.GetInt("/size_x");
            int slotsY = token.GetInt("/size_y");

            Inventory inventory = new Inventory(slotsX, slotsY, isPlayer);

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
                string tag = slotToken.GetString("/tag");

                if (!string.IsNullOrEmpty(id))
                {
                    slot.Add(id, amount, tag);
                }

                slotNum++;
            }

            return inventory;
        }
    }
}
