using SeeloewenCraft.game.graphics.ui_lib;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal abstract class CSlot : CRectangle
    {
        internal bool isSelected;

        protected CText cAmount;
        protected CTexture cTexture;
        protected CRectangle cDurability;
        protected CRectangle cDurabilityBack;

        private Color durBack = new Color(0.6f);

        public CSlot(Rectangle bounds) : base(new Color(0.8f), bounds)
        {
            (int x1, int y1) = (bounds.x1P + 10, bounds.y1P + 10);
            (int x2, int y2) = (bounds.x2P - 10, bounds.y2P - 10);

            //Item Texture
            cTexture = new CTexture(ItemRenderer.GetTextureMap(), null, new Rectangle(x1, y1, x2, y2));
            AddChild(cTexture);

            //Item 
            cAmount = new CText("", 2, new TextLayout(bounds.x2P - 5, TextHAlignment.RIGHT, bounds.y2P - 5, TextVAlignment.BOTTOM));
            AddChild(cAmount);

            //Durability
            (x1, y1) = (bounds.x1P + 5, bounds.y2P - 12);
            (x2, y2) = (bounds.x2P - 5, bounds.y2P - 5);
            cDurabilityBack = new CRectangle(durBack, new Rectangle(x1, y1, x2, y2));
            AddChild(cDurabilityBack);
            cDurability = new CRectangle(new Color(0f), new Rectangle(x1, y1, x2, y2));
            AddChild(cDurability);
        }

        public void Update(string id, int amount, float durability, bool isSelected = false)
        {
            this.isSelected = isSelected;

            cTexture.visible = !isSelected;
            cAmount.visible = !isSelected;
           
            cDurability.visible = true;
            cDurabilityBack.visible = true;

            cAmount.SetText(amount > 1 ? amount.ToString() : "");
            cTexture.SetId(id);

            //Hide durablity display if there is no durability
            if (durability == 0 || isSelected)
            {
                cDurability.visible = false;
                cDurabilityBack.visible = false;
                return;
            }

            //Update durablity display
            cDurabilityBack.visible = true;
            cDurability.visible = true;

            float g = Math.Min(1f, 2 * durability);
            float r = Math.Min(1f, 2 - 2 * durability);
            int x2 = (int)(bounds.x1P + 5 + ((bounds.x2P - 5) - (bounds.x1P + 5)) * durability);

            Rectangle oldBounds = cDurability.GetBounds();
            cDurability.SetBounds(new Rectangle(oldBounds.x1P, oldBounds.y1P, x2, oldBounds.y2P));
            cDurability.SetColor(new Color(r, g, 0f));
        }
    }
}
