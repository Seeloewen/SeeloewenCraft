using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.crafting
{
    public struct RecipePart
    {
        public string item;
        public int amount;

        public RecipePart(JsonToken token)
        {
            item = token.GetString("/item_id");
            amount = token.GetInt("/amount");
        }

    }
}
