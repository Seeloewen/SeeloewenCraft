
namespace SeeloewenCraft
{
    public class BlockList
    {

        Chunk chunk;
        public Block[] blocks;

        //-- Constructor --//

        public BlockList(Chunk chunk)
        {
            this.chunk = chunk;
            blocks = new Block[600];
        }

        //-- Custom Methods --//

        public void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("blocks");
            writer.WriteStartArray();

            foreach (Block block in blocks)
            {
                block.SaveToJson(writer);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        static public BlockList LoadFromJson(JsonToken documentToken, Chunk chunk, World world)
        {
            BlockList blockList = new BlockList(chunk);

            JsonToken blockArrayToken = documentToken.GetToken("/blocks");

            for (int i = 0; i < 600; i++)
            {
                JsonToken blockToken = blockArrayToken.GetToken($"/{i}");

                Block loadedBlock = Block.LoadFromJson(blockToken, chunk, world);
                blockList.Add(loadedBlock, loadedBlock.xPos, loadedBlock.yPos);
            }
            return blockList;
        }


        public void Add(Block block, int x, int y)
        {
            //Add the block to the index based on x and y pos
            int i = CalcIndex(x, y);
            if ((x <= 7 && x >= 0) && (y >= 0 && y <= 74))
            {
                block.xPos = x;
                block.yPos = y;
                block.chunk = chunk;
                blocks[i] = block;
            }
        }

        public Block Get(int x, int y)
        {
            //Get the block at the index based on x and y pos+
            if ((x <= 7 && x >= 0) && (y >= 0 && y <= 74))
            {
                int i = CalcIndex(x, y);
                return blocks[i];
            }
            return null;
        }

        public Block Get(Block block)
        {
            //Get the block at the index based on x and y pos
            if ((block.xPos <= 7 && block.xPos >= 0) && (block.yPos >= 0 && block.yPos <= 74))

            {
                int i = CalcIndex(block.xPos, block.yPos);
                return blocks[i];
            }
            return null;
        }

        public void Remove(int x, int y)
        {
            //Remove the block at the index based on x and y pos
            if ((x <= 7 && x >= 0) && (y >= 0 && y <= 74))

            {
                int i = CalcIndex(x, y);
                blocks[i] = null;
            }
        }

        public void Remove(Block block)
        {
            //Remove the block at the index based the actual block
            if ((block.xPos <= 7 && block.xPos >= 0) && (block.yPos >= 0 && block.yPos <= 74))
            {
                int i = CalcIndex(block.xPos, block.yPos);
                blocks[i] = null;
            }
        }

        public void Clear()
        {
            //Clear the blocklist
            blocks = new Block[600];
        }

        private int CalcIndex(int x, int y)
        {
            //Calculate the index based on x and y pos
            return x + y * 8;
        }
    }
}
