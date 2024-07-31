using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{

    public class Room1 : DungeonRoom
    {
        public Room1(World world) : base(world)
        {
            id = "sc:room_plains_1";
            type = "plains";

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    if (y == 0 || y == 11 || x == 0 || x == 6)
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new CobbleStoneBlock(world, false), isOccupied = true });
                    }
                    else
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new AirBlock(world, false), isOccupied = true });
                    }
                }
            }

            GetBlock(1, 3).block = new CobbleStoneBlock_StairTopLeft(world, false);
            GetBlock(5, 7).block = new CobbleStoneBlock_StairTopRight(world, false);
            GetBlock(4, 7).block = new CobbleStoneBlock_SlabTop(world, false);
            GetBlock(2, 3).block = new CobbleStoneBlock_SlabTop(world, false);

            SetDoor(6, 8, Direction.RIGHT);
            SetDoor(0, 4, Direction.LEFT);
        }
    }

    public class Room2 : DungeonRoom
    {
        public Room2(World world) : base(world)
        {
            id = "sc:room_plains_2";
            type = "plains";

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (y == 0 || y == 4 || x == 0 || x == 9)
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new CobbleStoneBlock(world, false), isOccupied = true });
                    }
                    else
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new AirBlock(world, false), isOccupied = true });
                    }
                }
            }

            GetBlock(3, 1).block = new TorchBlock(world, false);
            GetBlock(7, 1).block = new TorchBlock(world, false);

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(9, 1, Direction.RIGHT);
        }
    }

    public class Room3 : DungeonRoom
    {
        public Room3(World world) : base(world)
        {
            id = "sc:room_plains_3";
            type = "plains";

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (y == 0 || y == 4 || x == 0 || x == 4)
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new CobbleStoneBlock(world, false), isOccupied = true });
                    }
                    else
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new AirBlock(world, false), isOccupied = true });
                    }
                }
            }

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(2, 0, Direction.DOWN);
            SetDoor(4, 1, Direction.RIGHT);
            SetDoor(2, 4, Direction.UP);
        }
    }

    public class Room4 : DungeonRoom
    {
        public Room4(World world) : base(world)
        {
            id = "sc:room_plains_4";
            type = "plains";

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (y == 0 || y == 9 || x == 0 || x == 9)
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new CobbleStoneBlock(world, false), isOccupied = true });
                    }
                    else
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new AirBlock(world, false), isOccupied = true });
                    }
                }
            }

            GetBlock(3, 1).block = new CobbleStoneBlock_StairTopRight(world, false);
            GetBlock(7, 1).block = new CobbleStoneBlock_StairTopLeft(world, false);
            GetBlock(5, 1).block = new CobbleStoneBlock(world, false);
            GetBlock(5, 2).block = new CobbleStoneBlock(world, false);
            GetBlock(5, 3).block = new CobbleStoneBlock(world, false);
            GetBlock(5, 4).block = new WaterBlock_6(world, false);
            GetBlock(1, 6).block = new CobbleStoneBlock_StairTopLeft(world, false);
            GetBlock(2, 6).block = new CobbleStoneBlock_SlabTop(world, false);

            //Create a chest with random loot
            List<Item> chestLoot = new List<Item>();
            for (int i = 0; i < 5; i++)
            {
                chestLoot.AddRange(world.lootTables.plainsDungeonChest1.RollEntry().RollItems());
            }
            ChestBlock chest = new ChestBlock(world, false);
            foreach (Item item in chestLoot)
            {
                chest.blockInventory.AddItem(item);
            }
            GetBlock(2, 7).block = chest;

            SetDoor(2, 8, Direction.UP);
            SetDoor(9, 1, Direction.RIGHT);
            SetDoor(0, 1, Direction.LEFT);
        }
    }

    public class Room5 : DungeonRoom
    {
        public Room5(World world) : base(world)
        {
            id = "sc:room_plains_5";
            type = "plains";

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (y == 0 || y == 8 || x == 0 || x == 4)
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new CobbleStoneBlock(world, false), isOccupied = true });
                    }
                    else
                    {
                        blocks.Add(new DungeonBlock(x, y) { block = new AirBlock(world, false), isOccupied = true });
                    }
                }
            }

            GetBlock(2, 5).block = new CobbleStoneBlock_Center(world, false);

            SetDoor(2, 8, Direction.UP);
            SetDoor(2, 0, Direction.DOWN);
        }
    }
}
