using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class SprucePlanksItem_TopRight : ChiseledItem
    {
        public SprucePlanksItem_TopRight() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_topright_item", "sc:spruce_planks_topright", true, Images.SprucePlanksBlock_TopRight);
            unchiselItems.Add(new SprucePlanksItem_Center());
        }
    }

    public class SprucePlanksItem_TopLeft : ChiseledItem
    {
        public SprucePlanksItem_TopLeft() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_topleft_item", "sc:spruce_planks_topleft", true, Images.SprucePlanksBlock_TopLeft);
            unchiselItems.Add(new SprucePlanksItem_Center());
        }
    }

    public class SprucePlanksItem_BottomRight : ChiseledItem
    {
        public SprucePlanksItem_BottomRight() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_bottomright_item", "sc:spruce_planks_bottomright", true, Images.SprucePlanksBlock_BottomRight);
            unchiselItems.Add(new SprucePlanksItem_Center());
        }
    }

    public class SprucePlanksItem_BottomLeft : ChiseledItem
    {
        public SprucePlanksItem_BottomLeft() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_bottomleft_item", "sc:spruce_planks_bottomleft", true, Images.SprucePlanksBlock_BottomLeft);
            unchiselItems.Add(new SprucePlanksItem_Center());
        }
    }

    public class SprucePlanksItem_SlabRight : ChiseledItem
    {
        public SprucePlanksItem_SlabRight() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_slabright_item", "sc:spruce_planks_slabright", true, Images.SprucePlanksBlock_SlabRight);

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_SlabLeft : ChiseledItem
    {
        public SprucePlanksItem_SlabLeft() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_slableft_item", "sc:spruce_planks_slableft", true, Images.SprucePlanksBlock_SlabLeft);

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_SlabTop : ChiseledItem
    {
        public SprucePlanksItem_SlabTop() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_slabtop_item", "sc:spruce_planks_slabtop", true, Images.SprucePlanksBlock_SlabTop);

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_SlabBottom : ChiseledItem
    {
        public SprucePlanksItem_SlabBottom() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_slabbottom_item", "sc:spruce_planks_slabbottom", true, Images.SprucePlanksBlock_SlabBottom);

            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_StairTopRight : ChiseledItem
    {
        public SprucePlanksItem_StairTopRight() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_stairtopright_item", "sc:spruce_planks_stairtopright", true, Images.SprucePlanksBlock_StairTopRight);

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_StairTopLeft : ChiseledItem
    {
        public SprucePlanksItem_StairTopLeft() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_stairtopleft_item", "sc:spruce_planks_stairtopleft", true, Images.SprucePlanksBlock_StairTopLeft);

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_StairBottomRight : ChiseledItem
    {
        public SprucePlanksItem_StairBottomRight() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_stairbottomright_item", "sc:spruce_planks_stairbottomright", true, Images.SprucePlanksBlock_StairBottomRight);

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_StairBottomLeft : ChiseledItem
    {
        public SprucePlanksItem_StairBottomLeft() : base()
        {
            Init("Chiseled Spruce Planks", "sc:spruce_planks_stairbottomleft_item", "sc:spruce_planks_stairbottomleft", true, Images.SprucePlanksBlock_StairBottomLeft);

            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new SprucePlanksItem_Center());
            }
        }
    }

    public class SprucePlanksItem_Center : ChiseledItem
    {
        public SprucePlanksItem_Center() : base()
        {
            isChiseled = false;
            isPlacable = true;
            Init("Chiseled Spruce Planks", "sc:spruce_planks_center_item", "sc:spruce_planks_center", true, Images.SprucePlanksBlock_Center);
        }
    }
}
