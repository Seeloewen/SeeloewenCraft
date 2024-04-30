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
        wndGame wndGame;
        public ImageBrush imageBrush = new ImageBrush();
        public Canvas cvsBlock = new Canvas();
        public Border bdrBlock = new Border();
        public Block block;
        public int xPos;
        public int yPos;

        public BlockContainer(wndGame wndGame, int xPos, int yPos)
        {
            //Setup the block canvas and border
            bdrBlock.Width = 50;
            bdrBlock.Height = 50;
            bdrBlock.Child = cvsBlock;
            cvsBlock.Background = imageBrush;

            this.xPos = xPos;
            this.yPos = yPos;
            this.wndGame = wndGame;
        }

        public void SetBlock(Block block)
        {
            this.block = block;
            this.block.SetContainer(this);
        }

        public void Clear()
        {
            wndGame.RemoveFromParent(bdrBlock);
            block = null;
        }

    }
}
