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
        public Button btnClaim;
        public ProgressBar pbCrafting;
        public CraftingRecipe selectedRecipe;
        public int recipeProgress;
        public bool recipeRunning;
        public bool recipeClaimable;
        public string workstation;
        public int craftingProgressStep;
        System.Windows.Forms.Timer tmrCrafting = new System.Windows.Forms.Timer();

        public CraftingHandler(World world)
        {
            this.world = world;

            //Start the main timer
            tmrCrafting.Interval = 25;
            tmrCrafting.Tick += tmrCrafting_Tick;
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

        public void RenderCraftingRecipes(Canvas cvsRecipes, Canvas cvsIngredients, Button btnCraft, Button btnClaim, ProgressBar pbCrafting, string workstation)
        {
            //Set references and clear existing content
            if(!recipeRunning && !recipeClaimable)
            {
                this.workstation = workstation;
                this.cvsRecipes = cvsRecipes;
                this.cvsIngredients = cvsIngredients;
                this.btnCraft = btnCraft;
                this.btnClaim = btnClaim;
                this.pbCrafting = pbCrafting;
                this.cvsIngredients.Children.Clear();
                this.cvsRecipes.Children.Clear();
                this.btnCraft.IsEnabled = false;
                this.btnCraft.Visibility = Visibility.Visible;
                this.btnClaim.Visibility = Visibility.Hidden;
                this.pbCrafting.Visibility = Visibility.Hidden;
                selectedRecipe = null;
                recipeRunning = false;
                recipeClaimable = false;

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

                        Canvas.SetLeft(tblItem, 60);
                        Canvas.SetTop(tblItem, 17);
                        Canvas.SetLeft(cvsImage, 10);
                        Canvas.SetTop(cvsImage, 10);
                        Canvas.SetTop(cvsItem, top);

                        cvsItem.Children.Add(cvsImage);
                        cvsItem.Children.Add(tblItem);

                        cvsRecipes.Children.Add(cvsItem);

                        top += 50;
                    }
                }
            }       
        }

        public void RenderCraftingDetails(Canvas cvsIngredients, CraftingRecipe recipe)
        {
            int top = 10;
            cvsIngredients.Children.Clear();

            foreach (CraftingIngredient ingredient in recipe.ingredients)
            {
                //Create a canvas with details for each ingredient
                Canvas cvsItem = new Canvas() { Width = 200, Height = 50 };
                TextBlock tblItem = new TextBlock() { Text = $"{ingredient.item.name} - {world.player.inventory.GetItemAmount(ingredient.item.id)}/{ingredient.amount}", FontSize = 14, FontWeight = FontWeights.DemiBold };
                Canvas cvsImage = new Canvas() { Background = ingredient.item.image, Width = 20, Height = 20 };

                if (world.player.inventory.GetItemAmount(ingredient.item.id) < ingredient.amount)
                {
                    tblItem.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    tblItem.Foreground = new SolidColorBrush(Colors.Green);
                }

                Canvas.SetLeft(tblItem, 45);
                Canvas.SetTop(tblItem, top);
                Canvas.SetLeft(cvsImage, 12);
                Canvas.SetTop(cvsImage, top);

                cvsItem.Children.Add(cvsImage);
                cvsItem.Children.Add(tblItem);

                cvsIngredients.Children.Add(cvsItem);

                top += 30;
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

        public void ClaimItem(CraftingRecipe recipe)
        {
            recipeClaimable = false;
            //Add all outcome items to the players inventory
            foreach (Item item in recipe.outcomeItems)
            {
                world.player.inventory.AddItem(item);
            }
        }

        public bool RequiredItemsAvailable()
        {
            return true;

            //Check for each ingredient
            foreach (CraftingIngredient ingredient in selectedRecipe.ingredients)
            {
                if (world.player.inventory.GetItemAmount (ingredient.item.id) < ingredient.amount)
                {
                    return false;
                }
            }
            return true;
        }

        //-- Event Handlers --//

        private void cvsItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!recipeRunning && !recipeClaimable)
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
                btnCraft.IsEnabled = RequiredItemsAvailable();
            }
        }

        public void btnCraft_Click(object sender, RoutedEventArgs e)
        {
            recipeProgress = 0;
            pbCrafting.Visibility = Visibility.Visible;
            pbCrafting.Value = 0;
            btnCraft.Visibility = Visibility.Hidden;
            recipeRunning = true;

            tmrCrafting.Start();
        }

        public void btnClaim_Click(object sender, RoutedEventArgs e)
        {
            //Claim the item
            ClaimItem(selectedRecipe);

            //Render the UI new
            string tempRecipe = selectedRecipe.id;
            RenderCraftingRecipes(cvsRecipes, cvsIngredients, btnCraft, btnClaim, pbCrafting, workstation);
            selectedRecipe = GetRecipe(tempRecipe);
            RenderCraftingDetails(cvsIngredients, selectedRecipe);

            //Check if all requirements for crafting the recipe are met
            btnCraft.IsEnabled = RequiredItemsAvailable();
        }

        private void tmrCrafting_Tick(object sender, EventArgs e)
        {
            //Update all water blocks accordingly
            recipeProgress += 25;

            pbCrafting.Maximum = selectedRecipe.requiredTime;
            pbCrafting.Value += 25;

            if (recipeProgress >= selectedRecipe.requiredTime)
            {
                recipeRunning = false;
                recipeClaimable = true;
                tmrCrafting.Stop();
                pbCrafting.Visibility = Visibility.Hidden;
                btnClaim.Visibility = Visibility.Visible;
            }
        }
    }
}
