namespace SeeloewenCraft
{
    public class CropBlock : Block
    {
        public int growthTime;
        public int progress;
        public string seedId;
        public string productId;

        public CropBlock(bool isBackground) : base(isBackground) { }

        public void Init(string name, string id, int breakTime, string? itemId, int growthTime, string seedId, string productId, Tool effectiveTool, SealImage sImage)
        {
            base.Init(name, id, breakTime, itemId, effectiveTool, sImage);
            this.seedId = seedId;
            this.productId = productId;
            this.growthTime = growthTime;
        }

        public override void ShowAdditionalDebugInfo()
        {
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"growthTime={growthTime}");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"progress={progress}");
        }

        public virtual void UpdateProgress(int amount)
        {
            progress += amount;
        }

        public bool IsReady()
        {
            return progress >= growthTime;
        }

        public bool HasSpaceAbove(int xOffset, int yOffset, int width, int height)
        {
            //Checks the space above in a rectangular shape, starting from a specific offset from the current block
            for (int i = xOffset; i < width + xOffset; i++)
            {
                for (int j = yOffset; j < height + yOffset; j++)
                {
                    Block block = chunk.GetBlock(xPos + i, yPos - 1 - j);

                    if (block != null && block.isSolid)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Hahn war hier
        protected override void Drop(bool dropForeground)
        {
            //Sets the item id (which will drop) based on whether it's ready
            itemId = IsReady() ? productId : seedId;

            base.Drop(dropForeground);
        }
    }
}
