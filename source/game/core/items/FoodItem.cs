using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities.inventory;

namespace SeeloewenCraft.game.core.items
{
    public class FoodItem : Item
    {
        public double healAmount;

        public override void RightClickAction(Block block, InventorySlot invSlot)
        {
            //Heal the player and remove the item
            Game.world.player.Heal(healAmount);
            invSlot.Remove(1);
        }

    }
}