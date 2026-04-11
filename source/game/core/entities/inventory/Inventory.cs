using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.core.world.generation;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeeloewenCraft.game.core.entities.inventory
{
    public class Inventory : IGuiData
    {
        public virtual string guiId { get; set; } = "inventory";
        public virtual string tags { get; set; }

        internal static HashSet<Inventory> globalInventories = new HashSet<Inventory>();
        internal bool isOpen;

        public InventorySlot[] slots;

        private bool hasHotbar = false;
        internal bool isPlayerInv;
        internal int slotsX;
        internal int slotsY;
        

        public Inventory(int slotsX, int slotsY, string name = "Inventory", bool isPlayerInv = false)
        {
            this.slotsX = slotsX;
            this.slotsY = slotsY;
            this.isPlayerInv = isPlayerInv;

            ((IGuiData)this).AddTag("header", name);
            ((IGuiData)this).AddTag("slotsX", slotsX);
            ((IGuiData)this).AddTag("slotsY", slotsY);

            //Create inventory slots
            slots = new InventorySlot[slotsX * slotsY];
            int i = 0;
            for (int y = slotsY - 1; y >= 0; y--) //Start at the end to make sure items are being added to the hotbar first afterwards
            {
                for (int x = 0; x < slotsX; x++)
                {
                    slots[i] = new InventorySlot(this, x, y);
                    i++;
                }
            }

            globalInventories.Add(this); //This is used so that shifting items from one inv into another works
        }

        public void ShowGui()
        {
            ((IGuiData)this).Show();
            isOpen = true;
        }

        public void HideGui()
        {
            ((IGuiData)this).Hide();
            isOpen = false;
        }

        internal static InventorySlot GetGlobalSelectedInvSlot()
        {
            InventorySlot selectedSlot;

            //Go through all inventories and get the selected inventory slot
            foreach (Inventory inventory in globalInventories)
            {
                selectedSlot = inventory.GetSelectedInvSlot();
                if (selectedSlot != null) return selectedSlot;
            }

            return null;
        }

        public void InitHotbar()
        {
            hasHotbar = true;

            //Gets the bottom most row and sets those slots as hotbar
            for (int x = 0; x < slotsX; x++)
            {
                GetSlot(x, slotsY - 1).isHotbar = true;
                if (x == 0) GetSlot(x, slotsY - 1).SelectInHotbar();
            }
        }


        public int GetSelectedHotbarIndex()
        {
            if (!hasHotbar) return 0;

            for (int x = 0; x < slotsX; x++) //Go through the hotbar slots and get the index of the selected slot
            {
                if (GetSlot(x, slotsY - 1).isHotbarSelected) return x;
            }

            return 0;
        }

        public InventorySlot GetSelectedHotbarSlot()
        {
            if (!hasHotbar) return null;

            return GetSlot(GetSelectedHotbarIndex(), slotsY - 1);
        }

        public InventorySlot GetSelectedInvSlot()
        {
            //Get the currently selected inventory slot, NOT the hotbar slot
            foreach (InventorySlot slot in slots)
            {
                if (slot.isSelected) return slot;
            }

            return null;
        }

        public int GetItemAmount(string id)
        {
            int amount = 0;

            foreach (InventorySlot slot in slots)
            {
                //Sums up all the amounts of the different slots
                if (slot.id == id) amount += slot.amount;
            }

            return amount;
        }

        public int GetAvailableSpace(string id)
        {
            int space = 0;

            //Get the space that's available in total in the inventory by summing up all possible space in the slots
            foreach (InventorySlot slot in slots)
            {
                if (!slot.IsEmpty() && slot.id != id) continue; //If the slot is not empty, don't count it if the id mismatches

                space += slot.GetAvailableSpace();
            }

            return space;
        }

        public Item GetSelectedItem()
        {
            //Get the item from the currently selected Hotbar slot
            if (GetSelectedHotbarSlot() != null)
            {
                return ItemRegister.Get(GetSelectedHotbarSlot().id);
            }

            return null;
        }

        public InventorySlot GetSlot(int x, int y)
        {
            //Go through all slots and check if the x and y pos matches
            int i = x + y * slotsX;

            if (i > slots.Length - 1) return null;

            return slots[i];
        }

        public InventorySlot GetHotbarSlot(int x) => GetSlot(x, slotsY - 1);

        public List<InventorySlot> GetHotbarSlots()
        {
            //Only return a list of the hotbar slots
            List<InventorySlot> slots = new List<InventorySlot>();
            for (int x = 0; x < slotsX; x++)
            {
                slots.Add(GetSlot(x, slotsY - 1));
            }

            return slots;
        }

        public int Add(string id, int amount, string tag = "") //Returns the amount of items that weren't added
        {
            //First, go through all hotbar slots and check if it can be added to already filled slots
            if (hasHotbar)
            {
                amount = AddSpecific(id, amount, tag, GetHotbarSlots(), s => s.id != id || s.tag != tag);
                if (amount <= 0) return 0;
            }

            //Then, check if it can be added to already filled slots elsewhere in the inventory
            amount = AddSpecific(id, amount, tag, slots.ToList(), s => s.id != id || s.tag != tag);
            if (amount <= 0) return 0;

            //Next, fill the empty hotbar slots
            if (hasHotbar)
            {
                amount = AddSpecific(id, amount, tag, GetHotbarSlots(), s => !s.IsEmpty());
                if (amount <= 0) return 0;
            }

            //Finally, fill all remaining hotbar slots
            amount = AddSpecific(id, amount, tag, slots.ToList(), s => !s.IsEmpty());
            if (amount <= 0) return 0;

            return amount;
        }

        private int AddSpecific(string id, int amount, string tag, List<InventorySlot> subList, Func<InventorySlot, bool> skipCondition)
        {
            //Go through a specific range to slots and add the items, if the skip condition doesn't apply
            foreach (InventorySlot slot in subList)
            {
                if (amount <= 0) return 0;

                if (skipCondition(slot)) continue;
                amount = slot.Add(id, amount, tag);
            }

            return amount;
        }

        public int Remove(string id, int amount) //Returns the amount of items that weren't removed
        {
            //Go through all slots and check if the slot has the specified id
            foreach (InventorySlot slot in slots)
            {
                //If the amount is 0, all items have been removed and the process is done
                if (amount == 0) return 0;

                if (slot.id != id) continue;
                amount = slot.Remove(amount);
            }

            return amount;
        }

        public bool Has(string id)
        {
            //Go through each slot and check for the id to see if the inventory contains the item at all
            foreach (InventorySlot slot in slots)
            {
                if (slot.id == id) return true;
            }
            return false;
        }

        public bool HasEmptySlot() => GetAvailableSpace(null) > 0;

        public void DropAll(int x, int y)
        {
            //Get the selected slot and selected item
            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty()) continue;

                for (int i = 0; i < slot.amount; i++)
                {
                    Item item = ItemRegister.Get(slot.id);
                    if (item != null)
                    {
                        World.Get().AddEntity(new ItemEntity(item, slot.tag, //item type
                            x + 500 - ItemEntity.itemSizeX / 2, //posX
                            y + 500 - ItemEntity.itemSizeY / 2, //posY
                            Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY 
                    }
                }

            }
        }

        public void RollLootTable(LootTable lootTable, int rolls)
        {
            //Might not even work, needs rework anyways
            for(int i = 0; i < rolls; i++)
            {
                List<Item> items = lootTable.RollEntry().RollItems();
                foreach(Item item in items)
                {
                    Add(item.id, 1, item.tag);
                }
            }
        }

        public JObject ToJson()
        {
            JArray itemsArr = new JArray();
            foreach (InventorySlot slot in slots)
            {
                JObject slotObj = new JObject()
                {
                    { "id", slot.id },
                    { "amount", slot.amount  },
                    { "tag", slot.tag }
                };
            }

            JObject obj = new JObject()
            {
                { "size_x", slotsX},
                { "size_y", slotsY},
                { "has_hotbar", hasHotbar},
                { "slots", itemsArr }
            };

            return obj;
        }

        public static Inventory FromJson(JObject json, bool isPlayer)
        {
            //Get the inventory size
            int slotsX = json.Get<int>("size_x");
            int slotsY = json.Get<int>("size_y");

            Inventory inventory = new Inventory(slotsX, slotsY, isPlayerInv: isPlayer);

            //Get a possible hotbar
            bool hasHotbar = json.Get<bool>("has_hotbar");
            if (hasHotbar) inventory.InitHotbar();

            //Go through all the items in the array and add them to the inventory      
            JArray slotArrayToken = json.Get<JArray>("slots");
            for (int i = 0; i < slotsX * slotsY; i++)
            {
                JObject itemObj = (JObject)slotArrayToken[i];
                InventorySlot slot = inventory.slots[0];

                string id = itemObj.Get<string>("id");
                int amount = itemObj.Get<int>("amount");
                string tag = itemObj.Get<string>("tag");

                if (!string.IsNullOrEmpty(id))
                {
                    slot.Add(id, amount, tag);
                }
            }

            globalInventories.Add(inventory);

            return inventory;
        }


    }
}
