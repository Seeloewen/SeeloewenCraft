using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.graphics;

namespace SeeloewenCraft.game.core.blocks
{

    //-- Abstract Water --//

    public abstract class WaterBlock : Block
    {
        public WaterBlock(bool isInBackground) : base(isInBackground)
        {
            isSolid = false;
            WriteTag(BlockTags.REPLACEABLE);
            WriteTag(BlockTags.UNBREAKABLE);
            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.LIQUIDS_WATER);
        }

        public override bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            (bool c, int d) = CheckWaterTouch(startX, startY, endX, endY);
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            touchingStatus[Entity.TOUCHING_WATER] = c;
            switch (d)
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

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"waterLevel", $"{waterLevel}");
        }
    }

    //-- Water Blocks --//

    public class WaterBlock_1_Right : WaterBlock
    {
        public WaterBlock_1_Right(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_1_right_block", 0, null, Tool.None);
            waterLevel = 1;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 800 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_1_Left : WaterBlock
    {
        public WaterBlock_1_Left(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_1_left_block", 0, null, Tool.None);
            waterLevel = 1;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 1000 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_2_Right : WaterBlock
    {
        public WaterBlock_2_Right(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_2_right_block", 0, null, Tool.None);
            waterLevel = 2;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 600 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_2_Left : WaterBlock
    {
        public WaterBlock_2_Left(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_2_left_block", 0, null, Tool.None);
            waterLevel = 2;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 800 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_3_Right : WaterBlock
    {
        public WaterBlock_3_Right(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_3_right_block", 0, null, Tool.None);
            waterLevel = 3;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 400 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_3_Left : WaterBlock
    {
        public WaterBlock_3_Left(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_3_left_block", 0, null, Tool.None);
            waterLevel = 3;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 600 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_4_Right : WaterBlock
    {
        public WaterBlock_4_Right(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_4_right_block", 0, null, Tool.None);
            waterLevel = 4;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 200 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_4_Left : WaterBlock
    {
        public WaterBlock_4_Left(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_4_left_block", 0, null, Tool.None);
            waterLevel = 4;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 400 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_5_Right : WaterBlock
    {
        public WaterBlock_5_Right(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_5_right_block", 0, null, Tool.None);
            waterLevel = 5;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_5_Left : WaterBlock
    {
        public WaterBlock_5_Left(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_5_left_block", 0, null, Tool.None);
            waterLevel = 5;
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 200 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_6 : WaterBlock
    {
        public WaterBlock_6(bool isInBackground) : base(isInBackground)
        {
            Init("Water", "sc:water_6_block", 0, null, Tool.None);
            waterLevel = 6;
        }
    }

}
