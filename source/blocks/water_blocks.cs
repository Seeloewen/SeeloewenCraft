using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SeeloewenCraft
{

    //-- Water Blocks --//

    public class WaterBlock_1_Right : Block
    {
        public WaterBlock_1_Right(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 1;
            SetTexture();
            name = "Water";
            id = "sc:water_1_right_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_1_Right;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_1_Left : Block
    {
        public WaterBlock_1_Left(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 1;
            SetTexture();
            name = "Water";
            id = "sc:water_1_left_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_1_Left;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_2_Right : Block
    {
        public WaterBlock_2_Right(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 2;
            SetTexture();
            name = "Water";
            id = "sc:water_2_right_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_2_Right;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_2_Left : Block
    {
        public WaterBlock_2_Left(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 2;
            SetTexture();
            name = "Water";
            id = "sc:water_2_left_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_2_Left;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_3_Right : Block
    {
        public WaterBlock_3_Right(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 3;
            SetTexture();
            name = "Water";
            id = "sc:water_3_right_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_3_Right;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_3_Left : Block
    {
        public WaterBlock_3_Left(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 3;
            id = "sc:water_3_left_block";
            SetTexture();
            name = "Water";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_3_Left;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_4_Right : Block
    {
        public WaterBlock_4_Right(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 4;
            SetTexture();
            name = "Water";
            id = "sc:water_4_right_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_4_Right;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_4_Left : Block
    {
        public WaterBlock_4_Left(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 4;
            SetTexture();
            name = "Water";
            id = "sc:water_4_left_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_4_Left;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_5_Right : Block
    {
        public WaterBlock_5_Right(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 5;
            SetTexture();
            name = "Water";
            id = "sc:water_5_right_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_5_Right;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_5_Left : Block
    {
        public WaterBlock_5_Left(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 5;
            SetTexture();
            name = "Water";
            id = "sc:water_5_left_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_5_Left;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }

    public class WaterBlock_6 : Block
    {
        public WaterBlock_6(World world, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(world, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isBreakable = false;
            canBeMovedToBackground = false;
            waterLevel = 6;
            SetTexture();
            name = "Water";
            id = "sc:water_6_block";
            tags.Add("liquids/water");
        }

        override public void GenerateItem(World world, int id)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Water_6;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }
    }
}
