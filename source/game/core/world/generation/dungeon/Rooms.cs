using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.util;
using System;

namespace SeeloewenCraft.game.core.world.generation
{

    public class PlainsRoomCrossing : DungeonRoom
    {
        public PlainsRoomCrossing(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_crossing";
            type = DungeonType.Plains;

            CreateBasicShape(5, 9, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(1, 3).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(3, 5).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight(false));
            GetBlock(3, 4).block.SetForegroundBlock(new TorchBlock(false));
            GetBlock(2, 1).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 2).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 3).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 4).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 5).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 6).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 7).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 8).block.SetForegroundBlock(new LadderBlock(false));

            SetDoor(2, 0, Direction.DOWN);
            SetDoor(2, 8, Direction.UP);
            SetDoor(0, 4, Direction.LEFT);
            SetDoor(4, 1, Direction.RIGHT);
            SetDoor(4, 6, Direction.RIGHT);
        }
    }

    public class PlainsRoomPool : DungeonRoom
    {
        public PlainsRoomPool(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_pool";
            type = DungeonType.Plains;

            CreateBasicShape(7, 8, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(3, 4).block.SetForegroundBlock(new TorchBlock(false));
            GetBlock(1, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(2, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(3, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(4, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(5, 1).block.SetForegroundBlock(new WaterBlock_6(false));

            SetDoor(6, 5, Direction.RIGHT);
            SetDoor(0, 5, Direction.LEFT);
            SetDoor(3, 7, Direction.UP);
        }
    }

    public class PlainsRoomHuge : DungeonRoom
    {
        public PlainsRoomHuge(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_huge";
            type = DungeonType.Plains;

            CreateBasicShape(12, 12, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(1, 3).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(1, 4).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(1, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(1, 6).block.SetForegroundBlock(new CobblestoneBlock(false));

            GetBlock(2, 4).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(2, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(2, 6).block.SetForegroundBlock(new CobblestoneBlock(false));

            GetBlock(3, 5).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(3, 6).block.SetForegroundBlock(new CobblestoneBlock(false));

            GetBlock(3, 0).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(3, 1).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(3, 2).block.SetForegroundBlock(new LadderBlock(false));

            GetBlock(6, 6).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(5, 6).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(7, 6).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(8, 6).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(9, 6).block.SetForegroundBlock(new CobblestoneBlock(false));

            GetBlock(6, 9).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(6, 10).block.SetForegroundBlock(new CobblestoneBlock(false));

            BarrelBlock barrel = new BarrelBlock(false);
            barrel.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(6, 1).block.SetForegroundBlock(barrel);

            BarrelBlock barrel2 = new BarrelBlock(false);
            barrel2.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(7, 1).block.SetForegroundBlock(barrel2);

            BarrelBlock barrel3 = new BarrelBlock(false);
            barrel3.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(7, 2).block.SetForegroundBlock(barrel3);

            BarrelBlock barrel4 = new BarrelBlock(false);
            barrel4.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(8, 1).block.SetForegroundBlock(barrel4);

            ChestBlock chest = new ChestBlock(false);
            chest.InsertLootTable(LootTables.plainsDungeonChest, 6, rnd);
            GetBlock(2, 7).block.SetForegroundBlock(chest);

            GetBlock(10, 3).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight(false));
            GetBlock(10, 4).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(10, 5).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(10, 6).block.SetForegroundBlock(new LadderBlock(false));

            GetBlock(4, 6).block.SetForegroundBlock(new OakTrapDoor(false));
            GetBlock(3, 9).block.SetForegroundBlock(new TorchBlock(false));
            GetBlock(7, 4).block.SetForegroundBlock(new TorchBlock(false));

            SetDoor(0, 7, Direction.LEFT);
            SetDoor(11, 1, Direction.RIGHT);
            SetDoor(3, 0, Direction.RIGHT);
        }
    }

    public class PlainsRoomWell : DungeonRoom
    {
        public PlainsRoomWell(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_well";
            type = DungeonType.Plains;

            CreateBasicShape(11, 9, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(3, 1).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight(false));
            GetBlock(7, 1).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(5, 1).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(5, 2).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(5, 3).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(5, 4).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(1, 5).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(2, 5).block.SetForegroundBlock(new CobblestoneBlock_SlabTop(false));

            GetBlock(3, 6).block.SetForegroundBlock(new TorchBlock(false));
            GetBlock(7, 6).block.SetForegroundBlock(new TorchBlock(false));

            ChestBlock chest = new ChestBlock(false);
            chest.InsertLootTable(LootTables.plainsDungeonChest, 6, rnd);
            GetBlock(1, 6).block.SetForegroundBlock(chest);

            SetDoor(10, 1, Direction.RIGHT);
        }
    }

    public class PlainsRoomSmall : DungeonRoom
    {
        public PlainsRoomSmall(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_small";
            type = DungeonType.Plains;

            CreateBasicShape(5, 5, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(2, 1).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 2).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 3).block.SetForegroundBlock(new LadderBlock(false));
            GetBlock(2, 4).block.SetForegroundBlock(new LadderBlock(false));

            GetBlock(1, 2).block.SetForegroundBlock(new TorchBlock(false));
            GetBlock(3, 2).block.SetForegroundBlock(new TorchBlock(false));

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(4, 1, Direction.RIGHT);
            SetDoor(2, 4, Direction.UP);
        }
    }

    public class PlainsRoomLogs : DungeonRoom
    {
        public PlainsRoomLogs(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_logs";
            type = DungeonType.Plains;

            CreateBasicShape(7, 9, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(1, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(2, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(3, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(4, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(5, 1).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(1, 2).block.SetForegroundBlock(new WaterBlock_6(false));
            GetBlock(5, 2).block.SetForegroundBlock(new WaterBlock_6(false));

            GetBlock(2, 2).block.SetForegroundBlock(new SpruceLogBlock(false));
            GetBlock(3, 2).block.SetForegroundBlock(new SpruceLogBlock(false));
            GetBlock(4, 2).block.SetForegroundBlock(new SpruceLogBlock(false));

            ChestBlock chest = new ChestBlock(false);
            chest.InsertLootTable(LootTables.plainsDungeonChest, 6, rnd);
            GetBlock(3, 3).block.SetForegroundBlock(chest);

            GetBlock(3, 5).block.SetForegroundBlock(new TorchBlock(false));

            SetDoor(0, 3, Direction.LEFT);
            SetDoor(6, 3, Direction.RIGHT);
        }
    }

    public class PlainsRoomLong : DungeonRoom
    {
        public PlainsRoomLong(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_long";
            type = DungeonType.Plains;

            CreateBasicShape(11, 5, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(3, 2).block.SetForegroundBlock(new TorchBlock(false));
            GetBlock(7, 2).block.SetForegroundBlock(new TorchBlock(false));

            BarrelBlock barrel = new BarrelBlock(true);
            barrel.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(2, 1).block = barrel;

            BarrelBlock barrel2 = new BarrelBlock(true);
            barrel2.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(2, 2).block = barrel2;

            BarrelBlock barrel3 = new BarrelBlock(true);
            barrel3.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(3, 1).block = barrel3;

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(10, 1, Direction.RIGHT);
            SetDoor(5, 0, Direction.DOWN);
        }
    }

    public class PlainsRoomStairs : DungeonRoom
    {
        public PlainsRoomStairs(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_stairs";
            type = DungeonType.Plains;

            CreateBasicShape(8, 7, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(1, 4).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(1, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(2, 5).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));

            GetBlock(3, 1).block.SetForegroundBlock(new CobblestoneBlock_StairBottomRight(false));
            GetBlock(4, 1).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(4, 2).block.SetForegroundBlock(new CobblestoneBlock_StairBottomRight(false));
            GetBlock(5, 2).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(5, 3).block.SetForegroundBlock(new CobblestoneBlock_StairBottomRight(false));
            GetBlock(6, 3).block.SetForegroundBlock(new CobblestoneBlock(false));

            GetBlock(2, 4).block.SetForegroundBlock(new TorchBlock(false));

            BarrelBlock barrel = new BarrelBlock(false);
            barrel.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(5, 1).block.SetForegroundBlock(barrel);

            BarrelBlock barrel2 = new BarrelBlock(false);
            barrel2.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(6, 1).block.SetForegroundBlock(barrel2);

            BarrelBlock barrel3 = new BarrelBlock(false);
            barrel3.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(6, 2).block.SetForegroundBlock(barrel3);

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(7, 4, Direction.RIGHT);
        }
    }

    public class PlainsRoomPyramid : DungeonRoom
    {
        public PlainsRoomPyramid(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_pyramid";
            type = DungeonType.Plains;

            CreateBasicShape(11, 7, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(1, 3).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(1, 4).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(1, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(2, 3).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(2, 4).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(2, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(3, 4).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(3, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(4, 5).block.SetForegroundBlock(new CobblestoneBlock_StairTopLeft(false));
            GetBlock(6, 5).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight(false));
            GetBlock(7, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(7, 4).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight(false));
            GetBlock(8, 5).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(8, 4).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(8, 3).block.SetForegroundBlock(new CobblestoneBlock_StairTopRight(false));
            GetBlock(9, 3).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(9, 4).block.SetForegroundBlock(new CobblestoneBlock(false));
            GetBlock(9, 5).block.SetForegroundBlock(new CobblestoneBlock(false));

            GetBlock(5, 5).block.SetForegroundBlock(new TorchBlock(false));

            BarrelBlock barrel = new BarrelBlock(false);
            barrel.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(5, 1).block.SetForegroundBlock(barrel);

            BarrelBlock barrel2 = new BarrelBlock(false);
            barrel2.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(5, 2).block.SetForegroundBlock(barrel2);

            BarrelBlock barrel3 = new BarrelBlock(false);
            barrel3.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(4, 1).block.SetForegroundBlock(barrel3);

            BarrelBlock barrel4 = new BarrelBlock(false);
            barrel4.InsertLootTable(LootTables.plainsDungeonBarrel, 3, rnd);
            GetBlock(6, 1).block.SetForegroundBlock(barrel4);

            SetDoor(0, 1, Direction.LEFT);
            SetDoor(10, 1, Direction.RIGHT);
        }
    }

    public class PlainsRoomTree : DungeonRoom
    {
        public PlainsRoomTree(Random rnd) : base(rnd)
        {
            id = "sc:room_plains_tree";
            type = DungeonType.Plains;

            CreateBasicShape(9, 7, "sc:stone_bricks_block", "sc:cobblestone_block", "sc:mossy_cobblestone_block");

            GetBlock(4, 0).block.SetForegroundBlock(new DirtBlock(false));
            GetBlock(4, 1).block.SetForegroundBlock(new OakLogBlock(false));
            GetBlock(4, 2).block.SetForegroundBlock(new OakLogBlock(false));
            GetBlock(4, 3).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(4, 4).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(4, 5).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(5, 3).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(5, 4).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(3, 3).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(3, 4).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(6, 3).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(2, 3).block.SetForegroundBlock(new OakLeavesBlock(false));
            GetBlock(4, 6).block.SetForegroundBlock(new TorchBlock(false));

            SetDoor(0, 1, Direction.LEFT);
        }
    }
}
