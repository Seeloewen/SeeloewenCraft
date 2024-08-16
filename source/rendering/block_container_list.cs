using System.Collections.Generic;
using System.Windows.Controls;

namespace SeeloewenCraft
{
    public class BlockContainerList
    {
        List<BlockContainer> containerList = new List<BlockContainer>();
        
        int chunkIndex = 10000000; //Chunk index needs to be some random (preferebly unused number) to not be Null for the IsAvailable() check

        //-- Constructor --//

        public BlockContainerList()
        {
            //Add all necessary block containers to the list
            
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 75; y++)
                {
                    containerList.Add(new BlockContainer( x, y));
                }
            }
        }

        //-- Custom Methods --//

        public BlockContainer GetContainer(int x, int y)
        {
            //Return the container that has the specified coordinates
            foreach (BlockContainer container in containerList)
            {
                if (container.xPos == x && container.yPos == y)
                {
                    return container;
                }
            }
            Log.Write($"Could not get container for position x{x} y{y}", "Warning");
            return null;
        }

        public bool IsAvailable()
        {
            //Check if a chunk with the index of this list is currently loaded (which means it's not available)
            if (Game.world.GetLoadedChunk(chunkIndex) == null)
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
                Grid.SetRow(container.bdrBlock, container.yPos);
                Grid.SetColumn(container.bdrBlock, container.xPos);
            }
        }
    }
}
