using SeeloewenCraft.game.ui.ui_lib;
using System;

namespace SeeloewenCraft.game.ui
{
    class CSlot : CRectangle
    {
        CText cText;
        CTexture cTexture;
        CRectangle cDurability;
        CRectangle cDurabilityBack;

        private Color durBack = new Color(0.6f);

        public CSlot(Rectangle bounds) : base(new Color(0.8f), bounds)
        {
            (int x1, int y1) = (bounds.x1P + 10, bounds.y1P + 10);
            (int x2, int y2) = (bounds.x2P - 10, bounds.y2P - 10);

            //Item Texture
            cTexture = new CTexture(ItemRenderer.GetTextureMap(), null, new Rectangle(x1, y1, x2, y2));
            AddChild(cTexture);

            //Item 
            cText = new CText("", 2, new TextLayout(bounds.x2P - 5, TextHAlignment.RIGHT, bounds.y2P - 5, TextVAlignment.BOTTOM));
            AddChild(cText);

            //Durability
            (x1, y1) = (bounds.x1P + 5, bounds.y2P - 12);
            (x2, y2) = (bounds.x2P - 5, bounds.y2P - 5);
            cDurabilityBack = new CRectangle(durBack, new Rectangle(x1, y1, x2, y2));
            AddChild(cDurabilityBack);
            cDurability = new CRectangle(new Color(0f), new Rectangle(x1, y1, x2, y2));
            AddChild(cDurability);
        }

        public void SetAmount(int amount)
        {
            cText.SetText(amount > 1 ? amount.ToString() : "");
        }

        public void SetItem(string id)
        {
            cTexture.SetId(id);
        }

        public void SetDurability(float durability)
        {
            if (durability == 0)
            {
                cDurability.SetColor(new Color(0f, 0f, 0f, 0f));
                cDurabilityBack.SetColor(new Color(0f, 0f, 0f, 0f));
                return;
            }

            cDurabilityBack.SetColor(durBack);

            float g = Math.Min(1f, 2 * durability);
            float r = Math.Min(1f, 2 - 2 * durability);
            int x2 = (int)(bounds.x1P + 5 + ((bounds.x2P - 5) - (bounds.x1P + 5)) * durability);

            Rectangle oldBounds = cDurability.GetBounds();
            cDurability.SetBounds(new Rectangle(oldBounds.x1P, oldBounds.y1P, x2, oldBounds.y2P));
            cDurability.SetColor(new Color(r, g, 0f));
        }
    }
}
