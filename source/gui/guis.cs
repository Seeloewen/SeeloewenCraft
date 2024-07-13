using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Microsoft.VisualBasic.Logging;

namespace SeeloewenCraft
{
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

    public class NotificationGui : Gui
    {
        //Controls
        ScrollViewer svNotifications = new ScrollViewer() { Width = 356, Height = 495, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        ListView lvNotifications = new ListView() { Background = new SolidColorBrush(Colors.Transparent) };
        Button btnClose = new Button() { Width = 160, Height = 30, Content = "Close", FontSize = 16 };
        Button btnClearAll = new Button() { Width = 160, Height = 30, Content = "Clear All", FontSize = 16 };

        //References
        NotificationHandler notificationHandler;

        public NotificationGui(World world, int height, int width, int top, int left, string id, NotificationHandler notificationHandler) : base(world, height, width, top, left, id)
        {
            //Setup gui
            tblHeader.Text = "Notifications";
            this.notificationHandler = notificationHandler;

            Canvas.SetTop(tblHeader, 8);
            Canvas.SetLeft(tblHeader, 10);

            Canvas.SetLeft(svNotifications, 9);
            Canvas.SetTop(svNotifications, 40);
            cvsGui.Children.Add(svNotifications);

            Canvas.SetTop(btnClose, 545);
            Canvas.SetLeft(btnClose, 192);
            cvsGui.Children.Add(btnClose);

            Canvas.SetTop(btnClearAll, 545);
            Canvas.SetLeft(btnClearAll, 20);
            cvsGui.Children.Add(btnClearAll);

            btnClearAll.Click += btnClearAll_Click;
            btnClose.Click += btnClose_Click;

            svNotifications.Content = lvNotifications;
        }

        public void AddNotification(string message, ImageBrush image)
        {
            //Create components
            Border bdrNotification = new Border() { Width = 342, Height = 75, BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(3) };
            Canvas cvsNotification = new Canvas() { Background = new SolidColorBrush(Colors.DarkGray) };
            TextBlock tblNotification = new TextBlock() { FontSize = 18, Width = 250, Height = 50, TextWrapping = TextWrapping.Wrap };
            Canvas cvsImage = new Canvas() { Width = 50, Height = 50 };

            //Setup necessary components for showing the message
            bdrNotification.Child = cvsNotification;

            Canvas.SetTop(tblNotification, 10);
            Canvas.SetLeft(tblNotification, 75);
            cvsNotification.Children.Add(tblNotification);

            Canvas.SetTop(cvsImage, 10);
            Canvas.SetLeft(cvsImage, 10);
            cvsNotification.Children.Add(cvsImage);

            tblNotification.Text = message;
            cvsImage.Background = image;

            lvNotifications.Items.Add(bdrNotification);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Hide the notification list gui
            notificationHandler.HideGui();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //Ask the user whether they really want to clear the notification list
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to clear all notifications?", "Clear Notifications", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                //Clear the notification list
                case MessageBoxResult.Yes:
                    lvNotifications.Items.Clear();
                    break;
            }
        }
    }
}