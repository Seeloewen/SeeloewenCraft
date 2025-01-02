using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    public enum DungeonType
    {
        Plains
    }

    public class Dungeon
    {
        private List<DungeonBlock> blocks = new List<DungeonBlock>();
        private Random rnd;

        //-- Custom Methods --//

        public void CreateDungeon(Random chunkRnd, int width, int height, DungeonType type)
        {
            rnd = chunkRnd;

            //Create all dungeon blocks depending on with and height
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    blocks.Add(new DungeonBlock(x, y));
                }
            }

            //Needs to be overhauled to get a random starter room based on the type
            var starterRoom = RoomLibrary.GetRoom("sc:room_plains_crossing", rnd);

            //Create starter room and additional rooms
            PlaceRoom(starterRoom, 0, 0, Direction.RIGHT, 0, 0);
            GenerateRooms(7, type);
        }

        public void GenerateRooms(int iterationAmount, DungeonType type)
        {
            //WARNING: iterationAmount does not mean the amount of rooms that are created.
            //The actual amount of created rooms can vary because of door amounts. This int
            //simply tells you how often it will check for doors to append rooms to.


            List<DungeonBlock> doors;

            //Go a set amount of times through the process to place enough rooms
            for (int i = 0; i < iterationAmount; i++)
            {
                //Get all possible doors
                doors = GetDoors();

                //Get a possible room for each door
                foreach (DungeonBlock door in doors)
                {
                    var roomDetails = GetRoom(door.doorDirection, door, type);
                    if (roomDetails.room != null)
                    {
                        //If a possible room was found, place that room
                        PlaceRoom(roomDetails.room, door.x, door.y, door.doorDirection, roomDetails.doorX, roomDetails.doorY);
                        door.RemoveDoor();
                    }
                    else

                    {
                        //If no room is found, the door is no longer a door
                        door.RemoveDoor();
                        door.CloseDoor(type, door.doorDirection, GetBlock(door.x, door.y + 1));
                    }
                }
            }

            //Get all possible doors once again
            doors = GetDoors();

            //Go through all doors one last time to close all non-connected doors (doors that are still marked as doors)
            foreach (DungeonBlock door in doors)
            {
                door.RemoveDoor();
                door.CloseDoor(type, door.doorDirection, GetBlock(door.x, door.y + 1));
            }
        }

        private List<DungeonBlock> GetDoors()
        {
            List<DungeonBlock> doors = new List<DungeonBlock>();

            //Go through all dungeon blocks and get all doors
            foreach (DungeonBlock block in blocks)
            {
                if (block.IsDoor())
                {
                    doors.Add(block);
                }
            }

            return doors;
        }

        public DungeonBlock GetBlock(int x, int y)
        {
            //Compare x and y pos and return the correct block from the list
            foreach (DungeonBlock block in blocks)
            {
                if (block.x == x && block.y == y)
                {
                    return block;
                }
            }
            return null;
        }

        public (DungeonRoom room, int doorX, int doorY) GetRoom(Direction doorDirection, DungeonBlock sourceDoor, DungeonType type)
        {
            List<(DungeonRoom, int, int)> possibleRooms = new List<(DungeonRoom, int, int)>();

            //Go through all available rooms in the library
            foreach (DungeonRoom room in RoomLibrary.roomList)
            {
                if (room.type == type)
                {
                    //If the room is the correct type
                    foreach (DungeonBlock block in room.blocks)
                    {
                        //Check all doors if a door is available that has the needed direction
                        if (block.IsDoor() && block.doorDirection == TurnDirection(doorDirection))
                        {
                            //Get offset based on direction
                            int xOffset = 0;
                            int yOffset = 0;
                            switch (doorDirection)
                            {
                                case Direction.UP:
                                    yOffset++;
                                    break;
                                case Direction.DOWN:
                                    yOffset--;
                                    break;
                                case Direction.LEFT:
                                    xOffset--;
                                    break;
                                case Direction.RIGHT:
                                    xOffset++;
                                    break;
                            }

                            //Get possible space and needed space: Warning: Is currently not absolutely reliable and makes mistakes
                            var possibleSpace = GetPossibleSpace(sourceDoor.x + xOffset, sourceDoor.y + yOffset);
                            var necessarySpace = room.GetNecessarySpace(block.x, block.y);

                            //Compare possible to needed space and add the room to the list of possible rooms if there is enough space
                            if (possibleSpace.top >= necessarySpace.top
                                && possibleSpace.bottom >= necessarySpace.bottom
                                && possibleSpace.left >= necessarySpace.left
                                && possibleSpace.right >= necessarySpace.right)
                            {
                                //Mark the door as no longer being a possible door so it doesn't get checked again in the next iteration if this room gets chosen
                                DungeonRoom newRoom = RoomLibrary.GetRoom(room.id, rnd);
                                newRoom.GetBlock(block.x, block.y).RemoveDoor();
                                newRoom.GetBlock(block.x, block.y).HideDoor(block.doorDirection, newRoom.GetBlock(block.x, block.y + 1));

                                //Add the room to the possible rooms
                                possibleRooms.Add((newRoom, block.x - xOffset, block.y - yOffset));
                            }
                        }
                    }
                }
            }

            //Return a random possible room
            if (possibleRooms.Count > 0)
            {
                return possibleRooms[rnd.Next(0, possibleRooms.Count)];
            }
            return (null, 0, 0);
        }

        public (int top, int bottom, int right, int left) GetPossibleSpace(int x, int y)
        {
            int top = 0;
            int bottom = 0;
            int right = 0;
            int left = 0;
            int i;

            //Get space above

            i = 1;
            while (true)
            {
                if (GetBlock(x, y + i) != null && !GetBlock(x, y + i).isOccupied)
                {
                    top++;
                }
                else
                {
                    break;
                }

                i++;
            }

            //Get space below
            i = 1;
            while (true)
            {
                if (GetBlock(x, y - i) != null && !GetBlock(x, y - i).isOccupied)
                {
                    bottom++;
                }
                else
                {
                    break;
                }
                i++;
            }

            //Get space to the right
            i = 1;
            while (true)
            {
                if (GetBlock(x + i, y) != null && !GetBlock(x + i, y).isOccupied)
                {
                    right++;
                }
                else
                {
                    break;
                }
                i++;
            }

            //Get space to the left
            i = 1;
            while (true)
            {
                if (GetBlock(x - i, y) != null && !GetBlock(x - i, y).isOccupied)
                {
                    left++;
                }
                else
                {
                    break;
                }
                i++;
            }

            return (top, bottom, right, left);
        }

        public void PlaceRoom(DungeonRoom room, int x, int y, Direction direction, int doorX, int doorY)
        {
            //Add all blocks of the room to the actual block list and calculate new x and y pos based on the position of the doors
            foreach (DungeonBlock block in room.blocks)
            {
                block.x = block.x + x - doorX;
                block.y = block.y + y - doorY;

                blocks.Remove(GetBlock(block.x, block.y));
                blocks.Add(block);
            }
        }

        public Direction TurnDirection(Direction direction)
        {
            //Simply switch the direction
            switch (direction)
            {
                case Direction.UP:
                    return Direction.DOWN;
                case Direction.DOWN:
                    return Direction.UP;
                case Direction.LEFT:
                    return Direction.RIGHT;
                case Direction.RIGHT:
                    return Direction.LEFT;
                default:
                    return Direction.DOWN;
            }
        }

        public List<StructureComponent> GenerateDungeon(int x, int y)
        {
            //Create a component for each block in the dungeon block list that is occupied and add offset based on position of the dungeon inside the structure
            List<StructureComponent> components = new List<StructureComponent>();

            foreach (DungeonBlock block in blocks)
            {
                if (block.isOccupied)
                {
                    components.Add(new StructureComponent(block.x + x, block.y + y, block.block));
                }
            }

            return components;
        }
    }
}
