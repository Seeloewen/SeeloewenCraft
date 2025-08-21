namespace SeeloewenCraft.game.core.blocks
{
    public class CobblestoneBlock_BottomLeft : Block
    {
        public CobblestoneBlock_BottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_bottomleft", 1250, "sc:cobblestone_bottomleft_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 500, 500, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_BottomRight : Block
    {
        public CobblestoneBlock_BottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_bottomright", 1250, "sc:cobblestone_bottomright_item", Tool.Pickaxe);
            collision = new RectangleCollision(500, 1000, 500, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class CobbleStoneBlock_TopLeft : Block
    {
        public CobbleStoneBlock_TopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_topleft", 1250, "sc:cobblestone_topleft_item", Tool.Pickaxe);
            dropsOnWrongTool = false;
            collision = new RectangleCollision(0, 500, 0, 500);
        }
    }

    public class CobblestoneBlock_TopRight : Block
    {
        public CobblestoneBlock_TopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_topright", 1250, "sc:cobblestone_topright_item", Tool.Pickaxe);
            dropsOnWrongTool = false;
            collision = new RectangleCollision(500, 1000, 0, 500);
        }
    }

    public class CobblestoneBlock_SlabRight : Block
    {
        public CobblestoneBlock_SlabRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slabright", 1250, "sc:cobblestone_slabright_item", Tool.Pickaxe);
            collision = new RectangleCollision(500, 1000, 0, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_SlabLeft : Block
    {
        public CobblestoneBlock_SlabLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slableft", 1250, "sc:cobblestone_slableft_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 500, 0, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_SlabTop : Block
    {
        public CobblestoneBlock_SlabTop(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slabtop", 1250, "sc:cobblestone_slabtop_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 1000, 0, 500);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_SlabBottom : Block
    {
        public CobblestoneBlock_SlabBottom(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slabbottom", 1250, "sc:cobblestone_slabbottom_item", Tool.Pickaxe);
            collision = new RectangleCollision(0, 1000, 500, 1000);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_StairTopRight : Block
    {
        public CobblestoneBlock_StairTopRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairtopright", 1250, "sc:cobblestone_stairtopright_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([0, 500], [1000, 1000], [0, 500], [500, 1000]);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_StairTopLeft : Block
    {
        public CobblestoneBlock_StairTopLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairtopleft", 1250, "sc:cobblestone_stairtopleft_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 0], [1000, 500]);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_StairBottomRight : Block
    {
        public CobblestoneBlock_StairBottomRight(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairbottomright", 1250, "sc:cobblestone_stairbottomright_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([500, 0], [1000, 1000], [0, 500], [500, 1000]);
            dropsOnWrongTool = false;
        }
    }

    public class CobblestoneBlock_StairBottomLeft : Block
    {
        public CobblestoneBlock_StairBottomLeft(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairbottomleft", 1250, "sc:cobblestone_stairbottomleft_item", Tool.Pickaxe);
            collision = new MultipleRectangleCollision([0, 500], [500, 1000], [0, 500], [1000, 1000]);
            dropsOnWrongTool = false;
        }
    }

    public class CobbleStoneBlock_Center : Block
    {
        public CobbleStoneBlock_Center(bool isInBackground) : base(isInBackground)
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_center", 1250, "sc:cobblestone_center_item", Tool.Pickaxe);
            collision = new RectangleCollision(333, 666, 333, 666);
            dropsOnWrongTool = false;
        }
    }
}
