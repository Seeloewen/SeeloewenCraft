using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class SandStoneItem_TopRight : ChiseledItem
    {
        public SandStoneItem_TopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_topright_item";
            SetTexture();
            unchiselItems.Add(new SandStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_TopRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.SandStoneBlock_TopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_TopLeft : ChiseledItem
    {
        public SandStoneItem_TopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_topleft_item";
            SetTexture();
            unchiselItems.Add(new SandStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_TopLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_TopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_BottomRight : ChiseledItem
    {
        public SandStoneItem_BottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_bottomright_item";
            SetTexture();
            unchiselItems.Add(new SandStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_BottomRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_BottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_BottomLeft : ChiseledItem
    {
        public SandStoneItem_BottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_bottomleft_item";
            SetTexture();
            unchiselItems.Add(new SandStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_BottomLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_BottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_SlabRight : ChiseledItem
    {
        public SandStoneItem_SlabRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slabright_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_SlabRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_SlabRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_SlabLeft : ChiseledItem
    {
        public SandStoneItem_SlabLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slableft_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_SlabLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_SlabLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_SlabTop : ChiseledItem
    {
        public SandStoneItem_SlabTop() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slabtop_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_SlabTop( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_SlabTop.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_SlabBottom : ChiseledItem
    {
        public SandStoneItem_SlabBottom() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slabbottom_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_SlabBottom( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_SlabBottom.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_StairTopRight : ChiseledItem
    {
        public SandStoneItem_StairTopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairtopright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_StairTopRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_StairTopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_StairTopLeft : ChiseledItem
    {
        public SandStoneItem_StairTopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairtopleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_StairTopLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_StairTopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_StairBottomRight : ChiseledItem
    {
        public SandStoneItem_StairBottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairbottomright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_StairBottomRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_StairBottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_StairBottomLeft : ChiseledItem
    {
        public SandStoneItem_StairBottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairbottomleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_StairBottomLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_StairBottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SandStoneItem_Center : ChiseledItem
    {
        public SandStoneItem_Center() : base()
        {
            isChiseled = false;
            isPlacable = true;
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_center_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SandStoneBlock_Center( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SandStoneBlock_Center.GetTexture();
            cvsItem.Background = image;
        }
    }
}
