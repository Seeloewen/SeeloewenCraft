using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Windows.Graphics.Printing.PrintSupport;

namespace SeeloewenCraft
{
    public class DungeonBlock
    {
        public int x;
        public int y;
        private bool isDoor = false;
        public bool isOccupied = false;
        public Direction doorDirection;
        public Block block;

        //-- Constructor --//

        public DungeonBlock(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //-- Custom Methods --//

        public void SetDoor(DungeonType type, Direction doorDir, DungeonBlock blockAbove)
        {
            //Set the dungeonblock as door and set direction
            isDoor = true;
            doorDirection = doorDir;

            //Get the door blocks and set the block for the dungeonblock
            (Block bottom, Block top) door = RoomLibrary.GetDoorBlock(type, doorDir);

            block.SetForegroundBlock(door.bottom);
            if (doorDir == Direction.LEFT || doorDir == Direction.RIGHT)
            {
                blockAbove.block.SetForegroundBlock(door.top);
            }
        }

        public void HideDoor(Direction doorDir, DungeonBlock blockAbove)
        {
            //Remove the foreground blocks to hide the door blocks while keeping the door
            block.RemoveForegroundBlock();

            if ((doorDir == Direction.LEFT || doorDir == Direction.RIGHT) && blockAbove.block != null)
            {
                blockAbove.block.RemoveForegroundBlock();
            }
        }

        public void CloseDoor(DungeonType type, Direction doorDir, DungeonBlock blockAbove)
        {
            //Get the replacement blocks for covering up the door and set the block for the dungeonblock
            (Block bottom, Block top) doorReplacement = RoomLibrary.GetDoorReplacementBlock(type, doorDir);

            //Remove the door blocks from the dungeonblocks and paste replacements
            block.SetForegroundBlock(doorReplacement.bottom);

            if ((doorDir == Direction.LEFT || doorDir == Direction.RIGHT) && blockAbove.block != null)
            {
                blockAbove.block.SetForegroundBlock(doorReplacement.top);
            }
        }

        public void RemoveDoor()
        {
            //Set the dungeonblock as no longer being a door
            isDoor = false;
        }

        public bool IsDoor()
        {
            //Returns whether it's a door or not
            return isDoor;
        }
    }
}
