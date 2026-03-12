using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.util;
using System.Collections.Generic;

namespace SeeloewenCraft.game.core.crafting
{
    public record CraftingRecipe
    {
        public List<RecipePart> ingredients = new List<RecipePart>();
        public RecipePart outgredient;

        public string workstationId;
        public string id;
        public string displayName;
        public int requiredTime;

        public CraftingRecipe(JObject token)
        {
            //Get general constants
            id = token.Get<string>("id");
            workstationId = token.Get<string>("workstation");
            displayName = token.Get<string>("display_name");
            requiredTime = token.Get<int>("required_time");

            //Get Ingredients
            JArray ingredientListToken = token.Get<JArray>("ingredients");
            foreach (JObject ingObj in ingredientListToken)
            {
                ingredients.Add(new RecipePart(ingObj));
            }

            //Get Outgredient
            JObject outgredientToken = token.Get<JObject>("outgredient");
            outgredient = new RecipePart(outgredientToken);
        }
    }
}
