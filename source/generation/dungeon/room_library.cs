using System;
using System.Collections.Generic;
using System.Linq;
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
                    DungeonRoom newRoom = (DungeonRoom)Activator.CreateInstance(roomType, room.world);
                    return newRoom;
                }
            }

            return null;
        }

        public static void CreateDungeonRooms(World world)
        {
            //Add all the rooms to the library
            roomList.Add(new Room1(world));
            roomList.Add(new Room2(world));
            roomList.Add(new Room3(world));
            roomList.Add(new Room4(world));
            roomList.Add(new Room5(world));
        }
    }
}
