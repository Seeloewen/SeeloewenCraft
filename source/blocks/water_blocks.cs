using System;

namespace SeeloewenCraft
{

    //-- Abstract Water --//

    public abstract class WaterBlock : Block
    {
        public WaterBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            isReplacable = true;
            isBreakable = false;
            canBeMovedToBackground = false;
            name = "Water";
            tags.Add("liquids/water");
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 0);
        }

        public override void ShowAdditionalDebugInfo()
        {
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"waterLevel={waterLevel}");
        }
    }

    //-- Water Blocks --//

    public class WaterBlock_1_Right : WaterBlock
    {

        public WaterBlock_1_Right(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 1;
            SetTexture();
            id = "sc:water_1_right_block";
        }

        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 1);
        }

        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_1_Right;
        }
    }

    public class WaterBlock_1_Left : WaterBlock
    {
        public WaterBlock_1_Left(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 1;
            SetTexture();
            id = "sc:water_1_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, -1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_1_Left;
        }
    }

    public class WaterBlock_2_Right : WaterBlock
    {
        public WaterBlock_2_Right(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 2;
            SetTexture();
            id = "sc:water_2_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_2_Right;
        }
    }

    public class WaterBlock_2_Left : WaterBlock
    {
        public WaterBlock_2_Left(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 2;
            SetTexture();
            id = "sc:water_2_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, -1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_2_Left;
        }
    }

    public class WaterBlock_3_Right : WaterBlock
    {
        public WaterBlock_3_Right(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 3;
            SetTexture();
            id = "sc:water_3_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_3_Right;
        }
    }

    public class WaterBlock_3_Left : WaterBlock
    {
        public WaterBlock_3_Left(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 3;
            id = "sc:water_3_left_block";
            SetTexture();
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, -1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_3_Left;
        }
    }

    public class WaterBlock_4_Right : WaterBlock
    {
        public WaterBlock_4_Right(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 4;
            SetTexture();
            id = "sc:water_4_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_4_Right;
        }
    }

    public class WaterBlock_4_Left : WaterBlock
    {
        public WaterBlock_4_Left(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 4;
            SetTexture();
            id = "sc:water_4_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, -1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_4_Left;
        }
    }

    public class WaterBlock_5_Right : WaterBlock
    {
        public WaterBlock_5_Right(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 5;
            SetTexture();
            id = "sc:water_5_right_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, 1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_5_Right;
        }
    }

    public class WaterBlock_5_Left : WaterBlock
    {
        public WaterBlock_5_Left(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 5;
            SetTexture();
            id = "sc:water_5_left_block";
        }
        public override (bool, int) CheckWaterTouch(int startX, int startY, int endX, int endY)
        {
            return (true, -1);
        }
        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_5_Left;
        }
    }

    public class WaterBlock_6 : WaterBlock
    {
        public WaterBlock_6(World world, bool isInBackground) : base(world, isInBackground)
        {
            waterLevel = 6;
            SetTexture();
            id = "sc:water_6_block";
        }

        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_6;
        }
    }
}
