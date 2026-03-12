using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.blocks
{

    public abstract class WaterBlock : LiquidBlock
    {
        internal WaterBlock(string name, string id) : base(name, id)
        {
            liquidTag = BlockTags.LIQUIDS_WATER;
            WriteTag(liquidTag);
        }

        internal override LiquidBlock GetLiquid(int level, Direction dir, LiquidSource source)
        {
            string dirS = dir == Direction.RIGHT ? "right" : "left";

            LiquidBlock newBlock = (LiquidBlock)BlockRegister.Get(level switch
            {
                1 => $"sc:water_1_{dirS}_block",
                2 => $"sc:water_2_{dirS}_block",
                3 => $"sc:water_3_{dirS}_block",
                4 => $"sc:water_4_{dirS}_block",
                5 => $"sc:water_5_{dirS}_block",
                6 => $"sc:water_6_block",
                _ => null
            });

            newBlock.RemoveTag(BlockTags.LIQUID_SOURCE);

            SetSource(newBlock, source);

            return newBlock;
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

        internal virtual(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 0);
        }
    }

    //-- Water Blocks --//

    public class WaterBlock_1_Right : WaterBlock
    {
        internal WaterBlock_1_Right() : base("Water", "sc:water_1_right_block")
        {
            liquidLevel = 1;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 800 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_1_Left : WaterBlock
    {
        internal WaterBlock_1_Left() : base("Water", "sc:water_1_left_block")
        {
            liquidLevel = 1;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 1000 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_2_Right : WaterBlock
    {
        internal WaterBlock_2_Right() : base("Water", "sc:water_2_right_block")
        {
            liquidLevel = 2;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 600 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_2_Left : WaterBlock
    {
        internal WaterBlock_2_Left() : base("Water", "sc:water_2_left_block")
        {
            liquidLevel = 2;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 800 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_3_Right : WaterBlock
    {
        internal WaterBlock_3_Right() : base("Water", "sc:water_3_right_block")
        {
            liquidLevel = 3;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 400 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_3_Left : WaterBlock
    {
        internal WaterBlock_3_Left() : base("Water", "sc:water_3_left_block")
        {
            liquidLevel = 3;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 600 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_4_Right : WaterBlock
    {
        internal WaterBlock_4_Right() : base("Water", "sc:water_4_right_block")
        {
            liquidLevel = 4;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) + 200 ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_4_Left : WaterBlock
    {
        internal WaterBlock_4_Left() : base("Water", "sc:water_4_left_block")
        {
            liquidLevel = 4;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 400 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_5_Right : WaterBlock
    {
        internal WaterBlock_5_Right() : base("Water", "sc:water_5_right_block")
        {
            liquidLevel = 5;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > startX * (1.0 / 5) ? (true, 1) : (false, 0);
        }
    }

    public class WaterBlock_5_Left : WaterBlock
    {
        internal WaterBlock_5_Left() : base("Water", "sc:water_5_left_block")
        {
            liquidLevel = 5;
        }

        internal override(bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return endY > endX * -(1.0 / 5) + 200 ? (true, -1) : (false, 0);
        }
    }

    public class WaterBlock_6 : WaterBlock
    {
        internal WaterBlock_6() : base("Water", "sc:water_6_block")
        {
            liquidLevel = 6;
        }
    }

}
