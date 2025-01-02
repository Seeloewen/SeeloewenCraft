using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class SandStoneItem_TopRight : ChiseledItem
    {
        public SandStoneItem_TopRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_topright_item", "sc:sand_stone_topright", true, Images.SandStoneBlock_TopRight);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_TopLeft : ChiseledItem
    {
        public SandStoneItem_TopLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_topleft_item", "sc:sand_stone_topleft", true, Images.SandStoneBlock_TopLeft);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_BottomRight : ChiseledItem
    {
        public SandStoneItem_BottomRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_bottomright_item", "sc:sand_stone_bottomright", true, Images.SandStoneBlock_BottomRight);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_BottomLeft : ChiseledItem
    {
        public SandStoneItem_BottomLeft() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_bottomleft_item", "sc:sand_stone_bottomleft", true, Images.SandStoneBlock_BottomLeft);
            unchiselItems.Add(new SandStoneItem_Center());
        }
    }

    public class SandStoneItem_SlabRight : ChiseledItem
    {
        public SandStoneItem_SlabRight() : base()
        {
            Init("Chiseled Sand Stone", "sc:sand_stone_slabright_item", "sc:sand_stone_slabright", true, Images.SandStoneBlock_SlabRight);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_slableft_item", "sc:sand_stone_slableft", true, Images.SandStoneBlock_SlabLeft);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_slabtop_item", "sc:sand_stone_slabtop", true, Images.SandStoneBlock_SlabTop);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_slabbottom_item", "sc:sand_stone_slabbottom", true, Images.SandStoneBlock_SlabBottom);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_stairtopright_item", "sc:sand_stone_stairtopright", true, Images.SandStoneBlock_StairTopRight);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_stairtopleft_item", "sc:sand_stone_stairtopleft", true, Images.SandStoneBlock_StairTopLeft);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_stairbottomright_item", "sc:sand_stone_stairbottomright", true, Images.SandStoneBlock_StairBottomRight);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_stairbottomleft_item", "sc:sand_stone_stairbottomleft", true, Images.SandStoneBlock_StairBottomLeft);
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
            Init("Chiseled Sand Stone", "sc:sand_stone_center_item", "sc:sand_stone_center", true, Images.SandStoneBlock_Center);
        }
    }
}
