using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace SeeloewenCraft
{
    public class BlockContainer
    {
        private World world;
        public Canvas cvsBlock = new Canvas();
        public Canvas cvsForegroundBlock = new Canvas();
        public Border bdrBlock = new Border();
        public Rectangle rectDarkOverlay = new Rectangle();
        public Rectangle rectDarkOverlayLight = new Rectangle();
        public Block block;
        public int xPos;
        public int yPos;
        public bool previousBlockWasLightSource;
        public bool previousForegroundBlockWasLightSource;


        //-- Constructor --//

        public BlockContainer(World world, int xPos, int yPos)
        {
            //Pass the arguments
            this.xPos = xPos;
            this.yPos = yPos;
            this.world = world;

            //Setup the block canvas and border
            bdrBlock.Width = 50;
            bdrBlock.Height = 50;
            bdrBlock.Child = cvsBlock;

            //Setup the background Rectangle
            rectDarkOverlay.Width = 50;
            rectDarkOverlay.Height = 50;
            rectDarkOverlay.Fill = new SolidColorBrush(Colors.Black);
            rectDarkOverlay.Opacity = 0.3;
            rectDarkOverlay.Visibility = System.Windows.Visibility.Hidden;
            cvsBlock.Children.Add(rectDarkOverlay);

            cvsForegroundBlock.Width = 50;
            cvsForegroundBlock.Height = 50;
            cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);
            cvsBlock.Children.Add(cvsForegroundBlock);
            previousForegroundBlockWasLightSource = false;

            //Setup the light Rectangle
            rectDarkOverlayLight.Width = 50;
            rectDarkOverlayLight.Height = 50;
            rectDarkOverlayLight.Fill = new SolidColorBrush(Colors.Black);
            rectDarkOverlayLight.Opacity = 0;
            rectDarkOverlayLight.Visibility = System.Windows.Visibility.Visible;
            cvsBlock.Children.Add(rectDarkOverlayLight);

        }

        //-- Custom Methods --//

        public void RenderBlock(Block block)
        {
            //Remove the event handlers of the previous block
            if (this.block != null)
            {
                this.block.RemoveHandlersFromContainer();
            }

            //Pass the new position to the block and make a reference
            if (this.block != null)
            {
                previousBlockWasLightSource = this.block.isLightSource;
            }

            this.block = block;
            this.block.xPos = xPos;
            this.block.yPos = yPos;
            this.block.SetContainer(this);

            //Check if the block has a lightsource in range and set lightlevel
            if (Properties.Settings.Default.enableLighting)
            {
                block.SetLightLevel(block.RangeToLightSource());
                block.UpdateNearbyBlocks();
                SetLightOpacity();
            }
        }

        public void SetLightOpacity()
        {
            if (block != null)
            {
                rectDarkOverlayLight.Opacity = block.lightLevel;
            }
        }

        public void RenderForegroundBlock(Block block)
        {
            this.block.foregroundBlock = block;
            block.isForeground = true;
            cvsForegroundBlock.Background = block.image;

            //Check if the block has a lightsource in range and set lightlevel
            if (Properties.Settings.Default.enableLighting)
            {
                this.block.SetLightLevel(this.block.RangeToLightSource());
                this.block.UpdateNearbyBlocks();
                SetLightOpacity();
            }
        }

        public void RemoveForegroundBlock()
        {
            if(block != null && block.foregroundBlock != null)
            {
                previousForegroundBlockWasLightSource = block.foregroundBlock.isLightSource;
                block.foregroundBlock = null;
                cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);

                //Check if the block has a lightsource in range and set lightlevel
                if (Properties.Settings.Default.enableLighting)
                {
                    block.SetLightLevel(block.RangeToLightSource());
                    block.UpdateNearbyBlocks();
                    SetLightOpacity();
                }
            }
        }

        public void ShowDarkRectangle()
        {
            rectDarkOverlay.Visibility = System.Windows.Visibility.Visible;
        }

        public void HideDarkRectangle()
        {
            rectDarkOverlay.Visibility = System.Windows.Visibility.Hidden;
        }

        public void ClearFromChunk()
        {
            //Remove the event handlers of the previous block
            if (block != null)
            {
                block.RemoveHandlersFromContainer();
            }

            world.wndGame.RemoveFromParent(bdrBlock);
            RemoveForegroundBlock();
            block = null;
            previousBlockWasLightSource = false;
        }

        public void Clear()
        {
            //Remove the link between container and block
            RemoveForegroundBlock();
            if (block != null)
            {
                block.blockContainer = null;
            }
            block = null;
        }
    }
}
