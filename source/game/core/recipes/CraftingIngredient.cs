
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.crafting
{
    public class CraftingIngredient
    {

        public Item item;
        public int amount;

        //-- Constructor --//

        public CraftingIngredient(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
        public CraftingIngredient(JsonToken token)
        {
            item = ItemRegister.Get(token.GetString("/item_id"));

            amount = token.GetInt("/amount");
        }

    }
}
