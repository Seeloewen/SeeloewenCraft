namespace SeeloewenCraft
{
    public class ToolItem : Item
    {
        public int breakPower;
        public int maxDurability;
        public Tool type;

        public void Init(string name, string id, string blockId, int durability, int breakPower, Tool type, bool isPlacable, SealImage sImage)
        {
            base.Init(name, id, blockId, isPlacable, sImage);
            this.breakPower = breakPower;
            this.type = type;
            Game.unstackableItems.Add(id);
            maxDurability = durability;
            tag = $"durability={durability}";
        }
    }
}
