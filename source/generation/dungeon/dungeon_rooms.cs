using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{

    public class PlainsRoom1 : DungeonRoom
    {
        public PlainsRoom1() : base()
        {
            id = "sc:room_plains_1";
            type = DungeonType.Plains;

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    blocks.Add(new DungeonBlock(x, y) { block = new CobblestoneBlock( true), isOccupied = true});
                    if (y == 0 || y == 11 || x == 0 || x == 6)
                    {
                        GetBlock(x, y).block.SetForegroundBlock(new CobblestoneBlock( false));
                    }
                }
            }

            GetBlock(1, 3).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft( false));
            GetBlock(5, 7).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight( false));
            GetBlock(4, 7).block.SetForegroundBlock(new CobblestoneBlock_SlabTop( false));
            GetBlock(2, 3).block.SetForegroundBlock(new CobblestoneBlock_SlabTop( false));

            SetDoor(6, 8, Direction.RIGHT);
            SetDoor(0, 4, Direction.LEFT);
        }
    }

    public class PlainsRoom2 : DungeonRoom
    {
        public PlainsRoom2() : base()
        {
            id = "sc:room_plains_2";
            type = DungeonType.Plains;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    blocks.Add(new DungeonBlock(x, y) { block = new CobblestoneBlock( true), isOccupied = true });
                    if (y == 0 || y == 4 || x == 0 || x == 9)
                    {
                        GetBlock(x, y).block.SetForegroundBlock(new CobblestoneBlock( false));
                    }
                }
            }

            GetBlock(3, 1).block.SetForegroundBlock(new TorchBlock( false));
            GetBlock(7, 1).block.SetForegroundBlock(new TorchBlock( false));

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(9, 1, Direction.RIGHT);
        }
    }

    public class PlainsRoom3 : DungeonRoom
    {
        public PlainsRoom3() : base()
        {
            id = "sc:room_plains_3";
            type = DungeonType.Plains;

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    blocks.Add(new DungeonBlock(x, y) { block = new CobblestoneBlock( true), isOccupied = true });
                    if (y == 0 || y == 4 || x == 0 || x == 4)
                    {
                        GetBlock(x, y).block.SetForegroundBlock(new CobblestoneBlock( false));
                    }
                }
            }

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(2, 0, Direction.DOWN);
            SetDoor(4, 1, Direction.RIGHT);
            SetDoor(2, 4, Direction.UP);
        }
    }

    public class PlainsRoom4 : DungeonRoom
    {
        public PlainsRoom4() : base()
        {
            id = "sc:room_plains_4";
            type = DungeonType.Plains;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    blocks.Add(new DungeonBlock(x, y) { block = new CobblestoneBlock( true), isOccupied = true });
                    if (y == 0 || y == 9 || x == 0 || x == 9)
                    {
                        GetBlock(x, y).block.SetForegroundBlock(new CobblestoneBlock( false));
                    }
                }
            }

            GetBlock(3, 1).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight( false));
            GetBlock(7, 1).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft( false));
            GetBlock(5, 1).block.SetForegroundBlock(new CobblestoneBlock( false));
            GetBlock(5, 2).block.SetForegroundBlock(new CobblestoneBlock( false));
            GetBlock(5, 3).block.SetForegroundBlock(new CobblestoneBlock( false));
            GetBlock(5, 4).block.SetForegroundBlock(new WaterBlock_6( false));
            GetBlock(1, 6).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft( false));
            GetBlock(2, 6).block.SetForegroundBlock(new CobblestoneBlock_SlabTop( false));

            //Create a chest with random loot
            List<Item> chestLoot = new List<Item>();
            for (int i = 0; i < 5; i++)
            {
                chestLoot.AddRange(Game.world.lootTables.plainsDungeonChest1.RollEntry().RollItems());
            }
            ChestBlock chest = new ChestBlock( false);
            foreach (Item item in chestLoot)
            {
                chest.blockInventory.AddItem(item.id, 1);
            }
            GetBlock(2, 7).block.SetForegroundBlock(chest);

            SetDoor(2, 8, Direction.UP);
            SetDoor(9, 1, Direction.RIGHT);
            SetDoor(0, 1, Direction.LEFT);
        }
    }

    public class PlainsRoom5 : DungeonRoom
    {
        public PlainsRoom5() : base()
        {
            id = "sc:room_plains_5";
            type = DungeonType.Plains;

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    blocks.Add(new DungeonBlock(x, y) { block = new CobblestoneBlock( true), isOccupied = true });
                    if (y == 0 || y == 8 || x == 0 || x == 4)
                    {
                        GetBlock(x, y).block.SetForegroundBlock(new CobblestoneBlock( false));
                    }
                }
            }

            GetBlock(2, 5).block.SetForegroundBlock(new CobbleStoneBlock_Center( false));

            SetDoor(2, 8, Direction.UP);
            SetDoor(2, 0, Direction.DOWN);
        }
    }
}
