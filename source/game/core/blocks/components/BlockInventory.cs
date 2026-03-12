using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.core.blocks.components
{
    public class BlockInventory : BlockComponent
    {
        public Inventory inventory;

        public BlockInventory(int slotsX, int slotsY, string invName = "Block Inventory") : base()
        {
            inventory = new Inventory(slotsX, slotsY, invName);
        }

        public BlockInventory(Inventory inv) : base()
        {
            inventory = inv;
        }


        protected override JToken GetContentJson() => inventory.ToJson();

        public override void FromJson(JObject obj)
        {
            inventory = Inventory.FromJson(obj, false);
        }

        public override BlockComponentType GetType() => BlockComponentType.Inventory;

        public void InsertLootTable(LootTable lootTable, int amount) //TODO: Loottable rework
        {
            //Get all loot into a list
            List<Item> loot = new List<Item>();
            for (int i = 0; i < amount; i++)
            {
                loot.AddRange(lootTable.RollEntry().RollItems());
            }

            //Put the loot into the inventory
            foreach (Item item in loot)
            {
                inventory.Add(item.id, 1, item.tag);
            }
        }

        public void InsertLootTable(LootTable lootTable, int amount, Random rnd) //TODO: Loottable rework
        {
            //Get all loot into a list
            List<Item> loot = new List<Item>();
            for (int i = 0; i < amount; i++)
            {
                loot.AddRange(lootTable.RollEntry(rnd).RollItems(rnd));
            }

            //Put the loot into the inventory
            foreach (Item item in loot)
            {
                inventory.Add(item.id, 1, item.tag);
            }
        }
    }
}
