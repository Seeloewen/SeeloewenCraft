using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class craftingRecipe
    {
        World World;
        public List<craftingIngredient> ingredients = new List<craftingIngredient>();
        public string workstation;
        public List<Item> outcomeItems = new List<Item>();
       public string id;
        public string displayName;
        public ImageBrush displayImage;

        public craftingRecipe(World world, string workstation, string id, string displayName, ImageBrush displayImage)
        {
            this.workstation = workstation;
            this.id = id;
            this.displayName = displayName;
            this.displayImage = displayImage;

            world.craftingRecipeList.Add(this);
        }   
    }
}
