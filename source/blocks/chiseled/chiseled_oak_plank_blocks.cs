using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class OakPlanksBlock_BottomLeft : Block
    {
        public OakPlanksBlock_BottomLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_bottomleft";
            breakTime = 500;
            collision = new RectangleCollision(0, 500, 500, 1000);
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_BottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_BottomLeft.GetTexture();
        }
    }

    public class OakPlanksBlock_BottomRight : Block
    {
        public OakPlanksBlock_BottomRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_bottomright";
            collision = new RectangleCollision(500, 1000, 500, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_BottomRight();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_BottomRight.GetTexture();
        }
    }

    public class OakPlanksBlock_TopLeft : Block
    {
        public OakPlanksBlock_TopLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_topleft";
            collision = new RectangleCollision(0, 500, 0, 500);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_TopLeft();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_TopLeft.GetTexture();
        }
    }

    public class OakPlanksBlock_TopRight : Block
    {
        public OakPlanksBlock_TopRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_topright";
            collision = new RectangleCollision(500, 1000, 0, 500);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_TopRight();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_TopRight.GetTexture();
        }
    }

    public class OakPlanksBlock_SlabRight : Block
    {
        public OakPlanksBlock_SlabRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slabright";
            collision = new RectangleCollision(500, 1000, 0, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_SlabRight();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabRight.GetTexture();
        }
    }

    public class OakPlanksBlock_SlabLeft : Block
    {
        public OakPlanksBlock_SlabLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slableft";
            collision = new RectangleCollision(0, 500, 0, 1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_SlabLeft();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabLeft.GetTexture();
        }
    }

    public class OakPlanksBlock_SlabTop : Block
    {
        public OakPlanksBlock_SlabTop( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slabtop";
            collision = new RectangleCollision(0, 1000, 0, 500);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_SlabTop();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabTop.GetTexture();
        }
    }

    public class OakPlanksBlock_SlabBottom : Block
    {
        public OakPlanksBlock_SlabBottom( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_slabbottom";
            collision = new RectangleCollision(0,1000,500,1000);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_SlabBottom();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_SlabBottom.GetTexture();
        }
    }

    public class OakPlanksBlock_StairTopRight : Block
    {
        public OakPlanksBlock_StairTopRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairtopright";
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_StairTopRight();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_StairTopRight.GetTexture();
        }
    }

    public class OakPlanksBlock_StairTopLeft : Block
    {
        public OakPlanksBlock_StairTopLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairtopleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_StairTopLeft();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_StairTopLeft.GetTexture();
        }
    }

    public class OakPlanksBlock_StairBottomRight : Block
    {
        public OakPlanksBlock_StairBottomRight( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairbottomright";
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 500;
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_StairBottomRight();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_StairBottomRight.GetTexture();
        }
    }

    public class OakPlanksBlock_StairBottomLeft : Block
    {
        public OakPlanksBlock_StairBottomLeft( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_stairbottomleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_StairBottomLeft();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_StairBottomLeft.GetTexture();
        }
    }

    public class OakPlanksBlock_Center: Block
    {
        public OakPlanksBlock_Center( bool isInBackground) : base( isInBackground)
        {
            SetTexture();
            name = "Chiseled Oak Plank";
            id = "sc:oak_planks_center";
            collision = new RectangleCollision(333, 666, 333, 666);
            breakTime = 500;

        }

        override public void GenerateItem()
        {
            item = new OakPlanksItem_Center();
        }

        public override void SetTexture()
        {
            image = Images.OakPlanksBlock_Center.GetTexture();
        }
    }
}
