
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
        public CraftingIngredient(JsonToken token, World world)
        {
            item = ItemRegister.GenerateItem(token.GetString("/item_id"), world);

            amount = token.GetInt("/amount");
        }

    }
}
