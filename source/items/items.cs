using System.Windows.Documents;

namespace SeeloewenCraft
{
    //-- Items --//

    public class GrassItem : Item
    {
        public GrassItem() : base()
        {
            Init("Grass Block", "sc:grass_block_item", "sc:grass_block", true, Images.GrassBlock);
        }
    }

    public class StoneItem : Item
    {
        public StoneItem() : base()
        {
            Init("Stone Block", "sc:stone_block_item", "sc:stone_block", true, Images.StoneBlock);
        }
    }

    public class DirtItem : Item
    {
        public DirtItem() : base()
        {
            Init("Dirt", "sc:dirt_item", "sc:dirt_block", true, Images.Dirt);
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem() : base()
        {
            Init("Coal Ore", "sc:coal_ore_item", "sc:coal_ore_block", true, Images.CoalOre);
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem() : base()
        {
            Init("Diamond Ore", "sc:diamond_ore_item", "sc:diamond_ore_block", true, Images.DiamondOre);
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem() : base()
        {
            Init("Iron Ore", "sc:iron_ore_item", "sc:iron_ore_block", true, Images.IronOre);
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem() : base()
        {
            Init("Oak Log", "sc:oak_log_item", "sc:oak_log_block", true, Images.OakLog);
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem() : base()
        {
            Init("Oak Leaves", "sc:oak_leaves_item", "sc:oak_leaves_block", true, Images.OakLeaves);
        }
    }

    public class SpruceLogItem : Item
    {
        public SpruceLogItem() : base()
        {
            Init("Spruce Log", "sc:spruce_log_item", "sc:spruce_log_block", true, Images.SpruceLog);
        }
    }

    public class SpruceLeavesItem : Item
    {
        public SpruceLeavesItem() : base()
        {
            Init("Spruce Leaves", "sc:spruce_leaves_item", "sc:spruce_leaves_block", true, Images.SpruceLeaves);
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem() : base()
        {
            Init("Bedrock", "sc:bedrock_item", "sc:bedrock_block", true, Images.Bedrock);
        }
    }

    public class AirItem : Item
    {
        public AirItem() : base()
        {
            Init("Air", "sc:air_item", "sc:air_block", true, Images.Air);
        }
    }

    public class ChestItem : Item
    {
        public ChestItem() : base()
        {
            Init("Chest", "sc:chest_item", "sc:chest_block", true, Images.Chest);
        }
    }

    public class MagmaBlockItem : Item
    {
        public MagmaBlockItem() : base()
        {
            Init("Magma Block", "sc:magma_block_item", "sc:magma_block", true, Images.MagmaBlock);
        }
    }

    public class StoneHammerItem : Item
    {
        public StoneHammerItem() : base()
        {
            Init("Stone Hammer", "sc:stone_hammer_item", null, false, Images.StoneHammer);
            hasRightClickAction = true;
            tags.Add("tools/hammer");
        }

        public override void RightClickAction(Block block, object sender)
        {
            if (block.isBackground && block.canBeMovedToBackground && block.IsInRange() && block.GetForegroundBlock() == null)
            {
                block.MoveToNormal();
            }
            else if (!block.isBackground && block.IsInRange() && block.canBeMovedToBackground)
            {
                block.MoveToBackground();
            }
        }
    }

    public class TorchItem : Item
    {
        public TorchItem() : base()
        {
            Init("Torch", "sc:torch_item", "sc:torch_block", true, Images.Torch);
        }
    }

    public class WaterItem : Item
    {
        public WaterItem() : base()
        {
            Init("Water", "sc:water_item", "sc:water_6_block", true, Images.Water_6);
        }
    }

    public class PottedCactusItem : Item
    {
        public PottedCactusItem() : base()
        {
            Init("Potted Cactus", "sc:potted_cactus_item", "sc:potted_cactus_base", true, Images.PottedCactus);
        }
    }

    public class CraftingTable : Item
    {
        public CraftingTable() : base()
        {
            Init("Crafting Table", "sc:crafting_table_item", "sc:crafting_table_block", true, Images.CraftingTable);
        }
    }

    public class CobbleStoneItem : Item
    {
        public CobbleStoneItem() : base()
        {
            Init("Cobblestone", "sc:cobblestone_item", "sc:cobblestone_block", true, Images.CobbleStoneBlock);
        }
    }

    public class ChiselerItem : Item
    {
        public ChiselerItem() : base()
        {
            Init("Chiseler", "sc:chiseler_item", "sc:chiseler_block", true, Images.Chiseler);
        }
    }

    public class UnchiselerItem : Item
    {
        public UnchiselerItem() : base()
        {
            Init("Unchiseler", "sc:unchiseler_item", "sc:unchiseler_block", true, Images.Unchiseler);
        }
    }

    public class BoneItem : Item
    {
        public BoneItem() : base()
        {
            Init("Bone", "sc:bone_item", null, true, Images.Bone);
        }
    }

    public class ArrowItem : Item
    {
        public ArrowItem() : base()
        {
            Init("Arrow", "sc:arrow_item", null, true, Images.Arrow);
        }
    }


    public class SnowballItem : Item
    {
        public SnowballItem() : base()
        {
            Init("Snowball", "sc:snowball_item", null, true, Images.Snowball);
        }
    }

    public class SpruceDoorItem : Item
    {
        public SpruceDoorItem() : base()
        {
            Init("Spruce Door", "sc:spruce_door_item", "sc:spruce_door_base", true, Images.SpruceDoor);
        }
    }

    public class OakPlanksItem : Item
    {
        public OakPlanksItem() : base()
        {
            Init("Oak Planks", "sc:oak_planks_item", "sc:oak_planks_block", true, Images.OakPlanks);
        }
    }

    public class SprucePlanksItem : Item
    {
        public SprucePlanksItem() : base()
        {
            Init("Spruce Planks", "sc:spruce_planks_item", "sc:spruce_planks_block", true, Images.SprucePlanks);
        }
    }

    public class SandStoneItem : Item
    {
        public SandStoneItem() : base()
        {
            Init("Sand Stone", "sc:sand_stone_item", "sc:sand_stone_block", true, Images.SandStone);
        }
    }
}
