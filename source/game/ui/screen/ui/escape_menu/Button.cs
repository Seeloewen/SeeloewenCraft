using SeeloewenCraft.game.ui.ui_lib;
using System;

namespace SeeloewenCraft.game.ui
{
    internal class Button : CRectangle
    {
        Action onPress;

        Color colorNormal;
        Color colorHovered;
        Color colorPressed;
        bool hovered;
        bool pressed;

        CTexture cTexture;
        string texId;

        CText cText; 
        string text;

        internal Button(Action onPress, string text, string texId, TextureMap texMap, Rectangle bounds) : base(new Color(0, 0, 0), bounds)
        {
            this.onPress = onPress;
            this.texId = texId;
            this.text = text;

            cTexture = new CTexture(texMap, texId, bounds);
            AddChild(cTexture);

            Init();
        }

        internal Button(Action onPress, string text, Color colorNormal, Color colorHovered, Color colorPressed, Rectangle bounds) : base(new Color(0, 0, 0), bounds)
        {
            this.onPress = onPress;
            this.text = text;
            this.colorNormal = colorNormal;
            this.colorHovered = colorHovered;
            this.colorPressed = colorPressed;

            Init();
        }

        public void Init()
        {
            AddChild(new CBorder(3, new Color(0.2f)));
            (int centerX, int centerY) = bounds.GetCenter();
            TextLayout layout = new TextLayout(centerX, TextHAlignment.CENTER, centerY, TextVAlignment.CENTER);
            cText = new CText(text, 3, layout);
            AddChild(cText);
        }

        protected override void OnRender()
        {
            Color currentColor = colorNormal;

            if (pressed)
            {
                if(cTexture != null) cTexture.SetBrightness(0.65f);
                currentColor = colorPressed;
            }
            else if (hovered)
            {
                if (cTexture != null) cTexture.SetBrightness(0.99f);
                currentColor = colorHovered;
            }
            else
            {
                if (cTexture != null) cTexture.SetBrightness(0.85f);
            }

            if (cTexture != null) cTexture.SetId(texId);
            PrimitiveRenderer.DrawRectangle(bounds, currentColor);
        }


        protected override void OnMouseEnter()
        {
            hovered = true;
        }

        protected override void OnMouseLeave()
        {
            pressed = false;
            hovered = false;
        }

        protected override void OnClickEvent(ClickEvent e)
        {
            if (e.button == ButtonType.LEFT)
            {
                if (e.pressed)
                {
                    pressed = true;
                }
                else
                {
                    pressed = false;
                    onPress.Invoke();
                }
            }
        }



    }
}
