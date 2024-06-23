using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class BlockList
    {

        public Block[] blocks;

        //-- Constructor --//

        public BlockList()
        {
            blocks = new Block[600];
        }

        //-- Custom Methods --//

        public void saveToJson(JsonWriter writer)
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


        static public BlockList loadFromJson(JToken documentToken, Chunk chunk, wndGame wndGame)
        {
            BlockList blockList = new BlockList();

            JToken blockArrayToken = new JsonPointer("/blocks").Evaluate(documentToken);

            for (int i = 0; i < 600; i++)
            {
                JToken blockToken = new JsonPointer($"/{i}").Evaluate(blockArrayToken);

                blockList.Add(Block.LoadFromJson(blockToken, chunk, wndGame));
            }
            return blockList;

        }




        public void Add(Block block)
        {
            //Add the block to the index based on x and y pos
            //Check if the coordinate has a container and place the block into that container if possible
            int i = calcIndex(block.xPos, block.yPos);
            blocks[i] = block;
        }

        public void Add(Block block, int x, int y)
        { 
            //Add the block to the index based on x and y pos
            int i = calcIndex(x, y);
            blocks[i] = block;
        }

        public Block Get(int x, int y)
        {
            //Get the block at the index based on x and y pos
            int i = calcIndex(x, y);
            return blocks[i];
        }

        public Block Get(Block block)
        {
            //Get the block at the index based on x and y pos
            int i = calcIndex(block.xPos, block.yPos);
            return blocks[i];
        }

        public void Remove(int x, int y)
        {
            //Remove the block at the index based on x and y pos
            int i = calcIndex(x, y);
            blocks[i] = null;
        }

        public void Remove(Block block)
        {
            //Remove the block at the index based the actual block
            int i = calcIndex(block.xPos, block.yPos);
            blocks[i] = null;
        }

        public void Clear()
        {
            //Clear the blocklist
            blocks = new Block[600];
        }

        private int calcIndex(int x, int y)
        {
            //Calculate the index based on x and y pos
            return (x - 1) + (y - 1) * 8;
        }

        private int calcX(int index) //Not used
        {
            return (index % 8) + 1;
        }

        private int calcY(int index) //Not used
        {
            return (index / 8) - 1;
        }
    }
}
