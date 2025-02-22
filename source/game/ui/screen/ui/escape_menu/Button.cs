

using SeeloewenCraft.game.ui.ui_lib;
using System;

namespace SeeloewenCraft.game.ui
{
    internal class Button : CRectangle
    {

        Action onPress;

        Color color;
        Color hoverColor;
        Color pressedColor;

        bool hovered;
        bool pressed;




        internal Button(Action onPress, string text, Color color, Color hoverColor, Color pressedColor, Rectangle bounds) : base(color, bounds)
        {
            this.onPress = onPress;
            this.color = color;
            this.hoverColor = hoverColor;
            this.pressedColor = pressedColor;

            AddChild(new CBorder(3, new Color(0f, 0f, 0f)));
            (int centerX, int centerY) = bounds.GetCenter();
            TextLayout layout = new TextLayout(centerX, TextHAlignment.CENTER, centerY, TextVAlignment.CENTER);
            AddChild(new CText(text, 3, layout));
        }


        protected override void OnRender()
        {
            if (pressed)
            {
                PrimitiveRenderer.DrawRectangle(bounds, pressedColor);
            }
            else if (hovered)
            {
                PrimitiveRenderer.DrawRectangle(bounds, hoverColor);
            } else
            {
                PrimitiveRenderer.DrawRectangle(bounds, color);
            }

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
