namespace SeeloewenCraft.game.core.blocks
{
    public class WheatCropBlock : CropBlock
    {
        public WheatCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Wheat", "sc:wheat_crop_block", 0, "sc:seeds_item", Game.rnd.Next(1200000, 1800000), "sc:seeds_item", "sc:wheat_item", 1, 4, Tool.None);
            drops.Add(("sc:seeds_item", 1, 1));
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);

            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && growthState < 2)
            {
                growthState = 2;
            }
            else if (progress >= 2 * (growthTime / 3) && growthState < 3)
            {
                growthState = 3;
            }
            else if (IsReady())
            {
                growthState = 4;
            }
        }
    }

    public class CarrotCropBlock : CropBlock
    {
        public CarrotCropBlock(bool isInBackground) : base(isInBackground)
        {

            Init("Carrot", "sc:carrot_crop_block", 0, "sc:carrot_item", Game.rnd.Next(1200000, 1800000), "sc:carrot_item", "sc:carrot_item", 1, 3, Tool.None);
            drops.Add(("sc:carrot_item", 1, 1));
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);

            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && growthState < 2)
            {
                growthState = 2;
            }
            else if (progress >= 2 * (growthTime / 3) && growthState < 3)
            {
                growthState = 3;
            }
            else if (IsReady())
            {
                growthState = 4;
            }
        }
    }

    public class CottonCropBlock : CropBlock
    {
        public CottonCropBlock(bool isInBackground) : base(isInBackground)
        {

            Init("Cotton", "sc:cotton_crop_block", 0, "sc:cotton_item", Game.rnd.Next(800000, 1200000), "sc:cotton_item", "sc:cotton_item", 1, 2, Tool.None);
            drops.Add(("sc:cotton_item", 1, 1));
            isSolid = false;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);

            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady()) growthState = 2;
        }

        public override void RightClickAction()
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(800000, 1200000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:cotton_item", 1, 1));
            }
        }
    }

    public class BerryBushCropBlock : CropBlock
    {
        public BerryBushCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Berry Bush", "sc:berry_bush_crop_block", 0, "sc:berry_item", Game.rnd.Next(800000, 1500000), "sc:berry_item", "sc:berry_item", 1, 3, Tool.None);
            drops.Add(("sc:berry_item", 1, 1));
            isSolid = false;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_DIRT);

            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady()) growthState = 2;
        }

        public override void RightClickAction()
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(800000, 1500000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:berry_item", 1, 1));
            }
        }
    }

    public class SugarCaneBlock : CropBlock
    {
        protected bool canGrow = true;
        private int maxHeight = 3;

        public SugarCaneBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Sugar Cane", "sc:sugar_cane_block", 0, "sc:sugar_cane_item", Game.rnd.Next(1400000, 2000001), "sc:sugar_cane_item", "sc:sugar_cane_item", 0, 0, Tool.None);
            drops.Add(("sc:sugar_cane_item", 1, 1));
            isSolid = false;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_DIRT);

            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
            WriteTag(BlockTags.CROPS_SUGAR_CANE);

            //Implement additional check for nearby water (maybe)
        }

        public void PlaceBlockAbove(int currentY, int maxY)
        {
            Block blockAbove = GetBlockAbove();

            //Stop if new block would be above max height or block above is null
            if (currentY - 1 < maxY) return;
            if (blockAbove == null) return;

            if (blockAbove.HasTag(BlockTags.REPLACEABLE))
            {
                //Place the new block
                Block newBlockAbove = new SugarCaneBlock(false) { needsGround = (true, BlockTags.CROPS_SUGAR_CANE), canGrow = false };
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

            if (IsReady() && canGrow)
            {
                PlaceBlockAbove(yPos, yPos - maxHeight);
                progress = 0;
                growthTime = Game.rnd.Next(1400000, 2000001);
            }
        }
    }

    public class TomatoCropBlock : CropBlock
    {
        protected bool canGrow = true;
        private int maxHeight = 2;

        public TomatoCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Tomato", "sc:tomato_crop_block", 0, "sc:tomato_item", Game.rnd.Next(1200000, 1600000), "sc:tomato_item", "sc:tomato_item", 1, 3, Tool.None);
            drops.Add(("sc:tomato_item", 1, 1));
            isSolid = false;
            isBase = true;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);

            WriteTag(BlockTags.CROPS_TOMATO);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public void PlaceBlockAbove(int currentY, int maxY)
        {
            Block blockAbove = chunk.GetBlock(xPos, currentY - 1);

            //Stop if new block would be above max height or block above is null
            if (currentY - 1 < maxY || blockAbove == null) return;

            if (blockAbove.isBackground && blockAbove.GetForegroundBlock() == null)
            {
                //Place the new block in foreground
                Block newBlockAbove = new TomatoCropBlock(false) { needsGround = (true, BlockTags.CROPS_TOMATO), canGrow = false };
                blockAbove.SetForegroundBlock(newBlockAbove);

                growthTime = Game.rnd.Next(1200000, 1600000);
                progress = 0;
            }
            else if (blockAbove.HasTag(BlockTags.REPLACEABLE))
            {
                //Place the new block
                Block newBlockAbove = new TomatoCropBlock(false) { needsGround = (true, BlockTags.CROPS_TOMATO), canGrow = false };
                blockAbove.SetBlock(newBlockAbove);

                growthTime = Game.rnd.Next(1200000, 1600000);
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
                growthState = 2;
                if (canGrow)
                {
                    PlaceBlockAbove(yPos, yPos - maxHeight);
                }
            }
        }

        public override void RightClickAction()
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(10000, 20000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:tomato_item", 1, 1));
            }
        }
    }

    public class Rice_Base : Block
    {
        public Rice_Base(bool isInBackground) : base(isInBackground)
        {
            Init("Rice Base", "sc:rice_base", 500, "sc:rice_item", Tool.Axe);
            isBase = true;
            needsGround = (true, BlockTags.GROUNDS_DIRT);
            isSolid = false;
            WriteTag(BlockTags.DOESNT_DROP);

            connectedBlocks.Add((0, -1, "sc:rice_top"));
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }
    }

    public class Rice_Top : CropBlock
    {
        public Rice_Top(bool isInBackground) : base(isInBackground)
        {
            Init("Rice Top", "sc:rice_top", 0, "sc:bucket_rice_item", Game.rnd.Next(1000000, 2000000), "sc:bucket_rice_item", "sc:bucket_rice_item", 1, 1, Tool.None);
            isSolid = false;
            WriteTag(BlockTags.DOESNT_DROP);

            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && growthState < 2)
            {
                growthState = 2;
            }
            else if (progress >= 2 * (growthTime / 3) && growthState < 3)
            {
                growthState = 3;
            }
            else if (IsReady())
            {
                growthState = 4;
            }
        }

        public override void RightClickAction()
        {
            if (IsReady() && Game.world.player.inventory.GetSelectedHotbarSlot().slot.itemId == "sc:bucket_empty_item")
            {
                //Drop the item and reset the progress without breaking the block
                growthTime = Game.rnd.Next(1000000, 2000000);
                progress = 0;
                growthState = 1;

                Game.world.player.inventory.AddItem("sc:bucket_rice_item", 1, null);
                Game.world.player.inventory.RemoveItem("sc:bucket_empty_item", 1);
            }
        }
    }

    public class PumpkinCropBlock : CropBlock
    {
        public PumpkinCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Pumpkin", "sc:pumpkin_crop_block", 0, "sc:pumpkin_item", Game.rnd.Next(1400000, 2000001), "sc:pumpkin_seeds_item", "sc:pumpkin_item", 1, 1, Tool.None);
            drops.Add(("sc:pumpkin_seeds_item", 1, 1));
            isSolid = false;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && growthState < 2)
            {
                growthState = 2;
            }
            else if (progress >= 2 * (growthTime / 3) && growthState < 3)
            {
                growthState = 3;
            }
            else if (IsReady())
            {
                growthState = 4;
            }
        }

        public override void RightClickAction()
        {
            if (IsReady())
            {
                drops.Clear();

                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(1400000, 2000001);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:pumpkin_seeds_item", 1, 1));

            }
        }
    }

    public class CabbageCropBlock : CropBlock
    {
        public CabbageCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cabbage", "sc:cabbage_crop_block", 0, "sc:cabbage_seeds_item", Game.rnd.Next(1200000, 1800001), "sc:cabbage_seeds_item", "sc:cabbage_item", 1, 1, Tool.None);
            drops.Add(("sc:cabbage_seeds_item", 1, 1));
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && growthState < 2)
            {
                growthState = 2;
            }
            else if (progress >= 2 * (growthTime / 3) && growthState < 3)
            {
                growthState = 3;
            }
            else if (IsReady())
            {
                growthState = 4;
            }
        }
    }

    public class PotatoCropBlock : CropBlock
    {
        public PotatoCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Potato", "sc:potato_crop_block", 0, "sc:potato_item", Game.rnd.Next(1400000, 2000001), "sc:potato_item", "sc:potato_item", 1, 3, Tool.None);
            drops.Add(("sc:potato_item", 1, 1));
            isSolid = false;
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (progress >= growthTime / 3 && growthState < 2)
            {
                growthState = 2;
            }
            else if (progress >= 2 * (growthTime / 3) && growthState < 3)
            {
                growthState = 3;
            }
            else if (IsReady())
            {
                growthState = 4;
            }
        }
    }

    public class CucumberCropBlock : CropBlock
    {

        public CucumberCropBlock(bool isInBackground) : base(isInBackground)
        {
            Init("Cucumber", "sc:cucumber_crop_block", 0, "sc:cucumber_item", Game.rnd.Next(1200000, 1500000), "sc:cucumber_item", "sc:cucumber_item", 1, 2, Tool.None);
            drops.Add(("sc:cucumber_item", 1, 1));
            isSolid = false;
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public override void UpdateProgress(int amount)
        {
            base.UpdateProgress(amount);

            if (IsReady())
            {
                growthState = 2;
            }
        }

        public override void RightClickAction()
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();
                growthTime = Game.rnd.Next(1200000, 1500000);
                progress = 0;

                drops.Clear();
                drops.Add(("sc:cucumber _item", 1, 1));
            }
        }
    }
}
