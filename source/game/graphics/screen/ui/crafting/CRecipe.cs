using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CRecipe : CRectangle
    {
        private Color color = new Color(0.88f);
        private Color colorHovered = new Color(0.92f);
        private Color colorPressed = new Color(0.80f);

        internal static int WIDTH = 220;
        internal static int HEIGHT = 60;

        internal bool isSelected = false;

        internal string recipeId;

        private CCrafting handler; //Used to pass methods back to the main handler
        private CText cItemName;
        private CTexture cItemTexture;

        internal CRecipe(int x, int y, string itemId, string recipeId, CCrafting handler) : base(new Color(0.88f), new Rectangle(x, y, x + 220, y + 60))
        {
            Item item = ItemRegister.Get(itemId);
            cItemName = new CText(item.name, 2, new TextLayout(x + 70, TextHAlignment.LEFT, y + 22, TextVAlignment.TOP));
            cItemTexture = new CTexture("items", itemId, new Rectangle(x + 15, y + 10, x + 55, y + 50));

            AddChild(cItemTexture);
            AddChild(cItemName);

            this.handler = handler;
            this.recipeId = recipeId;
        }

        protected override void OnUpdate()
        {
            SetColor(isSelected ? colorPressed : (hovered ? colorHovered : color)); //Holy code

            base.OnUpdate();
        }

        protected override void OnClickEvent(ClickEvent e)
        {
            handler.ClearSelection();
            isSelected = true;
            handler.DisplayIngredients(recipeId);
        }
    }
}
