using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class BlockList
    {

        public Block[] blocks;

        //-- Constructor --//

        public BlockList() {
            blocks = new Block[600];
        }

        //-- Custom Methods --//

        public void Add(Block block)
        {
            //Add the block to the index based on x and y pos
            int i = calcIndex(block.xPos, block.yPos);
            blocks[i] = block;
        }

        public Block Get(int x, int y)
        {
            //Get the block at the index based on x and y pos
            int i = calcIndex(x, y);
            return blocks[i];
        }

        public void Remove(int x, int y) {
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
            return (x-1) + (y-1) * 8;
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
