using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CobbleStoneItem_TopRight : ChiseledItem
    {
        public CobbleStoneItem_TopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topright_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_TopRight( isInBackground);
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
        public CobbleStoneItem_TopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topleft_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_TopLeft( isInBackground);
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
        public CobbleStoneItem_BottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomright_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_BottomRight( isInBackground);
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
        public CobbleStoneItem_BottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomleft_item";
            SetTexture();
            unchiselItems.Add(new CobbleStoneItem_Center());
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_BottomLeft( isInBackground);
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
        public CobbleStoneItem_SlabRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabright_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabRight( isInBackground);
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
        public CobbleStoneItem_SlabLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slableft_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabLeft( isInBackground);
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
        public CobbleStoneItem_SlabTop() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabtop_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabTop( isInBackground);
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
        public CobbleStoneItem_SlabBottom() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabbottom_item";
            SetTexture();

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_SlabBottom( isInBackground);
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
        public CobbleStoneItem_StairTopRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairTopRight( isInBackground);
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
        public CobbleStoneItem_StairTopLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairTopLeft( isInBackground);
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
        public CobbleStoneItem_StairBottomRight() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomright_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairBottomRight( isInBackground);
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
        public CobbleStoneItem_StairBottomLeft() : base()
        {
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomleft_item";
            SetTexture();

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobblestoneBlock_StairBottomLeft( isInBackground);
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
        public CobbleStoneItem_Center() : base()
        {
            isChiseled = false;
            isPlacable = true;
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_center_item";
            SetTexture();
        }

        override public Block GenerateBlock(bool isInBackground)
        {
            block = new CobbleStoneBlock_Center( isInBackground);
            return block;
        }

        override public void SetTexture()
        {
            image = Images.CobbleStoneBlock_Center.GetTexture();
            cvsItem.Background = image;
        }
    }
}
