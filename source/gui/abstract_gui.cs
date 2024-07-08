using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public abstract class Gui
    {
        World world;
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

        public void Show()
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
        public ListView listView;

        public AlphaCrafterGui(World world, int height, int width, int top, int left, string id, Inventory inventory) : base(world, height, width, top, left, id)
        {
            this.inventory = inventory;

            tblHeader.Text = "Alpha Crafter";
            
            TextBlock tblRecipesHeader = new TextBlock() { FontSize = 18, Text = "Available Recipes", FontWeight = FontWeights.DemiBold};
            Canvas.SetLeft(tblRecipesHeader, 46);
            Canvas.SetTop(tblRecipesHeader, 45);

            TextBlock tblIngredients = new TextBlock() { FontSize = 18, Text = "Ingredients", FontWeight = FontWeights.DemiBold };
            Canvas.SetLeft(tblIngredients, 271);
            Canvas.SetTop(tblIngredients, 45);

            Canvas cvsRecipeDetails = new Canvas() { Width = 400, Height = 375, Background = new SolidColorBrush(Colors.White) };
            Canvas.SetLeft(cvsRecipeDetails, 270);
            Canvas.SetTop(cvsRecipeDetails, 80);

            Canvas cvsRecipes = new Canvas() { Width = 200, Height = 375 };
            Canvas.SetLeft(cvsRecipes, 45);
            Canvas.SetTop(cvsRecipes, 80);

            Button btnCraft = new Button() { Width = 125, Height = 30, Content = "Craft", FontSize = 18 };
            Canvas.SetLeft(btnCraft, 295);
            Canvas.SetTop(btnCraft, 475);

            cvsGui.Children.Add(tblRecipesHeader);
            cvsGui.Children.Add(cvsRecipes);
            cvsGui.Children.Add(cvsRecipeDetails);
            cvsGui.Children.Add(tblIngredients);
            cvsGui.Children.Add(btnCraft);
            world.craftingHandler.RenderCraftingRecipes(cvsRecipes, cvsRecipeDetails, btnCraft, "AlphaCrafter");
        }
    }
}
