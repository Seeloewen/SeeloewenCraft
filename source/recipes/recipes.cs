
namespace SeeloewenCraft
{
    public class RecipeCreator
    {
        

        //-- Constructor --//

        public RecipeCreator()
        {
            
        }

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
