
namespace SeeloewenCraft
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
            item = ItemRegister.GenerateItem(token.GetString("/item_id"));

            amount = token.GetInt("/amount");
        }

    }
}
