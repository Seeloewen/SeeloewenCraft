namespace SeeloewenCraft.game.core.blocks
{
    public class SprucePlanksBlock_BottomLeft : Block
    {
        internal SprucePlanksBlock_BottomLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_bottomleft", 500, "sc:spruce_planks_bottomleft_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 500, 500, 1000);
        }
    }

    public class SprucePlanksBlock_BottomRight : Block
    {
        internal SprucePlanksBlock_BottomRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_bottomright", 500, "sc:spruce_planks_bottomright_item", Tool.Axe)
        {
            collision = new RectangleCollision(500, 1000, 500, 1000);
        }
    }

    public class SprucePlanksBlock_TopLeft : Block
    {
        internal SprucePlanksBlock_TopLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_topleft", 500, "sc:spruce_planks_topleft_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 500, 0, 500);
        }
    }

    public class SprucePlanksBlock_TopRight : Block
    {
        internal SprucePlanksBlock_TopRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_topright", 500, "sc:spruce_planks_topright_item", Tool.Axe)
        {
            collision = new RectangleCollision(500, 1000, 0, 500);
        }
    }

    public class SprucePlanksBlock_SlabRight : Block
    {
        internal SprucePlanksBlock_SlabRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_slabright", 500, "sc:spruce_planks_slabright_item", Tool.Axe)
        {
            collision = new RectangleCollision(500, 1000, 0, 1000);
        }
    }

    public class SprucePlanksBlock_SlabLeft : Block
    {
        internal SprucePlanksBlock_SlabLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_slableft", 500, "sc:spruce_planks_slableft_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 500, 0, 1000);
        }
    }

    public class SprucePlanksBlock_SlabTop : Block
    {
        internal SprucePlanksBlock_SlabTop() : base("Chiseled Spruce Plank", "sc:spruce_planks_slabtop", 500, "sc:spruce_planks_slabtop_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 1000, 0, 500);
        }
    }

    public class SprucePlanksBlock_SlabBottom : Block
    {
        internal SprucePlanksBlock_SlabBottom() : base("Chiseled Spruce Plank", "sc:spruce_planks_slabbottom", 500, "sc:spruce_planks_slabbottom_item", Tool.Axe)
        {
            collision = new RectangleCollision(0, 1000, 500, 1000);
        }
    }

    public class SprucePlanksBlock_StairTopRight : Block
    {
        internal SprucePlanksBlock_StairTopRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairtopright", 500, "sc:spruce_planks_stairtopright_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class SprucePlanksBlock_StairTopLeft : Block
    {
        internal SprucePlanksBlock_StairTopLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairtopleft", 500, "sc:spruce_planks_stairtopleft_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
        }
    }

    public class SprucePlanksBlock_StairBottomRight : Block
    {
        internal SprucePlanksBlock_StairBottomRight() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairbottomright", 500, "sc:spruce_planks_stairbottomright_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class SprucePlanksBlock_StairBottomLeft : Block
    {
        internal SprucePlanksBlock_StairBottomLeft() : base("Chiseled Spruce Plank", "sc:spruce_planks_stairbottomleft", 500, "sc:spruce_planks_stairbottomleft_item", Tool.Axe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }
    }

    public class SprucePlanksBlock_Center : Block
    {
        internal SprucePlanksBlock_Center() : base("Chiseled Spruce Plank", "sc:spruce_planks_center", 500, "sc:spruce_planks_center_item", Tool.Axe)
        {
            collision = new RectangleCollision(333, 666, 333, 666);
        }
    }
}
