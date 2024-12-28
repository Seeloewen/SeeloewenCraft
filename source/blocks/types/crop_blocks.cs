namespace SeeloewenCraft
{
    public class WheatCropBlock : CropBlock
    {
        int state = 1;

        public WheatCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Wheat", "sc:wheat_crop_block", 0, "sc:seeds_item", Game.rnd.Next(10000, 20000), "sc:seeds_item", "sc:wheat_item", 1, 4, Tool.None, Images.Wheat_Stage1);
            drops.Add(("sc:seeds_item", 1, 1));
            isSolid = false;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1200000, 1800001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && state < 2)
            {
                state = 2;
                sImage = Images.Wheat_Stage2;
                blockContainer.UpdateTexture();
            }
            else if (progress >= 2 * (growthTime / 3) && state < 3)
            {
                state = 3;
                sImage = Images.Wheat_Stage3;
                blockContainer.UpdateTexture();
            }
            else if (IsReady())
            {
                state = 4;
                sImage = Images.Wheat_Stage4;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class CarrotCropBlock : CropBlock
    {
        int state = 1;

        public CarrotCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Carrot", "sc:carrot_crop_block", 0, "sc:carrot_item", Game.rnd.Next(10000, 20000), "sc:carrot_item", "sc:carrot_item", 1, 3, Tool.None, Images.Carrot_Stage1);
            drops.Add(("sc:carrot_item", 1, 1));
            isSolid = false;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1400000, 2000001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && state < 2)
            {
                state = 2;
                sImage = Images.Carrot_Stage2;
                blockContainer.UpdateTexture();
            }
            else if (progress >= 2 * (growthTime / 3) && state < 3)
            {
                state = 3;
                sImage = Images.Carrot_Stage3;
                blockContainer.UpdateTexture();
            }
            else if (IsReady())
            {
                state = 4;
                sImage = Images.Carrot_Stage4;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class CottonCropBlock : CropBlock
    {
        public CottonCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cotton", "sc:cotton_crop_block", 0, "sc:cotton_item", Game.rnd.Next(10000, 20000), "sc:cotton_item", "sc:cotton_item", 1, 2, Tool.None, Images.Cotton_Stage1);
            drops.Add(("sc:cotton_item", 1, 1));
            isSolid = false;
            hasRightClickAction = true;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1400000, 2000001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                sImage = Images.Cotton_Stage2;
                blockContainer.UpdateTexture();
            }
        }

        public override void RightClickAction(object sender)
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:cotton_item", 1, 1));

                sImage = Images.Cotton_Stage1;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class BerryBushCropBlock : CropBlock
    {
        public BerryBushCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Berry Bush", "sc:berry_bush_crop_block", 0, "sc:berry_item", Game.rnd.Next(10000, 20000), "sc:berry_item", "sc:berry_item", 1, 3, Tool.None, Images.Berry_Bush_Stage1);
            drops.Add(("sc:berry_item", 1, 1));
            isSolid = false;
            hasRightClickAction = true;
            needsGround = (true, ""); //Game.rnd.Next(1400000, 2000001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                sImage = Images.Berry_Bush_Stage2;
                blockContainer.UpdateTexture();
            }
        }

        public override void RightClickAction(object sender)
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:berry_item", 1, 1));

                sImage = Images.Berry_Bush_Stage1;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class SugarCaneBlock : CropBlock
    {
        protected bool shouldGrow = true;
        private int maxHeight = 3;

        public SugarCaneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sugar Cane", "sc:sugar_cane_block", 0, "sc:sugar_cane_item", Game.rnd.Next(10000, 20000), "sc:sugar_cane_item", "sc:sugar_cane_item", 0, 0, Tool.None, Images.SugarCane);
            drops.Add(("sc:sugar_cane_item", 1, 1));
            tags.Add("Crops/SugarCane");
            isSolid = false;
            hasRightClickAction = true;
            needsGround = (true, ""); //Game.rnd.Next(1400000, 2000001)

            //Implement additional check for nearby water (maybe)
        }

        public void PlaceBlockAbove(int currentY, int maxY)
        {
            Block blockAbove = chunk.GetBlock(xPos, currentY - 1);

            //Stop if new block would be above max height or block above is null
            if (currentY - 1 < maxY) return;
            if (blockAbove == null) return;

            if (blockAbove.isReplacable)
            {
                //Place the new block
                Block newBlockAbove = new SugarCaneBlock(false) { needsGround = (true, "Crops/SugarCane"), shouldGrow = false };
                blockAbove.SetBlock(newBlockAbove);
            }
            else if (blockAbove.id == "sc:sugar_cane_block")
            {
                //If the block above is a sugarcane, check its block above
                PlaceBlockAbove(currentY - 1, maxY);
            }
        }


        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady() && shouldGrow)
            {
                PlaceBlockAbove(yPos, yPos - maxHeight);
                progress = 0;
                growthTime = Game.rnd.Next(10000, 20000);
            }
        }
    }

    public class TomatoCropBlock : CropBlock
    {
        protected bool shouldGrow = true;
        private int maxHeight = 2;

        public TomatoCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Tomato", "sc:tomato_crop_block", 0, "sc:tomato_item", Game.rnd.Next(10000, 20000), "sc:tomato_item", "sc:tomato_item", 1, 3, Tool.None, Images.Tomato_Stage1);
            drops.Add(("sc:tomato_item", 1, 1));
            tags.Add("Crops/Tomato");
            isSolid = false;
            isBase = true;
            hasRightClickAction = true;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1400000, 2000001)
        }

        public void PlaceBlockAbove(int currentY, int maxY)
        {
            Block blockAbove = chunk.GetBlock(xPos, currentY - 1);

            //Stop if new block would be above max height or block above is null
            if (currentY - 1 < maxY || blockAbove == null) return;

            if (blockAbove.isBackground && blockAbove.GetForegroundBlock() == null)
            {
                //Place the new block in foreground
                Block newBlockAbove = new TomatoCropBlock(false) { needsGround = (true, "Crops/Tomato"), shouldGrow = false };
                blockAbove.SetForegroundBlock(newBlockAbove);

                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;
            }
            else if (blockAbove.isReplacable)
            {
                //Place the new block
                Block newBlockAbove = new TomatoCropBlock(false) { needsGround = (true, "Crops/Tomato"), shouldGrow = false };
                blockAbove.SetBlock(newBlockAbove);

                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;
            }
            else if (blockAbove.id == "sc:tomato_crop_block" || blockAbove.isBackground && blockAbove.GetForegroundBlock() != null && blockAbove.GetForegroundBlock().id == "sc:tomato_crop_block")
            {
                //If the block above is a tomato, check its block above
                PlaceBlockAbove(currentY - 1, maxY);
            }
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                sImage = Images.Tomato_Stage2;
                blockContainer.UpdateTexture();

                if (shouldGrow)
                {
                    PlaceBlockAbove(yPos, yPos - maxHeight);
                }
            }
        }

        public override void RightClickAction(object sender)
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:tomato_item", 1, 1));

                sImage = Images.Tomato_Stage1;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class Rice_Base : Block
    {
        public Rice_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Rice Base", "sc:rice_base", 500, "sc:rice_item", Tool.Axe, Images.Rice_Base);
            isBase = true;
            needsGround = (true, "ground/plant"); //Game.rnd.Next(1400000, 2000001)
            isSolid = false;
            doesntDrop = true;

            connectedBlocks.Add((0, -1, "sc:rice_top"));
        }
    }

    public class Rice_Top : CropBlock
    {
        int state = 1;

        public Rice_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Rice Top", "sc:rice_top", 0, "sc:bucket_rice_item", Game.rnd.Next(10000, 20000), "sc:bucket_rice_item", "sc:bucket_rice_item", 1, 1, Tool.None, Images.Rice_Top_Stage1);
            isSolid = false;
            doesntDrop = true;
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && state < 2)
            {
                state = 2;
                sImage = Images.Rice_Top_Stage2;
                blockContainer.UpdateTexture();
            }
            else if (progress >= 2 * (growthTime / 3) && state < 3)
            {
                state = 3;
                sImage = Images.Rice_Top_Stage3;
                blockContainer.UpdateTexture();
            }
            else if (IsReady())
            {
                state = 4;
                sImage = Images.Rice_Top_Stage4;
                blockContainer.UpdateTexture();
            }
        }

        public override void RightClickAction(object sender)
        {
            if (IsReady() && Game.world.player.inventory.GetSelectedHotbarSlot().slot.itemId == "sc:bucket_empty_item")
            {
                //Drop the item and reset the progress without breaking the block
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;
                state = 1;

                Game.world.player.inventory.AddItem("sc:bucket_rice_item", 1, null);
                Game.world.player.inventory.RemoveItem("sc:bucket_empty_item", 1);

                sImage = Images.Rice_Top_Stage1;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class PumpkinCropBlock : CropBlock
    {
        int state = 1;

        public PumpkinCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Pumpkin", "sc:pumpkin_crop_block", 0, "sc:pumpkin_item", Game.rnd.Next(10000, 20000), "sc:pumpkin_seeds_item", "sc:pumpkin_item", 1, 1, Tool.None, Images.Pumpkin_Stage1);
            drops.Add(("sc:pumpkin_seeds_item", 1, 1));
            isSolid = false;
            hasRightClickAction = true;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1400000, 2000001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && state < 2)
            {
                state = 2;
                sImage = Images.Pumpkin_Stage2;
                blockContainer.UpdateTexture();
            }
            else if (progress >= 2 * (growthTime / 3) && state < 3)
            {
                state = 3;
                sImage = Images.Pumpkin_Stage3;
                blockContainer.UpdateTexture();
            }
            else if (IsReady())
            {
                state = 4;
                sImage = Images.Pumpkin_Stage4;
                blockContainer.UpdateTexture();
            }
        }

        public override void RightClickAction(object sender)
        {
            if (IsReady())
            {
                drops.Clear();

                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;
                state = 1;

                drops.Clear();
                drops.Add(("sc:pumpkin_seeds_item", 1, 1));

                sImage = Images.Pumpkin_Stage1;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class CabbageCropBlock : CropBlock
    {
        int state = 1;

        public CabbageCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cabbage", "sc:cabbage_crop_block", 0, "sc:cabbage_seeds_item", Game.rnd.Next(10000, 20000), "sc:cabbage_seeds_item", "sc:cabbage_item", 1, 1, Tool.None, Images.Cabbage_Stage1);
            drops.Add(("sc:cabbage_seeds_item", 1, 1));
            isSolid = false;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1200000, 1800001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && state < 2)
            {
                state = 2;
                sImage = Images.Cabbage_Stage2;
                blockContainer.UpdateTexture();
            }
            else if (progress >= 2 * (growthTime / 3) && state < 3)
            {
                state = 3;
                sImage = Images.Cabbage_Stage3;
                blockContainer.UpdateTexture();
            }
            else if (IsReady())
            {
                state = 4;
                sImage = Images.Cabbage_Stage4;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class PotatoCropBlock : CropBlock
    {
        int state = 1;

        public PotatoCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Potato", "sc:potato_crop_block", 0, "sc:potato_item", Game.rnd.Next(10000, 20000), "sc:potato_item", "sc:potato_item", 1, 3, Tool.None, Images.Potato_Stage1);
            drops.Add(("sc:potato_item", 1, 1));
            isSolid = false;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1400000, 2000001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && state < 2)
            {
                state = 2;
                sImage = Images.Potato_Stage2;
                blockContainer.UpdateTexture();
            }
            else if (progress >= 2 * (growthTime / 3) && state < 3)
            {
                state = 3;
                sImage = Images.Potato_Stage3;
                blockContainer.UpdateTexture();
            }
            else if (IsReady())
            {
                state = 4;
                sImage = Images.Potato_Stage4;
                blockContainer.UpdateTexture();
            }
        }
    }

    public class CucumberCropBlock : CropBlock
    {

        public CucumberCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cucumber", "sc:cucumber_crop_block", 0, "sc:cucumber_item", Game.rnd.Next(10000, 20000), "sc:cucumber_item", "sc:cucumber_item", 1, 2, Tool.None, Images.Cucumber_Stage1);
            drops.Add(("sc:cucumber_item", 1, 1));
            isSolid = false;
            hasRightClickAction = true;
            needsGround = (true, "ground/farmland"); //Game.rnd.Next(1400000, 2000001)
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                sImage = Images.Cucumber_Stage2;
                blockContainer.UpdateTexture();
            }
        }

        public override void RightClickAction(object sender)
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:cucumber _item", 1, 1));

                sImage = Images.Cucumber_Stage1;
                blockContainer.UpdateTexture();
            }
        }
    }
}
