using System;

namespace SeeloewenCraft
{
    //-- Items --//

    public class GrassItem : Item
    {
        public GrassItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Grass";
            id = "sc:grass_block_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new GrassBlock(world, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.GrassBlock.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class StoneItem : Item
    {
        public StoneItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Stone";
            id = "sc:stone_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new StoneBlock(world, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.StoneBlock.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class DirtItem : Item
    {
        public DirtItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Dirt";
            id = "sc:dirt_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new DirtBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Dirt.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CoalOreItem : Item
    {
        public CoalOreItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Coal Ore";
            id = "sc:coal_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CoalOreBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.CoalOre.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class DiamondOreItem : Item
    {
        public DiamondOreItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Diamond Ore";
            id = "sc:diamond_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new DiamondOreBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.DiamondOre.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class IronOreItem : Item
    {
        public IronOreItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Iron Ore";
            id = "sc:iron_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new IronOreBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.IronOre.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakLogItem : Item
    {
        public OakLogItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Oak Log";
            id = "sc:oak_log_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakLogBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.OakLog.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakLeavesItem : Item
    {
        public OakLeavesItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Oak Leaves";
            id = "sc:oak_leaves_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakLeavesBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.OakLeaves.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SpruceLogItem : Item
    {
        public SpruceLogItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Spruce Log";
            id = "sc:spruce_log_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SpruceLogBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.SpruceLog.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SpruceLeavesItem : Item
    {
        public SpruceLeavesItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SpruceLeavesBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.SpruceLeaves.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class BedrockItem : Item
    {
        public BedrockItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Bedrock";
            id = "sc:bedrock_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new BedrockBlock(world, isInBackground);
            return block;
        }
        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Bedrock.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class AirItem : Item
    {
        public AirItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Air";
            id = "sc:air_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new AirBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Air.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class ChestItem : Item
    {
        public ChestItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Chest";
            id = "sc:chest_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new ChestBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Chest.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class MagmaBlockItem : Item
    {
        public MagmaBlockItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Magma Block";
            id = "sc:magma_block_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new MagmaBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.MagmaBlock.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class StoneHammerItem : Item
    {
        public StoneHammerItem(World world) : base(world)
        {
            isPlacable = false;
            hasRightClickAction = true;
            name = "Stone Hammer";
            id = "sc:stone_hammer_item";
            SetTexture();
            tags.Add("tools/hammer");
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            return null;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.StoneHammer.GetTexture();
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            //TO-DO: Implement timer when moving blocks to background

            if (block.isBackground && block.canBeMovedToBackground && block.IsInRange() && block.foregroundBlock == null)
            {
                block.MoveToForeground();
            }
            else if (!block.isBackground && block.IsInRange() && block.canBeMovedToBackground)
            {
                block.MoveToBackground();
            }
        }
    }

    public class TorchItem : Item
    {
        public TorchItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Torch";
            id = "sc:torch_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new TorchBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Torch.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class WaterItem : Item
    {
        public WaterItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Water";
            id = "sc:water_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new WaterBlock_6(world, isInBackground);
            block.isWaterSource = true;
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Water_6.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class PottedCactusItem : Item
    {
        public PottedCactusItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Potted Cactus";
            id = "sc:potted_cactus_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new PottedCactus_Base(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.PottedCactus.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CraftingTable : Item
    {
        public CraftingTable(World world) : base(world)
        {
            isPlacable = true;
            name = "Crafting Table";
            id = "sc:crafting_table_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CraftingTableBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.CraftingTable.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem : Item
    {
        public CobbleStoneItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Cobblestone";
            id = "sc:cobblestone_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.CobbleStoneBlock.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class ChiselerItem : Item
    {
        public ChiselerItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseler";
            id = "sc:chiseler_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new ChiselerBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Chiseler.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class UnchiselerItem : Item
    {
        public UnchiselerItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Unchiseler";
            id = "sc:unchiseler_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new UnchiselerBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Unchiseler.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class BoneItem : Item
    {
        public BoneItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Bone";
            id = "sc:bone_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            return null;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Bone.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class ArrowItem : Item
    {
        public ArrowItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Arrow";
            id = "sc:arrow_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            return null;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Arrow.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SnowballItem : Item
    {
        public SnowballItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Snowball";
            id = "sc:snowball_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            return null;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.Snowball.GetTexture()    ;
            cvsItem.Background = image;
        }
    }
}
