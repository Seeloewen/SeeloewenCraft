using SeeloewenCraft.game.graphics.ui_lib;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal abstract class CSlot : CRectangle
    {
        internal bool hasBackground = true;

        public int x;
        public int y;

        internal bool isPressed;
        internal bool isHovered;

        protected CText cAmount;
        protected CTexture cTexture;
        protected CRectangle cDurability;
        protected CRectangle cDurabilityBack;

        protected Color colorDurabilityBack = new Color(0.6f);
        protected Color color = new Color(0.66f);
        protected Color hoveredColor = new Color(0.71f);
        protected Color pressedColor = new Color(0.6f);

        public CSlot(Rectangle bounds) : base(new Color(0f), bounds)
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
            cDurabilityBack = new CRectangle(colorDurabilityBack, new Rectangle(x1, y1, x2, y2));
            AddChild(cDurabilityBack);
            cDurability = new CRectangle(new Color(0f), new Rectangle(x1, y1, x2, y2));
            AddChild(cDurability);
        }

        internal void Update(string id, int amount, float durability, bool isSelected = false)
        {
            //If the slot is selected, hide it as it's being displayed by the mouse follower
            cTexture.visible = !isSelected;
            cAmount.visible = !isSelected;
            cDurability.visible = !isSelected;
            cDurabilityBack.visible = !isSelected;

            if (isSelected) return;

            cAmount.SetText(amount > 1 ? amount.ToString() : "");
            cTexture.SetId(id);

            //Hide durablity display if there is no durability
            if (durability == 0)
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

        protected override void OnClickEvent(ClickEvent mouseClickEvent)
        {
            isPressed = mouseClickEvent.pressed;

            if (mouseClickEvent.button == ButtonType.LEFT)
            {
                if (isPressed && parent is CInventory cInv) cInv.invData.GetSlot(x, y).OnLeftClick();
            }
            else if (mouseClickEvent.button == ButtonType.RIGHT)
            {
                if (isPressed && parent is CInventory cInv) cInv.invData.GetSlot(x, y).OnRightClick();
            }
        }

        protected override void OnRender()
        {
            SetColor(new Color(0f, 0f, 0f, 0f));

            //Set color based on the pressstate (if it isn't following the mouse)
            if (hasBackground) SetColor(isPressed ? pressedColor : (isHovered ? hoveredColor : color)); //Please forgive me for this line

            base.OnRender();
        }

        protected override void OnMouseEnter()
        {
            isHovered = true;
            isPressed = false;
        }

        protected override void OnMouseLeave()
        {
            isHovered = false;
            isPressed = false;
        }
    }
}
