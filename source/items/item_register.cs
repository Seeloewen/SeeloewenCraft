
using Newtonsoft.Json.Linq;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    static class ItemRegister
    {
        public static Item GenerateItem(string id)
        {
            switch (id)
            {
                case "sc:grass_block_item":
                    return new GrassItem();

                case "sc:dirt_item":
                    return new DirtItem();

                case "sc:stone_item":
                    return new StoneItem();

                case "sc:oak_log_item":
                    return new OakLogItem();

                case "sc:oak_leaves_item":
                    return new OakLeavesItem();

                case "sc:spruce_log_item":
                    return new SpruceLogItem();

                case "sc:spruce_leaves_item":
                    return new SpruceLeavesItem();

                case "sc:coal_ore_item":
                    return new CoalOreItem();

                case "sc:iron_ore_item":
                    return new IronOreItem();

                case "sc:chest_item":
                    return new ChestItem();

                case "sc:bedrock_item":
                    return new BedrockItem();

                case "sc:magma_block_item":
                    return new MagmaBlockItem();

                case "sc:torch_item":
                    return new TorchItem();

                case "sc:potted_cactus_item":
                    return new PottedCactusItem();

                case "sc:water_item":
                    return new WaterItem();

                case "sc:stone_hammer_item":
                    return new StoneHammerItem();

                case "sc:air_item":
                    return new AirItem();

                case "sc:diamond_ore_item":
                    return new DiamondOreItem();

                case "sc:crafting_table_item":
                    return new CraftingTable();

                case "sc:cobblestone_topleft_item":
                    return new CobbleStoneItem_TopLeft();

                case "sc:cobblestone_topright_item":
                    return new CobbleStoneItem_TopRight();

                case "sc:cobblestone_bottomleft_item":
                    return new CobbleStoneItem_BottomLeft();

                case "sc:cobblestone_bottomright_item":
                    return new CobbleStoneItem_BottomRight();

                case "sc:cobblestone_slabright_item":
                    return new CobbleStoneItem_SlabRight();

                case "sc:cobblestone_slableft_item":
                    return new CobbleStoneItem_SlabLeft();

                case "sc:cobblestone_slabtop_item":
                    return new CobbleStoneItem_SlabTop();

                case "sc:cobblestone_slabbottom_item":
                    return new CobbleStoneItem_SlabBottom();

                case "sc:cobblestone_stairtopright_item":
                    return new CobbleStoneItem_StairTopRight();

                case "sc:cobblestone_stairtopleft_item":
                    return new CobbleStoneItem_StairTopLeft();

                case "sc:cobblestone_stairbottomright_item":
                    return new CobbleStoneItem_StairBottomRight();

                case "sc:cobblestone_stairbottomleft_item":
                    return new CobbleStoneItem_StairBottomLeft();

                case "sc:cobblestone_item":
                    return new CobbleStoneItem();

                case "sc:cobblestone_center_item":
                    return new CobbleStoneItem_Center();

                case "sc:chiseler_item":
                    return new ChiselerItem();

                case "sc:unchiseler_item":
                    return new UnchiselerItem();

                case "sc:snowball_item":
                    return new SnowballItem();

                case "sc:bone_item":
                    return new BoneItem();

                case "sc:arrow_item":
                    return new ArrowItem();

                case "sc:spruce_door_item":
                    return new SpruceDoorItem();
                case "sc:oak_planks_topleft_item":
                    return new OakPlanksItem_TopLeft();

                case "sc:oak_planks_topright_item":
                    return new OakPlanksItem_TopRight();

                case "sc:oak_planks_bottomleft_item":
                    return new OakPlanksItem_BottomLeft();

                case "sc:oak_planks_bottomright_item":
                    return new OakPlanksItem_BottomRight();

                case "sc:oak_planks_slabright_item":
                    return new OakPlanksItem_SlabRight();

                case "sc:oak_planks_slableft_item":
                    return new OakPlanksItem_SlabLeft();

                case "sc:oak_planks_slabtop_item":
                    return new OakPlanksItem_SlabTop();

                case "sc:oak_planks_slabbottom_item":
                    return new OakPlanksItem_SlabBottom();

                case "sc:oak_planks_stairtopright_item":
                    return new OakPlanksItem_StairTopRight();

                case "sc:oak_planks_stairtopleft_item":
                    return new OakPlanksItem_StairTopLeft();

                case "sc:oak_planks_stairbottomright_item":
                    return new OakPlanksItem_StairBottomRight();

                case "sc:oak_planks_stairbottomleft_item":
                    return new OakPlanksItem_StairBottomLeft();

                case "sc:oak_planks_item":
                    return new OakPlanksItem();

                case "sc:oak_planks_center_item":
                    return new OakPlanksItem_Center();

                case "sc:spruce_planks_topleft_item":
                    return new SprucePlanksItem_TopLeft();

                case "sc:spruce_planks_topright_item":
                    return new SprucePlanksItem_TopRight();

                case "sc:spruce_planks_bottomleft_item":
                    return new SprucePlanksItem_BottomLeft();

                case "sc:spruce_planks_bottomright_item":
                    return new SprucePlanksItem_BottomRight();

                case "sc:spruce_planks_slabright_item":
                    return new SprucePlanksItem_SlabRight();

                case "sc:spruce_planks_slableft_item":
                    return new SprucePlanksItem_SlabLeft();

                case "sc:spruce_planks_slabtop_item":
                    return new SprucePlanksItem_SlabTop();

                case "sc:spruce_planks_slabbottom_item":
                    return new SprucePlanksItem_SlabBottom();

                case "sc:spruce_planks_stairtopright_item":
                    return new SprucePlanksItem_StairTopRight();

                case "sc:spruce_planks_stairtopleft_item":
                    return new SprucePlanksItem_StairTopLeft();

                case "sc:spruce_planks_stairbottomright_item":
                    return new SprucePlanksItem_StairBottomRight();

                case "sc:spruce_planks_stairbottomleft_item":
                    return new SprucePlanksItem_StairBottomLeft();

                case "sc:spruce_planks_item":
                    return new SprucePlanksItem();

                case "sc:spruce_planks_center_item":
                    return new SprucePlanksItem_Center();

                case "sc:sand_stone_topleft_item":
                    return new SandStoneItem_TopLeft();

                case "sc:sand_stone_topright_item":
                    return new SandStoneItem_TopRight();

                case "sc:sand_stone_bottomleft_item":
                    return new SandStoneItem_BottomLeft();

                case "sc:sand_stone_bottomright_item":
                    return new SandStoneItem_BottomRight();

                case "sc:sand_stone_slabright_item":
                    return new SandStoneItem_SlabRight();

                case "sc:sand_stone_slableft_item":
                    return new SandStoneItem_SlabLeft();

                case "sc:sand_stone_slabtop_item":
                    return new SandStoneItem_SlabTop();

                case "sc:sand_stone_slabbottom_item":
                    return new SandStoneItem_SlabBottom();

                case "sc:sand_stone_stairtopright_item":
                    return new SandStoneItem_StairTopRight();

                case "sc:sand_stone_stairtopleft_item":
                    return new SandStoneItem_StairTopLeft();

                case "sc:sand_stone_stairbottomright_item":
                    return new SandStoneItem_StairBottomRight();

                case "sc:sand_stone_stairbottomleft_item":
                    return new SandStoneItem_StairBottomLeft();

                case "sc:sand_stone_item":
                    return new SandStoneItem();

                case "sc:sand_stone_center_item":
                    return new SandStoneItem_Center();

                default:
                    return new BedrockItem();
            }
        }
    }
}
