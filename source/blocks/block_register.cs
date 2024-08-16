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
                default:
                    return null;

            }
        }

    }
}
