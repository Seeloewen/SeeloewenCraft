using System;

namespace SeeloewenCraft
{
    //-- Items --//

    public class GrassItem : Item
    {
        public GrassItem() : base()
        {
            isPlacable = true;
            name = "Grass";
            id = "sc:grass_block_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new GrassBlock( isInBackground);
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
        public StoneItem() : base()
        {
            isPlacable = true;
            name = "Stone";
            id = "sc:stone_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new StoneBlock( isInBackground);
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
        public DirtItem() : base()
        {
            isPlacable = true;
            name = "Dirt";
            id = "sc:dirt_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new DirtBlock( isInBackground);
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
        public CoalOreItem() : base()
        {
            isPlacable = true;
            name = "Coal Ore";
            id = "sc:coal_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CoalOreBlock( isInBackground);
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
        public DiamondOreItem() : base()
        {
            isPlacable = true;
            name = "Diamond Ore";
            id = "sc:diamond_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new DiamondOreBlock( isInBackground);
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
        public IronOreItem() : base()
        {
            isPlacable = true;
            name = "Iron Ore";
            id = "sc:iron_ore_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new IronOreBlock( isInBackground);
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
        public OakLogItem() : base()
        {
            isPlacable = true;
            name = "Oak Log";
            id = "sc:oak_log_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakLogBlock( isInBackground);
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
        public OakLeavesItem() : base()
        {
            isPlacable = true;
            name = "Oak Leaves";
            id = "sc:oak_leaves_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakLeavesBlock( isInBackground);
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
        public SpruceLogItem() : base()
        {
            isPlacable = true;
            name = "Spruce Log";
            id = "sc:spruce_log_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SpruceLogBlock( isInBackground);
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
        public SpruceLeavesItem() : base()
        {
            isPlacable = true;
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SpruceLeavesBlock( isInBackground);
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
        public BedrockItem() : base()
        {
            isPlacable = true;
            name = "Bedrock";
            id = "sc:bedrock_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new BedrockBlock( isInBackground);
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
        public AirItem() : base()
        {
            isPlacable = true;
            name = "Air";
            id = "sc:air_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new AirBlock( isInBackground);
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
        public ChestItem() : base()
        {
            isPlacable = true;
            name = "Chest";
            id = "sc:chest_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new ChestBlock( isInBackground);
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
        public MagmaBlockItem() : base()
        {
            isPlacable = true;
            name = "Magma Block";
            id = "sc:magma_block_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new MagmaBlock( isInBackground);
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
        public StoneHammerItem() : base()
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
            isPlacable = true;
            name = "Torch";
            id = "sc:torch_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new TorchBlock( isInBackground);
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
        public WaterItem() : base()
        {
            isPlacable = true;
            name = "Water";
            id = "sc:water_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new WaterBlock_6( isInBackground);
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
        public PottedCactusItem() : base()
        {
            isPlacable = true;
            name = "Potted Cactus";
            id = "sc:potted_cactus_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new PottedCactus_Base( isInBackground);
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
        public CraftingTable() : base()
        {
            isPlacable = true;
            name = "Crafting Table";
            id = "sc:crafting_table_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CraftingTableBlock( isInBackground);
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
        public CobbleStoneItem() : base()
        {
            isPlacable = true;
            name = "Cobblestone";
            id = "sc:cobblestone_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock( isInBackground);
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
        public ChiselerItem() : base()
        {
            isPlacable = true;
            name = "Chiseler";
            id = "sc:chiseler_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new ChiselerBlock( isInBackground);
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
        public UnchiselerItem() : base()
        {
            isPlacable = true;
            name = "Unchiseler";
            id = "sc:unchiseler_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new UnchiselerBlock( isInBackground);
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
        public BoneItem() : base()
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
        public ArrowItem() : base()
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
        public SnowballItem() : base()
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


    public class SpruceDoorItem : Item
    {
        public SpruceDoorItem() : base()
        {
            isPlacable = true;
            name = "Spruce Door";
            id = "sc:spruce_door_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SpruceDoor_Base( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.SpruceDoor.GetTexture();
            cvsItem.Background = image;
        }
    }
}
