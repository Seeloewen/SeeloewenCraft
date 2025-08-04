using SeeloewenCraft.game.graphics.ui_lib;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;

namespace SeeloewenCraft.game.graphics
{
    internal class CCrafting : CGui
    {
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

        internal CCrafting(IGuiData data) : base(data, new Color(0.82f), new Rectangle(GuiSizes.mx - (WIDTH / 2), GuiSizes.my - (HEIGHT / 2), GuiSizes.mx + (WIDTH / 2), GuiSizes.my + (HEIGHT / 2)))
        {
            cBorder = new CBorder(5, new Color(0.5f));

            cHeader = new CText("MISSING_NAME", 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 15, TextVAlignment.TOP));

            cRecipeHeader = new CText("Recipes", 2, new TextLayout(bounds.x1P + 20, TextHAlignment.LEFT, bounds.y1P + 40, TextVAlignment.TOP));
            cIngredientsHeader = new CText("Ingredients", 2, new TextLayout(bounds.x1P + 255, TextHAlignment.LEFT, bounds.y1P + 40, TextVAlignment.TOP));

            cRecipePane = new CScrollPane(new Color(0.88f), new Rectangle(bounds.x1P + 20, bounds.y1P + 60, bounds.x1P + 240, bounds.y2P -80), 0);
            cIngredients = new CRectangle(new Color(0.88f), new Rectangle(bounds.x1P + 255, bounds.y1P + 60, bounds.x2P - 20, bounds.y2P - 80));

            cCraftButton = new CButton(null, "Craft", "sc:button_1", GeneralTextureMap.Get(), new Rectangle(GuiSizes.mx - 50, bounds.y2P - 65, GuiSizes.mx + 50, bounds.y2P - 20));

            AddChild(cBorder);
            AddChild(cHeader);
            AddChild(cCraftButton);
            AddChild(cRecipePane);
            AddChild(cIngredients);
            AddChild(cRecipeHeader);
            AddChild(cIngredientsHeader);

            InitRecipes();
        }

        private void InitRecipes()
        {
            CraftingHandler handler = (CraftingHandler)data;

            int i = 0;

            foreach (CraftingRecipe recipe in Game.world.craftingRecipeList)
            {
                if (recipe.workstation == handler.workstation) 
                {
                    CRecipe cRecipe = new CRecipe(cRecipePane.GetBounds().x1P, cRecipePane.GetBounds().y1P + i * CRecipe.HEIGHT, recipe.outgredients[0].item.id, recipe.id );

                    recipes.Add(cRecipe);
                    cRecipePane.AddScrollable(cRecipe);
                    i++;
                }
            }

            cRecipePane.maxI = Math.Max(recipes.Count * CRecipe.HEIGHT - cRecipePane.height, 0);
        }

        internal void DisplayIngredients(string id)
        {
            CraftingRecipe r = CraftingHandler.GetRecipe(id);
            cIngredients.ClearChildren();
            ingredients.Clear();
            int i = 0;

            foreach(CraftingIngredient ingredient in r.ingredients)
            {
                CIngredient cIngredient = new CIngredient(cIngredients.GetBounds().x1P, cIngredients.GetBounds().y1P + i * CIngredient.HEIGHT, ingredient.item.id, ingredient.amount);
                    
                ingredients.Add(cIngredient);
                cIngredients.AddChild(cIngredient);
                i++;
            }
        }
    }
}
