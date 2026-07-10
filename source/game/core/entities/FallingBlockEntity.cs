using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.entities
{
    class FallingBlockEntity : Entity
    {
        string blockType;

        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);
            if (onGround)
            {
                World.Get().RemoveEntity(id);
                Block block = BlockRegister.Get(blockType);
                int blockX = (posX + 500) / 1000; // +500 is for rounding
                int blockY = (posY + 500) / 1000;
                PositionData pos = PositionData.FromGlobalX(blockX, blockY);

                if (World.Get().GetBlock(pos) is AirBlock || World.Get().GetBlock(pos) is WaterBlock)
                {
                    World.Get().SetBlock(pos, block);
                }
                else
                {
                    World.Get().AddEntity(new ItemEntity(block.GetItem(), block.GetItem().tag, //item type
                            posX, posY, Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //pos and vel
                }
            }
        }

        protected override void SaveSpecialInfo(JObject obj)
        {
            obj.Add("block_type", blockType);
        }

        public FallingBlockEntity(int blockX, int blockY, string blockType)
            : base(1000, 1000, blockX * 1000, blockY * 1000, 0, 0)
        {
            type = "FallingBlockEntity";
            this.blockType = blockType;
        }

        public FallingBlockEntity(JObject obj)
            : base(obj, 1000, 1000)
        {
            blockType = obj.Get<string>("block_type");
        }
    }

}
