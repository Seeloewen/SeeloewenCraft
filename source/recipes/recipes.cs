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

        public RecipeCreator(World world)
        {
            this.world = world;
        }

        public void CreateRecipes()
        {
            hammer = new CraftingRecipe(world, "AlphaCrafter", "sc:recipe_hammer", "Hammer", world.images.Hammer, 5000);
            hammer.ingredients.Add(new CraftingIngredient(new StoneItem(world, null), 8));
            hammer.ingredients.Add(new CraftingIngredient(new OakLogItem(world, null), 3));
            hammer.outcomeItems.Add(new HammerItem(world, null));

            chest = new CraftingRecipe(world, "AlphaCrafter", "sc:chest", "Chest", world.images.ChestBlock, 300);
            chest.ingredients.Add(new CraftingIngredient(new OakLogItem(world, null), 10));
            chest.outcomeItems.Add(new HammerItem(world, null));
        }

        public CraftingRecipe hammer;
        public CraftingRecipe chest;

    }
}
