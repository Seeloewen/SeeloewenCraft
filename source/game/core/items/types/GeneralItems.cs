using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.core.items
{
    //-- Items --//

    public class GrassBlockItem : Item
    {
        public GrassBlockItem() : base()
        {
            Init("Grass Block", "sc:grass_block_item", "sc:grass_block", true);
        }
    }

    public class StoneBlockItem : Item
    {
        public StoneBlockItem() : base()
        {
            Init("Stone Block", "sc:stone_block_item", "sc:stone_block", true);
        }
    }

    public class DirtItem : Item
    {
        public DirtItem() : base()
        {
            Init("Dirt", "sc:dirt_item", "sc:dirt_block", true);
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem() : base()
        {
            Init("Coal Ore", "sc:coal_ore_item", "sc:coal_ore_block", true);
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem() : base()
        {
            Init("Diamond Ore", "sc:diamond_ore_item", "sc:diamond_ore_block", true);
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem() : base()
        {
            Init("Iron Ore", "sc:iron_ore_item", "sc:iron_ore_block", true);
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem() : base()
        {
            Init("Oak Log", "sc:oak_log_item", "sc:oak_log_block", true);
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem() : base()
        {
            Init("Oak Leaves", "sc:oak_leaves_item", "sc:oak_leaves_block", true);
        }
    }

    public class SpruceLogItem : Item
    {
        public SpruceLogItem() : base()
        {
            Init("Spruce Log", "sc:spruce_log_item", "sc:spruce_log_block", true);
        }
    }

    public class SpruceLeavesItem : Item
    {
        public SpruceLeavesItem() : base()
        {
            Init("Spruce Leaves", "sc:spruce_leaves_item", "sc:spruce_leaves_block", true);
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem() : base()
        {
            Init("Bedrock", "sc:bedrock_item", "sc:bedrock_block", true);
        }
    }

    public class ChestItem : Item
    {
        public ChestItem() : base()
        {
            Init("Chest", "sc:chest_item", "sc:chest_block", true);
        }
    }

    public class MagmaBlockItem : Item
    {
        public MagmaBlockItem() : base()
        {
            Init("Magma Block", "sc:magma_block_item", "sc:magma_block", true);
        }
    }

    public class TorchItem : Item
    {
        public TorchItem() : base()
        {
            Init("Torch", "sc:torch_item", "sc:torch_block", true);
        }
    }

    public class WaterItem : Item
    {
        public WaterItem() : base()
        {
            Init("Water", "sc:water_item", "sc:water_6_block", true);
        }
    }

    public class PottedCactusItem : Item
    {
        public PottedCactusItem() : base()
        {
            Init("Potted Cactus", "sc:potted_cactus_item", "sc:potted_cactus_base", true);
        }
    }

    public class CraftingTable : Item
    {
        public CraftingTable() : base()
        {
            Init("Crafting Table", "sc:crafting_table_item", "sc:crafting_table_block", true);
        }
    }

    public class CobbleStoneItem : Item
    {
        public CobbleStoneItem() : base()
        {
            Init("Cobblestone", "sc:cobblestone_item", "sc:cobblestone_block", true);
        }
    }

    public class ChiselerItem : Item
    {
        public ChiselerItem() : base()
        {
            Init("Chiseler", "sc:chiseler_item", "sc:chiseler_block", true);
        }
    }

    public class UnchiselerItem : Item
    {
        public UnchiselerItem() : base()
        {
            Init("Unchiseler", "sc:unchiseler_item", "sc:unchiseler_block", true);
        }
    }

    public class BoneItem : Item
    {
        public BoneItem() : base()
        {
            Init("Bone", "sc:bone_item", null, true);
        }
    }

    public class ArrowItem : Item
    {
        public ArrowItem() : base()
        {
            Init("Arrow", "sc:arrow_item", null, true);
        }
    }


    public class SnowballItem : Item
    {
        public SnowballItem() : base()
        {
            Init("Snowball", "sc:snowball_item", null, true);
        }
    }

    public class SpruceDoorItem : Item
    {
        public SpruceDoorItem() : base()
        {
            Init("Spruce Door", "sc:spruce_door_item", "sc:spruce_door_base", true);
        }
    }

    public class OakPlanksItem : Item
    {
        public OakPlanksItem() : base()
        {
            Init("Oak Planks", "sc:oak_planks_item", "sc:oak_planks_block", true);
        }
    }

    public class SprucePlanksItem : Item
    {
        public SprucePlanksItem() : base()
        {
            Init("Spruce Planks", "sc:spruce_planks_item", "sc:spruce_planks_block", true);
        }
    }

    public class SandStoneItem : Item
    {
        public SandStoneItem() : base()
        {
            Init("Sand Stone", "sc:sand_stone_item", "sc:sand_stone_block", true);
        }
    }

    public class ArcheologyPotItem : Item
    {
        public ArcheologyPotItem() : base()
        {
            Init("Archeology Pot", "sc:archeology_pot_item", "sc:archeology_pot_base", true);
        }
    }

    public class Cactus_LeftItem : Item
    {
        public Cactus_LeftItem() : base()
        {
            Init("Cactus Left", "sc:cactus_left_item", "sc:cactus_left", true);
        }
    }

    public class Cactus_RightItem : Item
    {
        public Cactus_RightItem() : base()
        {
            Init("Cactus Right Left", "sc:cactus_right_item", "sc:cactus_right", true);
        }
    }

    public class Cactus_BottomLeftItem : Item
    {
        public Cactus_BottomLeftItem() : base()
        {
            Init("Cactus Bottom Left", "sc:cactus_bottom_left_item", "sc:cactus_bottom_left", true);
        }
    }

    public class Cactus_BottomRightItem : Item
    {
        public Cactus_BottomRightItem() : base()
        {
            Init("Cactus Bottom Right", "sc:cactus_bottom_right_item", "sc:cactus_bottom_right", true);
        }
    }

    public class Cactus_TopRightItem : Item
    {
        public Cactus_TopRightItem() : base()
        {
            Init("Cactus Top Right", "sc:cactus_top_right_item", "sc:cactus_top_right", true);
        }
    }

    public class Cactus_TopLeftItem : Item
    {
        public Cactus_TopLeftItem() : base()
        {
            Init("Cactus Top Left", "sc:cactus_top_left_item", "sc:cactus_top_left", true);
        }
    }

    public class Cactus_CrossItem : Item
    {
        public Cactus_CrossItem() : base()
        {
            Init("Cactus Cross", "sc:cactus_cross_item", "sc:cactus_cross", true);
        }
    }

    public class Cactus_HorizontalItem : Item
    {
        public Cactus_HorizontalItem() : base()
        {
            Init("Cactus Horizontal", "sc:cactus_horizontal_item", "sc:cactus_horizontal", true);
        }
    }

    public class Cactus_VerticalItem : Item
    {
        public Cactus_VerticalItem() : base()
        {
            Init("Cactus Vertical", "sc:cactus_vertical_item", "sc:cactus_vertical", true);
        }
    }

    public class Cactus_TopItem : Item
    {
        public Cactus_TopItem() : base()
        {
            Init("Cactus Top", "sc:cactus_top_item", "sc:cactus_top", true);
        }
    }

    public class Cactus_TopFruitItem : Item
    {
        public Cactus_TopFruitItem() : base()
        {
            Init("Cactus Top Fruit", "sc:cactus_top_fruit_item", "sc:cactus_top_fruit", true);
        }
    }

    public class OakChairItem : Item
    {
        public OakChairItem() : base()
        {
            Init("Oak Chair", "sc:oak_chair_item", "sc:oak_chair_block", true);
        }
    }

    public class SpruceChairItem : Item
    {
        public SpruceChairItem() : base()
        {
            Init("Spruce Chair", "sc:spruce_chair_item", "sc:spruce_chair_block", true);
        }
    }

    public class FurnaceItem : Item
    {
        public FurnaceItem() : base()
        {
            Init("Furnace", "sc:furnace_item", "sc:furnace_block", true);
        }
    }

    public class OakDoor : Item
    {
        public OakDoor() : base()
        {
            Init("Oak Door", "sc:oak_door_item", "sc:oak_door_base", true);
        }
    }

    public class OakTrapDoorItem : Item
    {
        public OakTrapDoorItem() : base()
        {
            Init("Oak Trapdoor", "sc:oak_trapdoor_item", "sc:oak_trapdoor", true);
        }
    }

    public class SpruceTrapDoorItem : Item
    {
        public SpruceTrapDoorItem() : base()
        {
            Init("Spruce Trapdoor", "sc:spruce_trapdoor_item", "sc:spruce_trapdoor", true);
        }
    }

    public class AmethystOreItem : Item
    {
        public AmethystOreItem() : base()
        {
            Init("Amethyst Ore", "sc:amethyst_ore_item", "sc:amethyst_ore_block", true);
        }
    }

    public class AnvilItem : Item
    {
        public AnvilItem() : base()
        {
            Init("Anvil", "sc:anvil_item", "sc:anvil_block", true);
        }
    }

    public class BarrelItem : Item
    {
        public BarrelItem() : base()
        {
            Init("Barrel", "sc:barrel_item", "sc:barrel_block", true);
        }
    }

    public class BlueFlowerItem : Item
    {
        public BlueFlowerItem() : base()
        {
            Init("Blue Flower", "sc:blue_flower_item", "sc:blue_flower_block", true);
        }
    }

    public class BoneBlockItem : Item
    {
        public BoneBlockItem() : base()
        {
            Init("Bone Block", "sc:bone_block_item", "sc:bone_block", true);
        }
    }

    public class CactusFruitItem : Item
    {
        public CactusFruitItem() : base()
        {
            Init("Cactus Fruit", "sc:cactus_fruit_item", "sc:cactus_fruit_block", true);
        }
    }

    public class CandleItem : Item
    {
        public CandleItem() : base()
        {
            Init("Candle", "sc:candle_item", "sc:candle_block", true);
        }
    }
    public class CopperOreItem : Item
    {
        public CopperOreItem() : base()
        {
            Init("Copper Ore", "sc:copper_ore_item", "sc:copper_ore_block", true);
        }
    }

    public class DeadBushItem : Item
    {
        public DeadBushItem() : base()
        {
            Init("Dead Bush", "sc:dead_bush_item", "sc:dead_bush_block", true);
        }
    }

    public class EmeraldOreItem : Item
    {
        public EmeraldOreItem() : base()
        {
            Init("Emerald Ore", "sc:emerald_ore_item", "sc:emerald_ore_block", true);
        }
    }
    public class TungstenOreItem : Item
    {
        public TungstenOreItem() : base()
        {
            Init("Tungsten Ore", "sc:tungsten_ore_item", "sc:tungsten_ore_block", true);
        }
    }
    public class FlowerPotItem : Item
    {
        public FlowerPotItem() : base()
        {
            Init("Flower Pot", "sc:flower_pot_item", "sc:flower_pot_block", true);
        }
    }
    public class GoldOreItem : Item
    {
        public GoldOreItem() : base()
        {
            Init("Gold Ore", "sc:gold_ore_item", "sc:gold_ore_block", true);
        }
    }
    public class TinOreItem : Item
    {
        public TinOreItem() : base()
        {
            Init("Tin Ore", "sc:tin_ore_item", "sc:tin_ore_block", true);
        }
    }

    public class GrassItem : Item
    {
        public GrassItem() : base()
        {
            Init("Grass", "sc:grass_item", "sc:grass", true);
        }
    }

    public class IronGatesItem : Item
    {
        public IronGatesItem() : base()
        {
            Init("Iron Gates", "sc:iron_gates_item", "sc:iron_gates_block", true);
        }
    }
    public class LadderItem : Item
    {
        public LadderItem() : base()
        {
            Init("Ladder", "sc:ladder_item", "sc:ladder_block", true);
        }
    }

    public class MossyCobblestoneItem : Item
    {
        public MossyCobblestoneItem() : base()
        {
            Init("Mossy Cobblestone", "sc:mossy_cobblestone_item", "sc:mossy_cobblestone_block", true);
        }
    }

    public class OakSaplingItem : Item
    {
        public OakSaplingItem() : base()
        {
            Init("Oak Sapling", "sc:oak_sapling_item", "sc:oak_sapling_block", true);
        }
    }

    public class SpruceSaplingItem : Item
    {
        public SpruceSaplingItem() : base()
        {
            Init("Spruce Sapling", "sc:spruce_sapling_item", "sc:spruce_sapling_block", true);
        }
    }

    public class OakTableItem : Item
    {
        public OakTableItem() : base()
        {
            Init("Oak Table", "sc:oak_table_item", "sc:oak_table_block", true);
        }
    }

    public class SpruceTableItem : Item
    {
        public SpruceTableItem() : base()
        {
            Init("Spruce Table", "sc:spruce_table_item", "sc:spruce_table_block", true);
        }
    }

    public class SandItem : Item
    {
        public SandItem() : base()
        {
            Init("Sand", "sc:sand_item", "sc:sand_block", true);
        }
    }

    public class SandStoneBricksItem : Item
    {
        public SandStoneBricksItem() : base()
        {
            Init("Sand Stone Bricks", "sc:sand_stone_bricks_item", "sc:sand_stone_bricks_block", true);
        }
    }

    public class StoneBricksItem : Item
    {
        public StoneBricksItem() : base()
        {
            Init("Stone Bricks", "sc:stone_bricks_item", "sc:stone_bricks_block", true);
        }
    }

    public class YellowFlowerItem : Item
    {
        public YellowFlowerItem() : base()
        {
            Init("Yellow Flower", "sc:yellow_flower_item", "sc:yellow_flower_block", true);
        }
    }

    public class AmethystItem : Item
    {
        public AmethystItem() : base()
        {
            Init("Amethyst", "sc:amethyst_item", null, false);
        }
    }

    public class AppleItem : FoodItem
    {
        public AppleItem() : base()
        {
            Init("Apple", "sc:apple_item", null, false);
            healAmount = 2;
        }
    }

    public class BreadItem : FoodItem
    {
        public BreadItem() : base()
        {
            Init("Bread", "sc:bread_item", null, false);
            healAmount = 3;
        }
    }

    public class BucketEmptyItem : Item
    {
        public BucketEmptyItem() : base()
        {
            Init("Empty Bucket", "sc:bucket_empty_item", null, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            if (block is WaterBlock_6)
            {
                Player.Get().inventory.Remove(id, 1);
                Player.Get().inventory.Add("sc:bucket_water_item", 1);
                World.Get().SetBlock(block.GetPosData(), BlockRegister.Get("sc:air_block"));
            }
            else if (block is Rice_Top rice && rice.IsReady())
            {
                rice.RightClickAction();
            }
        }
    }

    public class BucketWaterItem : Item
    {
        public BucketWaterItem() : base()
        {
            Init("Water Bucket", "sc:bucket_water_item", null, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            if (block.HasTag(BlockTags.REPLACEABLE))
            {
                Player.Get().inventory.Remove(id, 1);
                Player.Get().inventory.Add("sc:bucket_empty_item", 1);
                World.Get().SetBlock(block.GetPosData(), BlockRegister.Get("sc:water_6_block"));
            }
        }
    }

    public class CoalItem : Item
    {
        public CoalItem() : base()
        {
            Init("Coal", "sc:coal_item", null, false);
        }
    }

    public class CopperBarItem : Item
    {
        public CopperBarItem() : base()
        {
            Init("Copper Bar", "sc:copper_bar_item", null, false);
        }
    }

    public class CroissantItem : FoodItem
    {
        public CroissantItem() : base()
        {
            Init("Croissant", "sc:croissant_item", null, false);
            healAmount = 2;
        }
    }

    public class DiamondItem : Item
    {
        public DiamondItem() : base()
        {
            Init("Diamond", "sc:diamond_item", null, false);
        }
    }

    public class EmeraldItem : Item
    {
        public EmeraldItem() : base()
        {
            Init("Emerald", "sc:emerald_item", null, false);
        }
    }

    public class FossilFragmentItem : Item
    {
        public FossilFragmentItem() : base()
        {
            Init("Fossil Fragment", "sc:fossil_fragment_item", null, false);
        }
    }

    public class GoldBarItem : Item
    {
        public GoldBarItem() : base()
        {
            Init("Gold Bar", "sc:gold_bar_item", null, false);
        }
    }

    public class IronBarItem : Item
    {
        public IronBarItem() : base()
        {
            Init("Iron Bar", "sc:iron_bar_item", null, false);
        }
    }

    public class IronRodItem : Item
    {
        public IronRodItem() : base()
        {
            Init("Iron Rod", "sc:iron_rod_item", null, false);
        }
    }

    public class PaperItem : Item
    {
        public PaperItem() : base()
        {
            Init("Paper", "sc:paper_item", null, false);
        }
    }

    public class PotShardItem : Item
    {
        public PotShardItem() : base()
        {
            Init("Pot Shard", "sc:pot_shard_item", null, false);
        }
    }

    public class RockItem : Item
    {
        public RockItem() : base()
        {
            Init("Rock", "sc:rock_item", null, false);
        }
    }

    public class StickItem : Item
    {
        public StickItem() : base()
        {
            Init("Stick", "sc:stick_item", null, false);
        }
    }

    public class TinBarItem : Item
    {
        public TinBarItem() : base()
        {
            Init("Tin Bar", "sc:tin_bar_item", null, false);
        }
    }

    public class TungstenBar : Item
    {
        public TungstenBar() : base()
        {
            Init("Tungsten Bar", "sc:tungsten_bar_item", null, false);
        }
    }
    public class WaxItem : Item
    {
        public WaxItem() : base()
        {
            Init("Wax", "sc:wax_item", null, false);
        }
    }

    public class GlassItem : Item
    {
        public GlassItem() : base()
        {
            Init("Glass", "sc:glass_item", "sc:glass_block", true);
        }
    }

    public class SeedsItem : Item
    {
        public SeedsItem() : base()
        {
            Init("Seeds", "sc:seeds_item", "sc:wheat_crop_block", true);
        }
    }

    public class CarrotItem : FoodItem
    {
        public CarrotItem() : base()
        {
            Init("Carrot", "sc:carrot_item", "sc:carrot_crop_block", true);
            healAmount = 1.5;
        }
    }

    public class WheatItem : Item
    {
        public WheatItem() : base()
        {
            Init("Wheat", "sc:wheat_item", null, false);
        }
    }

    public class FarmlandItem : Item
    {
        public FarmlandItem() : base()
        {
            Init("Farmland", "sc:farmland_item", "sc:farmland_block", true);
        }
    }

    public class BerryItem : FoodItem
    {
        public BerryItem() : base()
        {
            Init("Berry", "sc:berry_item", "sc:berry_bush_crop_block", true);
            healAmount = 1;
        }
    }

    public class CottonItem : Item
    {
        public CottonItem() : base()
        {
            Init("Cotton", "sc:cotton_item", "sc:cotton_crop_block", true);
        }
    }

    public class SugarCaneItem : Item
    {
        public SugarCaneItem() : base()
        {
            Init("Sugar Cane", "sc:sugar_cane_item", "sc:sugar_cane_crop_block", true);
        }
    }

    public class PumpkinItem : Item
    {
        public PumpkinItem() : base()
        {
            Init("Pumpkin", "sc:pumpkin_item", "sc:pumpkin_block", true);
        }
    }

    public class WoolItem : Item
    {
        public WoolItem() : base()
        {
            Init("Wool", "sc:wool_item", "sc:wool_block", true);
        }
    }

    public class LanternItem : Item
    {
        public LanternItem() : base()
        {
            Init("Lantern", "sc:lantern_item", "sc:lantern_block", true);
        }
    }

    public class BucketRiceItem : FoodItem
    {
        public BucketRiceItem() : base()
        {
            Init("Rice Bucket", "sc:bucket_rice_item", "sc:rice_base", true);
            healAmount = 2;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            base.RightClickAction(block, invSlot);

            Player.Get().inventory.    Add("sc:bucket_empty_item", 1);
        }
    }

    public class TomatoItem : FoodItem
    {
        public TomatoItem() : base()
        {
            Init("Tomato", "sc:tomato_item", "sc:tomato_crop_block", true);
            healAmount = 1.5;
        }
    }

    public class PumpkinSeedsItem : Item
    {
        public PumpkinSeedsItem() : base()
        {
            Init("Pumpkin Seeds", "sc:pumpkin_seeds_item", "sc:pumpkin_crop_block", true);
        }
    }

    public class SeehundiumItem : Item
    {
        public SeehundiumItem() : base()
        {
            Init("Seehundium", "sc:seehundium_item", null, false);
        }
    }

    public class SaladItem : FoodItem
    {
        public SaladItem() : base()
        {
            Init("Salad", "sc:salad_item", null, false);
            healAmount = 5;
        }
    }

    public class PotatoItem : FoodItem
    {
        public PotatoItem() : base()
        {
            Init("Potato", "sc:potato_item", "sc:potato_crop_block", true);
            healAmount = 1;
        }
    }

    public class CucumberItem : FoodItem
    {
        public CucumberItem() : base()
        {
            Init("Cucumber", "sc:cucumber_item", "sc:cucumber_crop_block", true);
            healAmount = 1.5;
        }
    }

    public class CabbageItem : FoodItem
    {
        public CabbageItem() : base()
        {
            Init("Cabbage", "sc:cabbage_item", null, false);
            healAmount = 1.5;
        }
    }

    public class CabbageSeedsItem : Item
    {
        public CabbageSeedsItem() : base()
        {
            Init("Cabbage Seeds", "sc:cabbage_seeds_item", "sc:cabbage_crop_block", true);
        }
    }
}
