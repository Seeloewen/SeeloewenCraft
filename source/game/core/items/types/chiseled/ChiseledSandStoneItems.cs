namespace SeeloewenCraft.game.core.items
{
    public class SandStoneItem_TopRight : ChiseledItem
    {
        public SandStoneItem_TopRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_topright_item", "sc:sand_stone_topright", true);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_TopLeft : ChiseledItem
    {
        public SandStoneItem_TopLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_topleft_item", "sc:sand_stone_topleft", true);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_BottomRight : ChiseledItem
    {
        public SandStoneItem_BottomRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_bottomright_item", "sc:sand_stone_bottomright", true);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_BottomLeft : ChiseledItem
    {
        public SandStoneItem_BottomLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_bottomleft_item", "sc:sand_stone_bottomleft", true);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_SlabRight : ChiseledItem
    {
        public SandStoneItem_SlabRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabright_item", "sc:sand_stone_slabright", true);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_SlabLeft : ChiseledItem
    {
        public SandStoneItem_SlabLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slableft_item", "sc:sand_stone_slableft", true);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_SlabTop : ChiseledItem
    {
        public SandStoneItem_SlabTop() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabtop_item", "sc:sand_stone_slabtop", true);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_SlabBottom : ChiseledItem
    {
        public SandStoneItem_SlabBottom() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabbottom_item", "sc:sand_stone_slabbottom", true);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_StairTopRight : ChiseledItem
    {
        public SandStoneItem_StairTopRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairtopright_item", "sc:sand_stone_stairtopright", true);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_StairTopLeft : ChiseledItem
    {
        public SandStoneItem_StairTopLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairtopleft_item", "sc:sand_stone_stairtopleft", true);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_StairBottomRight : ChiseledItem
    {
        public SandStoneItem_StairBottomRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairbottomright_item", "sc:sand_stone_stairbottomright", true);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_StairBottomLeft : ChiseledItem
    {
        public SandStoneItem_StairBottomLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_stairbottomleft_item", "sc:sand_stone_stairbottomleft", true);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SandStoneItem_Center());
            }
        }
    }

    public class SandStoneItem_Center : ChiseledItem
    {
        public SandStoneItem_Center() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_center_item", "sc:sand_stone_center", true);
        }
    }
}
