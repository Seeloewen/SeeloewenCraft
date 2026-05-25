using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using System;

namespace SeeloewenCraft.game.core.blocks
{
    public record CropProduct(string id, int min, int max);

    public abstract class CropBlock : Block
    {
        protected readonly int timeMin; //in ms
        protected readonly int timeMax; //in ms
        protected readonly int stageMax;

        protected int growthStage;
        internal double growthTime; //in s
        internal double progress = 0;

        protected CropProduct product;

        protected CropBlock(string name, string id, int timeMin, int timeMax, int stages, string itemId = null) : base(name, id, 0, itemId, Tool.None)
        {
            this.timeMax = timeMax;
            this.timeMin = timeMin;
            stageMax = stages;

            growthTime = Game.rnd.Next(timeMin, timeMax) / 1000;
            growthStage = 1;
            isSolid = false;

            WriteTag(BlockTags.CANT_BE_BACKGROUND);
            WriteTag(BlockTags.CAN_BE_AIR_LIGHTSOURCE);
        }


        protected override void AppendJson(JObject obj)
        {
            obj.Add("growth_time", growthTime);
            obj.Add("growth_stage", growthStage);
            obj.Add("progress", progress);
        }

        protected override void LoadAdditionalData(JObject obj)
        {
            growthStage = obj.Get<int>("growth_stage");
            progress = obj.Get<double>("progress");
            growthTime = obj.Get<double>("growth_time");
        }

        public override BlockState GetBlockState()
        {
            //Convert stage to enum that's used globally
            Enum.TryParse($"CROP_{growthStage}", out BlockState state);
            if (state != BlockState.DEFAULT) return state;
            return BlockState.CROP_1;
        }

        public void SetProduct(CropProduct product) => this.product = product;

        public override void AddDebugMenu()
        {
            base.AddDebugMenu();

            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "growthTime", $"{growthTime}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"progress");
        }

        public override void UpdateDebugMenu()
        {
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "growthTime", $"{growthTime} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "progress", $"{progress} ");
            base.UpdateDebugMenu();
        }

        protected override void DoSpecificUpdate(double dt)
        {
            progress += dt;

            //Update stage
            int n = stageMax - 1;
            double step = growthTime / n;
            growthStage = Math.Max(Math.Min((int)Math.Floor((progress / step) + 1.0), n), 1);
        }

        public bool IsReady() => progress >= growthTime;

        public bool HasSpaceAbove(int xOffset, int yOffset, int width, int height)
        {
            //Checks the space above in a rectangular shape, starting from a specific offset from the current block
            for (int i = xOffset; i < width + xOffset; i++)
            {
                for (int j = yOffset; j < height + yOffset; j++)
                {
                    Block block = chunk.GetBlock(posX + i, posY - 1 - j);

                    if (block != null && block.isSolid) return false;
                }
            }

            return true;
        }

        //Hahn war hier
        public override void Drop()
        {
            //drops already contains some base drops. This adds the finished drops as well.
            //If the item is ready, also add the product as a drop
            if (IsReady())
            {
                if (product != null) drops.Add((product.id, product.min, product.max));
                growthStage = 1;
                progress = 0;
                growthTime = Game.rnd.Next(timeMin, timeMax) / 1000;
            }

            base.Drop();
        }
    }
}
