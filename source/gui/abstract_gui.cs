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
        public Canvas cvsRecipeDetails = new Canvas() { Width = 400, Height = 375, Background = new SolidColorBrush(Colors.White) };
        public Canvas cvsRecipes = new Canvas() { Width = 200, Height = 375 };
        public Button btnCraft = new Button() { Width = 125, Height = 30, Content = "Craft", FontSize = 18 };
        public CraftingHandler craftingHandler;


        public AlphaCrafterGui(World world, int height, int width, int top, int left, string id, Inventory inventory) : base(world, height, width, top, left, id)
        {
            this.inventory = inventory;
            craftingHandler = new CraftingHandler(world);

            tblHeader.Text = "Alpha Crafter";
            
            //Add all the necessary components to the gui
            Canvas.SetLeft(tblRecipesHeader, 46);
            Canvas.SetTop(tblRecipesHeader, 45);
            cvsGui.Children.Add(tblRecipesHeader);

            Canvas.SetLeft(tblIngredients, 271);
            Canvas.SetTop(tblIngredients, 45);
            cvsGui.Children.Add(tblIngredients);

            Canvas.SetLeft(cvsRecipeDetails, 270);
            Canvas.SetTop(cvsRecipeDetails, 80);
            cvsGui.Children.Add(cvsRecipeDetails);

            Canvas.SetLeft(cvsRecipes, 45);
            Canvas.SetTop(cvsRecipes, 80);
            cvsGui.Children.Add(cvsRecipes);

            Canvas.SetLeft(btnCraft, 295);
            Canvas.SetTop(btnCraft, 475);

            cvsGui.Children.Add(btnCraft);

            btnCraft.Click += craftingHandler.btnCraft_Click;

            //Render the recipes
            craftingHandler.RenderCraftingRecipes(cvsRecipes, cvsRecipeDetails, btnCraft, "AlphaCrafter");
        }

        public override void Show()
        {
            cvsGui.Visibility = Visibility.Visible;
            isOpen = true;
            world.guiList.Add(this);

            //Render the recipes
            craftingHandler.RenderCraftingRecipes(cvsRecipes, cvsRecipeDetails, btnCraft, "AlphaCrafter");

        }
    }
}
