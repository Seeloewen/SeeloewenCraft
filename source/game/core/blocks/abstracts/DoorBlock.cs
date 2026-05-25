namespace SeeloewenCraft.game.core.blocks
{
    public abstract class DoorBlock : Block
    {
        public bool isOpen;

        public DoorBlock(string name, string id, string itemId = null) : base(name, id ,500, itemId, Tool.Axe)
        {
            blockState = BlockState.DOOR_CLOSED;
            WriteTag(BlockTags.RIGHTCLICKABLE);
        }

        public override void RightClickAction()
        {
            //Open or close the door, based on the current state and whether it's a foreground block
            if (isOpen)
            {
                if (isForeground)
                {
                    CloseForeground();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                if (isForeground)
                {
                    OpenForeground();
                }
                else
                {
                    Open();
                }
            }
        }

        public void Open()
        {
            //Open the current door block
            isOpen = true;
            isSolid = false;
            blockState = BlockState.DOOR_OPEN;

            //If it's a base block, also open all connected doorblocks
            if (isBase)
            {
                foreach (DoorBlock block in GetConnectedBlocks(false))
                {
                    block.Open();
                }
            }
        }

        public void OpenForeground()
        {
            //Open the current door block
            isOpen = true;
            isSolid = false;
            blockState = BlockState.DOOR_OPEN;

            //If it's a base block, also open all connected doorblocks
            if (isBase)
            {
                foreach (DoorBlock block in GetConnectedBlocks(true))
                {
                    block.OpenForeground();
                }
            }
        }

        public void Close()
        {
            //Close the current door block
            isOpen = false;
            isSolid = true;
            blockState = BlockState.DOOR_CLOSED;

            //If it's a base block, also close all connected doorblocks
            if (isBase)
            {
                foreach (DoorBlock block in GetConnectedBlocks(false))

                {
                    block.Close();
                }
            }
        }

        public void CloseForeground()
        {
            //Close the current door block
            isOpen = false;
            isSolid = true;
            blockState = BlockState.DOOR_CLOSED;

            //If it's a base block, also close all connected doorblocks
            if (isBase)
            {
                foreach (DoorBlock block in GetConnectedBlocks(true))

                {
                    block.CloseForeground();
                }
            }
        }
    }
}
