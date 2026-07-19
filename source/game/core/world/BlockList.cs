using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.util;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.game.core.world
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

        public JArray ToJson()
        {
            JArray blocksArr = new JArray();

            foreach (Block block in blocks)
            {
                JObject blockObj = block.ToJson();
                blocksArr.Add(blockObj);
            }

            return blocksArr;
        }

        static public BlockList FromJson(JArray blockListArray, Chunk chunk)
        {
            BlockList blockList = new BlockList(chunk);

            foreach (JObject blockObj in blockListArray)
            {   
                Block loadedBlock = Block.FromJson(blockObj);
                loadedBlock.GetForegroundBlock()?.chunk = chunk; 
                blockList.Add(loadedBlock, loadedBlock.posX, loadedBlock.posY);
            }
            return blockList;
        }


        public void Add(Block block, int x, int y)
        {
            //Add the block to the index based on x and y pos
            int i = CalcIndex(x, y);
            if ((x <= 7 && x >= 0) && (y >= 0 && y <= 74))
            {
                block.posX = x;
                block.posY = y;
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
            if ((block.posX <= 7 && block.posX >= 0) && (block.posY >= 0 && block.posY <= 74))

            {
                int i = CalcIndex(block.posX, block.posY);
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
            if ((block.posX <= 7 && block.posX >= 0) && (block.posY >= 0 && block.posY <= 74))
            {
                int i = CalcIndex(block.posX, block.posY);
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
