namespace SeeloewenCraft
{
    public class FoodItem : Item
    {
        public double healAmount;

        public override void RightClickAction(Block block, InventorySlot invSlot, object sender)
        {
            //Heal the player and remove the item
            Game.world.player.Heal(healAmount);
            invSlot.Remove(1);
            invSlot.inventory.UpdateHotbar();
        }
    }
}