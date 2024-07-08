using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class craftingHandler
    {
        World world;
        public Canvas cvsRecipes;
        public Canvas cvsIngredients;
        public Button btnCraft;

        public craftingHandler(World world)
        {
            this.world = world;
        }

        public void CreateRecipes()
        {
            hammer = new craftingRecipe(world, "AlphaCrafter", "sc:recipe_hammer", "Hammer", world.images.Hammer);
            hammer.ingredients.Add(new craftingIngredient(new StoneItem(world, null), 3));
            hammer.ingredients.Add(new craftingIngredient(new OakLogItem(world, null), 2));
            hammer.outcomeItems.Add(new HammerItem(world, null));
        }

        public craftingRecipe GetRecipe(string id)
        {
            foreach (craftingRecipe recipe in world.craftingRecipeList)
            {
                if (recipe.id == id)
                {
                    return recipe;
                }
            }
            return null;
        }

        public void RenderCraftingRecipes(Canvas cvsRecipes, Canvas cvsIngredients, Button btnCraft, string workstation)
        {
            this.cvsRecipes = cvsRecipes;
            this.cvsIngredients = cvsIngredients;
            this.btnCraft = btnCraft;

            int top = 0;
            foreach (craftingRecipe recipe in world.craftingRecipeList)
            {
                if (recipe.workstation == workstation)
                {
                    Canvas cvsItem = new Canvas() { Tag = recipe.id, Width = 186, Height = 50 };
                    cvsItem.MouseDown += cvsItem_MouseDown;
                    TextBlock tblItem = new TextBlock() { Text = recipe.displayName, FontSize = 16, FontWeight = FontWeights.DemiBold };
                    Canvas cvsImage = new Canvas() { Background = recipe.displayImage, Width = 40, Height = 40 };
                    Canvas.SetLeft(tblItem, 55);
                    Canvas.SetTop(tblItem, 12);
                    Canvas.SetLeft(cvsImage, 3);
                    Canvas.SetTop(cvsImage, 3);

                    cvsItem.Children.Add(cvsImage);
                    cvsItem.Children.Add(tblItem);
                    Canvas.SetTop(cvsImage, top);

                    cvsRecipes.Children.Add(cvsItem);

                    top += 50;
                }
            }
        }

        private void cvsItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var sourceElement = e.OriginalSource as DependencyObject;
            while (sourceElement != null && !(sourceElement is Canvas))
            {
                sourceElement = VisualTreeHelper.GetParent(sourceElement);
            }

            if (sourceElement is Canvas cvsItem)
            {
                if (cvsItem.Tag != null && cvsItem.Tag is string tag)
                {
                    RenderCraftingDetails(cvsIngredients, tag);
                }
            }
        }

        public void RenderCraftingDetails(Canvas cvsIngredients, string recipeId)
        {
            MessageBox.Show(recipeId);
            craftingRecipe recipe = GetRecipe(recipeId);

            int top = 5;

            foreach (craftingIngredient ingredient in recipe.ingredients)
            {
                Canvas cvsItem = new Canvas() { Width = 200, Height = 50 };
                TextBlock tblItem = new TextBlock() { Text = ingredient.item.name, FontSize = 14, FontWeight = FontWeights.DemiBold };
                Canvas cvsImage = new Canvas() { Background = ingredient.item.image, Width = 20, Height = 20 };
                Canvas.SetLeft(tblItem, 55);
                Canvas.SetTop(tblItem, top + 3);
                Canvas.SetLeft(cvsImage, 3);
                Canvas.SetTop(cvsImage, top);

                cvsItem.Children.Add(cvsImage);
                cvsItem.Children.Add(tblItem);

                top += 55;

                cvsIngredients.Children.Add(cvsItem);
            }
        }

        public void CraftItem()
        {

        }
        //-- Recipes --//

        public craftingRecipe hammer;
    }
}
