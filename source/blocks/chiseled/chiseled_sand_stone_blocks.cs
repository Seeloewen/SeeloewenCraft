using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class SandStoneBlock_BottomLeft : Block
    {
        public SandStoneBlock_BottomLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_bottomleft";
            breakTime = 1250;
            collision = new RectangleCollision(0, 500, 500, 1000);
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_BottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_BottomLeft.GetTexture();
        }
    }

    public class SandStoneBlock_BottomRight : Block
    {
        public SandStoneBlock_BottomRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_bottomright";
            collision = new RectangleCollision(500, 1000, 500, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_BottomRight();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_BottomRight.GetTexture();
        }
    }

    public class SandStoneBlock_TopLeft : Block
    {
        public SandStoneBlock_TopLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_topleft";
            collision = new RectangleCollision(0, 500, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_TopLeft();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_TopLeft.GetTexture();
        }
    }

    public class SandStoneBlock_TopRight : Block
    {
        public SandStoneBlock_TopRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_topright";
            collision = new RectangleCollision(500, 1000, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_TopRight();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_TopRight.GetTexture();
        }
    }

    public class SandStoneBlock_SlabRight : Block
    {
        public SandStoneBlock_SlabRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slabright";
            collision = new RectangleCollision(500, 1000, 0, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_SlabRight();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_SlabRight.GetTexture();
        }
    }

    public class SandStoneBlock_SlabLeft : Block
    {
        public SandStoneBlock_SlabLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slableft";
            collision = new RectangleCollision(0, 500, 0, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_SlabLeft();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_SlabLeft.GetTexture();
        }
    }

    public class SandStoneBlock_SlabTop : Block
    {
        public SandStoneBlock_SlabTop( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slabtop";
            collision = new RectangleCollision(0, 1000, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_SlabTop();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_SlabTop.GetTexture();
        }
    }

    public class SandStoneBlock_SlabBottom : Block
    {
        public SandStoneBlock_SlabBottom( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_slabbottom";
            collision = new RectangleCollision(0,1000,500,1000);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_SlabBottom();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_SlabBottom.GetTexture();
        }
    }

    public class SandStoneBlock_StairTopRight : Block
    {
        public SandStoneBlock_StairTopRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairtopright";
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_StairTopRight();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_StairTopRight.GetTexture();
        }
    }

    public class SandStoneBlock_StairTopLeft : Block
    {
        public SandStoneBlock_StairTopLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairtopleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_StairTopLeft();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_StairTopLeft.GetTexture();
        }
    }

    public class SandStoneBlock_StairBottomRight : Block
    {
        public SandStoneBlock_StairBottomRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairbottomright";
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 1250;
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_StairBottomRight();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_StairBottomRight.GetTexture();
        }
    }

    public class SandStoneBlock_StairBottomLeft : Block
    {
        public SandStoneBlock_StairBottomLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_stairbottomleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_StairBottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_StairBottomLeft.GetTexture();
        }
    }

    public class SandStoneBlock_Center: Block
    {
        public SandStoneBlock_Center( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Sand Stone";
            id = "sc:sand_stone_center";
            collision = new RectangleCollision(333, 666, 333, 666);
            breakTime = 1250;

        }

        override public void GenerateItem()
        {
            item = new SandStoneItem_Center();
        }

        public override void SetTexture()
        {
            image = Images.SandStoneBlock_Center.GetTexture();
        }
    }
}
