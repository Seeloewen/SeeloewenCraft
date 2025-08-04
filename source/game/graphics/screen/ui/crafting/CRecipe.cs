using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CRecipe : CRectangle
    {
        internal static int WIDTH = 220;
        internal static int HEIGHT = 60;

        private string recipeId;

        private CText cItemName;
        private CTexture cItemTexture;

        internal CRecipe(int x, int y, string itemId, string recipeId) : base(new Color(0.88f), new Rectangle(x, y, x + 220, y + 60))
        {
            Item item = ItemRegister.GenerateItem(itemId);
            cItemName = new CText(item.name, 2, new TextLayout(x + 70, TextHAlignment.LEFT, y + 27, TextVAlignment.TOP));
            cItemTexture = new CTexture(ItemRenderer.GetTextureMap(), itemId, new Rectangle(x + 15, y + 15, x + 55, y + 55));

            AddChild(cItemTexture);
            AddChild(cItemName);

            this.recipeId = recipeId;
        }

        protected override void OnClickEvent(ClickEvent e)
        {
            CCrafting crafting = (CCrafting)((CScrollPane)parent).parent; //This does not only look terrible, it is terrible. Please fix.

            crafting.DisplayIngredients(recipeId);
        }
    }
}
