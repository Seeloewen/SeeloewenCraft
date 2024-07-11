using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SeeloewenCraft
{
    public class CraftingHandler
    {
        //References
        private System.Windows.Forms.Timer tmrCrafting = new System.Windows.Forms.Timer();
        public ProgressBar pbCraftingBlock = new ProgressBar() { Height = 12, Width = 40 };
        public World world;
        private Block block;
        private Canvas cvsRecipes;
        private Canvas cvsIngredients;
        private Button btnCraft;
        private Button btnClaim;
        public ProgressBar pbCrafting;
        private TextBox tbAmount;
        public CraftingRecipe selectedRecipe;

        //Constants
        public string workstation;

        //Variables
        public int recipeProgress;
        public bool recipeRunning;
        public bool recipeClaimable;
        private int craftingProgressStep;
        public int amount = 1;

        //-- Constructor --//

        public CraftingHandler(World world, Block block)
        {
            this.world = world;
            this.block = block;

            //Setup some components
            Canvas.SetTop(pbCraftingBlock, 20);
            Canvas.SetLeft(pbCraftingBlock, 5);

            tmrCrafting.Interval = 25;
            tmrCrafting.Tick += tmrCrafting_Tick;
        }

        //-- Custom Methods --//

        public CraftingRecipe GetRecipe(string id)
        {
            //Go through the recipe list and find the recipe with the specified id
            foreach (CraftingRecipe recipe in world.craftingRecipeList)
            {
                if (recipe.id == id)
                {
                    return recipe;
                }
            }
            return null;
        }

        public void RenderCraftingRecipes(Canvas cvsRecipes, Canvas cvsIngredients, Button btnCraft, Button btnClaim, ProgressBar pbCrafting, TextBox tbAmount, string workstation)
        {
            if (!recipeRunning && !recipeClaimable)
            {
                //Set references to gui
                this.workstation = workstation;
                this.cvsRecipes = cvsRecipes;
                this.cvsIngredients = cvsIngredients;
                this.btnCraft = btnCraft;
                this.btnClaim = btnClaim;
                this.pbCrafting = pbCrafting;
                this.tbAmount = tbAmount;

                //Reset previous changes and variables to default
                this.cvsIngredients.Children.Clear();
                this.cvsRecipes.Children.Clear();
                this.btnCraft.IsEnabled = false;
                this.btnCraft.Visibility = Visibility.Visible;
                this.btnClaim.Visibility = Visibility.Hidden;
                this.pbCrafting.Visibility = Visibility.Hidden;
                this.tbAmount.Text = "1";
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
                        Canvas cvsItem = new Canvas() { Tag = recipe.id, Width = 200, Height = 60 };
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

                        top += 60;
                    }
                }
            }
        }

        public void RenderCraftingDetails(Canvas cvsIngredients, CraftingRecipe recipe)
        {
            if (recipe != null)
            {
                int top = 10;
                cvsIngredients.Children.Clear();

                //List all ingredients
                foreach (CraftingIngredient ingredient in recipe.ingredients)
                {
                    //Create a canvas with details for each ingredient
                    Canvas cvsItem = new Canvas() { Width = 200, Height = 50 };
                    TextBlock tblItem = new TextBlock() { Text = $"{ingredient.item.name} - {world.player.inventory.GetItemAmount(ingredient.item.id)}/{ingredient.amount * amount}", FontSize = 17, FontWeight = FontWeights.DemiBold };
                    Canvas cvsImage = new Canvas() { Background = ingredient.item.image, Width = 30, Height = 30 };

                    if (world.player.inventory.GetItemAmount(ingredient.item.id) < ingredient.amount * amount)
                    {
                        tblItem.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        tblItem.Foreground = new SolidColorBrush(Colors.Green);
                    }

                    Canvas.SetLeft(tblItem, 55);
                    Canvas.SetTop(tblItem, top + 2);
                    Canvas.SetLeft(cvsImage, 12);
                    Canvas.SetTop(cvsImage, top);

                    cvsItem.Children.Add(cvsImage);
                    cvsItem.Children.Add(tblItem);

                    cvsIngredients.Children.Add(cvsItem);

                    top += 40;
                }
            }
        }

        public void Claim(CraftingRecipe recipe)
        {
            recipeClaimable = false;
            //Add all outcome items to the players inventory
            for (int i = 0; i < amount; i++)
            {
                foreach (Item item in recipe.outcomeItems)
                {
                    world.player.inventory.AddItem(item);
                }
            }
        }

        public bool RequiredItemsAvailable()
        {
            //Check for each ingredient
            foreach (CraftingIngredient ingredient in selectedRecipe.ingredients)
            {
                if (world.player.inventory.GetItemAmount(ingredient.item.id) < ingredient.amount * amount)
                {
                    return false;
                }
            }
            return true;
        }

        public void Craft(CraftingRecipe recipe, bool isNew)
        {
            //Check if the crafting recipe is new or continued (when the chunk gets loaded for example)
            if (isNew)
            {
                //If it's new, remove the required materials based on the amount from the players inventory
                foreach (CraftingIngredient ingredient in recipe.ingredients)
                {
                    for (int i = 0; i < ingredient.amount * amount; i++)
                    {
                        world.player.inventory.RemoveItem(ingredient.item.id);
                    }
                }
                RenderCraftingDetails(cvsIngredients, recipe);
            }

            //Reset the progress of the possible previous recipe
            recipeProgress = 0;
            pbCrafting.Visibility = Visibility.Visible;
            pbCrafting.Value = 0;
            btnCraft.Visibility = Visibility.Hidden;
            recipeRunning = true;
            tbAmount.IsEnabled = true;

            //If possible, also render the progressbar on the block
            if (block.blockContainer != null)
            {
                pbCraftingBlock.Value = 0;
                block.blockContainer.cvsBlock.Children.Add(pbCraftingBlock);
            }

            //Actually start the crafting timer
            tmrCrafting.Start();
        }

        //-- Event Handlers --//

        private void cvsItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!recipeRunning && !recipeClaimable)
            {
                //Set the background to 'unselected' for every recipe
                foreach (UIElement element in cvsRecipes.Children)
                {
                    if (element is Canvas canvas)
                    {
                        canvas.Background = new SolidColorBrush(Colors.Transparent);
                    }
                }

                //Get the currently selected crafting recipe by checking the clicked element and its parents for the correct tag
                string tag = "";
                var sourceElement = e.OriginalSource as DependencyObject;
                while (sourceElement != null)
                {
                    //Get the recipe canvas
                    if (sourceElement is Canvas canvas && canvas.Tag != null && !string.IsNullOrEmpty(canvas.Tag.ToString()))
                    {
                        //Set canvas state to 'selected' and set tag
                        canvas.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 134, 134, 134));
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
            //Begin crafting the item
            Craft(selectedRecipe, true);
        }

        public void btnClaim_Click(object sender, RoutedEventArgs e)
        {
            //Claim the item
            Claim(selectedRecipe);

            //Refresh the UI
            string tempRecipe = selectedRecipe.id;
            RenderCraftingRecipes(cvsRecipes, cvsIngredients, btnCraft, btnClaim, pbCrafting, tbAmount, workstation);
            selectedRecipe = GetRecipe(tempRecipe);
            RenderCraftingDetails(cvsIngredients, selectedRecipe);

            //Hide progress bar
            if (block.blockContainer != null)
            {
                block.blockContainer.cvsBlock.Children.Remove(pbCraftingBlock);
            }

            //Check if all requirements for crafting the recipe are met
            btnCraft.IsEnabled = RequiredItemsAvailable();
        }

        private void tmrCrafting_Tick(object sender, EventArgs e)
        {
            //Update the timer and progress bars
            recipeProgress += 25;

            pbCrafting.Maximum = selectedRecipe.requiredTime * amount;
            pbCrafting.Value += 25;

            if (block.blockContainer != null)
            {
                pbCraftingBlock.Maximum = selectedRecipe.requiredTime * amount;
                pbCraftingBlock.Value += 25;
            }

            if (recipeProgress >= selectedRecipe.requiredTime * amount)
            {
                //If the timer is complete, finish the process and reset everything to default
                recipeRunning = false;
                recipeClaimable = true;
                tmrCrafting.Stop();
                pbCrafting.Visibility = Visibility.Hidden;
                btnClaim.Visibility = Visibility.Visible;
                tbAmount.IsEnabled = true;

                //Show notification and log that crafting process is complete
                world.notificationHandler.ShowNotification($"Crafting for x{amount} {selectedRecipe.displayName} completed!", 3000, world.images.AlphaCrafter);
                world.log.Write($"Completed crafting for {amount}x {selectedRecipe.id} at workstation {workstation} (X: {block.xPos}, Y: {block.yPos}, Chunk: {block.chunk.index})", "Info");
            }
        }
    }
}
