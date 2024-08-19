using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class SprucePlanksItem_TopRight : ChiseledItem
    {
        public SprucePlanksItem_TopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_topright_item";
            SetTexture();
            unchiselItems.Add(new SprucePlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_TopRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.SprucePlanksBlock_TopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_TopLeft : ChiseledItem
    {
        public SprucePlanksItem_TopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_topleft_item";
            SetTexture();
            unchiselItems.Add(new SprucePlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_TopLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_TopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_BottomRight : ChiseledItem
    {
        public SprucePlanksItem_BottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_bottomright_item";
            SetTexture();
            unchiselItems.Add(new SprucePlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_BottomRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_BottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_BottomLeft : ChiseledItem
    {
        public SprucePlanksItem_BottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_bottomleft_item";
            SetTexture();
            unchiselItems.Add(new SprucePlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_BottomLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_BottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_SlabRight : ChiseledItem
    {
        public SprucePlanksItem_SlabRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_slabright_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_SlabRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_SlabLeft : ChiseledItem
    {
        public SprucePlanksItem_SlabLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_slableft_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_SlabLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_SlabTop : ChiseledItem
    {
        public SprucePlanksItem_SlabTop() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_slabtop_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_SlabTop( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabTop.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_SlabBottom : ChiseledItem
    {
        public SprucePlanksItem_SlabBottom() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_slabbottom_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_SlabBottom( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabBottom.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_StairTopRight : ChiseledItem
    {
        public SprucePlanksItem_StairTopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_stairtopright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_StairTopRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairTopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_StairTopLeft : ChiseledItem
    {
        public SprucePlanksItem_StairTopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_stairtopleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_StairTopLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairTopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_StairBottomRight : ChiseledItem
    {
        public SprucePlanksItem_StairBottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_stairbottomright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_StairBottomRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairBottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_StairBottomLeft : ChiseledItem
    {
        public SprucePlanksItem_StairBottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_stairbottomleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new SprucePlanksBlock_StairBottomLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairBottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class SprucePlanksItem_Center : ChiseledItem
    {
        public SprucePlanksItem_Center() : base()
        {
            isChiseled = false;
            isPlacable = true;
            name = "Chiseled Spruce Planks";
            id = "sc:spruce_plankss_center_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_Center( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.SprucePlanksBlock_Center.GetTexture();
            cvsItem.Background = image;
        }
    }
}
