using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            image = world.images.GrassBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.StoneBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.DirtBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.CoalOreBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.DiamondOreBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.IronOreBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.OakLogBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.OakLeavesBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.SpruceLogBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.SpruceLeavesBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.BedrockBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.AirBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class ChestItem : Item
    {
        public ChestItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Chest";
            id = "chest_item";
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
            image = world.images.ChestBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
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
            image = world.images.MagmaBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class HammerItem : Item
    {
        public HammerItem(World world) : base(world)
        {
            isPlacable = false;
            hasRightClickAction = true;
            name = "Hammer";
            id = "sc:hammer_item";
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
            image = world.images.Hammer;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
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
            canBeForeground = true;
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
            image = world.images.Torch;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterItem : Item
    {
        public WaterItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Water";
            id = "sc:water_item";
            canBeForeground = false;
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
            image = world.images.Water_6;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class Plant2Item : Item
    {
        public Plant2Item(World world) : base(world)
        {
            canBeForeground = true;
            isPlacable = true;
            name = "Plant2";
            id = "sc:plant_2_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new Plant2Block_Base(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = world.images.Plant2;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class AlphaCrafterItem : Item
    {
        public AlphaCrafterItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Alpha Crafter";
            id = "sc:alpha_crafter_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new AlphaCrafterBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = world.images.AlphaCrafter;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class QuarterOakPlankItem : Item
    {
        public QuarterOakPlankItem(World world) : base(world)
        {
            isPlacable = true;
            name = "Quarter Oak Plank";
            id = "sc:quarter_oak_plank_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new QuarterOakPlankBlock(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = world.images.QuarterOakPlankBlock;
            cvsItem.Background = image;
        }

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }
    }
}
