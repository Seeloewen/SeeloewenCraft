using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class BlockContainer
    {
        private wndGame wndGame;
        public Canvas cvsBlock = new Canvas();
        public Border bdrBlock = new Border();
        public Block block;
        public int xPos;
        public int yPos;

        //-- Constructor --//
        
        public BlockContainer(wndGame wndGame, int xPos, int yPos)
        {
            //Pass the arguments
            this.xPos = xPos;
            this.yPos = yPos;
            this.wndGame = wndGame;

            //Setup the block canvas and border
            bdrBlock.Width = 50;
            bdrBlock.Height = 50;
            bdrBlock.Child = cvsBlock;
        }

        //-- Custom Methods --//

        public void SetBlock(Block block)
        {
            //Remove the event handlers of the previous block
            if (this.block != null)
            {
                this.block.RemoveHandlersFromContainer();
            }
            
            //Pass the new position to the block and make a reference
            this.block = block;
            this.block.xPos = xPos;
            this.block.yPos = yPos;
            this.block.SetContainer(this);
        }

        public void ClearFromChunk()
        {
            //Remove the event handlers of the previous block
            if (block != null)
            {
                block.RemoveHandlersFromContainer();
            }

            wndGame.RemoveFromParent(bdrBlock);
            block = null;
        }

        public void Clear()
        {
            if (block != null)
            {
                block.blockContainer = null;
            }
            block = null;
        }
    }
}
