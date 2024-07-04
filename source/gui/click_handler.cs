using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class ClickHandler
    {
        World world;

        public ClickHandler(World world)
        {
            this.world = world;
        }

        public void DoRightClick(Block block, object sender)
        {
            //Check if selected item should do an action
            Item selectedItem = world.player.inventory.GetSelectedItem();
            if (selectedItem != null && selectedItem.hasRightClickAction)
            {
                selectedItem.RightClickAction(block, sender);
                return;
            }

            //Check if the block has action, else place it 
            if (block != null && block.hasRightClickAction)
            {
                block.RightClickAction(sender);
            }
            else if (block != null && !block.hasRightClickAction)
            {
                //Check if the block meets all requirements to be placed in foreground of another block
                if (block.IsInRange() && selectedItem != null && selectedItem.canBeForeground && block.foregroundBlock == null && block.isBackground)
                {
                    if (selectedItem.block == null) selectedItem.GenerateBlock(block.xPos, block.yPos, block.chunk, block.isBackground);

                    if (selectedItem.block != null)
                    {
                        //If it`s part of a construct, check if it has enough space
                        if (selectedItem.block.isBase && block.ConnectedBlocksHaveEnoughSpace(selectedItem.block, true))
                        {
                            block.PlaceInForeground(selectedItem.block);
                            block.PlaceConnectedForegroundBlocks(selectedItem.block);

                            //Remove the item from the inventory
                            world.player.inventory.RemoveItem(selectedItem);
                        }
                        else if (!selectedItem.block.isBase)
                        {
                            block.PlaceInForeground(selectedItem.block);

                            //Remove the item from the inventory
                            world.player.inventory.RemoveItem(selectedItem);
                        }

                    }
                }
                //Check if the block isn't in background and can be replaced
                else if (block.IsInRange() && block.isReplacable && !block.IsCollidingWithPlayer(sender) && !block.isBackground && selectedItem != null)
                {
                    if (selectedItem.block == null)
                    {
                        selectedItem.GenerateBlock(block.xPos, block.yPos, block.chunk, block.isBackground);
                    }

                    if (selectedItem.block != null)
                    {
                        //If it`s part of a construct, check if it has enough space
                        if (selectedItem.block.isBase && block.ConnectedBlocksHaveEnoughSpace(selectedItem.block, false))
                        {
                            block.PlaceNewBlock(selectedItem.block);
                            block.PlaceConnectedBlocks(selectedItem.block);

                            //Remove the item from the inventory
                            world.player.inventory.RemoveItem(selectedItem);
                        }
                        else if (!selectedItem.block.isBase)
                        {
                            block.PlaceNewBlock(selectedItem.block);

                            //Remove the item from the inventory
                            world.player.inventory.RemoveItem(selectedItem);
                        }
                    }

                }

                block.chunk.GetBlock(block.xPos, block.yPos).DisplayDebugInformation();
            }
        }

        public void DoLeftClick(Block block, object sender)
        {
            int oldXPos = block.xPos;
            int oldYPos = block.yPos;

            //If the block is in range
            if (block.IsInRange())
            {
                //Check if the block is foreground or background
                if (block.foregroundBlock == null)
                {
                    if (block.isBase)
                    {
                        //If the block is base of construct, also delete the construct blocks
                        foreach (Block conBlock in block.connectedBlocks)
                        {
                            conBlock.BreakBlock(true, false);
                        }
                        block.BreakBlock(true, false);
                    }
                    else if (block.baseBlock != null)
                    {
                        //If the block is part of construct, delete base block
                        block.baseBlock.BreakBlock(true, false);
                        foreach (Block conBlock in block.baseBlock.connectedBlocks)
                        {
                            conBlock.BreakBlock(true, false);
                        }
                    }
                    else block.BreakBlock(true, false);
                }
                else
                {
                    if (block.foregroundBlock.isBase)
                    {
                        //If the block is base of construct, also delete the construct blocks
                        foreach (Block conBlock in block.foregroundBlock.connectedBlocks)
                        {
                            conBlock.chunk.GetBlock(conBlock.xPos, conBlock.yPos).BreakBlock(true, false);
                        }
                        block.BreakBlock(true, false);
                    }
                    else if (block.foregroundBlock.baseBlock != null)
                    {
                        //If the block is part of construct, delete base block
                        block.foregroundBlock.baseBlock.chunk.GetBlock(block.foregroundBlock.baseBlock.xPos, block.foregroundBlock.baseBlock.yPos).BreakBlock(true, false);
                        foreach (Block conBlock in block.foregroundBlock.baseBlock.connectedBlocks)
                        {
                            conBlock.chunk.GetBlock(conBlock.xPos, conBlock.yPos).BreakBlock(true, false);
                        }
                    }
                    else block.BreakBlock(true, false);
                }
            }

            block.chunk.GetBlock(oldXPos, oldYPos).DisplayDebugInformation();
        }
    }
}