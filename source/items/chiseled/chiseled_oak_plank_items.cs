namespace SeeloewenCraft
{
    public class OakPlanksItem_TopRight : ChiseledItem
    {
        public OakPlanksItem_TopRight() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_topright_item", "sc:oak_planks_topright", true, Images.OakPlanksBlock_TopRight);
            unchiselItems.Add(new OakPlanksItem_Center());
        }
    }

    public class OakPlanksItem_TopLeft : ChiseledItem
    {
        public OakPlanksItem_TopLeft() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_topleft_item", "sc:oak_planks_topleft", true, Images.OakPlanksBlock_TopLeft);
            unchiselItems.Add(new OakPlanksItem_Center());
        }
    }

    public class OakPlanksItem_BottomRight : ChiseledItem
    {
        public OakPlanksItem_BottomRight() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_bottomright_item", "sc:oak_planks_bottomright", true, Images.OakPlanksBlock_BottomRight);
            unchiselItems.Add(new OakPlanksItem_Center());
        }
    }

    public class OakPlanksItem_BottomLeft : ChiseledItem
    {
        public OakPlanksItem_BottomLeft() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_bottomleft_item", "sc:oak_planks_bottomleft", true, Images.OakPlanksBlock_BottomLeft);
            unchiselItems.Add(new OakPlanksItem_Center());
        }
    }

    public class OakPlanksItem_SlabRight : ChiseledItem
    {
        public OakPlanksItem_SlabRight() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slabright_item", "sc:oak_planks_slabright", true, Images.OakPlanksBlock_SlabRight);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_SlabLeft : ChiseledItem
    {
        public OakPlanksItem_SlabLeft() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slableft_item", "sc:oak_planks_slableft", true, Images.OakPlanksBlock_SlabLeft);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_SlabTop : ChiseledItem
    {
        public OakPlanksItem_SlabTop() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slabtop_item", "sc:oak_planks_slabtop", true, Images.OakPlanksBlock_SlabTop);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_SlabBottom : ChiseledItem
    {
        public OakPlanksItem_SlabBottom() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_slabbottom_item", "sc:oak_planks_slabbottom", true, Images.OakPlanksBlock_SlabBottom);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_StairTopRight : ChiseledItem
    {
        public OakPlanksItem_StairTopRight() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairtopright_item", "sc:oak_planks_stairtopright", true, Images.OakPlanksBlock_StairTopRight);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_StairTopLeft : ChiseledItem
    {
        public OakPlanksItem_StairTopLeft() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairtopleft_item", "sc:oak_planks_stairtopleft", true, Images.OakPlanksBlock_StairTopLeft);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_StairBottomRight : ChiseledItem
    {
        public OakPlanksItem_StairBottomRight() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairbottomright_item", "sc:oak_planks_stairbottomright", true, Images.OakPlanksBlock_StairBottomRight);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_StairBottomLeft : ChiseledItem
    {
        public OakPlanksItem_StairBottomLeft() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_stairbottomleft_item", "sc:oak_planks_stairbottomleft", true, Images.OakPlanksBlock_StairBottomLeft);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new OakPlanksItem_Center());
            }
        }
    }

    public class OakPlanksItem_Center : ChiseledItem
    {
        public OakPlanksItem_Center() : base()
        {
            Init("Chiseled Oak Plank", "sc:oak_planks_center_item", "sc:oak_planks_center", true, Images.OakPlanksBlock_Center);
        }
    }
}
