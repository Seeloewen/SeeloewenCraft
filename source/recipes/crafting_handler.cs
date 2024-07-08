using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class CraftingHandler
    {
        World world;
        public Canvas cvsRecipes;
        public Canvas cvsIngredients;
        public Button btnCraft;
        public CraftingRecipe selectedRecipe;

        public CraftingHandler(World world)
        {
            this.world = world;
        }

        public CraftingRecipe GetRecipe(string id)
        {
            foreach (CraftingRecipe recipe in world.craftingRecipeList)
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
            //Set references and clear existing content
            this.cvsRecipes = cvsRecipes;
            this.cvsIngredients = cvsIngredients;
            this.btnCraft = btnCraft;
            this.cvsIngredients.Children.Clear();
            this.cvsRecipes.Children.Clear();
            this.btnCraft.IsEnabled = false;
            selectedRecipe = null;

            int top = 0;
            foreach (CraftingRecipe recipe in world.craftingRecipeList)
            {
                //Go through recipes and find the correct one
                if (recipe.workstation == workstation)
                {
                    //Setup contents of the recipe
                    Canvas cvsItem = new Canvas() { Tag = recipe.id, Width = 186, Height = 50 };
                    TextBlock tblItem = new TextBlock() { Text = recipe.displayName, FontSize = 16, FontWeight = FontWeights.DemiBold };
                    Canvas cvsImage = new Canvas() { Background = recipe.displayImage, Width = 40, Height = 40 };

                    cvsItem.MouseDown += cvsItem_MouseDown;

                    Canvas.SetLeft(tblItem, 55);
                    Canvas.SetTop(tblItem, 12);
                    Canvas.SetLeft(cvsImage, 3);
                    Canvas.SetTop(cvsImage, 3);
                    Canvas.SetTop(cvsImage, top);

                    cvsItem.Children.Add(cvsImage);
                    cvsItem.Children.Add(tblItem);

                    cvsRecipes.Children.Add(cvsItem);

                    top += 50;
                }
            }
        }

        public void RenderCraftingDetails(Canvas cvsIngredients, CraftingRecipe recipe)
        {
            int top = 5;
            cvsIngredients.Children.Clear();

            foreach (CraftingIngredient ingredient in recipe.ingredients)
            {
                //Create a canvas with details for each ingredient
                Canvas cvsItem = new Canvas() { Width = 200, Height = 50 };
                TextBlock tblItem = new TextBlock() { Text = $"{ingredient.item.name} - {world.player.inventory.GetItemAmount(ingredient.item.id)}/{ingredient.amount}", FontSize = 14, FontWeight = FontWeights.DemiBold };
                Canvas cvsImage = new Canvas() { Background = ingredient.item.image, Width = 20, Height = 20 };

                Canvas.SetLeft(tblItem, 55);
                Canvas.SetTop(tblItem, top + 3);
                Canvas.SetLeft(cvsImage, 3);
                Canvas.SetTop(cvsImage, top);

                cvsItem.Children.Add(cvsImage);
                cvsItem.Children.Add(tblItem);

                cvsIngredients.Children.Add(cvsItem);

                top += 55;
            }
        }

        public void CraftItem(CraftingRecipe recipe)
        {
            //Add all outcome items to the players inventory
            foreach(Item item in recipe.outcomeItems)
            {
                world.player.inventory.AddItem(item);
            }
        }

        public bool RequiredItemsAvailable()
        {
            return true;
        }

        //-- Event Handlers --//

        private void cvsItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Get the currently selected crafting recipe by checking the clicked element and its parents for the correct tag
            string tag = "";
            var sourceElement = e.OriginalSource as DependencyObject;
            while (sourceElement != null)
            {
                if (sourceElement is Canvas canvas && canvas.Tag != null && !string.IsNullOrEmpty(canvas.Tag.ToString()))
                {
                    tag = canvas.Tag.ToString();
                    break;
                }
                sourceElement = VisualTreeHelper.GetParent(sourceElement);
            }

            selectedRecipe = GetRecipe(tag);

            //Render stuff like ingredients and check if the required items are available
            RenderCraftingDetails(cvsIngredients, selectedRecipe);

            //Check if all requirements for crafting the recipe are met
            btnCraft.IsEnabled = true;
        }

        public void btnCraft_Click(object sender, RoutedEventArgs e)
        {
            //Craft the item
            CraftItem(selectedRecipe);
        }

        //-- Recipes --//
    }
}
