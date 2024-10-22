namespace SeeloewenCraft
{
    public class ToolItem : Item
    {
        public double breakPower;
        public int maxDurability;
        public Tool type;
        public Material material;

        public void Init(string name, string id, string blockId, int durability, double breakPower, Tool type, Material material, bool isPlacable, SealImage sImage)
        {
            base.Init(name, id, blockId, isPlacable, sImage);
            this.breakPower = breakPower;
            this.type = type;
            this.material = material;
            Game.unstackableItems.Add(id);
            maxDurability = durability;
            tag = $"durability={durability}";
        }

        public void HammerRightClickAction(Block block)
        {
            if (block.isBackground && block.canBeMovedToBackground && block.IsInRange() && block.GetForegroundBlock() == null)
            {
                block.MoveToNormal();
            }
            else if (!block.isBackground && block.IsInRange() && block.canBeMovedToBackground)
            {
                block.MoveToBackground();
            }
        }
    }

    public abstract class ScytheToolItem : ToolItem
    {
        public ScytheToolItem() : base()
        {
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            if (block.tags.Contains("scytheable"))
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
            Init("Wood Hammer", "sc:wood_hammer_item", null, 64, 1.25, Tool.Hammer, Material.Wood, false, Images.WoodHammer);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            HammerRightClickAction(block);
        }
    }

    public class WoodPickaxeItem : ToolItem
    {
        public WoodPickaxeItem() : base()
        {
            Init("Wood Pickaxe", "sc:wood_pickaxe_item", null, 64, 3, Tool.Pickaxe, Material.Wood, false, Images.WoodPickaxe);
        }
    }

    public class WoodAxeItem : ToolItem
    {
        public WoodAxeItem() : base()
        {
            Init("Wood Axe", "sc:wood_axe_item", null, 64, 1.25, Tool.Axe, Material.Wood, false, Images.WoodAxe);
        }
    }

    public class WoodShovelItem : ToolItem
    {
        public WoodShovelItem() : base()
        {
            Init("Wood Shovel", "sc:wood_shovel_item", null, 64, 1.25, Tool.Shovel, Material.Wood, false, Images.WoodShovel);
        }
    }

    public class WoodScytheItem : ScytheToolItem
    {
        public WoodScytheItem() : base()
        {
            Init("Wood Scythe", "sc:wood_scythe_item", null, 64, 0, Tool.Scythe, Material.Wood, false, Images.WoodScythe);
        }

    }

    public class WoodSwordItem : ToolItem
    {
        public WoodSwordItem() : base()
        {
            Init("Wood Sword", "sc:wood_sword_item", null, 64, 0, Tool.Sword, Material.Wood, false, Images.WoodSword);
        }
    }

    #endregion

    #region Stone Items

    public class StoneHammerItem : ToolItem
    {
        public StoneHammerItem() : base()
        {
            Init("Stone Hammer", "sc:stone_hammer_item", null, 128, 1.5, Tool.Hammer, Material.Stone, false, Images.StoneHammer);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            HammerRightClickAction(block);
        }
    }

    public class StonePickaxeItem : ToolItem
    {
        public StonePickaxeItem() : base()
        {
            Init("Stone Pickaxe", "sc:stone_pickaxe_item", null, 128, 4.25, Tool.Pickaxe, Material.Stone, false, Images.StonePickaxe);
        }
    }

    public class StoneAxeItem : ToolItem
    {
        public StoneAxeItem() : base()
        {
            Init("Stone Axe", "sc:stone_axe_item", null, 128, 1.75, Tool.Axe, Material.Stone, false, Images.StoneAxe);
        }
    }

    public class StoneShovelItem : ToolItem
    {
        public StoneShovelItem() : base()
        {
            Init("Stone Shovel", "sc:stone_shovel_item", null, 128, 1.65, Tool.Shovel, Material.Stone, false, Images.StoneShovel);
        }
    }

    public class StoneScytheItem : ScytheToolItem
    {
        public StoneScytheItem() : base()
        {
            Init("Stone Scythe", "sc:stone_scythe_item", null, 128, 0, Tool.Scythe, Material.Stone, false, Images.StoneScythe);
        }
    }

    public class StoneSwordItem : ToolItem
    {
        public StoneSwordItem() : base()
        {
            Init("Stone Sword", "sc:stone_sword_item", null, 128, 0, Tool.Sword, Material.Stone, false, Images.StoneSword);
        }
    }

    #endregion        

    #region Tin Items

    public class TinHammerItem : ToolItem
    {
        public TinHammerItem() : base()
        {
            Init("Tin Hammer", "sc:tin_hammer_item", null, 256, 2, Tool.Hammer, Material.Tin, false, Images.TinHammer);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            HammerRightClickAction(block);
        }
    }

    public class TinPickaxeItem : ToolItem
    {
        public TinPickaxeItem() : base()
        {
            Init("Tin Pickaxe", "sc:tin_pickaxe_item", null, 256, 6, Tool.Pickaxe, Material.Tin, false, Images.TinPickaxe);
        }
    }

    public class TinAxeItem : ToolItem
    {
        public TinAxeItem() : base()
        {
            Init("Tin Axe", "sc:tin_axe_item", null, 256, 2, Tool.Axe, Material.Tin, false, Images.TinAxe);
        }
    }

    public class TinShovelItem : ToolItem
    {
        public TinShovelItem() : base()
        {
            Init("Tin Shovel", "sc:tin_shovel_item", null, 256, 2, Tool.Shovel, Material.Tin, false, Images.TinShovel);
        }
    }

    public class TinScytheItem : ScytheToolItem
    {
        public TinScytheItem() : base()
        {
            Init("Tin Scythe", "sc:tin_scythe_item", null, 256, 0, Tool.Scythe, Material.Tin, false, Images.TinScythe);
        }
    }

    public class TinSwordItem : ToolItem
    {
        public TinSwordItem() : base()
        {
            Init("Tin Sword", "sc:tin_sword_item", null, 256, 0, Tool.Sword, Material.Tin, false, Images.TinSword);
        }
    }

    #endregion

    #region Iron Items

    public class IronHammerItem : ToolItem
    {
        public IronHammerItem() : base()
        {
            Init("Iron Hammer", "sc:iron_hammer_item", null, 512, 1.75, Tool.Hammer, Material.Iron, false, Images.IronHammer);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            HammerRightClickAction(block);
        }
    }

    public class IronPickaxeItem : ToolItem
    {
        public IronPickaxeItem() : base()
        {
            Init("Iron Pickaxe", "sc:iron_pickaxe_item", null, 512, 8, Tool.Pickaxe, Material.Iron, false, Images.IronPickaxe);
        }
    }

    public class IronAxeItem : ToolItem
    {
        public IronAxeItem() : base()
        {
            Init("Iron Axe", "sc:iron_axe_item", null, 512, 2.5, Tool.Axe, Material.Iron, false, Images.IronAxe);
        }
    }

    public class IronShovelItem : ToolItem
    {
        public IronShovelItem() : base()
        {
            Init("Iron Shovel", "sc:iron_shovel_item", null, 512, 2.25, Tool.Shovel, Material.Iron, false, Images.IronShovel);
        }
    }

    public class IronScytheItem : ScytheToolItem
    {
        public IronScytheItem() : base()
        {
            Init("Iron Scythe", "sc:iron_scythe_item", null, 512, 0, Tool.Scythe, Material.Iron, false, Images.IronScythe);
        }
    }

    public class IronSwordItem : ToolItem
    {
        public IronSwordItem() : base()
        {
            Init("Iron Sword", "sc:iron_sword_item", null, 512, 0, Tool.Sword, Material.Iron, false, Images.IronSword);
        }
    }

    #endregion

    #region Diamond Items

    public class DiamondHammerItem : ToolItem
    {
        public DiamondHammerItem() : base()
        {
            Init("Diamond Hammer", "sc:diamond_hammer_item", null, 1024, 2.25, Tool.Hammer, Material.Diamond, false, Images.DiamondHammer);
            hasRightClickAction = true;
        }

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            HammerRightClickAction(block);
        }
    }

    public class DiamondPickaxeItem : ToolItem
    {
        public DiamondPickaxeItem() : base()
        {
            Init("Diamond Pickaxe", "sc:diamond_pickaxe_item", null, 1024, 10, Tool.Pickaxe, Material.Diamond, false, Images.DiamondPickaxe);
        }
    }

    public class DiamondAxeItem : ToolItem
    {
        public DiamondAxeItem() : base()
        {
            Init("Diamond Axe", "sc:diamond_axe_item", null, 1024, 3.5, Tool.Axe, Material.Diamond, false, Images.DiamondAxe);
        }
    }

    public class DiamondShovelItem : ToolItem
    {
        public DiamondShovelItem() : base()
        {
            Init("Diamond Shovel", "sc:diamond_shovel_item", null, 1024, 2.75, Tool.Shovel, Material.Diamond, false, Images.DiamondShovel);
        }
    }

    public class DiamondScytheItem : ScytheToolItem
    {
        public DiamondScytheItem() : base()
        {
            Init("Diamond Scythe", "sc:diamond_scythe_item", null, 1024, 0, Tool.Scythe, Material.Diamond, false, Images.DiamondScythe);
        }
    }

    public class DiamondSwordItem : ToolItem
    {
        public DiamondSwordItem() : base()
        {
            Init("Diamond Sword", "sc:diamond_sword_item", null, 1024, 0, Tool.Sword, Material.Diamond, false, Images.DiamondSword);
        }
    }

    #endregion
}
