using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public List<Item> outcomeItems = new List<Item>();
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
    }
}
