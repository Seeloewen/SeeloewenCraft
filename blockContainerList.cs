using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class blockContainerList
    {
        wndGame wndGame;
        List<BlockContainer> containerList = new List<BlockContainer>();
        int chunkIndex = 10000000;

        public blockContainerList(wndGame wndGame)
        {
            this.wndGame = wndGame;
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 76; y++)
                {
                    containerList.Add(new BlockContainer(wndGame, x, y));
                }
            }

        }

        public BlockContainer GetContainer(int x, int y)
        {
            foreach(BlockContainer container in containerList)
            {
                if(container.xPos == x && container.yPos == y)
                {
                    return container;
                }
            }
            return null;
        }

        public bool IsAvailable()
        {
            if (wndGame.GetChunk(chunkIndex) == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void AssignToChunk(Chunk chunk)
        {
            chunkIndex = chunk.index;
            foreach (BlockContainer container in containerList)
            {
                Console.WriteLine($"{container.xPos}, {container.yPos}");
                container.Clear();

                chunk.grdChunk.Children.Add(container.bdrBlock);
                Grid.SetRow(container.bdrBlock, container.yPos - 1);
                Grid.SetColumn(container.bdrBlock, container.xPos - 1);
            }
        }
    }
}
