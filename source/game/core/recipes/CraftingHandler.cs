using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.notifications;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System.Collections.Generic;

namespace SeeloewenCraft.game.core.crafting
{
    public class CraftingHandler : IGuiData
    {
        internal static List<CraftingHandler> craftingHandlers; //Used to update all handlers across the game

        public string guiId { get; set; } = "crafting_handler";
        public string tags { get; set; }

        public static List<CraftingRecipe> recipeList;

        public CraftingRecipe currentRecipe;
        private readonly Block block;

        public readonly string workstationId;
        public readonly string workstationName;

        public double recipeProgress;
        public bool recipeRunning;
        public bool recipeClaimable;
        public int recipeAmount = 1;

        public static void Init() //Should be called BEFORE initializing the blockregister to avoid issues
        {
            craftingHandlers = new List<CraftingHandler>();
        }

        public static void LoadRecipes() //Should be loaded AFTER initializing items
        {
            recipeList = new List<CraftingRecipe>();

            //Setup all recipes
            JsonToken recipeListToken = JsonUtil.ReadResource("recipes.json").GetToken("/recipes");

            for (int i = 0; i < recipeListToken.GetArrayLength(); i++)
            {
                JsonToken recipeToken = recipeListToken.GetToken($"/{i}");
                recipeList.Add(new CraftingRecipe(recipeToken));
            }
        }

        public static void Update(double dt) => craftingHandlers.ForEach(h => h.OnUpdate(dt));

        public CraftingHandler(Block block, string workstationId, string workstationName)
        {
            this.workstationId = workstationId;
            this.workstationName = workstationName;
            this.block = block;
            block.blockState = BlockState.RECIPE_IDLE;

            ((IGuiData)this).AddTag("header", workstationName);

            craftingHandlers.Add(this);
        }

        public static CraftingRecipe GetRecipe(string id)
        {
            //Go through the recipe list and find the recipe with the specified id
            foreach (CraftingRecipe recipe in recipeList)
            {
                if (recipe.id == id) return recipe;
            }
            return null;
        }

        public bool Claim() //Returns whether the items were actually added to the inventory and thus successfully claimed
        {
            if (Player.Get().inventory.GetAvailableSpace(currentRecipe.outgredient.item) >= currentRecipe.outgredient.amount)
            {
                //Add all outcome items to the players inventory
                for (int i = 0; i < recipeAmount; i++)
                {
                    RecipePart o = currentRecipe.outgredient;
                    Player.Get().inventory.Add(o.item, o.amount, Item.GetDefaultTag(o.item));
                }

                Reset();

                return true;
            }

            return false;
        }

        private void Reset()
        {
            recipeClaimable = false;
            recipeRunning = false;
            currentRecipe = null;
            recipeProgress = 0;
        }

        public static bool RequiredItemsAvailable(CraftingRecipe r, int amount)
        {
            //Check for each ingredient
            foreach (RecipePart ingredient in r.ingredients)
            {
                if (Player.Get().inventory.GetItemAmount(ingredient.item) < ingredient.amount * amount)
                {
                    return false;
                }
            }
            return true;
        }

        public void BeginCrafting(CraftingRecipe recipe, int recipeAmount, bool isNew = true)
        {
            this.recipeAmount = recipeAmount;

            if (isNew) //Only remove items when the recipe is new, not when it's loaded in from save for example
            {
                //Remove the required materials based on the amount from the players inventory
                foreach (RecipePart ingredient in recipe.ingredients)
                {
                    Player.Get().inventory.Remove(ingredient.item, ingredient.amount * recipeAmount);
                }
            }

            recipeRunning = true;
            currentRecipe = recipe;
            if (block != null) block.blockState = BlockState.RECIPE_RUNNING;
        }

        private void FinishCrafting()
        {
            block.blockState = BlockState.RECIPE_IDLE;
            recipeClaimable = true;

            //Get the item that belongs to this workstation to display in the notification
            Item displayItem = null;
            if (block != null) displayItem = ItemRegister.Get(block.itemId);

            NotificationHandler.Notify(displayItem != null ? displayItem.id : "sc:crafting_table_item", $"Crafting for {recipeAmount}x {currentRecipe.displayName} completed!");
            if (block != null) Log.Write($"Completed crafting for {recipeAmount}x {currentRecipe.id} at workstation {workstationId} (x{block.xPos} y{block.yPos}, Chunk {block.chunk.index})", LogType.GENERAL, LogLevel.INFO);
        }

        private void OnUpdate(double dt)
        {
            if (recipeRunning == false || recipeClaimable) return;

            recipeProgress += dt * 1000;

            if (recipeProgress >= currentRecipe.requiredTime * recipeAmount) FinishCrafting();
        }

        public float GetCraftingProgress() => (float)recipeProgress / (currentRecipe.requiredTime * recipeAmount);
    }
}
