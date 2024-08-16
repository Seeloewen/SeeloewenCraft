using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    //Library of all available rooms
    public static class RoomLibrary
    {
        public static List<DungeonRoom> roomList = new List<DungeonRoom>();

        //-- Constructor --//

        public static DungeonRoom GetRoom(string id)
        {
            //Get the correct room with the given id and return a new instance of that room
            foreach (DungeonRoom room in roomList)
            {
                if (room.id == id)
                {
                    //Create a new block of the same type and place it below the original block
                    Type roomType = room.GetType();
                    DungeonRoom newRoom = (DungeonRoom)Activator.CreateInstance(roomType);
                    return newRoom;
                }
            }

            return null;
        }

        public static void CreateDungeonRooms()
        {
            //Add all the rooms to the library
            roomList.Add(new PlainsRoom1());
            roomList.Add(new PlainsRoom2());
            roomList.Add(new PlainsRoom3());
            roomList.Add(new PlainsRoom4());
            roomList.Add(new PlainsRoom5());
        }

        public static (Block, Block) GetDoorBlock(DungeonType type, Direction dir)
        {
            //Returns the block that is used for creating a door, based on dungeon type and door direction
            switch (type)
            {
                case DungeonType.Plains:
                    if (dir == Direction.RIGHT || dir == Direction.LEFT)
                    {
                        return (new SpruceDoor_Base(false), new SpruceDoor_Top(false));
                    }
                    else
                    {
                        return (new OakLeavesBlock(false), null);
                    }
                default:
                    return (null, null);
            }
        }

        public static (Block, Block) GetDoorReplacementBlock(DungeonType type, Direction dir)
        {
            switch (type)
            {
                case DungeonType.Plains:
                    if (dir == Direction.RIGHT || dir == Direction.LEFT)
                    {
                        return (new CobblestoneBlock(false), new CobblestoneBlock(false));
                    }
                    else
                    {
                        return (new CobblestoneBlock(false), null);
                    }
                default:
                    return (null, null);
            }
        }
    }
}
