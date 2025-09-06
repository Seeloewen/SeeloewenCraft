using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.graphics;
using System.Windows;

namespace SeeloewenCraft.game.core.crafting
{
    public class UnchiselHandler : IGuiData
    {
        public string guiId { get; set; } = "unchisel_handler";
        public string tags { get; set; }

        public Inventory inv;

        public UnchiselHandler()
        {
            inv = new Inventory(1, 1);
        }

        public void Unchisel()
        {
            //Check if there's an item in the slot and the item is a chiselitem
            if (!inv.slots[0].IsEmpty())
            {
                Item item = ItemRegister.Get(inv.slots[0].id);
                bool unchiselSuccess = false;
                int successItems = 0;

                if (item is ChiseledItem chisItem && chisItem.isChiseled)
                {
                    for (int i = 0; i < inv.slots[0].amount; i++)
                    {
                        //Add the output to the inventory
                        foreach (Item outItem in chisItem.Unchisel())
                        {
                            int remaining = Game.world.player.inventory.Add(outItem.id, 1, outItem.tag);

                            if (remaining > 0)
                            {
                                inv.slots[0].Remove(successItems);
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
                if (unchiselSuccess) inv.slots[0].Remove(inv.slots[0].amount);

            }
            else
            {
                MessageBox.Show("Please select an item to unchisel!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}