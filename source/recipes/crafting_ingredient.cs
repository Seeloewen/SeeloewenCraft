
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
                    item = new GrassItem(world);
                    break;
                case "sc:dirt_item":
                    item = new DirtItem(world);
                    break;
                case "sc:stone_item":
                    item = new StoneItem(world);
                    break;
                case "sc:oak_log_item":
                    item = new OakLogItem(world);
                    break;
                case "sc:oak_leaves_item":
                    item = new OakLeavesItem(world);
                    break;
                case "sc:spruce_log_item":
                    item = new SpruceLogItem(world);
                    break;
                case "sc:spruce_leaves_item":
                    item = new SpruceLeavesItem(world);
                    break;
                case "sc:coal_ore_item":
                    item = new CoalOreItem(world);
                    break;
                case "sc:iron_ore_item":
                    item = new IronOreItem(world);
                    break;
                case "sc:chest_item":
                    item = new ChestItem(world);
                    break;
                case "sc:bedrock_item":
                    item = new BedrockItem(world);
                    break;
                case "sc:magma_block_item":
                    item = new MagmaBlockItem(world);
                    break;
                case "sc:torch_item":
                    item = new TorchItem(world);
                    break;
                case "sc:plant_2_item":
                    item = new Plant2Item(world);
                    break;
                case "sc:water_item":
                    item = new WaterItem(world);
                    break;
                case "sc:hammer_item":
                    item = new HammerItem(world);
                    break;
                case "sc:air_item":
                    item = new AirItem(world);
                    break;
                case "sc:diamond_ore_item":
                    item = new DiamondOreItem(world);
                    break;
                case "sc:alpha_crafter_item":
                    item = new AlphaCrafterItem(world);
                    break;
            }

            amount = token.GetInt("/amount");
        }

    }
}
