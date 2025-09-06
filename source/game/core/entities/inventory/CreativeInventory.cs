using SeeloewenCraft.game.core.items;
using System;
using System.Linq;

namespace SeeloewenCraft.game.core.entities.inventory
{
    public class CreativeInventory : Inventory
    {
        public override string guiId { get; set; } = "creative_inventory";
        public override string tags { get; set; }

        public CreativeInventory() : base(9, GetSlotsY(ItemRegister.GetItemAmount()), "Creative Inventory")
        {
            //Construct inventory in reverse so the slots get filled from top to bottom
            int slotsY = GetSlotsY(ItemRegister.GetItemAmount());
            slots = new CreativeSlot[9 * slotsY];
            for (int y = 0; y < slotsY; y++) //Start at the end to make sure items are being added to the hotbar first afterwards
            {
                for (int x = 0; x < 9; x++)
                {
                    slots[x + y * 9] = new CreativeSlot(this, x, y);
                }
            }

            //Add all the items from the register to the creative inventory, except 
            foreach (string item in ItemRegister.GetItemIds())
            {
                if(!unlistedItems.Contains(item)) Add(item, 1, ItemRegister.Get(item).tag);
            }

            globalInventories.Remove(this);
        }

        public static int GetSlotsY(int itemAmount) => (int)Math.Ceiling(itemAmount / 9d);

        private static readonly string[] unlistedItems =
        {
            "sc:modded_item",
            "sc:air_item"
        };
    }
}
