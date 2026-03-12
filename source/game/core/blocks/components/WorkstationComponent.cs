using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using System;

namespace SeeloewenCraft.game.core.blocks.components
{
    public class WorkstationComponent : BlockComponent
    {
        public CraftingHandler craftingHandler;

        public WorkstationComponent(Block b, string workstationId, string workstationName)
        {
            craftingHandler = new CraftingHandler(b, workstationId, workstationName);
        }

        protected override JToken GetContentJson()
        {
            JObject contentObj = new JObject
            {
                { "recipe_running", craftingHandler.recipeRunning },
            };

            if (craftingHandler.recipeRunning)
            {
                contentObj.Add("recipe_id", craftingHandler.currentRecipe.id);
                contentObj.Add("recipe_progress", craftingHandler.recipeProgress);
                contentObj.Add("recipe_amount", craftingHandler.recipeAmount);
            }

            return contentObj;
        }

        public override void AddDebugMenu()
        {
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeClaimable={craftingHandler.recipeClaimable}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"selectedRecipe={craftingHandler.currentRecipe}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"recipeProgress={craftingHandler.recipeProgress}");
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"amount={craftingHandler.recipeAmount}");
            }
        }

        public override void FromJson(JObject obj)
        {
            bool running = obj.Get<bool>("recipe_running");

            if (running)
            {
                CraftingRecipe recipe = CraftingHandler.GetRecipe(obj.Get<string>("recipe_id"));
                int amount = obj.Get<int>("recipe_amount");
                double progress = obj.Get<double>("recipe_progress");

                craftingHandler.ContinueCrafting(recipe, amount, progress);
            }
        }

        public override BlockComponentType GetType() => BlockComponentType.Workstation;

    }
}
