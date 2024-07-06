using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Windows.Markup;
using System.IO.Packaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Json.Pointer;

namespace SeeloewenCraft
{
    public class Inventory
    {
        World world;
        public InventoryGui inventoryGui;
        public List<InventorySlot> slotList = new List<InventorySlot>();
        public List<HotbarSlot> hotbarSlotList = new List<HotbarSlot>();
        public Grid grdInventory = new Grid();
        public Grid grdHotbar = new Grid();
        public string inventoryDirectory;
        public bool hasHotbar = false;

        //-- Constructuror --//

        public Inventory(World world, bool hasHotbar)
        {
            //Set the attributes
            this.world = world;
            this.hasHotbar = hasHotbar;
            inventoryGui = new InventoryGui(world, 412, 859, 150, 175, "sc:inventory", this);

            //Create the inventory grid
            grdInventory.Width = 800;
            grdInventory.Height = 356;
            Canvas.SetLeft(grdInventory, 28);
            Canvas.SetTop(grdInventory, 36);
            inventoryGui.cvsGui.Children.Add(grdInventory);

            //Add colums and rows to the grid
            for (int i = 0; i < 9; i++)
            {
                grdInventory.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 4; i++)
            {
                grdInventory.RowDefinitions.Add(new RowDefinition());
            }

            //Create inventory slots
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    slotList.Add(new InventorySlot(world, x, y));
                }
            }

            //Add all inventory slots to the inventory
            foreach (InventorySlot slot in slotList)
            {
                grdInventory.Children.Add(slot.bdrSlot);
                Grid.SetRow(slot.bdrSlot, slot.yPos);
                Grid.SetColumn(slot.bdrSlot, slot.xPos);
            }

            if (hasHotbar == true)
            {
                //Create the hotbar grid
                grdHotbar.Width = 675;
                grdHotbar.Height = 75;
                grdHotbar.Visibility = Visibility.Visible;
                grdHotbar.Background = new SolidColorBrush(Colors.Gray);
                Canvas.SetLeft(grdHotbar, 10);
                Canvas.SetTop(grdHotbar, 10);
                Panel.SetZIndex(grdHotbar, 4);
                world.wndGame.cvsGame.Children.Add(grdHotbar);

                //Create the hotbar rows and colums
                for (int i = 0; i < 9; i++)
                {
                    grdHotbar.ColumnDefinitions.Add(new ColumnDefinition());
                }
                grdHotbar.RowDefinitions.Add(new RowDefinition());

                //Create the hotbar slots
                for (int x = 0; x < 9; x++)
                {
                    foreach (InventorySlot slot in slotList)
                    {
                        if (slot.yPos == 3 && slot.xPos == x)
                        {
                            hotbarSlotList.Add(new HotbarSlot(world, x, slot));
                        }
                    }

                }

                //Add all hotbar slots to the hotbar grid
                foreach (HotbarSlot slot in hotbarSlotList)
                {
                    grdHotbar.Children.Add(slot.bdrSlot);
                    Grid.SetRow(slot.bdrSlot, 0);
                    Grid.SetColumn(slot.bdrSlot, slot.xPos);
                }
            }
        }

        //-- Custom Methods --//

        public int GetSelectedIndex()
        {

            int c = 0;
            foreach (HotbarSlot slot in hotbarSlotList)
            {
                if (slot.isSelected)
                {
                    return c;
                }
                c++;
            }
            return 0;
        }

        public void AddItem(Item item)
        {
            bool isAdded = false;

            //Go through each inventory slot
            foreach (InventorySlot slot in world.player.inventory.slotList)
            {
                if (slot.items.Count > 0)
                {
                    //Check if the item is already in the slot and the max amount is not reached
                    if (slot.items[0].name == item.name && slot.itemAmount < 64)
                    {
                        //Add the item to the slot and update it's attributes
                        slot.AddToSlot(item);
                        item.slot = slot;
                        item.xPos = slot.xPos;
                        item.yPos = slot.yPos;
                        isAdded = true;

                        //Update the hotbar
                        UpdateHotbar();

                        break;
                    }
                }
            }

            if (isAdded == false)
            {
                //If the item couldn't be added to existing stacks
                foreach (InventorySlot slot in world.player.inventory.slotList)
                {
                    //Check for empty slots and add it to that
                    if (slot.items.Count == 0)
                    {
                        //Add the item to the slot and set the attributes
                        slot.SetItem(item);
                        item.slot = slot;
                        item.xPos = slot.xPos;
                        item.yPos = slot.yPos;
                        isAdded = true;

                        //Update the hotbar
                        UpdateHotbar();

                        break;
                    }
                }
            }
        }

        public void RemoveItem(Item rItem)
        {
            List<InventorySlot> clearList = new List<InventorySlot>();
            List<Item> removeList = new List<Item>();

            //Go through each inventory slot to find the item
            foreach (InventorySlot slot in world.player.inventory.slotList)
            {
                if (slot.items.Count > 0)
                {
                    foreach (Item item in slot.items)
                    {
                        //If the item is found
                        if (item == rItem)
                        {
                            //Check if it's only 1 item or a stack of multiple
                            if (slot.itemAmount > 1)
                            {
                                //If it's a stack, add the item to a remove list
                                removeList.Add(item);
                                slot.itemAmount--;
                                slot.tblItemAmount.Text = slot.itemAmount.ToString();
                                break;
                            }
                            else
                            {
                                //If not, add the slot to the clear list
                                clearList.Add(slot);
                            }
                        }
                    }
                }
            }

            foreach (InventorySlot slot in clearList)
            {
                //Clear all slots in the clear list
                slot.ClearSlot();
            }
            foreach (InventorySlot slot in world.player.inventory.slotList)
            {
                //Remove every slot in the remove list
                foreach (Item item in removeList)
                {
                    slot.items.Remove(item);
                }
            }

            //Update the hotbar because of possible changes
            UpdateHotbar();
        }

        public void ShowInventory()
        {
            //Show the inventory and hide the hotbar
            if (world.wndGame.bdrMenu.Visibility != Visibility.Visible)
            {
                Canvas.SetTop(grdInventory, 36);
                inventoryGui.Show();
                HideHotbar();
            }
        }

        public void HideInventory()
        {
            //Hide the inventoy, update and show the hotbar
            UpdateHotbar();
            ShowHotbar();
            inventoryGui.Hide();
        }

        public void ShowHotbar()
        {
            if (hasHotbar == true)
            {
                //Show the hotbar
                grdHotbar.Visibility = Visibility.Visible;
            }
        }

        public void HideHotbar()
        {
            if (hasHotbar == true)
            {
                //Hide the hotbar
                grdHotbar.Visibility = Visibility.Hidden;
            }
        }
        public void UpdateHotbar()
        {
            //Check if the inventory has a hotbar in the first place
            if (hasHotbar == true)
            {
                //Go through each hotbar slot
                foreach (HotbarSlot hotbarSlot in hotbarSlotList)
                {
                    if (hotbarSlot.slot.items.Count > 0)
                    {
                        //If there's an item in the inventory slot mapped to the hotbar slot, show it in the hotbar
                        hotbarSlot.cvsSlot.Background = hotbarSlot.slot.items[0].cvsItem.Background;
                        hotbarSlot.tblItemAmount.Text = hotbarSlot.slot.itemAmount.ToString();
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

        public void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();

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
                if (slot.items.Count > 0)
                {
                    writer.WriteValue(slot.items[0].id);
                }
                else
                {
                    writer.WriteValue("Null");
                }


                writer.WritePropertyName("amount");
                writer.WriteValue(slot.itemAmount);

                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        public Item GetSelectedItem()
        {
            foreach (HotbarSlot slot in world.player.inventory.hotbarSlotList)
            {
                //Check if the slot is selected and has an item
                if (slot.isSelected == true && slot.slot.items.Count > 0)
                {
                    return slot.slot.items[slot.slot.items.Count - 1];
                }
            }
            return null;
        }

        public static Inventory LoadFromJson(JToken token, World world)
        {
            bool hasHotbar = (bool)new JsonPointer("/has_hotbar").Evaluate(token);
            Inventory inventory = new Inventory(world, hasHotbar);
            JToken slotArrayToken = new JsonPointer("/slots").Evaluate(token);

            int slotNum = 0;

            foreach (InventorySlot slot in inventory.slotList)
            {
                JToken slotToken = new JsonPointer($"/slots/{slotNum}/item").Evaluate(token);

                string id = (string)new JsonPointer("/id").Evaluate(slotToken);
                int amount = (int)new JsonPointer("/amount").Evaluate(slotToken);

                for (int i = 0; i < amount; i++)
                {
                    switch (id)
                    {
                        case "sc:grass_block_item":
                            slot.AddToSlot(new GrassItem(world, null));
                            break;
                        case "sc:dirt_item":
                            slot.AddToSlot(new DirtItem(world, null));
                            break;
                        case "sc:stone_item":
                            slot.AddToSlot(new StoneItem(world, null));
                            break;
                        case "sc:oak_log_item":
                            slot.AddToSlot(new OakLogItem(world, null));
                            break;
                        case "sc:oak_leaves_item":
                            slot.AddToSlot(new OakLeavesItem(world, null));
                            break;
                        case "sc:spruce_log_item":
                            slot.AddToSlot(new SpruceLogItem(world, null));
                            break;
                        case "sc:spruce_leaves_item":
                            slot.AddToSlot(new SpruceLeavesItem(world, null));
                            break;
                        case "sc:coal_ore_item":
                            slot.AddToSlot(new CoalOreItem(world, null));
                            break;
                        case "sc:iron_ore_item":
                            slot.AddToSlot(new IronOreItem(world, null));
                            break;
                        case "sc:chest_item":
                            slot.AddToSlot(new ChestItem(world, null));
                            break;
                        case "sc:bedrock_item":
                            slot.AddToSlot(new BedrockItem(world, null));
                            break;
                        case "sc:magma_block_item":
                            slot.AddToSlot(new MagmaBlockItem(world, null));
                            break;
                        case "sc:torch_item":
                            slot.AddToSlot(new TorchItem(world, null));
                            break;
                        case "sc:plant_2_item":
                            slot.AddToSlot(new Plant2Item(world, null));
                            break;
                        case "sc:water_item":
                            slot.AddToSlot(new WaterItem(world, null));
                            break;
                        case "sc:hammer_item":
                            slot.AddToSlot(new HammerItem(world, null));
                            break;
                        case "sc:air_item":
                            slot.AddToSlot(new AirItem(world, null));
                            break;
                        case "sc:diamond_ore_item":
                            slot.AddToSlot(new DiamondOreItem(world, null));
                            break;
                    }
                }
                slotNum++;
            }
            return inventory;
        }

    }
}