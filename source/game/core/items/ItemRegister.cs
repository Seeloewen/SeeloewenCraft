using System;
using System.Collections.Generic;
using System.Linq;

namespace SeeloewenCraft.game.core.items
{
    public static class ItemRegister
    {
        private static Dictionary<string, Type> itemMappings = new Dictionary<string, Type>();

        public static int GetItemAmount() => itemMappings.Count;

        public static string[] GetItemIds()
        {
            string[] items = new string[GetItemAmount()];

            //Create a list of all the item ids that are registered
            int i = 0;
            foreach (var item in itemMappings)
            {
                items[i] = item.Key;
                i++;
            }

            return items;
        }

        public static void Init()
        {
            //Get all block types (spoiler: magic)
            var blockTypes = typeof(Item).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(Item).IsAssignableFrom(t));

            //Register all block types
            foreach (Type type in blockTypes)
            {
                Item item = (Item)Activator.CreateInstance(type, nonPublic: true);
                itemMappings[item.id] = type;
            }
        }

        public static Item Get(string id)
        {
            if(string.IsNullOrEmpty(id)) return null;

            //Retrieve the type from the mappings and create a new instance
            if (!itemMappings.ContainsKey(id)) throw new Exception($"ItemRegister has received an invalid block id: {id}"); //debug code, replace with exception safe behaviour

            Type type = itemMappings[id];

            Item block = (Item)Activator.CreateInstance(type, nonPublic: true);
            return block;
        }
    }
}
