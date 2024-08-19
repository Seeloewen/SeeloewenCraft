namespace SeeloewenCraft
{
    static public class BlockRegister
    {

        public static Block GenerateBlock(string id)
        {
            switch (id)
            {
                case "sc:grass_block":
                    return new GrassBlock(false);
                case "sc:dirt_block":
                    return new DirtBlock(false);
                case "sc:stone_block":
                    return new StoneBlock(false);
                case "sc:air_block":
                    return new AirBlock(false);
                case "sc:bedrock_block":
                    return new BedrockBlock(false);
                case "sc:diamond_ore_block":
                    return new DiamondOreBlock(false);
                case "sc:iron_ore_block":
                    return new IronOreBlock(false);
                case "sc:coal_ore_block":
                    return new CoalOreBlock(false);
                case "sc:oak_log_block":
                    return new OakLogBlock(false);
                case "sc:oak_leaves_block":
                    return new OakLeavesBlock(false);
                case "sc:spruce_log_block":
                    return new SpruceLogBlock(false);
                case "sc:spruce_leaves_block":
                    return new SpruceLeavesBlock(false);
                case "sc:chest_block":
                    return new ChestBlock(false);
                case "sc:magma_block":
                    return new MagmaBlock(false);
                case "sc:torch_block":
                    return new TorchBlock(false);
                case "sc:water_1_right_block":
                    return new WaterBlock_1_Right(false);
                case "sc:water_1_left_block":
                    return new WaterBlock_1_Left(false);
                case "sc:water_2_right_block":
                    return new WaterBlock_2_Right(false);
                case "sc:water_2_left_block":
                    return new WaterBlock_2_Left(false);
                case "sc:water_3_right_block":
                    return new WaterBlock_3_Right(false);
                case "sc:water_3_left_block":
                    return new WaterBlock_3_Left(false);
                case "sc:water_4_right_block":
                    return new WaterBlock_4_Right(false);
                case "sc:water_4_left_block":
                    return new WaterBlock_4_Left(false);
                case "sc:water_5_right_block":
                    return new WaterBlock_5_Right(false);
                case "sc:water_5_left_block":
                    return new WaterBlock_5_Left(false);
                case "sc:water_6_block":
                    return new WaterBlock_6(false);
                /*case "sc:modded_block":
                    //if()
                    string type = blockToken.GetString("/type");
                    return new ModdedBlock(type,  false);*/
                case "sc:crafting_table_block":
                    return new CraftingTableBlock(false);
                case "sc:cobblestone_topleft":
                    return new CobbleStoneBlock_TopLeft(false);
                case "sc:cobblestone_topright":
                    return new CobblestoneBlock_TopRight(false);
                case "sc:cobblestone_bottomleft":
                    return new CobblestoneBlock_BottomLeft(false);
                case "sc:cobblestone_bottomright":
                    return new CobblestoneBlock_BottomRight(false);
                case "sc:cobblestone_slabright":
                    return new CobblestoneBlock_SlabRight(false);
                case "sc:cobblestone_slableft":
                    return new CobblestoneBlock_SlabLeft(false);
                case "sc:cobblestone_slabtop":
                    return new CobblestoneBlock_SlabTop(false);
                case "sc:cobblestone_slabbottom":
                    return new CobblestoneBlock_SlabBottom(false);
                case "sc:cobblestone_stairtopright":
                    return new CobblestoneBlock_StairTopRight(false);
                case "sc:cobblestone_stairtopleft":
                    return new CobblestoneBlock_StairTopLeft(false);
                case "sc:cobblestone_stairbottomright":
                    return new CobblestoneBlock_StairBottomRight(false);
                case "sc:cobblestone_stairbottomleft":
                    return new CobblestoneBlock_StairBottomLeft(false);
                case "sc:cobblestone_block":
                    return new CobblestoneBlock(false);
                case "sc:chiseler_block":
                    return new ChiselerBlock(false);
                case "sc:unchiseler_block":
                    return new UnchiselerBlock(false);
                case "sc:cobblestone_center":
                    return new CobbleStoneBlock_Center(false);
                case "sc:potted_cactus_base":
                    return new PottedCactus_Base(false);
                case "sc:potted_cactus_top":
                    return new PottedCactus_Top(false);
                case "sc:spruce_door_base":
                    return new SpruceDoor_Base(false);
                case "sc:spruce_door_top":
                    return new SpruceDoor_Top(false);
                case "sc:oak_planks_topleft":
                    return new OakPlanksBlock_TopLeft(false);
                case "sc:oak_planks_topright":
                    return new OakPlanksBlock_TopRight(false);
                case "sc:oak_planks_bottomleft":
                    return new OakPlanksBlock_BottomLeft(false);
                case "sc:oak_planks_bottomright":
                    return new OakPlanksBlock_BottomRight(false);
                case "sc:oak_planks_slabright":
                    return new OakPlanksBlock_SlabRight(false);
                case "sc:oak_planks_slableft":
                    return new OakPlanksBlock_SlabLeft(false);
                case "sc:oak_planks_slabtop":
                    return new OakPlanksBlock_SlabTop(false);
                case "sc:oak_planks_slabbottom":
                    return new OakPlanksBlock_SlabBottom(false);
                case "sc:oak_planks_stairtopright":
                    return new OakPlanksBlock_StairTopRight(false);
                case "sc:oak_planks_stairtopleft":
                    return new OakPlanksBlock_StairTopLeft(false);
                case "sc:oak_planks_stairbottomright":
                    return new OakPlanksBlock_StairBottomRight(false);
                case "sc:oak_planks_stairbottomleft":
                    return new OakPlanksBlock_StairBottomLeft(false);
                case "sc:spruce_planks_topleft":
                    return new SprucePlanksBlock_TopLeft(false);
                case "sc:spruce_planks_topright":
                    return new SprucePlanksBlock_TopRight(false);
                case "sc:spruce_planks_bottomleft":
                    return new SprucePlanksBlock_BottomLeft(false);
                case "sc:spruce_planks_bottomright":
                    return new SprucePlanksBlock_BottomRight(false);
                case "sc:spruce_planks_slabright":
                    return new SprucePlanksBlock_SlabRight(false);
                case "sc:spruce_planks_slableft":
                    return new SprucePlanksBlock_SlabLeft(false);
                case "sc:spruce_planks_slabtop":
                    return new SprucePlanksBlock_SlabTop(false);
                case "sc:spruce_planks_slabbottom":
                    return new SprucePlanksBlock_SlabBottom(false);
                case "sc:spruce_planks_stairtopright":
                    return new SprucePlanksBlock_StairTopRight(false);
                case "sc:spruce_planks_stairtopleft":
                    return new SprucePlanksBlock_StairTopLeft(false);
                case "sc:spruce_planks_stairbottomright":
                    return new SprucePlanksBlock_StairBottomRight(false);
                case "sc:spruce_planks_stairbottomleft":
                    return new SprucePlanksBlock_StairBottomLeft(false);
                case "sc:sand_stone_topleft":
                    return new SandStoneBlock_TopLeft(false);
                case "sc:sand_stone_topright":
                    return new SandStoneBlock_TopRight(false);
                case "sc:sand_stone_bottomleft":
                    return new SandStoneBlock_BottomLeft(false);
                case "sc:sand_stone_bottomright":
                    return new SandStoneBlock_BottomRight(false);
                case "sc:sand_stone_slabright":
                    return new SandStoneBlock_SlabRight(false);
                case "sc:sand_stone_slableft":
                    return new SandStoneBlock_SlabLeft(false);
                case "sc:sand_stone_slabtop":
                    return new SandStoneBlock_SlabTop(false);
                case "sc:sand_stone_slabbottom":
                    return new SandStoneBlock_SlabBottom(false);
                case "sc:sand_stone_stairtopright":
                    return new SandStoneBlock_StairTopRight(false);
                case "sc:sand_stone_stairtopleft":
                    return new SandStoneBlock_StairTopLeft(false);
                case "sc:sand_stone_stairbottomright":
                    return new SandStoneBlock_StairBottomRight(false);
                case "sc:sand_stone_stairbottomleft":
                    return new SandStoneBlock_StairBottomLeft(false);
                case "sc:sand_stone_block":
                    return new SandStoneBlock(false);
                case "sc:oak_planks_block":
                    return new OakPlanksBlock(false);
                case "sc:spruce_planks_block":
                    return new SprucePlanksBlock(false);
                default:
                    return null;

            }
        }

    }
}
