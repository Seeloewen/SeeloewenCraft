using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CobbleStoneItem_TopRight : ChiseledItem
    {
        public CobbleStoneItem_TopRight(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topright_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center(world));
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_TopRight(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.CobbleStoneBlock_TopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_TopLeft : ChiseledItem
    {
        public CobbleStoneItem_TopLeft(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topleft_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center(world));
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_TopLeft(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_TopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_BottomRight : ChiseledItem
    {
        public CobbleStoneItem_BottomRight(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomright_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center(world));
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_BottomRight(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_BottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_BottomLeft : ChiseledItem
    {
        public CobbleStoneItem_BottomLeft(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomleft_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center(world));
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_BottomLeft(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_BottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_SlabRight : ChiseledItem
    {
        public CobbleStoneItem_SlabRight(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabright_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabRight(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_SlabLeft : ChiseledItem
    {
        public CobbleStoneItem_SlabLeft(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slableft_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabLeft(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_SlabTop : ChiseledItem
    {
        public CobbleStoneItem_SlabTop(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabtop_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabTop(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabTop.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_SlabBottom : ChiseledItem
    {
        public CobbleStoneItem_SlabBottom(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabbottom_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabBottom(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabBottom.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_StairTopRight : ChiseledItem
    {
        public CobbleStoneItem_StairTopRight(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairTopRight(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairTopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_StairTopLeft : ChiseledItem
    {
        public CobbleStoneItem_StairTopLeft(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairTopLeft(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairTopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_StairBottomRight : ChiseledItem
    {
        public CobbleStoneItem_StairBottomRight(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairBottomRight(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairBottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_StairBottomLeft : ChiseledItem
    {
        public CobbleStoneItem_StairBottomLeft(World world) : base(world)
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center(world));
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairBottomLeft(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairBottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class CobbleStoneItem_Center : ChiseledItem
    {
        public CobbleStoneItem_Center(World world) : base(world)
        {
            isChiseled = false;
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_center_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_Center(world, isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_Center.GetTexture();
            cvsItem.Background = image;
        }
    }
}
