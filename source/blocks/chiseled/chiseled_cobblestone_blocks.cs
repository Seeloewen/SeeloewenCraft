using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CobblestoneBlock_BottomLeft : Block
    {
        public CobblestoneBlock_BottomLeft(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomleft";
            breakTime = 1250;
            collision = new RectangleCollision(0, 500, 500, 1000);
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_BottomLeft(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_BottomLeft;
        }
    }

    public class CobblestoneBlock_BottomRight : Block
    {
        public CobblestoneBlock_BottomRight(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_bottomright";
            collision = new RectangleCollision(500, 1000, 500, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_BottomRight(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_BottomRight;
        }
    }

    public class CobbleStoneBlock_TopLeft : Block
    {
        public CobbleStoneBlock_TopLeft(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topleft";
            collision = new RectangleCollision(0, 500, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_TopLeft(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_TopLeft;
        }
    }

    public class CobblestoneBlock_TopRight : Block
    {
        public CobblestoneBlock_TopRight(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_topright";
            collision = new RectangleCollision(500, 1000, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_TopRight(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_TopRight;
        }
    }

    public class CobblestoneBlock_SlabRight : Block
    {
        public CobblestoneBlock_SlabRight(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabright";
            collision = new RectangleCollision(500, 1000, 0, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_SlabRight(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabRight;
        }
    }

    public class CobblestoneBlock_SlabLeft : Block
    {
        public CobblestoneBlock_SlabLeft(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slableft";
            collision = new RectangleCollision(0, 500, 0, 1000);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_SlabLeft(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabLeft;
        }
    }

    public class CobblestoneBlock_SlabTop : Block
    {
        public CobblestoneBlock_SlabTop(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabtop";
            collision = new RectangleCollision(0, 1000, 0, 500);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_SlabTop(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabTop;
        }
    }

    public class CobblestoneBlock_SlabBottom : Block
    {
        public CobblestoneBlock_SlabBottom(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_slabbottom";
            collision = new RectangleCollision(0,1000,500,1000);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_SlabBottom(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_SlabBottom;
        }
    }

    public class CobblestoneBlock_StairTopRight : Block
    {
        public CobblestoneBlock_StairTopRight(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopright";
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_StairTopRight(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairTopRight;
        }
    }

    public class CobblestoneBlock_StairTopLeft : Block
    {
        public CobblestoneBlock_StairTopLeft(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairtopleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_StairTopLeft(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairTopLeft;
        }
    }

    public class CobblestoneBlock_StairBottomRight : Block
    {
        public CobblestoneBlock_StairBottomRight(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomright";
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            breakTime = 1250;
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_StairBottomRight(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairBottomRight;
        }
    }

    public class CobblestoneBlock_StairBottomLeft : Block
    {
        public CobblestoneBlock_StairBottomLeft(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_stairbottomleft";
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_StairBottomLeft(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_StairBottomLeft;
        }
    }

    public class CobbleStoneBlock_Center: Block
    {
        public CobbleStoneBlock_Center(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Chiseled Cobblestone";
            id = "sc:cobblestone_center";
            collision = new RectangleCollision(333, 666, 333, 666);
            breakTime = 1250;

        }

        override public void GenerateItem(World world)
        {
            item = new CobbleStoneItem_Center(world);
        }

        public override void SetTexture()
        {
            image = Images.CobbleStoneBlock_Center;
        }
    }
}
