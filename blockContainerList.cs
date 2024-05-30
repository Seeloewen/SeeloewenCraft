using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class BlockContainerList
    {
        List<BlockContainer> containerList = new List<BlockContainer>();
        wndGame wndGame;
        int chunkIndex = 10000000; //Chunk index needs to be some random (preferebly unused number) to not be Null for the IsAvailable() check

        //-- Constructor --//

        public BlockContainerList(wndGame wndGame)
        {
            //Add all necessary block containers to the list
            this.wndGame = wndGame;
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 76; y++)
                {
                    containerList.Add(new BlockContainer(wndGame, x, y));
                }
            }
        }

        //-- Custom Methods --//

        public BlockContainer GetContainer(int x, int y)
        {
            //Return the container that has the specified coordinates
            foreach(BlockContainer container in containerList)
            {
                if(container.xPos == x && container.yPos == y)
                {
                    return container;
                }
            }
            wndGame.log.Write($"Could not get container for position x{x} y{y}", "Warning");
            return null;
        }

        public bool IsAvailable()
        {
            //Check if a chunk with the index of this list is currently loaded (which means it's not available)
            if (wndGame.GetFromCurrentChunks(chunkIndex) == null)
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
            //Set the new index 
            chunkIndex = chunk.index;

            foreach (BlockContainer container in containerList)
            {
                //Clear each containers from their previous chunks
                container.ClearFromChunk();

                //Add the containers to the chunk grid
                chunk.grdChunk.Children.Add(container.bdrBlock);
                Grid.SetRow(container.bdrBlock, container.yPos - 1);
                Grid.SetColumn(container.bdrBlock, container.xPos - 1);
            }
        }
    }
}
