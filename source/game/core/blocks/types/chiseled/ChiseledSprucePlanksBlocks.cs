namespace SeeloewenCraft.game.core.blocks
{
    public class SprucePlanksBlock_BottomLeft : Block
    {
        public SprucePlanksBlock_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_bottomleft", 500, "sc:spruce_planks_bottomleft_item", Tool.Axe);
            collision = new RectangleCollision(0, 500, 500, 1000);
        }
    }

    public class SprucePlanksBlock_BottomRight : Block
    {
        public SprucePlanksBlock_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_bottomright", 500, "sc:spruce_planks_bottomright_item", Tool.Axe);
            collision = new RectangleCollision(500, 1000, 500, 1000);
        }
    }

    public class SprucePlanksBlock_TopLeft : Block
    {
        public SprucePlanksBlock_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_topleft", 500, "sc:spruce_planks_topleft_item", Tool.Axe);
            collision = new RectangleCollision(0, 500, 0, 500);
        }
    }

    public class SprucePlanksBlock_TopRight : Block
    {
        public SprucePlanksBlock_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_topright", 500, "sc:spruce_planks_topright_item", Tool.Axe);
            collision = new RectangleCollision(500, 1000, 0, 500);
        }
    }

    public class SprucePlanksBlock_SlabRight : Block
    {
        public SprucePlanksBlock_SlabRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_slabright", 500, "sc:spruce_planks_slabright_item", Tool.Axe);
            collision = new RectangleCollision(500, 1000, 0, 1000);
        }
    }

    public class SprucePlanksBlock_SlabLeft : Block
    {
        public SprucePlanksBlock_SlabLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_slableft", 500, "sc:spruce_planks_slableft_item", Tool.Axe);
            collision = new RectangleCollision(0, 500, 0, 1000);
        }
    }

    public class SprucePlanksBlock_SlabTop : Block
    {
        public SprucePlanksBlock_SlabTop(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_slabtop", 500, "sc:spruce_planks_slabtop_item", Tool.Axe);
            collision = new RectangleCollision(0, 1000, 0, 500);
        }
    }

    public class SprucePlanksBlock_SlabBottom : Block
    {
        public SprucePlanksBlock_SlabBottom(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_slabbottom", 500, "sc:spruce_planks_slabbottom_item", Tool.Axe);
            collision = new RectangleCollision(0, 1000, 500, 1000);
        }
    }

    public class SprucePlanksBlock_StairTopRight : Block
    {
        public SprucePlanksBlock_StairTopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_stairtopright", 500, "sc:spruce_planks_stairtopright_item", Tool.Axe);
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class SprucePlanksBlock_StairTopLeft : Block
    {
        public SprucePlanksBlock_StairTopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_stairtopleft", 500, "sc:spruce_planks_stairtopleft_item", Tool.Axe);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
        }
    }

    public class SprucePlanksBlock_StairBottomRight : Block
    {
        public SprucePlanksBlock_StairBottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_stairbottomright", 500, "sc:spruce_planks_stairbottomright_item", Tool.Axe);
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class SprucePlanksBlock_StairBottomLeft : Block
    {
        public SprucePlanksBlock_StairBottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_stairbottomleft", 500, "sc:spruce_planks_stairbottomleft_item", Tool.Axe);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }
    }

    public class SprucePlanksBlock_Center : Block
    {
        public SprucePlanksBlock_Center(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Spruce Plank", "sc:spruce_planks_center", 500, "sc:spruce_planks_center_item", Tool.Axe);
            collision = new RectangleCollision(333, 666, 333, 666);
        }
    }
}
