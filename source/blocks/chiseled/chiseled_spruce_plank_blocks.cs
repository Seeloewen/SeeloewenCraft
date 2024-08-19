using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class SprucePlanksBlock_BottomLeft : Block
    {
        public SprucePlanksBlock_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_bottomleft";
            breakTime = 500;
            collision = new RectangleCollision(0, 500, 500, 1000);
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_BottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_BottomLeft.GetTexture();
        }
    }

    public class SprucePlanksBlock_BottomRight : Block
    {
        public SprucePlanksBlock_BottomRight(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_bottomright";
            collision = new RectangleCollision(500, 1000, 500, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_BottomRight();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_BottomRight.GetTexture();
        }
    }

    public class SprucePlanksBlock_TopLeft : Block
    {
        public SprucePlanksBlock_TopLeft(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_topleft";
            collision = new RectangleCollision(0, 500, 0, 500);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_TopLeft();
        }

        public override void SetTexture()
        { 
            image = Images.SprucePlanksBlock_TopLeft.GetTexture();
        }
    }

    public class SprucePlanksBlock_TopRight : Block
    {
        public SprucePlanksBlock_TopRight(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_topright";
            collision = new RectangleCollision(500, 1000, 0, 500);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_TopRight();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_TopRight.GetTexture();
        }
    }

    public class SprucePlanksBlock_SlabRight : Block
    {
        public SprucePlanksBlock_SlabRight(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_slabright";
            collision = new RectangleCollision(500, 1000, 0, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_SlabRight();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabRight.GetTexture();
        }
    }

    public class SprucePlanksBlock_SlabLeft : Block
    {
        public SprucePlanksBlock_SlabLeft(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_slableft";
            collision = new RectangleCollision(0, 500, 0, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_SlabLeft();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabLeft.GetTexture();
        }
    }

    public class SprucePlanksBlock_SlabTop : Block
    {
        public SprucePlanksBlock_SlabTop(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_slabtop";
            collision = new RectangleCollision(0, 1000, 0, 500);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_SlabTop();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabTop.GetTexture();
        }
    }

    public class SprucePlanksBlock_SlabBottom : Block
    {
        public SprucePlanksBlock_SlabBottom(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_slabbottom";
            collision = new RectangleCollision(0, 1000, 500, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_SlabBottom();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_SlabBottom.GetTexture();
        }
    }

    public class SprucePlanksBlock_StairTopRight : Block
    {
        public SprucePlanksBlock_StairTopRight(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_stairtopright";
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_StairTopRight();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairTopRight.GetTexture();
        }
    }

    public class SprucePlanksBlock_StairTopLeft : Block
    {
        public SprucePlanksBlock_StairTopLeft(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_stairtopleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_StairTopLeft();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairTopLeft.GetTexture();
        }
    }

    public class SprucePlanksBlock_StairBottomRight : Block
    {
        public SprucePlanksBlock_StairBottomRight(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_stairbottomright";
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_StairBottomRight();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairBottomRight.GetTexture();
        }
    }

    public class SprucePlanksBlock_StairBottomLeft : Block
    {
        public SprucePlanksBlock_StairBottomLeft(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_stairbottomleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_StairBottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_StairBottomLeft.GetTexture();
        }
    }

    public class SprucePlanksBlock_Center : Block
    {
        public SprucePlanksBlock_Center(bool isInBackground) : base(isInBackground)
        {
            SetTexture();
            name = "Chiseled Spruce Plank";
            id = "sc:spruce_planks_center";
            collision = new RectangleCollision(333, 666, 333, 666);
            breakTime = 500;

        }

        override public void GenerateItem()
        {
            item = new SprucePlanksItem_Center();
        }

        public override void SetTexture()
        {
            image = Images.SprucePlanksBlock_Center.GetTexture();
        }
    }
}
