using System.Windows.Media;

namespace SeeloewenCraft
{
    public class DoorBlock : Block
    {
        public ImageBrush imgOpen;
        public ImageBrush imgClose;
        public bool isOpen;

        public DoorBlock( bool isInBackground) : base( isInBackground) { }

        public override void RightClickAction(object sender)
        {
            //Open or close the door, based on the current state
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Open()
        {
            //Open the current door block
            isOpen = true;
            image = imgOpen;
            blockContainer.cvsBlock.Background = image;
            isSolid = false;

            //If it's a base block, also open all connected doorblocks
            if (isBase)
            {
                foreach (DoorBlock block in GetConnectedBlocks())
                {
                    block.Open();
                }
            }
        }

        public void Close()
        {
            //Close the current door block
            isOpen = false;
            image = imgClose;
            isSolid = true;
            blockContainer.cvsBlock.Background = image;

            //If it's a base block, also close all connected doorblocks
            if (isBase)
            {
                foreach (DoorBlock block in GetConnectedBlocks())

                {
                    block.Close();
                }
            }
        }
    }
}
