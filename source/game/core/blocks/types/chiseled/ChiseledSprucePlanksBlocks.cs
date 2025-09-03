namespace SeeloewenCraft.game.core.blocks
{
    public class SprucePlanksBlock_BottomLeft : Block
    {
        public SprucePlanksBlock_BottomLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_bottomleft", 500, "sc:spruce_planks_bottomleft_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 500, 500, 1000);
        }
    }

    public class SprucePlanksBlock_BottomRight : Block
    {
        public SprucePlanksBlock_BottomRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_bottomright", 500, "sc:spruce_planks_bottomright_item", Tool.Axe)
        {
            collision = new RectangleCollision(500, 1000, 500, 1000);
        }
    }

    public class SprucePlanksBlock_TopLeft : Block
    {
        public SprucePlanksBlock_TopLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_topleft", 500, "sc:spruce_planks_topleft_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 500, 0, 500);
        }
    }

    public class SprucePlanksBlock_TopRight : Block
    {
        public SprucePlanksBlock_TopRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_topright", 500, "sc:spruce_planks_topright_item", Tool.Axe)
        {
            collision = new RectangleCollision(500, 1000, 0, 500);
        }
    }

    public class SprucePlanksBlock_SlabRight : Block
    {
        public SprucePlanksBlock_SlabRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_slabright", 500, "sc:spruce_planks_slabright_item", Tool.Axe)
        {
            collision = new RectangleCollision(500, 1000, 0, 1000);
        }
    }

    public class SprucePlanksBlock_SlabLeft : Block
    {
        public SprucePlanksBlock_SlabLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_slableft", 500, "sc:spruce_planks_slableft_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 500, 0, 1000);
        }
    }

    public class SprucePlanksBlock_SlabTop : Block
    {
        public SprucePlanksBlock_SlabTop() : base("Chiseled Spruce Plank", "sc:spruce_planks_slabtop", 500, "sc:spruce_planks_slabtop_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 1000, 0, 500);
        }
    }

    public class SprucePlanksBlock_SlabBottom : Block
    {
        public SprucePlanksBlock_SlabBottom() : base("Chiseled Spruce Plank", "sc:spruce_planks_slabbottom", 500, "sc:spruce_planks_slabbottom_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 1000, 500, 1000);
        }
    }

    public class SprucePlanksBlock_StairTopRight : Block
    {
        public SprucePlanksBlock_StairTopRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairtopright", 500, "sc:spruce_planks_stairtopright_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class SprucePlanksBlock_StairTopLeft : Block
    {
        public SprucePlanksBlock_StairTopLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairtopleft", 500, "sc:spruce_planks_stairtopleft_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
        }
    }

    public class SprucePlanksBlock_StairBottomRight : Block
    {
        public SprucePlanksBlock_StairBottomRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairbottomright", 500, "sc:spruce_planks_stairbottomright_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class SprucePlanksBlock_StairBottomLeft : Block
    {
        public SprucePlanksBlock_StairBottomLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairbottomleft", 500, "sc:spruce_planks_stairbottomleft_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }
    }

    public class SprucePlanksBlock_Center : Block
    {
        public SprucePlanksBlock_Center() : base("Chiseled Spruce Plank", "sc:spruce_planks_center", 500, "sc:spruce_planks_center_item", Tool.Axe)
        {
            collision = new RectangleCollision(333, 666, 333, 666);
        }
    }
}
