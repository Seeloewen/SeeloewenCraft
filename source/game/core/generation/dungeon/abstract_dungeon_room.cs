using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public abstract class DungeonRoom
    {
        public List<DungeonBlock> blocks = new List<DungeonBlock>();
        protected Random rnd;
        public DungeonType type;
        public string id;

        //-- Custom Methods --//

        public DungeonRoom(Random chunkRnd)
        {
            rnd = chunkRnd;
        }

        public (int top, int bottom, int right, int left) GetNecessarySpace(int x, int y)
        {
            int top = 0;
            int bottom = 0;
            int right = 0;
            int left = 0;
            int i;

            //Get space above
            i = 1;
            while (true)
            {
                if (GetBlock(x, y + i) != null)
                {
                    top++;
                }
                else
                {
                    break;
                }
                i++;
            }

            //Get space below
            i = 1;
            while (true)
            {
                if (GetBlock(x, y - i) != null)
                {
                    bottom++;
                }
                else
                {
                    break;
                }
                i++;
            }

            //Get space to the right
            i = 1;
            while (true)
            {
                if (GetBlock(x + i, y) != null)
                {
                    right++;
                }
                else
                {
                    break;
                }
                i++;
            }

            //Get space to the left
            i = 1;
            while (true)
            {
                if (GetBlock(x - i, y) != null)
                {
                    left++;
                }
                else
                {
                    break;
                }
                i++;
            }

            return (top, bottom, right, left);
        }
        public DungeonBlock GetBlock(int x, int y)
        {
            //Compare x and y pos and return the correct block
            foreach (DungeonBlock block in blocks)
            {
                if (block.x == x && block.y == y)
                {
                    return block;
                }
            }
            return null;
        }

        public void SetDoor(int x, int y, Direction dir)
        {
            //Replace the block with an air-door (later maybe a real door?)
            GetBlock(x, y).SetDoor(type, dir, GetBlock(x, y + 1));
        }

        public void CreateBasicShape(int width, int height, string blockBackgroundId, string blockFrameId, string altBlockFrameId)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Place the background block
                    Block backBlock = BlockRegister.GenerateBlock(blockBackgroundId);
                    backBlock.MoveToBackground();
                    blocks.Add(new DungeonBlock(x, y) { block = backBlock, isOccupied = true });

                    //Place frame blocks
                    if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                    {
                        if (rnd.Next(0, 3) > 0)
                        {
                            GetBlock(x, y).block.SetForegroundBlock(BlockRegister.GenerateBlock(blockFrameId));
                        }
                        else
                        {
                            GetBlock(x, y).block.SetForegroundBlock(BlockRegister.GenerateBlock(altBlockFrameId));
                        }
                    }
                }
            }
        }
    }
}
