namespace SeeloewenCraft.game.core.blocks
{
    public class OakPlanksBlock_BottomLeft : Block
    {
        internal OakPlanksBlock_BottomLeft() : base("Chiseled Oak Plank", "sc:oak_planks_bottomleft", 500, "sc:oak_planks_bottomleft_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 500, 500, 1000);
        }
    }

    public class OakPlanksBlock_BottomRight : Block
    {
        internal OakPlanksBlock_BottomRight() : base("Chiseled Oak Plank", "sc:oak_planks_bottomright", 500, "sc:oak_planks_bottomright_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(500, 1000, 500, 1000);
        }
    }

    public class OakPlanksBlock_TopLeft : Block
    {
        internal OakPlanksBlock_TopLeft() : base("Chiseled Oak Plank", "sc:oak_planks_topleft", 500, "sc:oak_planks_topleft_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 500, 0, 500);
        }
    }

    public class OakPlanksBlock_TopRight : Block
    {
        internal OakPlanksBlock_TopRight() : base("Chiseled Oak Plank", "sc:oak_planks_topright", 500, "sc:oak_planks_topright_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(500, 1000, 0, 500);
        }
    }

    public class OakPlanksBlock_SlabRight : Block
    {
        internal OakPlanksBlock_SlabRight() : base("Chiseled Oak Plank", "sc:oak_planks_slabright", 500, "sc:oak_planks_slabright_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(500, 1000, 0, 1000);
        }
    }

    public class OakPlanksBlock_SlabLeft : Block
    {
        internal OakPlanksBlock_SlabLeft() : base("Chiseled Oak Plank", "sc:oak_planks_slableft", 500, "sc:oak_planks_slableft_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 500, 0, 1000);
        }
    }

    public class OakPlanksBlock_SlabTop : Block
    {
        internal OakPlanksBlock_SlabTop() : base("Chiseled Oak Plank", "sc:oak_planks_slabtop", 500, "sc:oak_planks_slabtop_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 1000, 0, 500);
        }
    }

    public class OakPlanksBlock_SlabBottom : Block
    {
        internal OakPlanksBlock_SlabBottom() : base("Chiseled Oak Plank", "sc:oak_planks_slabbottom", 500, "sc:oak_planks_slabbottom_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 1000, 500, 1000);
        }
    }

    public class OakPlanksBlock_StairTopRight : Block
    {
        internal OakPlanksBlock_StairTopRight() : base("Chiseled Oak Plank", "sc:oak_planks_stairtopright", 500, "sc:oak_planks_stairtopright_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class OakPlanksBlock_StairTopLeft : Block
    {
        internal OakPlanksBlock_StairTopLeft() : base("Chiseled Oak Plank", "sc:oak_planks_stairtopleft", 500, "sc:oak_planks_stairtopleft_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
        }
    }

    public class OakPlanksBlock_StairBottomRight : Block
    {
        internal OakPlanksBlock_StairBottomRight() : base("Chiseled Oak Plank", "sc:oak_planks_stairbottomright", 500, "sc:oak_planks_stairbottomright_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class OakPlanksBlock_StairBottomLeft : Block
    {
        internal OakPlanksBlock_StairBottomLeft() : base("Chiseled Oak Plank", "sc:oak_planks_stairbottomleft", 500, "sc:oak_planks_stairbottomleft_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }
    }

    public class OakPlanksBlock_Center : Block
    {
        internal OakPlanksBlock_Center() : base("Chiseled Oak Plank", "sc:oak_planks_center", 500, "sc:oak_planks_center_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(333, 666, 333, 666);
        }
    }
}
