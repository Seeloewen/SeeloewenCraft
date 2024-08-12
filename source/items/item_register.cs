
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

                case "sc:potted_cactus_item":
                    return new PottedCactusItem(world);

                case "sc:water_item":
                    return new WaterItem(world);

                case "sc:stone_hammer_item":
                    return new StoneHammerItem(world);

                case "sc:air_item":
                    return new AirItem(world);

                case "sc:diamond_ore_item":
                    return new DiamondOreItem(world);

                case "sc:crafting_table_item":
                    return new CraftingTable(world);

                case "sc:cobblestone_topleft_item":
                    return new CobbleStoneItem_TopLeft(world);

                case "sc:cobblestone_topright_item":
                    return new CobbleStoneItem_TopRight(world);

                case "sc:cobblestone_bottomleft_item":
                    return new CobbleStoneItem_BottomLeft(world);

                case "sc:cobblestone_bottomright_item":
                    return new CobbleStoneItem_BottomRight(world);

                case "sc:cobblestone_slabright_item":
                    return new CobbleStoneItem_SlabRight(world);

                case "sc:cobblestone_slableft_item":
                    return new CobbleStoneItem_SlabLeft(world);

                case "sc:cobblestone_slabtop_item":
                    return new CobbleStoneItem_SlabTop(world);

                case "sc:cobblestone_slabbottom_item":
                    return new CobbleStoneItem_SlabBottom(world);

                case "sc:cobblestone_stairtopright_item":
                    return new CobbleStoneItem_StairTopRight(world);

                case "sc:cobblestone_stairtopleft_item":
                    return new CobbleStoneItem_StairTopLeft(world);

                case "sc:cobblestone_stairbottomright_item":
                    return new CobbleStoneItem_StairBottomRight(world);

                case "sc:cobblestone_stairbottomleft_item":
                    return new CobbleStoneItem_StairBottomLeft(world);

                case "sc:cobblestone_item":
                    return new CobbleStoneItem(world);

                case "sc:cobblestone_center_item":
                    return new CobbleStoneItem_Center(world);

                case "sc:chiseler_item":
                    return new ChiselerItem(world);

                case "sc:unchiseler_item":
                    return new UnchiselerItem(world);

                case "sc:snowball_item":
                    return new SnowballItem(world);

                case "sc:bone_item":
                    return new BoneItem(world);

                case "sc:arrow_item":
                    return new ArrowItem(world);
            }
            return null;
        }


    }
}
