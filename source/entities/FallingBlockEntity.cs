
namespace SeeloewenCraft.entity
{
    class FallingBlockEntity : Entity
    {
        string blockType;

        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);
            if (onGround)
            {
                Game.world.RemoveEntity(id);
                Block block = BlockRegister.GenerateBlock(blockType);
                int blockX = (posX + 500) / 1000; // +500 is for rounding
                int blockY = (posY + 500) / 1000;
                if (Game.world.GetBlock(blockX, blockY) is AirBlock || Game.world.GetBlock(blockX, blockY) is WaterBlock)
                {
                    Game.world.SetBlock(block, blockX, blockY);
                } else
                {
                    Game.world.AddEntity(new ItemEntity(block.GetItem(), block.GetItem().tag, //item type
                            posX, posY, Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //pos and vel
                }
            }
        }

        protected override void SaveSpecialInfo(JsonWriter writer)
        {
            writer.WritePropertyName("block_type");
            writer.WriteValue(blockType);
        }

        public FallingBlockEntity(int blockX, int blockY, string blockType)
            : base(1000, 1000, blockX * 1000, blockY * 1000, 0, 0, BlockRegister.GenerateBlock(blockType).image)
        {
            type = "FallingBlockEntity";
            this.blockType = blockType;
        }

        public FallingBlockEntity(JsonToken token) 
            : base(token, 1000, 1000, BlockRegister.GenerateBlock(token.GetString("/block_type")).image)
        {
            blockType = token.GetString("/block_type");
        }
    }

}
