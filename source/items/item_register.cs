namespace SeeloewenCraft
{
    static class ItemRegister
    {
        public static Item GenerateItem(string id)
        {
            switch (id)
            {
                case "sc:grass_block_item":
                    return new GrassBlockItem();

                case "sc:dirt_item":
                    return new DirtItem();

                case "sc:stone_block_item":
                    return new StoneBlockItem();

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

                case "sc:amethyst_ore_item":
                    return new AmethystOreItem();

                case "sc:anvil_item":
                    return new AnvilItem();

                case "sc:barrel_item":
                    return new BarrelItem();

                case "sc:blue_flower_item":
                    return new BlueFlowerItem();

                case "sc:bone_block_item":
                    return new BoneBlockItem();

                case "sc:cactus_fruit_item":
                    return new CactusFruitItem();

                case "sc:candle_item":
                    return new CandleItem();

                case "sc:copper_ore_item":
                    return new CopperOreItem();

                case "sc:dead_bush_item":
                    return new DeadBushItem();

                case "sc:emerald_ore_item":
                    return new EmeraldOreItem();

                case "sc:tungsten_ore_item":
                    return new TungstenOreItem();

                case "sc:flower_pot_item":
                    return new FlowerPotItem();

                case "sc:gold_ore_item":
                    return new GoldOreItem();

                case "sc:tin_ore_item":
                    return new TinOreItem();

                case "sc:grass_item":
                    return new GrassItem();

                case "sc:iron_gates_item":
                    return new IronGatesItem();

                case "sc:ladder_item":
                    return new LadderItem();

                case "sc:mossy_cobblestone_item":
                    return new MossyCobblestoneItem();

                case "sc:oak_sapling_item":
                    return new OakSaplingItem();

                case "sc:spruce_sapling_item":
                    return new SpruceSaplingItem();

                case "sc:oak_table_item":
                    return new OakTableItem();

                case "sc:spruce_table_item":
                    return new SpruceTableItem();

                case "sc:sand_item":
                    return new SandItem();

                case "sc:sand_stone_bricks_item":
                    return new SandStoneBricksItem();

                case "sc:stone_bricks_item":
                    return new StoneBricksItem();

                case "sc:yellow_flower_item":
                    return new YellowFlowerItem();

                case "sc:amethyst_item":
                    return new AmethystItem();

                case "sc:apple_item":
                    return new AppleItem();

                case "sc:bread_item":
                    return new BreadItem();

                case "sc:bucket_empty_item":
                    return new BucketEmptyItem();

                case "sc:bucket_water_item":
                    return new BucketWaterItem();

                case "sc:coal_item":
                    return new CoalItem();

                case "sc:copper_bar_item":
                    return new CopperBarItem();

                case "sc:croissant_item":
                    return new CroissantItem();

                case "sc:diamond_item":
                    return new DiamondItem();

                case "sc:emerald_item":
                    return new EmeraldItem();

                case "sc:fossil_fragment_item":
                    return new FossilFragmentItem();

                case "sc:gold_bar_item":
                    return new GoldBarItem();

                case "sc:iron_bar_item":
                    return new IronBarItem();

                case "sc:iron_rod_item":
                    return new IronRodItem();

                case "sc:paper_item":
                    return new PaperItem();

                case "sc:pot_shard_item":
                    return new PotShardItem();

                case "sc:rock_item":
                    return new RockItem();

                case "sc:stick_item":
                    return new StickItem();

                case "sc:tin_bar_item":
                    return new TinBarItem();

                case "sc:tungsten_bar_item":
                    return new TungstenBar();

                case "sc:archeology_pot_item":
                    return new ArcheologyPotItem();

                case "sc:cactus_bottom_left_item":
                    return new Cactus_BottomLeftItem();

                case "sc:cactus_bottom_right_item":
                    return new Cactus_BottomRightItem();

                case "sc:cactus_top_right_item":
                    return new Cactus_TopRightItem();

                case "sc:cactus_top_left_item":
                    return new Cactus_TopLeftItem();

                case "sc:cactus_cross_item":
                    return new Cactus_CrossItem();

                case "sc:cactus_horizontal_item":
                    return new Cactus_HorizontalItem();

                case "sc:cactus_vertical_item":
                    return new Cactus_VerticalItem();

                case "sc:cactus_top_item":
                    return new Cactus_TopItem();

                case "sc:cactus_top_fruit_item":
                    return new Cactus_TopFruitItem();

                case "sc:oak_chair_item":
                    return new OakChairItem();

                case "sc:spruce_chair_item":
                    return new SpruceChairItem();

                case "sc:furnace_item":
                    return new FurnaceItem();

                case "sc:oak_door_item":
                    return new OakDoor();

                case "sc:oak_trapdoor_item":
                    return new OakTrapDoorItem();

                case "sc:spruce_trapdoor_item":
                    return new SpruceTrapDoorItem();

                case "sc:wax_item":
                    return new WaxItem();

                case "sc:stone_sword_item":
                    return new StoneSwordItem();

                case "sc:stone_pickaxe_item":
                    return new StonePickaxeItem();

                case "sc:stone_axe_item":
                    return new StoneAxeItem();

                case "sc:stone_shovel_item":
                    return new StoneShovelItem();

                case "sc:stone_scythe_item":
                    return new StoneScytheItem();

                case "sc:wood_hammer_item":
                    return new WoodHammerItem();

                case "sc:wood_sword_item":
                    return new WoodSwordItem();

                case "sc:wood_pickaxe_item":
                    return new WoodPickaxeItem();

                case "sc:wood_axe_item":
                    return new WoodAxeItem();

                case "sc:wood_shovel_item":
                    return new WoodShovelItem();

                case "sc:wood_scythe_item":
                    return new WoodScytheItem();

                case "sc:tin_hammer_item":
                    return new TinHammerItem();

                case "sc:tin_sword_item":
                    return new TinSwordItem();

                case "sc:tin_pickaxe_item":
                    return new TinPickaxeItem();

                case "sc:tin_axe_item":
                    return new TinAxeItem();

                case "sc:tin_shovel_item":
                    return new TinShovelItem();

                case "sc:tin_scythe_item":
                    return new TinScytheItem();

                case "sc:iron_hammer_item":
                    return new IronHammerItem();

                case "sc:iron_sword_item":
                    return new IronSwordItem();

                case "sc:iron_pickaxe_item":
                    return new IronPickaxeItem();

                case "sc:iron_axe_item":
                    return new IronAxeItem();

                case "sc:iron_shovel_item":
                    return new IronShovelItem();

                case "sc:iron_scythe_item":
                    return new IronScytheItem();

                case "sc:diamond_hammer_item":
                    return new DiamondHammerItem();

                case "sc:diamond_sword_item":
                    return new DiamondSwordItem();

                case "sc:diamond_pickaxe_item":
                    return new DiamondPickaxeItem();

                case "sc:diamond_axe_item":
                    return new DiamondAxeItem();

                case "sc:diamond_shovel_item":
                    return new DiamondShovelItem();

                case "sc:diamond_scythe_item":
                    return new DiamondScytheItem();

                case "sc:glass_item":
                    return new GlassItem();

                case "sc:cactus_left_item":
                    return new Cactus_LeftItem();

                case "sc:cactus_right_item":
                    return new Cactus_RightItem();

                case "sc:wheat_item":
                    return new WheatItem();

                case "sc:carrot_item":
                    return new CarrotItem();

                case "sc:seeds_item":
                    return new SeedsItem();

                case "sc:farmland_item":
                    return new FarmlandItem();

                default:
                    return null;
            }
        }
    }
}
