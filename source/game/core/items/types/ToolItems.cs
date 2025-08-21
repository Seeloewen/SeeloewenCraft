using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities.inventory;

namespace SeeloewenCraft.game.core.items
{
    public abstract class ScytheToolItem : ToolItem
    {
        public ScytheToolItem() : base()
        {
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            Block foregroundBlock = block.GetForegroundBlock();
            Block blockAbove = block.GetBlockAbove();

            //Stop action if block not in range or block has a solid block above
            if (!block.IsInRange() || block.GetBlockAbove().isSolid)
            {
                return;
            }

            if (block.isBackground && foregroundBlock != null && foregroundBlock.HasTag(BlockTags.SCYTHEABLE))  //Foreground block
            {
                block.SetForegroundBlock(new FarmlandBlock(block.isBackground));
                invSlot.RemoveDurablity();
            }
            else if (block.HasTag(BlockTags.SCYTHEABLE)) //Background or normal block
            {
                block.SetBlock(new FarmlandBlock(block.isBackground));
                invSlot.RemoveDurablity();
            }
        }
    }

    #region Wood Items

    public class WoodHammerItem : ToolItem
    {
        public WoodHammerItem() : base()
        {
            Init("Wood Hammer", "sc:wood_hammer_item", null, 64, 1.25, Tool.Hammer, Material.Wood, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            HammerRightClickAction(block);
        }
    }

    public class WoodPickaxeItem : ToolItem
    {
        public WoodPickaxeItem() : base()
        {
            Init("Wood Pickaxe", "sc:wood_pickaxe_item", null, 64, 3, Tool.Pickaxe, Material.Wood, false);
        }
    }

    public class WoodAxeItem : ToolItem
    {
        public WoodAxeItem() : base()
        {
            Init("Wood Axe", "sc:wood_axe_item", null, 64, 1.25, Tool.Axe, Material.Wood, false);
        }
    }

    public class WoodShovelItem : ToolItem
    {
        public WoodShovelItem() : base()
        {
            Init("Wood Shovel", "sc:wood_shovel_item", null, 64, 1.25, Tool.Shovel, Material.Wood, false);
        }
    }

    public class WoodScytheItem : ScytheToolItem
    {
        public WoodScytheItem() : base()
        {
            Init("Wood Scythe", "sc:wood_scythe_item", null, 64, 0, Tool.Scythe, Material.Wood, false);
        }

    }

    public class WoodSwordItem : ToolItem
    {
        public WoodSwordItem() : base()
        {
            Init("Wood Sword", "sc:wood_sword_item", null, 64, 0, Tool.Sword, Material.Wood, false);
        }
    }

    #endregion

    #region Stone Items

    public class StoneHammerItem : ToolItem
    {
        public StoneHammerItem() : base()
        {
            Init("Stone Hammer", "sc:stone_hammer_item", null, 128, 1.5, Tool.Hammer, Material.Stone, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            HammerRightClickAction(block);
        }
    }

    public class StonePickaxeItem : ToolItem
    {
        public StonePickaxeItem() : base()
        {
            Init("Stone Pickaxe", "sc:stone_pickaxe_item", null, 128, 4.25, Tool.Pickaxe, Material.Stone, false);
        }
    }

    public class StoneAxeItem : ToolItem
    {
        public StoneAxeItem() : base()
        {
            Init("Stone Axe", "sc:stone_axe_item", null, 128, 1.75, Tool.Axe, Material.Stone, false);
        }
    }

    public class StoneShovelItem : ToolItem
    {
        public StoneShovelItem() : base()
        {
            Init("Stone Shovel", "sc:stone_shovel_item", null, 128, 1.65, Tool.Shovel, Material.Stone, false);
        }
    }

    public class StoneScytheItem : ScytheToolItem
    {
        public StoneScytheItem() : base()
        {
            Init("Stone Scythe", "sc:stone_scythe_item", null, 128, 0, Tool.Scythe, Material.Stone, false);
        }
    }

    public class StoneSwordItem : ToolItem
    {
        public StoneSwordItem() : base()
        {
            Init("Stone Sword", "sc:stone_sword_item", null, 128, 0, Tool.Sword, Material.Stone, false);
        }
    }

    #endregion

    #region Tin Items

    public class TinHammerItem : ToolItem
    {
        public TinHammerItem() : base()
        {
            Init("Tin Hammer", "sc:tin_hammer_item", null, 256, 2, Tool.Hammer, Material.Tin, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            HammerRightClickAction(block);
        }
    }

    public class TinPickaxeItem : ToolItem
    {
        public TinPickaxeItem() : base()
        {
            Init("Tin Pickaxe", "sc:tin_pickaxe_item", null, 256, 6, Tool.Pickaxe, Material.Tin, false);
        }
    }

    public class TinAxeItem : ToolItem
    {
        public TinAxeItem() : base()
        {
            Init("Tin Axe", "sc:tin_axe_item", null, 256, 2, Tool.Axe, Material.Tin, false);
        }
    }

    public class TinShovelItem : ToolItem
    {
        public TinShovelItem() : base()
        {
            Init("Tin Shovel", "sc:tin_shovel_item", null, 256, 2, Tool.Shovel, Material.Tin, false);
        }
    }

    public class TinScytheItem : ScytheToolItem
    {
        public TinScytheItem() : base()
        {
            Init("Tin Scythe", "sc:tin_scythe_item", null, 256, 0, Tool.Scythe, Material.Tin, false);
        }
    }

    public class TinSwordItem : ToolItem
    {
        public TinSwordItem() : base()
        {
            Init("Tin Sword", "sc:tin_sword_item", null, 256, 0, Tool.Sword, Material.Tin, false);
        }
    }

    #endregion

    #region Iron Items

    public class IronHammerItem : ToolItem
    {
        public IronHammerItem() : base()
        {
            Init("Iron Hammer", "sc:iron_hammer_item", null, 512, 1.75, Tool.Hammer, Material.Iron, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            HammerRightClickAction(block);
        }
    }

    public class IronPickaxeItem : ToolItem
    {
        public IronPickaxeItem() : base()
        {
            Init("Iron Pickaxe", "sc:iron_pickaxe_item", null, 512, 8, Tool.Pickaxe, Material.Iron, false);
        }
    }

    public class IronAxeItem : ToolItem
    {
        public IronAxeItem() : base()
        {
            Init("Iron Axe", "sc:iron_axe_item", null, 512, 2.5, Tool.Axe, Material.Iron, false);
        }
    }

    public class IronShovelItem : ToolItem
    {
        public IronShovelItem() : base()
        {
            Init("Iron Shovel", "sc:iron_shovel_item", null, 512, 2.25, Tool.Shovel, Material.Iron, false);
        }
    }

    public class IronScytheItem : ScytheToolItem
    {
        public IronScytheItem() : base()
        {
            Init("Iron Scythe", "sc:iron_scythe_item", null, 512, 0, Tool.Scythe, Material.Iron, false);
        }
    }

    public class IronSwordItem : ToolItem
    {
        public IronSwordItem() : base()
        {
            Init("Iron Sword", "sc:iron_sword_item", null, 512, 0, Tool.Sword, Material.Iron, false);
        }
    }

    #endregion

    #region Diamond Items

    public class DiamondHammerItem : ToolItem
    {
        public DiamondHammerItem() : base()
        {
            Init("Diamond Hammer", "sc:diamond_hammer_item", null, 1024, 2.25, Tool.Hammer, Material.Diamond, false);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            HammerRightClickAction(block);
        }
    }

    public class DiamondPickaxeItem : ToolItem
    {
        public DiamondPickaxeItem() : base()
        {
            Init("Diamond Pickaxe", "sc:diamond_pickaxe_item", null, 1024, 10, Tool.Pickaxe, Material.Diamond, false);
        }
    }

    public class DiamondAxeItem : ToolItem
    {
        public DiamondAxeItem() : base()
        {
            Init("Diamond Axe", "sc:diamond_axe_item", null, 1024, 3.5, Tool.Axe, Material.Diamond, false);
        }
    }

    public class DiamondShovelItem : ToolItem
    {
        public DiamondShovelItem() : base()
        {
            Init("Diamond Shovel", "sc:diamond_shovel_item", null, 1024, 2.75, Tool.Shovel, Material.Diamond, false);
        }
    }

    public class DiamondScytheItem : ScytheToolItem
    {
        public DiamondScytheItem() : base()
        {
            Init("Diamond Scythe", "sc:diamond_scythe_item", null, 1024, 0, Tool.Scythe, Material.Diamond, false);
        }
    }

    public class DiamondSwordItem : ToolItem
    {
        public DiamondSwordItem() : base()
        {
            Init("Diamond Sword", "sc:diamond_sword_item", null, 1024, 0, Tool.Sword, Material.Diamond, false);
        }
    }

    #endregion
}
