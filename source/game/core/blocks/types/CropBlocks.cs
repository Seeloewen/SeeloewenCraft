namespace SeeloewenCraft.game.core.blocks
{
    public class WheatCropBlock : CropBlock
    {
        public WheatCropBlock() : base("Wheat", "sc:wheat_crop_block", 1200000, 1800000, "sc:seeds_item")
        {
            drops.Add(("sc:seeds_item", 1, 1));
            SetProduct(new CropProduct("sc:wheat_item", 1, 4));
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
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
        public CarrotCropBlock() : base("Carrot", "sc:carrot_crop_block", 1200000, 1800000, "sc:carrot_item")
        {
            drops.Add(("sc:carrot_item", 1, 1));
            SetProduct(new CropProduct("sc:carrot_item", 1, 3));
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
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
        public CottonCropBlock() : base("Cotton", "sc:cotton_crop_block", 800000, 1200000, "sc:cotton_item")
        {
            drops.Add(("sc:cotton_item", 1, 1));
            SetProduct(new CropProduct("sc:cotton_item", 1, 2));
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
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

                drops.Clear();
                drops.Add(("sc:cotton_item", 1, 1));
            }
        }
    }

    public class BerryBushCropBlock : CropBlock
    {
        public BerryBushCropBlock() : base("Berry Bush", "sc:berry_bush_crop_block", 800000, 1500000, "sc:berry_item")
        {
            drops.Add(("sc:berry_item", 1, 1));
            SetProduct(new CropProduct("sc:berry_item", 1, 3));
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_DIRT);
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

                drops.Clear();
                drops.Add(("sc:berry_item", 1, 1));
            }
        }
    }

    public class SugarCaneBlock : CropBlock
    {
        protected bool canGrow = true;
        private const int maxHeight = 3;

        public SugarCaneBlock() : base("Sugar Cane", "sc:sugar_cane_block", 1400000, 2000000, "sc:sugar_cane_item")
        {
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_DIRT);

            WriteTag(BlockTags.CROPS_SUGAR_CANE);

            //Implement additional check for nearby water (maybe)
        }

        public void PlaceBlockAbove(int currentY, int maxY)
        {
            Block blockAbove = GetBlockAbove();

            //Stop if new block would be above max height or block above is null
            if (currentY - 1 < maxY || blockAbove == null) return;

            if (blockAbove.HasTag(BlockTags.REPLACEABLE))
            {
                //Place the new block
                SugarCaneBlock newBlockAbove = (SugarCaneBlock)BlockRegister.Get(id);
                newBlockAbove.needsGround = (true, BlockTags.CROPS_SUGAR_CANE);
                newBlockAbove.canGrow = false;
                blockAbove.Replace(newBlockAbove);
            }
            else if (blockAbove.id == id)
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
                growthTime = Game.rnd.Next(timeMin, timeMax);
            }
        }
    }

    public class TomatoCropBlock : CropBlock
    {
        protected bool canGrow = true;
        private const int maxHeight = 2;

        public TomatoCropBlock() : base("Tomato", "sc:tomato_crop_block", 1200000, 1600000, "sc:tomato_item")
        {
            drops.Add(("sc:tomato_item", 1, 1));
            SetProduct(new CropProduct("sc:tomato_item", 1, 3));
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);

            WriteTag(BlockTags.RIGHTCLICKABLE);
            WriteTag(BlockTags.CROPS_TOMATO);
        }

        public void PlaceBlockAbove(int currentY, int maxY)
        {
            Block blockAbove = GetBlockAbove();

            //Stop if new block would be above max height or block above is null
            if (currentY - 1 < maxY || blockAbove == null) return;

            if (blockAbove.HasTag(BlockTags.REPLACEABLE))
            {
                //Place the new block
                TomatoCropBlock newBlockAbove = (TomatoCropBlock)BlockRegister.Get(id);
                newBlockAbove.needsGround = (true, BlockTags.CROPS_TOMATO);
                newBlockAbove.canGrow = false;
                blockAbove.Replace(newBlockAbove);
            }
            else if (blockAbove.id == id)
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
                growthState = 2; //Relevant for harvesting the crop

                if (canGrow)
                {
                    PlaceBlockAbove(yPos, yPos - maxHeight);
                    growthTime = Game.rnd.Next(timeMin, timeMax);
                    progress = 0;
                }
            }
        }

        public override void RightClickAction()
        {
            if (IsReady())
            {
                //Drop the item and reset the progress without breaking the block
                Drop();

                drops.Clear();
                drops.Add(("sc:tomato_item", 1, 1));
            }
        }
    }

    public class Rice_Base : Block
    {
        public Rice_Base() : base("Rice Base", "sc:rice_base_crop_block", 0, "sc:rice_item")
        {
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
        public Rice_Top() : base("Rice Top", "sc:rice_top_crop_block", 1000000, 2000000, "sc:bucket_rice_item")
        {
            WriteTag(BlockTags.DOESNT_DROP);
            baseBlock = (0, 1);
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
                growthTime = Game.rnd.Next(timeMin, timeMax);
                progress = 0;
                growthState = 1;

                Game.world.player.inventory.AddItem("sc:bucket_rice_item", 1, null);
                Game.world.player.inventory.RemoveItem("sc:bucket_empty_item", 1);
            }
        }
    }

    public class PumpkinCropBlock : CropBlock
    {
        public PumpkinCropBlock() : base("Pumpkin", "sc:pumpkin_crop_block", 1400000, 2000000, "sc:pumpkin_seeds_item")
        {
            drops.Add(("sc:pumpkin_seeds_item", 1, 1));
            SetProduct(new CropProduct("sc:pumpkin_item", 1, 1));
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
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
                drops.Clear(); //Remove seeds from drops

                //Drop the item and reset the progress without breaking the block
                Drop();

                drops.Clear();
                drops.Add(("sc:pumpkin_seeds_item", 1, 1));
            }
        }
    }

    public class CabbageCropBlock : CropBlock
    {
        public CabbageCropBlock() : base("Cabbage", "sc:cabbage_crop_block",1200000, 1800000, "sc:cabbage_seeds_item")
        {
            drops.Add(("sc:cabbage_seeds_item", 1, 1));
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
            SetProduct(new CropProduct("sc:cabbage_item", 1, 1));
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
        public PotatoCropBlock() : base("Potato", "sc:potato_crop_block", 1400000, 2000000, "sc:potato_item")
        {
            drops.Add(("sc:potato_item", 1, 1));
            SetProduct(new CropProduct("sc:potato_item", 1, 3));
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
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

        public CucumberCropBlock() : base("Cucumber", "sc:cucumber_crop_block", 1200000, 1500000, "sc:cucumber_item")
        {
            drops.Add(("sc:cucumber_item", 1, 1));
            SetProduct(new CropProduct("sc:cucumber_item", 1, 2));
            WriteTag(BlockTags.RIGHTCLICKABLE);
            needsGround = (true, BlockTags.GROUNDS_FARMLAND);
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

                drops.Clear();
                drops.Add(("sc:cucumber_item", 1, 1));
            }
        }
    }
}
