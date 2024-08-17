
using System.Windows;
using System.Windows.Documents;
using Windows.Web.Http;

namespace SeeloewenCraft
{
    public class ClickHandler
    {

        public void DoRightClick(Block block, object sender)
        {
            //Check if selected item should do an action
            HotbarSlot selectedSlot = Game.world.player.inventory.GetSelectedHotbarSlot();
            Item selectedItem = null;

            if (selectedSlot != null && !string.IsNullOrEmpty(selectedSlot.slot.itemId))
            {
                selectedItem = ItemRegister.GenerateItem(selectedSlot.slot.itemId);
            }

            if (selectedItem != null && selectedItem.hasRightClickAction)
            {
                selectedItem.RightClickAction(block, sender);
                return;
            }

            if (block != null)
            {
                //Check if the block has a foreground block that has an action
                if (block.GetForegroundBlock() != null && block.GetForegroundBlock().hasRightClickAction)
                {
                    block.GetForegroundBlock().RightClickAction(sender);
                }
                //Check if the block has an action
                else if (block.hasRightClickAction)
                {
                    block.RightClickAction(sender);
                }
                //Place the block
                else if (!block.hasRightClickAction)
                {
                    //Check if the block meets all requirements to be placed in foreground of another block
                    if (block.IsInRange() && selectedItem != null && block.GetForegroundBlock() == null && block.isBackground)
                    {
                        if (selectedItem.block == null) selectedItem.GenerateBlock(block.isBackground);

                        if (selectedItem.block != null)
                        {
                            //If it`s part of a construct, check if it has enough space
                            if (selectedItem.block.isBase && block.ConBlocksHaveSpace(selectedItem.block, true))
                            {
                                block.SetForegroundBlock(selectedItem.block);
                                block.PlaceConnectedForegroundBlocks(selectedItem.block);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }
                            else if (!selectedItem.block.isBase)
                            {
                                block.SetForegroundBlock(selectedItem.block);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }

                        }
                    }
                    //Check if the block isn't in background and can be replaced
                    else if (block.IsInRange() && block.isReplacable && !block.IsCollidingWithPlayer(sender) && !block.isBackground && selectedItem != null)
                    {
                        if (selectedItem.block == null)
                        {
                            selectedItem.GenerateBlock(block.isBackground);
                        }

                        if (selectedItem.block != null)
                        {
                            //If it`s part of a construct, check if it has enough space
                            if (selectedItem.block.isBase && block.ConBlocksHaveSpace(selectedItem.block, false))
                            {
                                block.SetBlock(selectedItem.block);
                                block.PlaceConnectedBlocks(selectedItem.block);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }
                            else if (!selectedItem.block.isBase)
                            {
                                block.SetBlock(selectedItem.block);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }
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
                if (block.GetForegroundBlock() == null)
                {
                    //If the block is a base block
                    if (block.isBase)
                    {
                        BreakConstruct(block);
                    }
                    //If the block is part of a construct
                    else if (block.GetBaseBlock() != null)
                    {
                        BreakConstruct(block.GetBaseBlock());

                    }
                    //If it's just a normal block
                    else
                    {
                        block.BreakBlock(true, false);
                    }
                }
                else
                {
                    //If the foreground block is a base block
                    if (block.GetForegroundBlock().isBase)
                    {
                        BreakForegroundConstruct(block);
                    }
                    //If the foreground block is part of a construct
                    else if (block.GetForegroundBlock().GetBaseBlock() != null)
                    {
                        Block baseBlock = block.GetForegroundBlock().GetBaseBlock();
                        BreakForegroundConstruct(baseBlock.chunk.GetBlock(baseBlock.xPos, baseBlock.yPos));
                    }
                    else
                    {
                        //If it's just a normal foreground block
                        block.BreakBlock(true, false);
                    }
                }
            }


            block.chunk.GetBlock(oldXPos, oldYPos).DisplayDebugInformation();
        }

        private void BreakConstruct(Block baseBlock)
        {
            //Remove all connected blocks that are part of the construct
            foreach (Block conBlock in baseBlock.GetConnectedBlocks(false))
            {
                conBlock.BreakBlock(true, false);
            }

            //Finally remove the base
            baseBlock.BreakBlock(true, false);
        }

        private void BreakForegroundConstruct(Block baseBlock)
        {
            //Remove all connected blocks that are part of the construct (foreground variant)
            foreach (Block conBlock in baseBlock.GetForegroundBlock().GetConnectedBlocks(false))
            {
                conBlock.BreakBlock(true, false);
            }

            //Finally remove the base
            baseBlock.BreakBlock(true, false);
        }
    }
}