using System.Windows.Media;

namespace SeeloewenCraft
{
    public class DoorBlock : Block
    {
        public ImageBrush imgOpen;
        public ImageBrush imgClose;
        public bool isOpen;

        public DoorBlock(bool isInBackground) : base(isInBackground)
        {
            state = "closed";
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
            state = "open";

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
            state = "open";

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
            state = "closed";

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
            state = "closed";

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
