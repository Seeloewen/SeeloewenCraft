using System;
using System.Collections.Generic;
using System.Linq;

namespace SeeloewenCraft.game.core.blocks
{
    public static class BlockRegister
    {
        private static Dictionary<string, Type> blockMappings = new Dictionary<string, Type>();

        public static void Init()
        {
            //Get all block types (spoiler: magic)
            var blockTypes = typeof(Block).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(Block).IsAssignableFrom(t));

            //Register all block types
            foreach (var type in blockTypes)
            {
                Block block = (Block)Activator.CreateInstance(type, nonPublic: true);
                blockMappings[block.id] = type;
            }
        }

        public static Block Get(string id)
        {
            //Retrieve the type from the mappings and create a new instance
            if(!blockMappings.ContainsKey(id)) throw new Exception($"BlockRegister has received an invalid block id: {id}"); //debug code, replace with exception safe behaviour
            
            Type type = blockMappings[id];

            Block block = (Block)Activator.CreateInstance(type, nonPublic: true);
            return block;
        }
    }
}
