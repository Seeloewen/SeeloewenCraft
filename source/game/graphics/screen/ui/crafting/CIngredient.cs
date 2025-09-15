using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal class CIngredient : CRectangle
    {
        internal static int WIDTH = 525;
        internal static int HEIGHT = 30;

        private CText cText;
        private CTexture cTexture;

        private string id;
        private string name;
        private int amount;

        internal CIngredient(int x, int y, string id, int amount) : base(new Color(0.88f), new Rectangle(x, y, x + WIDTH, y + HEIGHT))
        {
            Item item = ItemRegister.Get(id);

            cText = new CText(item.name, 2, new TextLayout(x + 30, TextHAlignment.LEFT, y + 7, TextVAlignment.TOP));
            cTexture = new CTexture("items", id, new Rectangle(x + 5, y + 5, x + 25, y + 25));

            AddChild(cText);
            AddChild(cTexture);

            this.id = id;
            name = item.name;
            this.amount = amount;
        }

        protected override void OnUpdate(double dt)
        {
            cText.SetText($"{name} ({Player.Get().inventory.GetItemAmount(id)}/{amount})");

            base.OnUpdate(dt);
        }

    }
}
