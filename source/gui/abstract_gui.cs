using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace SeeloewenCraft
{
    public abstract class Gui
    {
        public World world;
        public Inventory inventory;
        public Canvas cvsGui;
        public TextBlock tblHeader;
        public string id;
        public bool isOpen = false;

        //-- Constructor --//

        public Gui(World world, int height, int width, int top, int left, string id)
        {
            //Create references
            this.world = world;
            this.id = id;

            //Setup Canvas
            cvsGui = new Canvas();
            cvsGui.Background = world.images.Gui;
            cvsGui.Width = width;
            cvsGui.Height = height;
            cvsGui.Visibility = Visibility.Hidden;
            Canvas.SetTop(cvsGui, top);
            Canvas.SetLeft(cvsGui, left);
            Panel.SetZIndex(cvsGui, 4);
            world.wndGame.cvsGame.Children.Add(cvsGui);

            //Setup Header
            tblHeader = new TextBlock();
            tblHeader.FontSize = 20;
            tblHeader.Text = id;
            tblHeader.FontWeight = FontWeights.SemiBold;
            Canvas.SetLeft(tblHeader, 25);
            Canvas.SetTop(tblHeader, 2);
            cvsGui.Children.Add(tblHeader);
        }

        public virtual void Show()
        {
            cvsGui.Visibility = Visibility.Visible;
            isOpen = true;
            world.guiList.Add(this);
        }

        public void Hide()
        {
            cvsGui.Visibility = Visibility.Hidden;
            isOpen = false;
            world.guiList.Remove(this);

        }

        public void SetTop(int newTop)
        {
            Canvas.SetTop(cvsGui, newTop);
        }

        public void SetLeft(int newLeft)
        {
            Canvas.SetLeft(cvsGui, newLeft);
        }
    }

    public class InventoryGui : Gui
    {
        public InventoryGui(World world, int height, int width, int top, int left, string id, Inventory inventory) : base(world, height, width, top, left, id)
        {
            this.inventory = inventory;

            tblHeader.Text = "Inventory";
            tblHeader.FontSize = 18;
            Canvas.SetLeft(tblHeader, 20);
        }
    }

    public class AlphaCrafterGui : Gui
    {
        public TextBlock tblRecipesHeader = new TextBlock() { FontSize = 18, Text = "Available Recipes", FontWeight = FontWeights.DemiBold };
        public TextBlock tblIngredients = new TextBlock() { FontSize = 18, Text = "Ingredients", FontWeight = FontWeights.DemiBold };
        public TextBlock tblAmount = new TextBlock() { FontSize = 18, Text = "Amount:", FontWeight = FontWeights.DemiBold };
        public TextBox tbAmount = new TextBox() { FontSize = 18, Text = "1", FontWeight = FontWeights.DemiBold, Width = 45 };
        public ScrollViewer svRecipeDetails = new ScrollViewer() { Width = 400, Height = 375, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        public Canvas cvsRecipeDetails = new Canvas() { Background = new SolidColorBrush(Colors.LightGray) };
        public ScrollViewer svRecipes = new ScrollViewer() { Width = 200, Height = 375, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        public Canvas cvsRecipes = new Canvas() { Background = new SolidColorBrush(Colors.LightGray) };
        public Button btnCraft = new Button() { Width = 125, Height = 30, Content = "Craft Item", FontSize = 18 };
        public Button btnClaim = new Button() { Width = 125, Height = 30, Content = "Claim Item", FontSize = 18, Visibility = Visibility.Hidden, Background = new SolidColorBrush(Colors.LightGreen) };
        public ProgressBar pbCrafting = new ProgressBar() { Width = 380, Height = 30, Visibility = Visibility.Hidden };
        public CraftingHandler craftingHandler;


        public AlphaCrafterGui(World world, int height, int width, int top, int left, string id, Inventory inventory, Block block) : base(world, height, width, top, left, id)
        {
            this.inventory = inventory;
            craftingHandler = block.craftingHandler;

            tblHeader.Text = "Alpha Crafter";

            //Add all the necessary components to the gui
            Canvas.SetLeft(tblRecipesHeader, 46);
            Canvas.SetTop(tblRecipesHeader, 45);
            cvsGui.Children.Add(tblRecipesHeader);

            Canvas.SetLeft(tblIngredients, 271);
            Canvas.SetTop(tblIngredients, 45);
            cvsGui.Children.Add(tblIngredients);

            Canvas.SetLeft(svRecipeDetails, 270);
            Canvas.SetTop(svRecipeDetails, 80);
            svRecipeDetails.Content = cvsRecipeDetails;
            cvsGui.Children.Add(svRecipeDetails);

            Canvas.SetLeft(svRecipes, 45);
            Canvas.SetTop(svRecipes, 80);
            svRecipes.Content = cvsRecipes;
            cvsGui.Children.Add(svRecipes);

            Canvas.SetLeft(btnCraft, 295);
            Canvas.SetTop(btnCraft, 475);
            cvsGui.Children.Add(btnCraft);

            Canvas.SetLeft(btnClaim, 295);
            Canvas.SetTop(btnClaim, 475);
            cvsGui.Children.Add(btnClaim);

            Canvas.SetLeft(pbCrafting, 200);
            Canvas.SetTop(pbCrafting, 475);
            cvsGui.Children.Add(pbCrafting);

            Canvas.SetLeft(tblAmount, 50);
            Canvas.SetTop(tblAmount, 475);
            cvsGui.Children.Add(tblAmount);

            Canvas.SetLeft(tbAmount, 130);
            Canvas.SetTop(tbAmount, 475);
            cvsGui.Children.Add(tbAmount);

            btnCraft.Click += craftingHandler.btnCraft_Click;
            btnClaim.Click += craftingHandler.btnClaim_Click;
            tbAmount.TextChanged += tbAmount_TextChanged;
            tbAmount.PreviewTextInput += tbAmount_PreviewTextInput;

            //Render the recipes
            craftingHandler.RenderCraftingRecipes(cvsRecipes, cvsRecipeDetails, btnCraft, btnClaim, pbCrafting, tbAmount, "AlphaCrafter");
        }

        public override void Show()
        {
            cvsGui.Visibility = Visibility.Visible;
            isOpen = true;
            world.guiList.Add(this);

            //Render the recipes
            craftingHandler.RenderCraftingRecipes(cvsRecipes, cvsRecipeDetails, btnCraft, btnClaim, pbCrafting, tbAmount, "AlphaCrafter");
        }

        private void tbAmount_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbAmount.Text))
            {
                craftingHandler.amount = Convert.ToInt32(tbAmount.Text);
                if (craftingHandler.amount > 64)
                {
                    craftingHandler.amount = 64;
                    tbAmount.Text = "64";
                }
                craftingHandler.RenderCraftingDetails(cvsRecipeDetails, craftingHandler.selectedRecipe);
            }
            else
            {
                craftingHandler.amount = 1;
                craftingHandler.RenderCraftingDetails(cvsRecipeDetails, craftingHandler.selectedRecipe);
            }
        }

        private void tbAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
