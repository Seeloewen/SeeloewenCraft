using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class RecipeCreator
    {
        World world;

        //-- Constructor --//

        public RecipeCreator(World world)
        {
            this.world = world;
        }

        //-- Custom Methods --//

        public void CreateRecipes()
        {
            //Setup all recipes
            JsonToken recipeListToken = JsonUtil.ReadResource("recipes.json").GetToken("/recipes");

            for (int i = 0; i < recipeListToken.GetArrayLength(); i++)
            {
                JsonToken recipeToken = recipeListToken.GetToken($"/{i}");

                world.craftingRecipeList.Add(new CraftingRecipe(recipeToken, world));
            }
        }
    }
}
