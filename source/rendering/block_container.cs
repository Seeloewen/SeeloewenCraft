using Microsoft.Windows.Themes;
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
        public Rectangle rectDarkOverlayNight = new Rectangle();
        public ProgressBar pbBlock = new ProgressBar();
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

            //Setup the light Rectangle
            rectDarkOverlayNight.Width = 50;
            rectDarkOverlayNight.Height = 50;
            rectDarkOverlayNight.Fill = new SolidColorBrush(Colors.Black);
            rectDarkOverlayNight.Opacity = 0;
            rectDarkOverlayNight.Visibility = System.Windows.Visibility.Visible;
            cvsBlock.Children.Add(rectDarkOverlayNight);
        }

        //-- Custom Methods --//

        public void RenderBlock(Block block)
        {

            if (this.block != null)
            {
                //Remove the handlers from the previous container and check if previous block was lightsource
                this.block.RemoveHandlersFromContainer();
                previousBlockWasLightSource = this.block.isLightSource;
            }


            //Pass the new position to the block and make a reference
            this.block = block;
            this.block.xPos = xPos;
            this.block.yPos = yPos;
            this.block.SetContainer(this);

            //If the new block is a workstation that has an action running, show the progressbar
            if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
            {
                block.craftingHandler.ShowBlockProgressbar();
            }

            //Check if the block has a lightsource in range and set lightlevel
            if (world.settings.enableLighting)
            {
                block.SetLightLevel(block.RangeToLightSource());
                block.UpdateNearbyBlocks();
                SetLightOpacity();
                SetNightState(world.nightState);
            }
        }

        public void SetLightOpacity()
        {
            if (block != null && block.blockContainer != null)
            {
                rectDarkOverlayLight.Opacity = block.lightLevel;
            }
        }

        public void SetNightState(int nightState)
        {
            bool hasLightInRange = false;

            foreach (Block block in block.GetBlocksInRange(world.lightRange))
            {
                //If this containers' block is a non-air lightsource, it should make all blocks in its radius visible
                if (block != null && block.blockContainer != null && this.block.isLightSource && this.block.id != "sc:air_block" && block.id != "sc:air_block")
                {
                    block.blockContainer.rectDarkOverlayNight.Opacity = 0;
                    hasLightInRange = true;
                }
                //If it's some other block, check if it has a light source that is not air in its radius
                else if (block != null && block.isLightSource && block.id != "sc:air_block")
                {
                    hasLightInRange = true;
                    break;
                }
            }


            //If the block does not have a light in range and is not a air block, set its state based on the night
            if (!hasLightInRange || block.id == "sc:air_block")
            {
                switch (nightState)
                {
                    case 0:
                        rectDarkOverlayNight.Opacity = 0;
                        break;
                    case 1:
                        rectDarkOverlayNight.Opacity = 0.2;
                        break;
                    case 2:
                        rectDarkOverlayNight.Opacity = 0.4;
                        break;
                    case 3:
                        rectDarkOverlayNight.Opacity = 0.6;
                        break;
                    case 4:
                        rectDarkOverlayNight.Opacity = 0.8;
                        break;
                    case 5:
                        rectDarkOverlayNight.Opacity = 0.95;
                        break;
                }
            }
            else
            {
                rectDarkOverlayNight.Opacity = 0;
            }
        }

        public void RenderForegroundBlock(Block block)
        {
            //If the new block is a workstation that has an action running, show the progressbar
            if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
            {
                block.craftingHandler.ShowBlockProgressbar();
            }

            this.block.foregroundBlock = block;
            block.isForeground = true;
            cvsForegroundBlock.Background = block.image;

            //Check if the block has a lightsource in range and set lightlevel
            if (world.settings.enableLighting)
            {
                this.block.SetLightLevel(this.block.RangeToLightSource());
                this.block.UpdateNearbyBlocks();
                SetLightOpacity();
                SetNightState(world.nightState);
            }
        }

        public void RemoveForegroundBlock()
        {
            if (block != null && block.foregroundBlock != null)
            {
                previousForegroundBlockWasLightSource = block.foregroundBlock.isLightSource;
                block.foregroundBlock = null;
                cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);

                //Check if the block has a lightsource in range and set lightlevel
                if (world.settings.enableLighting)
                {
                    block.SetLightLevel(block.RangeToLightSource());
                    block.UpdateNearbyBlocks();
                    SetLightOpacity();
                    SetNightState(world.nightState);
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
            if (block != null)
            {
                //If it's a workstation that has an action running, hide the progressbar before assigning to new block
                if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
                {
                    block.craftingHandler.HideBlockProgressbar();
                }

                //Remove the event handlers of the previous block
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
                //If it's a workstation that has an action running, hide the progressbar before assigning to new block
                if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
                {
                    block.craftingHandler.HideBlockProgressbar();
                }

                block.blockContainer = null;
            }
            block = null;
        }
    }
}
