using SeeloewenCraft.game.graphics.ui_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Item item = ItemRegister.GenerateItem(id);

            cText = new CText(item.name, 2, new TextLayout(x + 30, TextHAlignment.LEFT, y + 7, TextVAlignment.TOP));
            cTexture = new CTexture(ItemRenderer.GetTextureMap(), id, new Rectangle(x + 5, y + 5, x + 25, y + 25));

            AddChild(cText);
            AddChild(cTexture);

            this.id = id;
            name = item.name;
            this.amount = amount;
        }

        protected override void OnUpdate()
        {
            cText.SetText($"{name} ({Game.world.player.inventory.GetItemAmount(id)}/{amount})");

            base.OnUpdate();
        }

    }
}
