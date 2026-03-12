using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.crafting
{
    public struct RecipePart
    {
        public readonly string item;
        public readonly int amount;

        public RecipePart(JObject token)
        {
            item = token.Get<string>("item_id");
            amount = token.Get<int>("amount");
        }

    }
}
