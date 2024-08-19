using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class OakPlanksItem_TopRight : ChiseledItem
    {
        public OakPlanksItem_TopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_topright_item";
            SetTexture();
            unchiselItems.Add(new OakPlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_TopRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            //Set the texture of the block on the canvas
            image = Images.OakPlanksBlock_TopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_TopLeft : ChiseledItem
    {
        public OakPlanksItem_TopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_topleft_item";
            SetTexture();
            unchiselItems.Add(new OakPlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_TopLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_TopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_BottomRight : ChiseledItem
    {
        public OakPlanksItem_BottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_bottomright_item";
            SetTexture();
            unchiselItems.Add(new OakPlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_BottomRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_BottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_BottomLeft : ChiseledItem
    {
        public OakPlanksItem_BottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_bottomleft_item";
            SetTexture();
            unchiselItems.Add(new OakPlanksItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_BottomLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_BottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_SlabRight : ChiseledItem
    {
        public OakPlanksItem_SlabRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slabright_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_SlabRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_SlabLeft : ChiseledItem
    {
        public OakPlanksItem_SlabLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slableft_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_SlabLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_SlabTop : ChiseledItem
    {
        public OakPlanksItem_SlabTop() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slabtop_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_SlabTop( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabTop.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_SlabBottom : ChiseledItem
    {
        public OakPlanksItem_SlabBottom() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slabbottom_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_SlabBottom( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabBottom.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_StairTopRight : ChiseledItem
    {
        public OakPlanksItem_StairTopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairtopright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_StairTopRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_StairTopRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_StairTopLeft : ChiseledItem
    {
        public OakPlanksItem_StairTopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairtopleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_StairTopLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_StairTopLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_StairBottomRight : ChiseledItem
    {
        public OakPlanksItem_StairBottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairbottomright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_StairBottomRight( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_StairBottomRight.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_StairBottomLeft : ChiseledItem
    {
        public OakPlanksItem_StairBottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairbottomleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_StairBottomLeft( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_StairBottomLeft.GetTexture();
            cvsItem.Background = image;
        }
    }

    public class OakPlanksItem_Center : ChiseledItem
    {
        public OakPlanksItem_Center() : base()
        {
            isChiseled = false;
            isPlacable = true;
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_center_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new OakPlanksBlock_Center( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.OakPlanksBlock_Center.GetTexture();
            cvsItem.Background = image;
        }
    }
}
