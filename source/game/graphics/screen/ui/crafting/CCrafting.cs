using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.graphics.ui_lib;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SeeloewenCraft.game.graphics
{
    internal class CCrafting : CGui
    {
        private CraftingHandler handler;

        internal static int WIDTH = 800;
        internal static int HEIGHT = 500;

        private List<CRecipe> recipes = new List<CRecipe>();
        private List<CIngredient> ingredients = new List<CIngredient>();

        CBorder cBorder;
        CText cHeader;
        CText cRecipeHeader;
        CText cIngredientsHeader;
        CScrollPane cRecipePane;
        CRectangle cIngredients;
        CButton cCraftButton;
        CButton cClaimButton;
        CProgressBar cCraftingProgress;
        CTextBox cAmount;
        CText cAmountHeader;

        internal CCrafting(IGuiData data) : base(data, new Color(0.82f), new Rectangle(GuiSizes.mx - (WIDTH / 2), GuiSizes.my - (HEIGHT / 2), GuiSizes.mx + (WIDTH / 2), GuiSizes.my + (HEIGHT / 2)))
        {
            handler = (CraftingHandler)data;

            cBorder = new CBorder(5, new Color(0.5f));
            cHeader = new CText(data.GetTag<string>("header"), 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));

            cRecipeHeader = new CText("Recipes", 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 40, TextVAlignment.TOP));
            cIngredientsHeader = new CText("Ingredients", 2, new TextLayout(bounds.x1P + 255, TextHAlignment.LEFT, bounds.y1P + 40, TextVAlignment.TOP));

            cRecipePane = new CScrollPane(new Color(0.88f), new Rectangle(bounds.x1P + 20, bounds.y1P + 60, bounds.x1P + 240, bounds.y2P - 80), 0);
            cIngredients = new CRectangle(new Color(0.88f), new Rectangle(bounds.x1P + 255, bounds.y1P + 60, bounds.x2P - 20, bounds.y2P - 80));

            cCraftButton = new CButton(CCraftButton_Click, "Craft", "sc:button_1", "general", new Rectangle(GuiSizes.mx - 50, bounds.y2P - 65, GuiSizes.mx + 50, bounds.y2P - 20));
            cClaimButton = new CButton(CClaimButton_Click, "Claim", "sc:button_1", "general", new Rectangle(GuiSizes.mx - 50, bounds.y2P - 65, GuiSizes.mx + 50, bounds.y2P - 20));
            cClaimButton.visible = false;

            cCraftingProgress = new CProgressBar(new Color(0.7f), new Color(0.3f, 0.58f, 0.82f), new Rectangle(bounds.x1P + 20, bounds.y2P - 55, bounds.x2P - 20, bounds.y2P - 30));
            cCraftingProgress.visible = false;

            cAmountHeader = new CText("Amount:", 2, new TextLayout(bounds.x1P + 22, TextHAlignment.LEFT, bounds.y2P - 48, TextVAlignment.TOP));
            cAmount = new CTextBox(bounds.x1P + 110, bounds.y2P - 60, 75, 40, new Color(0.69f));
            cAmount.GetField().SetText("1");

            AddChild(cBorder);
            AddChild(cHeader);
            AddChild(cCraftButton);
            AddChild(cRecipePane);
            AddChild(cIngredients);
            AddChild(cRecipeHeader);
            AddChild(cIngredientsHeader);
            AddChild(cCraftingProgress);
            AddChild(cClaimButton);
            AddChild(cAmount);
            AddChild(cAmountHeader);

            InitRecipes();
        }

        protected override void OnUpdate(double dt)
        {
            if (handler.recipeRunning)
            {
                //When a recipe is running, show the progressbar and update it
                cCraftingProgress.visible = true;
                cCraftButton.visible = false;
                cAmount.visible = false;
                cAmountHeader.visible = false;
                cCraftingProgress.SetProgress(handler.GetCraftingProgress());

                if (handler.recipeClaimable)
                {
                    //If the crafting is done, show the claim button
                    cClaimButton.visible = true;
                    cCraftingProgress.visible = false;
                }
            }
        }

        private void CCraftButton_Click() //TODO: error when no recipe selected or insufficient resources
        {
            CraftingRecipe recipe = GetSelectedRecipe();
            int amount = 1;

            cAmount.GetField().editMode = true;

            //Check if the amount is valid before crafting
            if (!int.TryParse(cAmount.GetField().GetText(), out amount))
            {
                MessageBox.Show("Please enter a valid amount for crafting", "Invalid amount", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (recipe != null && CraftingHandler.RequiredItemsAvailable(recipe, amount))
            {
                handler.BeginCrafting(recipe, amount);
                cCraftButton.visible = false;
                cCraftingProgress.visible = true;
            }
            else
            {
                MessageBox.Show("You do not have enough resources to craft this!", "Not enough resources", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CClaimButton_Click()
        {
            if (handler.Claim())
            {
                //If the items were successfully claimed, reset the view
                cClaimButton.visible = false;
                cCraftButton.visible = true;
                cCraftingProgress.visible = false;
                cAmount.visible = true;
                cAmountHeader.visible = true;
            }
        }

        private void InitRecipes()
        {
            int i = 0;

            foreach (CraftingRecipe recipe in CraftingHandler.recipeList)
            {
                if (recipe.workstationId == handler.workstationId)
                {
                    //Render all recipes that belong to this workstation
                    CRecipe cRecipe = new CRecipe(cRecipePane.GetBounds().x1P, cRecipePane.GetBounds().y1P + i * CRecipe.HEIGHT, recipe.outgredient.item, recipe.id, this);

                    recipes.Add(cRecipe);
                    cRecipePane.AddScrollable(cRecipe);
                    i++;
                }
            }

            cRecipePane.maxI = Math.Max(recipes.Count * CRecipe.HEIGHT - cRecipePane.height, 0);
        }

        internal void DisplayIngredients(string id)
        {
            //Clear all previous entry
            cIngredients.ClearChildren();
            ingredients.Clear();
            int i = 0;

            CraftingRecipe r = CraftingHandler.GetRecipe(id);
            foreach (RecipePart ingredient in r.ingredients)
            {
                //Display all ingredients
                CIngredient cIngredient = new CIngredient(cIngredients.GetBounds().x1P + 5, 5 + cIngredients.GetBounds().y1P + i * CIngredient.HEIGHT, ingredient.item, ingredient.amount);

                ingredients.Add(cIngredient);
                cIngredients.AddChild(cIngredient);
                i++;
            }
        }

        private CraftingRecipe GetSelectedRecipe()
        {
            foreach (CRecipe cRecipe in recipes)
            {
                if (cRecipe.isSelected) return CraftingHandler.GetRecipe(cRecipe.recipeId);
            }

            return null;
        }

        internal void ClearSelection()
        {
            foreach (CRecipe cRecipe in recipes)
            {
                cRecipe.isSelected = false;
            }
        }
    }
}
