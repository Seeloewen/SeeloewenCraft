using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace SeeloewenCraft
{
    public class BlockContainer
    {
        private World world;
        public Canvas cvsBlock = new Canvas();
        public Canvas cvsForegroundBlock = new Canvas();
        public Canvas cvsBreak = new Canvas();
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

            //Setup foregroundblock canvas
            cvsForegroundBlock.Width = 50;
            cvsForegroundBlock.Height = 50;
            cvsForegroundBlock.Background = new SolidColorBrush(Colors.Transparent);
            cvsBlock.Children.Add(cvsForegroundBlock);
            previousForegroundBlockWasLightSource = false;

            //Setup foregroundblock canvas
            cvsBreak.Width = 50;
            cvsBreak.Height = 50;
            cvsBreak.Background = new SolidColorBrush(Colors.Transparent);
            cvsBlock.Children.Add(cvsBreak);
            cvsBreak.Visibility = Visibility.Hidden;

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
            if (Settings.enableLighting)
            {
                block.SetLightLevel(block.RangeToLightSource());
                block.UpdateNearbyBlocks();
                SetLightOpacity();
                SetNightState(world.nightState);
            }
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
            foreach (Block block in block.GetBlocksInRange(world.lightRange))
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
            foreach (Block block in block.GetBlocksInRange(world.lightRange))
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
            foreach (Block bl in block.GetBlocksInRange(world.lightRange))
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
            switch(state)
            {
                case 0:
                    breakState = 0;
                    cvsBreak.Background = new SolidColorBrush(Colors.Transparent);
                    cvsBreak.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    breakState = 1;
                    cvsBreak.Background = Images.Break_1; ;
                    cvsBreak.Visibility = Visibility.Visible;
                    break;
                case 2:
                    breakState = 2;
                    cvsBreak.Background = Images.Break_2;
                    break;
                case 3:
                    breakState = 3;
                    cvsBreak.Background = Images.Break_3; ;
                    break;
                case 4:
                    breakState = 4;
                    cvsBreak.Background = Images.Break_4; ;
                    break;
                case 5:
                    breakState = 5;
                    cvsBreak.Background = Images.Break_5; ;
                    break;
                default:
                    breakState = 5;
                    cvsBreak.Background = Images.Break_5; ;
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

            this.block.foregroundBlock = block;
            block.isForeground = true;
            cvsForegroundBlock.Background = block.image;

            //Check if the block has a lightsource in range and set lightlevel
            if (Settings.enableLighting)
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
                if (Settings.enableLighting)
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
