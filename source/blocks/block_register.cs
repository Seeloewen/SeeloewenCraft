
namespace SeeloewenCraft
{
    static public class BlockRegister
    {

        public static Block GenerateBlock(string id, World world)
        {
            switch (id)
            {
                case "sc:grass_block":
                    return new GrassBlock(world, false);
                case "sc:dirt_block":
                    return new DirtBlock(world, false);
                case "sc:stone_block":
                    return new StoneBlock(world, false);
                case "sc:air_block":
                    return new AirBlock(world, false);
                case "sc:bedrock_block":
                    return new BedrockBlock(world, false);
                case "sc:diamond_ore_block":
                    return new DiamondOreBlock(world, false);
                case "sc:iron_ore_block":
                    return new IronOreBlock(world, false);
                case "sc:coal_ore_block":
                    return new CoalOreBlock(world, false);
                case "sc:oak_log_block":
                    return new OakLogBlock(world, false);
                case "sc:oak_leaves_block":
                    return new OakLeavesBlock(world, false);
                case "sc:spruce_log_block":
                    return new SpruceLogBlock(world, false);
                case "sc:spruce_leaves_block":
                    return new SpruceLeavesBlock(world, false);
                case "sc:chest_block":
                    return new ChestBlock(world, false);
                case "sc:magma_block":
                    return new MagmaBlock(world, false);
                case "sc:torch_block":
                    return new TorchBlock(world, false);
                case "sc:water_1_right_block":
                    return new WaterBlock_1_Right(world, false);
                case "sc:water_1_left_block":
                    return new WaterBlock_1_Left(world, false);
                case "sc:water_2_right_block":
                    return new WaterBlock_2_Right(world, false);
                case "sc:water_2_left_block":
                    return new WaterBlock_2_Left(world, false);
                case "sc:water_3_right_block":
                    return new WaterBlock_3_Right(world, false);
                case "sc:water_3_left_block":
                    return new WaterBlock_3_Left(world, false);
                case "sc:water_4_right_block":
                    return new WaterBlock_4_Right(world, false);
                case "sc:water_4_left_block":
                    return new WaterBlock_4_Left(world, false);
                case "sc:water_5_right_block":
                    return new WaterBlock_5_Right(world, false);
                case "sc:water_5_left_block":
                    return new WaterBlock_5_Left(world, false);
                case "sc:water_6_block":
                    return new WaterBlock_6(world, false);
                /*case "sc:modded_block":
                    //if()
                    string type = blockToken.GetString("/type");
                    return new ModdedBlock(type, world, false);*/
                case "sc:alpha_crafter_block":
                    return new AlphaCrafterBlock(world, false);
                default:
                    return null;

            }
        }

    }
}
