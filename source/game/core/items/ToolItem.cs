using SeeloewenCraft.game.core.blocks;

namespace SeeloewenCraft.game.core.items
{
    public class ToolItem : Item
    {
        public double breakPower;
        public int maxDurability;
        public Tool type;
        public Material material;

        public void Init(string name, string id, string blockId, int durability, double breakPower, Tool type, Material material, bool isPlacable)
        {
            base.Init(name, id, blockId, isPlacable);
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
}
