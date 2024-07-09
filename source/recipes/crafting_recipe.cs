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
        World World;
        public List<CraftingIngredient> ingredients = new List<CraftingIngredient>();
        public string workstation;
        public List<Item> outcomeItems = new List<Item>();
       public string id;
        public string displayName;
        public ImageBrush displayImage;
        public int requiredTime;

        public CraftingRecipe(World world, string workstation, string id, string displayName, ImageBrush displayImage, int requiredTime)
        {
            this.workstation = workstation;
            this.id = id;
            this.displayName = displayName;
            this.displayImage = displayImage;
            this.requiredTime = requiredTime;

            world.craftingRecipeList.Add(this);
        }   
    }
}
