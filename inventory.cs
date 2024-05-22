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

namespace SeeloewenCraft
{
    public class Inventory
    {
        public List<InventorySlot> slotList = new List<InventorySlot>();
        public List<HotbarSlot> hotbarSlotList = new List<HotbarSlot>();
        public Grid grdInventory = new Grid();
        public Grid grdHotbar = new Grid();
        wndGame wndGame;
        public string inventoryDirectory;
        public bool isShown = false;
        public bool hasHotbar = false;
        public int id = 0;

        public Inventory(wndGame wndGame, int id, bool hasHotbar)
        {
            //Set the attributes
            this.id = id;
            this.wndGame = wndGame;
            this.hasHotbar = hasHotbar;

            //Create the inventory grid
            grdInventory.Width = 900;
            grdInventory.Height = 400;
            grdInventory.Visibility = Visibility.Hidden;
            grdInventory.Background = new SolidColorBrush(Colors.Gray);
            Canvas.SetLeft(grdInventory, 150);
            Canvas.SetTop(grdInventory, 100);
            Panel.SetZIndex(grdInventory, 4);
            wndGame.cvsGame.Children.Add(grdInventory);

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
                    slotList.Add(new InventorySlot(wndGame, x, y));
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
                wndGame.cvsGame.Children.Add(grdHotbar);

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
                            hotbarSlotList.Add(new HotbarSlot(wndGame, x, slot));
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

        public void AddItem(Item item)
        {
            bool isAdded = false;

            //Go through each inventory slot
            foreach (InventorySlot slot in wndGame.player.inventory.slotList)
            {
                if (slot.items.Count > 0)
                {
                    //Check if the item is already in the slot and the max amount is not reached
                    if (slot.items[0].itemName == item.itemName && slot.itemAmount < 64)
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
                foreach (InventorySlot slot in wndGame.player.inventory.slotList)
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
            foreach (InventorySlot slot in wndGame.player.inventory.slotList)
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
            foreach (InventorySlot slot in wndGame.player.inventory.slotList)
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
            grdInventory.Visibility = Visibility.Visible;
            HideHotbar();
            isShown = true;
        }

        public void HideInventory()
        {
            //Hide the inventoy, update and show the hotbar
            UpdateHotbar();
            ShowHotbar();
            grdInventory.Visibility = Visibility.Hidden;
            isShown = false;
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

        public void SaveInventory(string path)
        {
            //Check if the inventory directory exists
            if (!Directory.Exists(string.Format("{0}/inventory{1}/", path, id)))
            {
                Directory.CreateDirectory(string.Format("{0}/inventory{1}/", path, id));
            }
            inventoryDirectory = string.Format("{0}/inventory{1}/", path, id);

            //Go through each inventory slot
            foreach (InventorySlot slot in slotList)
            {
                //Clear the file
                File.WriteAllText(string.Format("{0}/{1}-{2}.txt", inventoryDirectory, slot.xPos, slot.yPos), "");

                //Write all items to the slot file
                foreach (Item item in slot.items)
                {
                    File.AppendAllText(string.Format("{0}/{1}-{2}.txt", inventoryDirectory, slot.xPos, slot.yPos), string.Format("{0};{1}\n", item.GetType().ToString().Replace("SeeloewenCraft.", ""), item.id));
                }
            }
        }

        public void LoadInventory(string path, int id)
        {
            //Set the ID to map it to the correct block
            this.id = id;
            
            //Get all inventory slot files from the inventory directory
            string[] files = Directory.GetFiles(string.Format("{0}/Inventory{1}", path, id));

            //Go through each inventory slot
            foreach (InventorySlot slot in slotList)
            {
                //Go through each slot file
                foreach(string file in files)
                {
                    //Format the filename and check if the slot and filename match
                    string f = file.Replace(string.Format("{0}/Inventory{1}", path, id), "").Replace(".txt", "").Replace("\\", "");
                    string[] fileSplit = f.Split('-');
                    if (slot.xPos == Convert.ToInt32(fileSplit[0]) && slot.yPos == Convert.ToInt32(fileSplit[1]))
                    {
                        //Go through each line (=item) in the file and add it to the inventory
                        foreach(string item in File.ReadLines(file))
                        {
                            string[] itemSplit = item.Split(';');
                            if (itemSplit[0] == "GrassItem")
                            {
                                slot.AddToSlot(new GrassItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "DirtItem")
                            {
                                slot.AddToSlot(new DirtItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "StoneItem")
                            {
                                slot.AddToSlot(new StoneItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "OakLogItem")
                            {
                                slot.AddToSlot(new OakLogItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "OakLeavesItem")
                            {
                                slot.AddToSlot(new OakLeavesItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "CoalOreItem")
                            {
                                slot.AddToSlot(new CoalOreItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "IronOreItem")
                            {
                                slot.AddToSlot(new IronOreItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "DiamondOreItem")
                            {
                                slot.AddToSlot(new DiamondOreItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "ChestItem")
                            {
                                slot.AddToSlot(new ChestItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                            else if (itemSplit[0] == "BedrockItem")
                            {
                                slot.AddToSlot(new BedrockItem(wndGame, Convert.ToInt32(fileSplit[1]), null));
                            }
                        }
                    }
                }
            }
        }
    }
}
