using HighPrecisionTimer;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util.logging;
using System;
using System.Windows;

namespace SeeloewenCraft.game.core.crafting
{
    public class CraftingHandler : IGuiData
    {
        public string guiId { get; set; } = "crafting_handler";
        public string tags { get; set; }

        public MultimediaTimer tmrCrafting = new MultimediaTimer() { Interval = 25 }; //TODO: Replace with gameloop

        public CraftingRecipe currentRecipe;
        private readonly Block block;

        public readonly string workstationId;
        public readonly string workstationName;

        public int recipeProgress;
        public bool recipeRunning;
        public bool recipeClaimable;
        public int recipeAmount = 1;

        public CraftingHandler(Block block, string workstationId, string workstationName)
        {
            this.workstationId = workstationId;
            this.workstationName = workstationName;
            this.block = block;

            ((IGuiData)this).AddTag("header", workstationName);

            tmrCrafting.Elapsed += tmrCrafting_Tick;
        }

        public static CraftingRecipe GetRecipe(string id)
        {
            //Go through the recipe list and find the recipe with the specified id
            foreach (CraftingRecipe recipe in Game.world.craftingRecipeList)
            {
                if (recipe.id == id)
                {
                    return recipe;
                }
            }
            return null;
        }

        public bool Claim() //Returns whether the items were actually added to the inventory and thus successfully claimed
        {
            if (Game.world.player.inventory.GetAvailableSpace() >= currentRecipe.outgredients.Count && (Game.world.player.inventory.HasEmptySlot() || Game.world.player.inventory.HasItem(currentRecipe.outgredients[0].item.id)))
            {
                //Add all outcome items to the players inventory
                for (int i = 0; i < recipeAmount; i++)
                {
                    foreach (CraftingIngredient craftingIngredient in currentRecipe.outgredients)
                    {
                        Game.world.player.inventory.AddItem(craftingIngredient.item.id, craftingIngredient.amount, craftingIngredient.item.tag);
                    }
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
            foreach (CraftingIngredient ingredient in r.ingredients)
            {
                if (Game.world.player.inventory.GetItemAmount(ingredient.item.id) < ingredient.amount * amount)
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
                foreach (CraftingIngredient ingredient in recipe.ingredients)
                {
                    Game.world.player.inventory.RemoveItem(ingredient.item.id, ingredient.amount * recipeAmount);
                }
            }

            recipeRunning = true;
            currentRecipe = recipe;

            tmrCrafting.Start();
        }

        private void FinishCrafting()
        {
            recipeClaimable = true;
        }

        private void UpdateCraftingProgress()
        {
            recipeProgress += 25;

            if (recipeProgress >= currentRecipe.requiredTime * recipeAmount)
            {
                FinishCrafting();
                tmrCrafting.Stop();

                NotificationHandler.ShowNotification($"Crafting for x{recipeAmount} {currentRecipe.displayName} completed!", 3000);
                if (block != null) Log.Write($"Completed crafting for {recipeAmount}x {currentRecipe.id} at workstation {workstationId} (x{block.xPos} y{block.yPos}, Chunk {block.chunk.index})", LogType.GENERAL, LogLevel.INFO);
            }
        }

        public float GetCraftingProgress() => (float)recipeProgress / (currentRecipe.requiredTime * recipeAmount);

        private void tmrCrafting_Tick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { UpdateCraftingProgress(); }));
        }
    }
}
