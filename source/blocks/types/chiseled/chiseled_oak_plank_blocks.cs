namespace SeeloewenCraft
{
    public class OakPlanksBlock_BottomLeft : Block
    {
        public OakPlanksBlock_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_bottomleft", 500, "sc:oak_planks_bottomleft_item", Tool.Pickaxe, Images.OakPlanksBlock_BottomLeft);
            collision = new RectangleCollision(0, 500, 500, 1000);
        }
    }

    public class OakPlanksBlock_BottomRight : Block
    {
        public OakPlanksBlock_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_bottomright", 500, "sc:oak_planks_bottomright_item", Tool.Pickaxe, Images.OakPlanksBlock_BottomRight);
            collision = new RectangleCollision(500, 1000, 500, 1000);
        }
    }

    public class OakPlanksBlock_TopLeft : Block
    {
        public OakPlanksBlock_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_topleft", 500, "sc:oak_planks_topleft_item", Tool.Pickaxe, Images.OakPlanksBlock_TopLeft);
            collision = new RectangleCollision(0, 500, 0, 500);
        }
    }

    public class OakPlanksBlock_TopRight : Block
    {
        public OakPlanksBlock_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_topright", 500, "sc:oak_planks_topright_item", Tool.Pickaxe, Images.OakPlanksBlock_TopRight);
            collision = new RectangleCollision(500, 1000, 0, 500);
        }
    }

    public class OakPlanksBlock_SlabRight : Block
    {
        public OakPlanksBlock_SlabRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slabright", 500, "sc:oak_planks_slabright_item", Tool.Pickaxe, Images.OakPlanksBlock_SlabRight);
            collision = new RectangleCollision(500, 1000, 0, 1000);
        }
    }

    public class OakPlanksBlock_SlabLeft : Block
    {
        public OakPlanksBlock_SlabLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slableft", 500, "sc:oak_planks_slableft_item", Tool.Pickaxe, Images.OakPlanksBlock_SlabLeft);
            collision = new RectangleCollision(0, 500, 0, 1000);
        }
    }

    public class OakPlanksBlock_SlabTop : Block
    {
        public OakPlanksBlock_SlabTop(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slabtop", 500, "sc:oak_planks_slabtop_item", Tool.Pickaxe, Images.OakPlanksBlock_SlabTop);
            collision = new RectangleCollision(0, 1000, 0, 500);
        }
    }

    public class OakPlanksBlock_SlabBottom : Block
    {
        public OakPlanksBlock_SlabBottom(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slabbottom", 500, "sc:oak_planks_slabbottom_item", Tool.Pickaxe, Images.OakPlanksBlock_SlabBottom);
            collision = new RectangleCollision(0, 1000, 500, 1000);
        }
    }

    public class OakPlanksBlock_StairTopRight : Block
    {
        public OakPlanksBlock_StairTopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairtopright", 500, "sc:oak_planks_stairtopright_item", Tool.Pickaxe, Images.OakPlanksBlock_StairTopRight);
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class OakPlanksBlock_StairTopLeft : Block
    {
        public OakPlanksBlock_StairTopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairtopleft", 500, "sc:oak_planks_stairtopleft_item", Tool.Pickaxe, Images.OakPlanksBlock_StairTopLeft);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
        }
    }

    public class OakPlanksBlock_StairBottomRight : Block
    {
        public OakPlanksBlock_StairBottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairbottomright", 500, "sc:oak_planks_stairbottomright_item", Tool.Pickaxe, Images.OakPlanksBlock_StairBottomRight);
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
        }
    }

    public class OakPlanksBlock_StairBottomLeft : Block
    {
        public OakPlanksBlock_StairBottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairbottomleft", 500, "sc:oak_planks_stairbottomleft_item", Tool.Pickaxe, Images.OakPlanksBlock_StairBottomLeft);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
        }
    }

    public class OakPlanksBlock_Center : Block
    {
        public OakPlanksBlock_Center(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_center", 500, "sc:oak_planks_center_item", Tool.Pickaxe, Images.OakPlanksBlock_Center);
            collision = new RectangleCollision(333, 666, 333, 666);
        }
    }
}
