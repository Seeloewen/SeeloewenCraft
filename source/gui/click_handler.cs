namespace SeeloewenCraft
{
    public class ClickHandler
    {

        public void DoRightClick(Block block)
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
                selectedItem.RightClickAction(block, selectedSlot.slot);
                return;
            }

            if (block != null)
            {
                //Check if the block has a foreground block that has an action
                if (block.GetForegroundBlock() != null && block.GetForegroundBlock().hasRightClickAction)
                {
                    block.GetForegroundBlock().RightClickAction();
                }

                //Check if the block has an action
                else if (block.hasRightClickAction)
                {
                    block.RightClickAction();
                }
                //Place the block
                else if (!block.hasRightClickAction && selectedItem != null)
                {
                    Block newBlock = selectedItem.GetBlock();
                    Block blockBelow = block.GetBlockBelow();

                    //Don't place the block if it needs a ground but there is none
                    if(newBlock != null && newBlock.needsGround.doesNeed && !block.CanStayOnBlockBelow(newBlock, blockBelow))
                    {
                        return;
                    }

                    //Check if the block meets all requirements to be placed in foreground of another block
                    if (block.IsInRange() && block.GetForegroundBlock() == null && block.isBackground)
                    {
                        if (newBlock != null)
                        {
                            //If it`s part of a construct, check if it has enough space
                            if (newBlock.isBase && block.ConBlocksHaveSpace(newBlock, true))
                            {
                                block.SetForegroundBlock(newBlock);
                                block.PlaceConnectedForegroundBlocks(newBlock);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }
                            else if (!newBlock.isBase)
                            {
                                block.SetForegroundBlock(newBlock);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }

                        }
                    }
                    //Check if the block isn't in background and can be replaced
                    else if (block.IsInRange() && block.isReplacable && !block.IsCollidingWithPlayer() && !block.isBackground)
                    {
                        if (newBlock != null)
                        {
                            //If it`s part of a construct, check if it has enough space
                            if (newBlock.isBase && block.ConBlocksHaveSpace(newBlock, false))
                            {
                                block.SetBlock(newBlock);
                                block.PlaceConnectedBlocks(newBlock);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }
                            else if (!newBlock.isBase)
                            {
                                block.SetBlock(newBlock);

                                //Remove the item from the inventory
                                selectedSlot.slot.Remove(1);
                                selectedSlot.slot.inventory.UpdateHotbar();
                            }

                            newBlock.tags.Add("placedManually");
                        }
                    }
                }

                block.chunk.GetBlock(block.xPos, block.yPos).AddDebugMenu();
            }
        }

        public void DoLeftClick(Block block)
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
                        block.BreakBlock(true, false, true);
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
                        block.BreakBlock(true, false, true);
                    }
                }

                HotbarSlot selectedSlot = Game.world.player.inventory.GetSelectedHotbarSlot();
                selectedSlot.slot.RemoveDurablity();
            }


            block.chunk.GetBlock(oldXPos, oldYPos).AddDebugMenu();
        }

        private void BreakConstruct(Block baseBlock)
        {
            //Remove all connected blocks that are part of the construct
            foreach (Block conBlock in baseBlock.GetConnectedBlocks(false))
            {
                conBlock.BreakBlock(true, false, true);
            }

            //Finally remove the base
            baseBlock.BreakBlock(true, false, true);
        }

        private void BreakForegroundConstruct(Block baseBlock)
        {
            //Remove all connected blocks that are part of the construct (foreground variant)
            foreach (Block conBlock in baseBlock.GetForegroundBlock().GetConnectedBlocks(false))
            {
                conBlock.BreakBlock(true, false, true);
            }

            //Finally remove the base
            baseBlock.BreakBlock(true, false, true);
        }
    }
}