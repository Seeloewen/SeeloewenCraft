

using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class BlockList
    {

        public Block[] blocks;

        public BlockList() {
            blocks = new Block[600];
        }


        public void Add(Block block)
        {
            int i = calcIndex(block.xPos, block.yPos);
            blocks[i] = block;
        }

        public Block Get(int x, int y)
        {
            int i = calcIndex(x, y);
            return blocks[i];
        }

        public void Remove(int x, int y) {
            int i = calcIndex(x, y);
            blocks[i] = null;
        }

        public void Remove(Block block)
        {
            int i = calcIndex(block.xPos, block.yPos);
            blocks[i] = null;
        }

        public void Clear()
        {
            blocks = new Block[600];
        }


        private int calcIndex(int x, int y)
        {
            return (x-1) + (y-1) * 8;
        }

        private int calcX(int index)
        {
            return (index % 8) + 1;
        }

        private int calcY(int index)
        {
            return (index / 8) - 1;
        }



    }
}
