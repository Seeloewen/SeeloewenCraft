
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
            HotbarSlot selectedSlot = world.player.inventory.GetSelectedHotbarSlot();
            Item selectedItem = null;

            if (selectedSlot != null && !string.IsNullOrEmpty(selectedSlot.slot.itemId))
            {
                selectedItem = ItemRegister.GenerateItem(selectedSlot.slot.itemId, world);
            }

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
                if (block.IsInRange() && selectedItem != null && block.GetForegroundBlock() == null && block.isBackground)
                {
                    if (selectedItem.block == null) selectedItem.GenerateBlock(block.isBackground);

                    if (selectedItem.block != null)
                    {
                        //If it`s part of a construct, check if it has enough space
                        if (selectedItem.block.isBase && block.ConnectedBlocksHaveEnoughSpace(selectedItem.block, true))
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
                        if (selectedItem.block.isBase && block.ConnectedBlocksHaveEnoughSpace(selectedItem.block, false))
                        {
                            block.PlaceNewBlock(selectedItem.block);
                            block.PlaceConnectedBlocks(selectedItem.block);

                            //Remove the item from the inventory
                            selectedSlot.slot.Remove(1);
                            selectedSlot.slot.inventory.UpdateHotbar();
                        }
                        else if (!selectedItem.block.isBase)
                        {
                            block.PlaceNewBlock(selectedItem.block);

                            //Remove the item from the inventory
                            selectedSlot.slot.Remove(1);
                            selectedSlot.slot.inventory.UpdateHotbar();
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
                if (block.GetForegroundBlock() == null)
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
                    if (block.GetForegroundBlock().isBase)
                    {
                        //If the block is base of construct, also delete the construct blocks
                        foreach (Block conBlock in block.GetForegroundBlock().connectedBlocks)
                        {
                            conBlock.chunk.GetBlock(conBlock.xPos, conBlock.yPos).BreakBlock(true, false);
                        }
                        block.BreakBlock(true, false);
                    }
                    else if (block.GetForegroundBlock().baseBlock != null)
                    {
                        //If the block is part of construct, delete base block
                        block.GetForegroundBlock().baseBlock.chunk.GetBlock(block.GetForegroundBlock().baseBlock.xPos, block.GetForegroundBlock().baseBlock.yPos).BreakBlock(true, false);
                        foreach (Block conBlock in block.GetForegroundBlock().baseBlock.connectedBlocks)
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