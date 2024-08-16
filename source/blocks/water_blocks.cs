
using SeeloewenCraft.entity;

namespace SeeloewenCraft
{

    //-- Abstract Water --//

    public abstract class WaterBlock : Block
    {
        public WaterBlock( bool isInBackground) : base( isInBackground)
        {
            isSolid = false;
            isReplacable = true;
            isBreakable = false;
            canBeMovedToBackground = false;
            name = "Water";
            tags.Add("liquids/water");
        }

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            (bool c, int d) = CheckWaterTouch(startX, startY, endX, endY);
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            touchingStatus[Entity.TOUCHING_WATER] = c;
            switch(d)
            {
                case -1:
                    touchingStatus[Entity.TOUCHING_WATER_LEFT] = true;
                    break;
                case 1:
                    touchingStatus[Entity.TOUCHING_WATER_RIGHT] = true;
                    break;
                case 0:
                    break;
            }
            return touchingStatus;
        }

        public virtual (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 0);
        }

        public override void ShowAdditionalDebugInfo()
        {
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"waterLevel={waterLevel}");
        }
    }

    //-- Water Blocks --//

    public class WaterBlock_1_Right : WaterBlock
    {

        public WaterBlock_1_Right( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 1;
            SetTexture();
            id = "sc:water_1_right_block";
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if(endY > startX * (1.0/5) + 800)
            {
                return (true, 1);
            }
            return (false, 0);
        }

        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_1_Right.GetTexture();
        }
    }

    public class WaterBlock_1_Left : WaterBlock
    {
        public WaterBlock_1_Left( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 1;
            SetTexture();
            id = "sc:water_1_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > endX * -(1.0 / 5) + 1000)
            {
                return (true, -1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_1_Left.GetTexture();
        }
    }

    public class WaterBlock_2_Right : WaterBlock
    {
        public WaterBlock_2_Right( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 2;
            SetTexture();
            id = "sc:water_2_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > startX * (1.0 / 5) + 600)
            {
                return (true, 1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_2_Right.GetTexture()   ;
        }
    }

    public class WaterBlock_2_Left : WaterBlock
    {
        public WaterBlock_2_Left( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 2;
            SetTexture();
            id = "sc:water_2_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > endX * -(1.0 / 5) + 800)
            {
                return (true, -1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_2_Left.GetTexture();
        }
    }

    public class WaterBlock_3_Right : WaterBlock
    {
        public WaterBlock_3_Right( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 3;
            SetTexture();
            id = "sc:water_3_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > startX * (1.0 / 5) + 400)
            {
                return (true, 1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_3_Right.GetTexture();
        }
    }

    public class WaterBlock_3_Left : WaterBlock
    {
        public WaterBlock_3_Left( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 3;
            id = "sc:water_3_left_block";
            SetTexture();
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > endX * -(1.0 / 5) + 600)
            {
                return (true, -1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_3_Left.GetTexture();
        }
    }

    public class WaterBlock_4_Right : WaterBlock
    {
        public WaterBlock_4_Right( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 4;
            SetTexture();
            id = "sc:water_4_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > startX * (1.0 / 5) + 200)
            {
                return (true, 1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_4_Right.GetTexture();
        }
    }

    public class WaterBlock_4_Left : WaterBlock
    {
        public WaterBlock_4_Left( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 4;
            SetTexture();
            id = "sc:water_4_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > endX * -(1.0 / 5) + 400)
            {
                return (true, -1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_4_Left.GetTexture();
        }
    }

    public class WaterBlock_5_Right : WaterBlock
    {
        public WaterBlock_5_Right( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 5;
            SetTexture();
            id = "sc:water_5_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > startX * (1.0 / 5))
            {
                return (true, 1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_5_Right.GetTexture();
        }
    }

    public class WaterBlock_5_Left : WaterBlock
    {
        public WaterBlock_5_Left( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 5;
            SetTexture();
            id = "sc:water_5_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            if (endY > endX * -(1.0 / 5) + 200)
            {
                return (true, -1);
            }
            return (false, 0);
        }
        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_5_Left.GetTexture();
        }
    }

    public class WaterBlock_6 : WaterBlock
    {
        public WaterBlock_6( bool isInBackground) : base( isInBackground)
        {
            waterLevel = 6;
            SetTexture();
            id = "sc:water_6_block";
        }

        override public void GenerateItem()
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = Images.Water_6.GetTexture();
        }
    }
}
