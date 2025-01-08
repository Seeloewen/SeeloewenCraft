using System.Collections.Generic;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class CraftingRecipe
    {
        //References
        
        public ImageBrush displayImage;
        public List<CraftingIngredient> outgredients = new List<CraftingIngredient>();
        public List<CraftingIngredient> ingredients = new List<CraftingIngredient>();

        //Constants
        public string workstation;
        public string id;
        public string displayName;
        public int requiredTime;

        //-- Constructor --//

        public CraftingRecipe( string workstation, string id, string displayName, ImageBrush displayImage, int requiredTime)
        {
            //Set all constants
            this.workstation = workstation;
            this.id = id;
            this.displayName = displayName;
            this.displayImage = displayImage;
            this.requiredTime = requiredTime;

            //Add the recipe to the main list so it can be accessed in the future
            Game.world.craftingRecipeList.Add(this);
        }

        public CraftingRecipe(JsonToken token)
        {
            //Get general constants
            id = token.GetString("/id");
            workstation = token.GetString("/workstation");
            displayName = token.GetString("/display_name");
            requiredTime = token.GetInt("/required_time");

            //Get Ingredients
            JsonToken ingredientListToken = token.GetToken("/ingredients");
            for (int i = 0; i < ingredientListToken.GetArrayLength(); i++)
            {
                JsonToken ingredientToken = ingredientListToken.GetToken($"/{i}");
                ingredients.Add(new CraftingIngredient(ingredientToken));
            }

            //Get Outgredients
            JsonToken outgredientListToken = token.GetToken("/outgredients");
            for (int i = 0; i < outgredientListToken.GetArrayLength(); i++)
            {
                JsonToken outgredientToken = outgredientListToken.GetToken($"/{i}");
                outgredients.Add(new CraftingIngredient(outgredientToken));
            }


            displayImage = outgredients[0].item.image;
        }
    }
}
