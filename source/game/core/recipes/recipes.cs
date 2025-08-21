
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.crafting
{
    public class RecipeCreator
    {

        //-- Custom Methods --//

        public void CreateRecipes()
        {
            //Setup all recipes
            JsonToken recipeListToken = JsonUtil.ReadResource("recipes.json").GetToken("/recipes");

            for (int i = 0; i < recipeListToken.GetArrayLength(); i++)
            {
                JsonToken recipeToken = recipeListToken.GetToken($"/{i}");

                Game.world.craftingRecipeList.Add(new CraftingRecipe(recipeToken));
            }
        }
    }
}
