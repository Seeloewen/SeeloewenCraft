namespace SeeloewenCraft.game.core.blocks
{
    public class SandStoneBlock_BottomLeft : Block
    {
        public SandStoneBlock_BottomLeft() : base("Chiseled Sand Stone", "sc:sand_stone_bottomleft", 1250, "sc:sand_stone_bottomleft_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 500, 500, 1000);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_BottomRight : Block
    {
        public SandStoneBlock_BottomRight() : base("Chiseled Sand Stone", "sc:sand_stone_bottomright", 1250, "sc:sand_stone_bottomright_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(500, 1000, 500, 1000);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_TopLeft : Block
    {
        public SandStoneBlock_TopLeft() : base("Chiseled Sand Stone", "sc:sand_stone_topleft", 1250, "sc:sand_stone_topleft_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 500, 0, 500);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_TopRight : Block
    {
        public SandStoneBlock_TopRight() : base("Chiseled Sand Stone", "sc:sand_stone_topright", 1250, "sc:sand_stone_topright_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(500, 1000, 0, 500);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_SlabRight : Block
    {
        public SandStoneBlock_SlabRight() : base("Chiseled Sand Stone", "sc:sand_stone_slabright", 1250, "sc:sand_stone_slabright_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(500, 1000, 0, 1000);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_SlabLeft : Block
    {
        public SandStoneBlock_SlabLeft() : base("Chiseled Sand Stone", "sc:sand_stone_slableft", 1250, "sc:sand_stone_slableft_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 500, 0, 1000);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_SlabTop : Block
    {
        public SandStoneBlock_SlabTop() : base("Chiseled Sand Stone", "sc:sand_stone_slabtop", 1250, "sc:sand_stone_slabtop_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 1000, 0, 500);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_SlabBottom : Block
    {
        public SandStoneBlock_SlabBottom() : base("Chiseled Sand Stone", "sc:sand_stone_slabbottom", 1250, "sc:sand_stone_slabbottom_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(0, 1000, 500, 1000);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_StairTopRight : Block
    {
        public SandStoneBlock_StairTopRight() : base("Chiseled Sand Stone", "sc:sand_stone_stairtopright", 1250, "sc:sand_stone_stairtopright_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_StairTopLeft : Block
    {
        public SandStoneBlock_StairTopLeft() : base("Chiseled Sand Stone", "sc:sand_stone_stairtopleft", 1250, "sc:sand_stone_stairtopleft_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_StairBottomRight : Block
    {
        public SandStoneBlock_StairBottomRight() : base("Chiseled Sand Stone", "sc:sand_stone_stairbottomright", 1250, "sc:sand_stone_stairbottomright_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_StairBottomLeft : Block
    {
        public SandStoneBlock_StairBottomLeft() : base("Chiseled Sand Stone", "sc:sand_stone_stairbottomleft", 1250, "sc:sand_stone_stairbottomleft_item", Tool.Pickaxe)
        {
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }

    public class SandStoneBlock_Center : Block
    {
        public SandStoneBlock_Center() : base("Chiseled Sand Stone", "sc:sand_stone_center", 1250, "sc:sand_stone_center_item", Tool.Pickaxe)
        {
            collision = new RectangleCollision(333, 666, 333, 666);
            WriteTag(BlockTags.TOOL_SPECIFIC);
        }
    }
}
