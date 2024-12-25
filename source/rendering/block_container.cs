using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace SeeloewenCraft
{
    public class BlockContainer
    {
        public Canvas cvsBlock = new Canvas();
        public Canvas cvsForegroundBlock = new Canvas();
        public Canvas cvsChanges = new Canvas();
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
        public int breakState = 0;
        public int hammerState = 0;


        //-- Constructor --//

        public BlockContainer(int xPos, int yPos)
        {
            //Pass the arguments
            this.xPos = xPos;
            this.yPos = yPos;

            //Setup the block canvas and border
            bdrBlock.Width = 50;
            bdrBlock.Height = 50;
            bdrBlock.Child = cvsBlock;

            //Setup the background Rectangle
            rectDarkOverlay.Width = 50;
            rectDarkOverlay.Height = 50;
            rectDarkOverlay.Fill = new SolidColorBrush(Colors.Black);
            rectDarkOverlay.Opacity = 0.3;
            rectDarkOverlay.Visibility = Visibility.Hidden;
            cvsBlock.Children.Add(rectDarkOverlay);

            //Setup foregroundblock canvas
            cvsForegroundBlock.Width = 50;
            cvsForegroundBlock.Height = 50;
            cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);
            cvsBlock.Children.Add(cvsForegroundBlock);
            previousForegroundBlockWasLightSource = false;

            //Setup foregroundblock canvas
            cvsChanges.Width = 50;
            cvsChanges.Height = 50;
            cvsChanges.Background = new SolidColorBrush(Colors.Transparent);
            cvsBlock.Children.Add(cvsChanges);
            cvsChanges.Visibility = Visibility.Hidden;

            //Setup the light Rectangle
            rectDarkOverlayLight.Width = 50;
            rectDarkOverlayLight.Height = 50;
            rectDarkOverlayLight.Fill = new SolidColorBrush(Colors.Black);
            rectDarkOverlayLight.Opacity = 0;
            rectDarkOverlayLight.Visibility = Visibility.Visible;
            cvsBlock.Children.Add(rectDarkOverlayLight);

            //Setup the light Rectangle
            rectDarkOverlayNight.Width = 50;
            rectDarkOverlayNight.Height = 50;
            rectDarkOverlayNight.Fill = new SolidColorBrush(Colors.Black);
            rectDarkOverlayNight.Opacity = 0;
            rectDarkOverlayNight.Visibility = Visibility.Visible;
            cvsBlock.Children.Add(rectDarkOverlayNight);
        }

        //-- Custom Methods --//

        public void RenderBlock(Block block)
        {
            ClearRender();

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

            UpdateLighting();
        }

        public void SetLightOpacity()
        {
            //Set blocks light opacity based on light level
            if (block != null && block.blockContainer != null)
            {
                rectDarkOverlayLight.Opacity = block.lightLevel;
            }
        }

        public void SetNightState(int nightState)
        {
            if (block != null && block.blockContainer != null)
            {
                if (nightState == 0)
                {
                    //If it's not night, all further checks would be unnecessary
                    block.blockContainer.rectDarkOverlayNight.Opacity = 0;
                }
                else //If it's some form of night
                {
                    //If this container's block was previously a light source but is no longer one, all blocks in range need to be checked if they have a lightsource. If not, update them accordingly
                    if ((previousBlockWasLightSource || previousForegroundBlockWasLightSource) && !block.IsLightSource(true))
                    {
                        UpdateNearbyBlocksNightState(nightState);
                    }

                    //If this container's block is a non-air lightsource, it should make all blocks in its radius visible
                    if (block.IsLightSource(true))
                    {
                        block.blockContainer.rectDarkOverlayNight.Opacity = 0;
                        NightLightUpNearbyBlocks();
                    }

                    //If it's some other block, check if it has a light source that is not air in its radius
                    if (!block.IsLightSource(true))
                    {
                        if (HasLightInRange(block) && block.id != "sc:air_block")
                        {
                            block.blockContainer.rectDarkOverlayNight.Opacity = 0;
                        }
                        else
                        {
                            block.blockContainer.rectDarkOverlayNight.Opacity = GetNightOpacity(nightState);
                        }
                    }
                }
            }
        }

        private void UpdateNearbyBlocksNightState(int nightState)
        {
            //Go through each block nearby and check if it has a light in range
            foreach (Block block in block.GetBlocksInRange(Game.world.lightRange))
            {
                if (block != null && block.blockContainer != null)
                {
                    //Update the blocks nearby based on if they have light in range or not
                    if ((HasLightInRange(block) && block.id != "sc:air_block") || block.IsLightSource(true))
                    {
                        block.blockContainer.rectDarkOverlayNight.Opacity = 0;
                    }
                    else
                    {
                        block.blockContainer.rectDarkOverlayNight.Opacity = GetNightOpacity(nightState);
                    }
                }
            }
        }

        private void NightLightUpNearbyBlocks()
        {
            //Update all blocks nearby to have light state during night
            block.blockContainer.rectDarkOverlayNight.Opacity = 0;
            foreach (Block block in block.GetBlocksInRange(Game.world.lightRange))
            {
                if (block != null && block.blockContainer != null && block.id != "sc:air_block")
                {
                    block.blockContainer.rectDarkOverlayNight.Opacity = 0;
                }
            }
        }

        private bool HasLightInRange(Block block) //Note that air blocks do not count as light sources in this check because this is for night lighting
        {
            //Check each block in range if it's a light source
            foreach (Block bl in block.GetBlocksInRange(Game.world.lightRange))
            {
                //If the block is a lightsource or its foregoundblock is a lightsource
                if (bl != null && bl.IsLightSource(true))
                {
                    return true;
                }
            }
            return false;
        }

        public void SetBreakState(int state)
        {
            switch (state)
            {
                case 0:
                    breakState = 0;
                    cvsChanges.Background = new SolidColorBrush(Colors.Transparent);
                    cvsChanges.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    breakState = 1;
                    cvsChanges.Background = Images.Break_1.GetTexture(); ;
                    cvsChanges.Visibility = Visibility.Visible;
                    break;
                case 2:
                    breakState = 2;
                    cvsChanges.Background = Images.Break_2.GetTexture();
                    break;
                case 3:
                    breakState = 3;
                    cvsChanges.Background = Images.Break_3.GetTexture(); ;
                    break;
                case 4:
                    breakState = 4;
                    cvsChanges.Background = Images.Break_4.GetTexture(); ;
                    break;
                case 5:
                    breakState = 5;
                    cvsChanges.Background = Images.Break_5.GetTexture(); ;
                    break;
                default:
                    breakState = 5;
                    cvsChanges.Background = Images.Break_5.GetTexture(); ;
                    break;
            }
        }

        public void SetHammerState(int state)
        {
            switch (state)
            {
                case 0:
                    hammerState = 0;
                    cvsChanges.Background = new SolidColorBrush(Colors.Transparent);
                    cvsChanges.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    hammerState = 1;
                    cvsChanges.Background = Images.Hammer_1.GetTexture(); ;
                    cvsChanges.Visibility = Visibility.Visible;
                    break;
                case 2:
                    hammerState = 2;
                    cvsChanges.Background = Images.Hammer_2.GetTexture();
                    break;
                case 3:
                    hammerState = 3;
                    cvsChanges.Background = Images.Hammer_3.GetTexture(); ;
                    break;
                case 4:
                    hammerState = 4;
                    cvsChanges.Background = Images.Hammer_4.GetTexture(); ;
                    break;
                case 5:
                    hammerState = 5;
                    cvsChanges.Background = Images.Hammer_5.GetTexture(); ;
                    break;
                default:
                    hammerState = 5;
                    cvsChanges.Background = Images.Hammer_5.GetTexture(); ;
                    break;
            }
        }

        public double GetNightOpacity(int nightState)
        {
            //Return opacity based on nightState
            switch (nightState)
            {
                case 0:
                    return 0;
                case 1:
                    return 0.2;
                case 2:
                    return 0.4;
                case 3:
                    return 0.6;
                case 4:
                    return 0.8;
                case 5:
                    return 0.95;
                default:
                    return 0;
            }
        }

        public void RenderForegroundBlock(Block block)
        {
            //If the new block is a workstation that has an action running, show the progressbar
            if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
            {
                block.craftingHandler.ShowBlockProgressbar();
            }

            cvsForegroundBlock.Background = block.image;
            UpdateLighting();
        }

        public void UnrenderForegroundBlock()
        {
            if (block != null && block.GetForegroundBlock() != null)
            {
                previousForegroundBlockWasLightSource = block.GetForegroundBlock().isLightSource;
                cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        public void UpdateLighting()
        {
            //Check if the block has a lightsource in range and set lightlevel
            if (!StartOptions.disableLighting)
            {
                block.SetLightLevel(block.RangeToLightSource());
                block.UpdateNearbyBlocks();
                SetLightOpacity();
                SetNightState(Game.world.nightState);
            }
        }

        public void UpdateTexture()
        {
            if (block != null)
            {
                cvsBlock.Background = block.sImage.GetTexture();
            }

            if (block.GetForegroundBlock() != null)
            {
                cvsForegroundBlock.Background = block.GetForegroundBlock().sImage.GetTexture();
            }
        }

        public void ShowDarkRectangle()
        {
            rectDarkOverlay.Visibility = Visibility.Visible;
        }

        public void HideDarkRectangle()
        {
            rectDarkOverlay.Visibility = Visibility.Hidden;
        }

        public void ClearFromChunk()
        {
            if (block != null)
            {
                //If it's a workstation that has an action running, hide the progressbar
                if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
                {
                    block.craftingHandler.HideBlockProgressbar();
                }

                //Remove the event handlers, like click events
                block.RemoveHandlersFromContainer();
            }

            //Clear the link and unrender everything
            ClearLink();
            ClearRender();

            //Remove the border from the canvas so it can be assigned to a new one
            Game.world.wndGame.RemoveFromParent(bdrBlock);
        }

        public void ClearLink()
        {
            //Remove the link between block and container
            if (block != null)
            {
                block.blockContainer = null;
            }
            block = null;
            previousBlockWasLightSource = false;
            previousForegroundBlockWasLightSource = false;
        }

        public void ClearRender()
        {
            //Clear the display slot
            UnrenderForegroundBlock();

            cvsBlock.Background = new SolidColorBrush(Colors.Transparent);
            cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);

            if (block != null)
            {
                //If it's a workstation that has an action running, hide the progressbar before assigning to new block
                if (block.craftingHandler != null && (block.craftingHandler.recipeRunning || block.craftingHandler.recipeClaimable))
                {
                    block.craftingHandler.HideBlockProgressbar();
                }
            }
        }
    }
}
