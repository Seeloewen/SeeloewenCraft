namespace SeeloewenCraft
{
    public class SandStoneBlock_BottomLeft : Block
    {
        public SandStoneBlock_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_bottomleft", 1250, "sc:sand_stone_bottomleft_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 500, 500, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_BottomRight : Block
    {
        public SandStoneBlock_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_bottomright", 1250, "sc:sand_stone_bottomright_item", Tool.Pickaxe);
            collision = new RectangleCollision(500, 1000, 500, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_TopLeft : Block
    {
        public SandStoneBlock_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_topleft", 1250, "sc:sand_stone_topleft_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 500, 0, 500);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_TopRight : Block
    {
        public SandStoneBlock_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_topright", 1250, "sc:sand_stone_topright_item", Tool.Pickaxe);
            collision = new RectangleCollision(500, 1000, 0, 500);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_SlabRight : Block
    {
        public SandStoneBlock_SlabRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabright", 1250, "sc:sand_stone_slabright_item", Tool.Pickaxe);
            collision = new RectangleCollision(500, 1000, 0, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_SlabLeft : Block
    {
        public SandStoneBlock_SlabLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slableft", 1250, "sc:sand_stone_slableft_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 500, 0, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_SlabTop : Block
    {
        public SandStoneBlock_SlabTop(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabtop", 1250, "sc:sand_stone_slabtop_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 1000, 0, 500);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_SlabBottom : Block
    {
        public SandStoneBlock_SlabBottom(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabbottom", 1250, "sc:sand_stone_slabbottom_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 1000, 500, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_StairTopRight : Block
    {
        public SandStoneBlock_StairTopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairtopright", 1250, "sc:sand_stone_stairtopright_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_StairTopLeft : Block
    {
        public SandStoneBlock_StairTopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairtopleft", 1250, "sc:sand_stone_stairtopleft_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_StairBottomRight : Block
    {
        public SandStoneBlock_StairBottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairbottomright", 1250, "sc:sand_stone_stairbottomright_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_StairBottomLeft : Block
    {
        public SandStoneBlock_StairBottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairbottomleft", 1250, "sc:sand_stone_stairbottomleft_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
            dropsOnWrongTool = false;
        }
    }

    public class SandStoneBlock_Center : Block
    {
        public SandStoneBlock_Center(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_center", 1250, "sc:sand_stone_center_item", Tool.Pickaxe);
            collision = new RectangleCollision(333, 666, 333, 666);
            dropsOnWrongTool = false;
        }
    }
}
