using SeeloewenCraft.game.graphics;
using System.CodeDom;

namespace SeeloewenCraft.game.core.blocks
{
    public record CropProduct(string id, int min, int max);

    public abstract class CropBlock : Block
    {
        protected int growthState { get => int.Parse(state); set => state = $"{value}"; }

        protected readonly int timeMin;
        protected readonly int timeMax;

        public int growthTime;
        public int progress;
        private CropProduct product;

        protected CropBlock(string name, string id, int timeMin, int timeMax, string itemId = null) : base(name, id, 0, itemId, Tool.None)
        {
            this.timeMax = timeMax;
            this.timeMin = timeMin;

            growthTime = Game.rnd.Next(timeMin, timeMax);
            growthState = 1;
            isSolid = false;

            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }

        public void SetProduct(CropProduct product) => this.product = product;

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();

            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "growthTime", $"{growthTime}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"progress");
        }

        public virtual void UpdateProgress(int amount) => progress += amount;

        public bool IsReady() => progress >= growthTime;

        public bool HasSpaceAbove(int xOffset, int yOffset, int width, int height)
        {
            //Checks the space above in a rectangular shape, starting from a specific offset from the current block
            for (int i = xOffset; i < width + xOffset; i++)
            {
                for (int j = yOffset; j < height + yOffset; j++)
                {
                    Block block = chunk.GetBlock(xPos + i, yPos - 1 - j);

                    if (block != null && block.isSolid) return false;
                }
            }

            return true;
        }

        //Hahn war hier
        protected override void Drop()
        {
            //drops already contains some base drops. This adds the finished drops as well.
            //If the item is ready, also add the product as a drop
            if (IsReady())
            {
                if(product != null) drops.Add((product.id, product.min, product.max));
                growthState = 1;
                progress = 0;
                growthTime = Game.rnd.Next(timeMin, timeMax);
            }

            base.Drop();
        }
    }
}
