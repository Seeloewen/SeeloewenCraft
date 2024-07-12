using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class CraftingRecipe
    {
        //References
        World World;
        public ImageBrush displayImage;
        public List<CraftingIngredient> outgredients = new List<CraftingIngredient>();
        public List<CraftingIngredient> ingredients = new List<CraftingIngredient>();

        //Constants
        public string workstation;
        public string id;
        public string displayName;
        public int requiredTime;

        //-- Constructor --//

        public CraftingRecipe(World world, string workstation, string id, string displayName, ImageBrush displayImage, int requiredTime)
        {
            //Set all constants
            this.workstation = workstation;
            this.id = id;
            this.displayName = displayName;
            this.displayImage = displayImage;
            this.requiredTime = requiredTime;

            //Add the recipe to the main list so it can be accessed in the future
            world.craftingRecipeList.Add(this);
        }

        public CraftingRecipe(JsonToken token, World world)
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
                ingredients.Add(new CraftingIngredient(ingredientToken, world));
            }

            //Get Outgredients
            JsonToken outgredientListToken = token.GetToken("/outgredients");
            for (int i = 0; i < outgredientListToken.GetArrayLength(); i++)
            {
                JsonToken outgredientToken = outgredientListToken.GetToken($"/{i}");
                outgredients.Add(new CraftingIngredient(outgredientToken, world));
            }

            displayImage = outgredients[0].item.image;
        }
    }
}
