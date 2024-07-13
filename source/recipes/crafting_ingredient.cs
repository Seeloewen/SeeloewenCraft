using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
namespace SeeloewenCraft
{
    public class CraftingIngredient
    {

        public Item item;
        public int amount;

        //-- Constructor --//

        public CraftingIngredient(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
        public CraftingIngredient(JsonToken token, World world)
        {

            switch (token.GetString("/item_id"))
            {
                case "sc:grass_block_item":
                    item = new GrassItem(world, null);
                    break;
                case "sc:dirt_item":
                    item = new DirtItem(world, null);
                    break;
                case "sc:stone_item":
                    item = new StoneItem(world, null);
                    break;
                case "sc:oak_log_item":
                    item = new OakLogItem(world, null);
                    break;
                case "sc:oak_leaves_item":
                    item = new OakLeavesItem(world, null);
                    break;
                case "sc:spruce_log_item":
                    item = new SpruceLogItem(world, null);
                    break;
                case "sc:spruce_leaves_item":
                    item = new SpruceLeavesItem(world, null);
                    break;
                case "sc:coal_ore_item":
                    item = new CoalOreItem(world, null);
                    break;
                case "sc:iron_ore_item":
                    item = new IronOreItem(world, null);
                    break;
                case "sc:chest_item":
                    item = new ChestItem(world, null);
                    break;
                case "sc:bedrock_item":
                    item = new BedrockItem(world, null);
                    break;
                case "sc:magma_block_item":
                    item = new MagmaBlockItem(world, null);
                    break;
                case "sc:torch_item":
                    item = new TorchItem(world, null);
                    break;
                case "sc:plant_2_item":
                    item = new Plant2Item(world, null);
                    break;
                case "sc:water_item":
                    item = new WaterItem(world, null);
                    break;
                case "sc:hammer_item":
                    item = new HammerItem(world, null);
                    break;
                case "sc:air_item":
                    item = new AirItem(world, null);
                    break;
                case "sc:diamond_ore_item":
                    item = new DiamondOreItem(world, null);
                    break;
                case "sc:alpha_crafter_item":
                    item = new AlphaCrafterItem(world, null);
                    break;
            }

            amount = token.GetInt("/amount");
        }

    }
}
