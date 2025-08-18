using SeeloewenCraft.game.graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeloewenCraft.game.core
{
    public class UnchiselHandler : IGuiData
    {
        public string guiId { get; set; } = "unchisel_handler";
        public string tags { get; set; }

        public Inventory inv;

        public UnchiselHandler()
        {
            inv = new Inventory(1, 1, false);
        }

        public void Unchisel()
        {
            //Check if there's an item in the slot and the item is a chiselitem
            if (!inv.slotList[0].IsEmpty())
            {
                Item item = ItemRegister.GenerateItem(inv.slotList[0].itemId);
                bool unchiselSuccess = false;
                int successItems = 0;

                if (item is ChiseledItem chisItem && chisItem.isChiseled)
                {
                    for (int i = 0; i < inv.slotList[0].amount; i++)
                    {
                        //Add the output to the inventory
                        foreach (Item outItem in chisItem.Unchisel())
                        {
                            Game.world.player.inventory.AddItem(outItem.id, 1, outItem.tag, out int remainingItems);

                            if (remainingItems > 0)
                            {
                                inv.slotList[0].Remove(successItems);
                                MessageBox.Show("This item cannot be unchiseled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        successItems++;
                    }

                    unchiselSuccess = true;
                }
                else
                {
                    MessageBox.Show("This item cannot be unchiseled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //If an item was unchiseled, clear the slot
                if (unchiselSuccess) inv.slotList[0].Remove(inv.slotList[0].amount);

            }
            else
            {
                MessageBox.Show("Please select an item to unchisel!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}