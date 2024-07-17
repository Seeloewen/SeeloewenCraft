
using Newtonsoft.Json.Linq;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    static class ItemRegister
    {

        public static Item GenerateItem(string id, World world)
        {
            switch (id)
            {
                case "sc:grass_block_item":
                    return new GrassItem(world);
                    
                case "sc:dirt_item":
                    return new DirtItem(world);
                    
                case "sc:stone_item":
                    return new StoneItem(world);
                    
                case "sc:oak_log_item":
                    return new OakLogItem(world);
                    
                case "sc:oak_leaves_item":
                    return new OakLeavesItem(world);
                    
                case "sc:spruce_log_item":
                    return new SpruceLogItem(world);
                    
                case "sc:spruce_leaves_item":
                    return new SpruceLeavesItem(world);
                    
                case "sc:coal_ore_item":
                    return new CoalOreItem(world);
                    
                case "sc:iron_ore_item":
                    return new IronOreItem(world);
                    
                case "sc:chest_item":
                    return new ChestItem(world);
                    
                case "sc:bedrock_item":
                    return new BedrockItem(world);
                    
                case "sc:magma_block_item":
                    return new MagmaBlockItem(world);
                    
                case "sc:torch_item":
                    return new TorchItem(world);
                    
                case "sc:plant_2_item":
                    return new Plant2Item(world);
                    
                case "sc:water_item":
                    return new WaterItem(world);
                    
                case "sc:hammer_item":
                    return new HammerItem(world);
                    
                case "sc:air_item":
                    return new AirItem(world);
                    
                case "sc:diamond_ore_item":
                    return new DiamondOreItem(world);
                    
                case "sc:alpha_crafter_item":
                    return new AlphaCrafterItem(world);
            }
            return null;
        }


    }
}
