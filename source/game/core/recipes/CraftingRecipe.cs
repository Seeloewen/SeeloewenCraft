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

        public CraftingRecipe(JsonToken token)
        {
            //Get general constants
            id = token.GetString("/id");
            workstationId = token.GetString("/workstation");
            displayName = token.GetString("/display_name");
            requiredTime = token.GetInt("/required_time");

            //Get Ingredients
            JsonToken ingredientListToken = token.GetToken("/ingredients");
            for (int i = 0; i < ingredientListToken.GetArrayLength(); i++)
            {
                JsonToken ingredientToken = ingredientListToken.GetToken($"/{i}");
                ingredients.Add(new RecipePart(ingredientToken));
            }

            //Get Outgredient
            JsonToken outgredientToken = token.GetToken("/outgredient");
            outgredient = new RecipePart(outgredientToken);   
        }
    }
}
