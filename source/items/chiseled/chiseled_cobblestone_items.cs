using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CobbleStoneItem_TopRight : ChiseledItem
    {
        public CobbleStoneItem_TopRight() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_topright_item", "sc:cobblestone_topright", true, Images.CobbleStoneBlock_TopRight);
            unchiselItems.Add(new CobbleStoneItem_Center());
        }
    }

    public class CobbleStoneItem_TopLeft : ChiseledItem
    {
        public CobbleStoneItem_TopLeft() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_topleft_item", "sc:cobblestone_topleft", true, Images.CobbleStoneBlock_TopLeft);
            unchiselItems.Add(new CobbleStoneItem_Center());
        }
    }

    public class CobbleStoneItem_BottomRight : ChiseledItem
    {
        public CobbleStoneItem_BottomRight() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_bottomright_item", "sc:cobblestone_bottomright", true, Images.CobbleStoneBlock_BottomRight);
            unchiselItems.Add(new CobbleStoneItem_Center());
        }
    }

    public class CobbleStoneItem_BottomLeft : ChiseledItem
    {
        public CobbleStoneItem_BottomLeft() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_bottomleft_item", "sc:cobblestone_bottomleft", true, Images.CobbleStoneBlock_BottomLeft);
            unchiselItems.Add(new CobbleStoneItem_Center());
        }
    }

    public class CobbleStoneItem_SlabRight : ChiseledItem
    {
        public CobbleStoneItem_SlabRight() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slabright_item", "sc:cobblestone_slabright", true, Images.CobbleStoneBlock_SlabRight);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_SlabLeft : ChiseledItem
    {
        public CobbleStoneItem_SlabLeft() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slableft_item", "sc:cobblestone_slableft", true, Images.CobbleStoneBlock_SlabLeft);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_SlabTop : ChiseledItem
    {
        public CobbleStoneItem_SlabTop() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slabtop_item", "sc:cobblestone_slabtop", true, Images.CobbleStoneBlock_SlabTop);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_SlabBottom : ChiseledItem
    {
        public CobbleStoneItem_SlabBottom() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_slabbottom_item", "sc:cobblestone_slabbottom", true, Images.CobbleStoneBlock_SlabBottom);
            for (int i = 0; i < 2; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_StairTopRight : ChiseledItem
    {
        public CobbleStoneItem_StairTopRight() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairtopright_item", "sc:cobblestone_stairtopright", true, Images.CobbleStoneBlock_StairTopRight);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_StairTopLeft : ChiseledItem
    {
        public CobbleStoneItem_StairTopLeft() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairtopleft_item", "sc:cobblestone_stairtopleft", true, Images.CobbleStoneBlock_StairTopLeft);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_StairBottomRight : ChiseledItem
    {
        public CobbleStoneItem_StairBottomRight() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairbottomright_item", "sc:cobblestone_stairbottomright", true, Images.CobbleStoneBlock_StairBottomRight);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_StairBottomLeft : ChiseledItem
    {
        public CobbleStoneItem_StairBottomLeft() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_stairbottomleft_item", "sc:cobblestone_stairbottomleft", true, Images.CobbleStoneBlock_StairBottomLeft);
            for (int i = 0; i < 3; i++)
            {
                unchiselItems.Add(new CobbleStoneItem_Center());
            }
        }
    }

    public class CobbleStoneItem_Center : ChiseledItem
    {
        public CobbleStoneItem_Center() : base()
        {
            Init("Chiseled Cobblestone", "sc:cobblestone_center_item", "sc:cobblestone_center", true, Images.CobbleStoneBlock_Center);
        }
    }
}
