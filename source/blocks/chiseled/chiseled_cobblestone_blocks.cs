using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CobblestoneBlock_BottomLeft : Block
    {
        public CobblestoneBlock_BottomLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomleft";
            breakTime = 1250;
            collision = new RectangleCollision(0, 500, 500, 1000);
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_BottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_BottomLeft.GetTexture();
        }
    }

    public class CobblestoneBlock_BottomRight : Block
    {
        public CobblestoneBlock_BottomRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomright";
            collision = new RectangleCollision(500, 1000, 500, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_BottomRight();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_BottomRight.GetTexture();
        }
    }

    public class CobbleStoneBlock_TopLeft : Block
    {
        public CobbleStoneBlock_TopLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topleft";
            collision = new RectangleCollision(0, 500, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_TopLeft();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_TopLeft.GetTexture();
        }
    }

    public class CobblestoneBlock_TopRight : Block
    {
        public CobblestoneBlock_TopRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topright";
            collision = new RectangleCollision(500, 1000, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_TopRight();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_TopRight.GetTexture();
        }
    }

    public class CobblestoneBlock_SlabRight : Block
    {
        public CobblestoneBlock_SlabRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabright";
            collision = new RectangleCollision(500, 1000, 0, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_SlabRight();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabRight.GetTexture();
        }
    }

    public class CobblestoneBlock_SlabLeft : Block
    {
        public CobblestoneBlock_SlabLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slableft";
            collision = new RectangleCollision(0, 500, 0, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_SlabLeft();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabLeft.GetTexture();
        }
    }

    public class CobblestoneBlock_SlabTop : Block
    {
        public CobblestoneBlock_SlabTop( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabtop";
            collision = new RectangleCollision(0, 1000, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_SlabTop();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabTop.GetTexture();
        }
    }

    public class CobblestoneBlock_SlabBottom : Block
    {
        public CobblestoneBlock_SlabBottom( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabbottom";
            collision = new RectangleCollision(0,1000,500,1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_SlabBottom();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabBottom.GetTexture();
        }
    }

    public class CobblestoneBlock_StairTopRight : Block
    {
        public CobblestoneBlock_StairTopRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopright";
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_StairTopRight();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairTopRight.GetTexture();
        }
    }

    public class CobblestoneBlock_StairTopLeft : Block
    {
        public CobblestoneBlock_StairTopLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_StairTopLeft();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairTopLeft.GetTexture();
        }
    }

    public class CobblestoneBlock_StairBottomRight : Block
    {
        public CobblestoneBlock_StairBottomRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomright";
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_StairBottomRight();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairBottomRight.GetTexture();
        }
    }

    public class CobblestoneBlock_StairBottomLeft : Block
    {
        public CobblestoneBlock_StairBottomLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_StairBottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairBottomLeft.GetTexture();
        }
    }

    public class CobbleStoneBlock_Center: Block
    {
        public CobbleStoneBlock_Center( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_center";
            collision = new RectangleCollision(333, 666, 333, 666);
            breakTime = 1250;

        }

        override public void GenerateItem()
        {
            item = new CobbleStoneItem_Center();
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_Center.GetTexture();
        }
    }
}
