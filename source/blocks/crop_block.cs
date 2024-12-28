using SeeloewenCraft.gl_rendering;

namespace SeeloewenCraft
{
    public class CropBlock : Block
    {
        public int growthTime;
        public int progress;
        public string seedId;
        public string productId;
        public int productMin;
        public int productMax;

        public CropBlock(bool isBackground) : base(isBackground) { }

        public void Init(string name, string id, int breakTime, string? itemId, int growthTime, string seedId, string productId, int productMin, int productMax, Tool effectiveTool, SealImage sImage)
        {
            base.Init(name, id, breakTime, itemId, effectiveTool, sImage);
            this.seedId = seedId;
            this.productId = productId;
            this.growthTime = growthTime;
            this.productMin = productMin; 
            this.productMax = productMax;
        }

        public override void AddDebugMenu()
        {
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "growthTime", $"{growthTime}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"progress");
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
            //If the item is ready, also add the product as a drop
            if(IsReady())
            {
                drops.Add((productId, productMin, productMax));
            }

            base.Drop(dropForeground);
        }
    }
}
