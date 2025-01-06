
using OpenTK.Graphics.OpenGL;
using System;
using Windows.Devices.Enumeration;

namespace SeeloewenCraft.gl_rendering
{
    internal class Button
    {
        int x1, y1, x2, y2;

        string text;

        bool hovered;
        bool muwh; // mouse up while hovering (least scuffed variable name)
        bool pressed;

        Action OnClick;

        internal Button(Action OnClick, string text, int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.OnClick = OnClick;
            this.text = text;
        }

        internal void Render(PrimitiveRenderer renderer, TextRenderer textRenderer)
        {
            renderer.Begin();

            int mx = x1 + (x2 - x1) / 2;
            int my = y1 + (y2 - y1) / 2;


            (float s1, float t1) = Resolution.PixelToScreen(x1, y1);
            (float s2, float t2) = Resolution.PixelToScreen(x2, y2);
            renderer.DrawRectangle(s1, t1, s2, t2, 0f, 0f, 0f);


            float b = pressed
                    ? 0.8f : hovered
                    ? 0.7f
                    : 0.6f;
            (s1, t1) = Resolution.PixelToScreen(x1 + 2, y1 + 2);
            (s2, t2) = Resolution.PixelToScreen(x2 - 2, y2 - 2);
            renderer.DrawRectangle(s1, t1, s2, t2, b, b, b);


            renderer.End();

            textRenderer.Begin();
            int width = TextRenderer.GetWidth(text, 3);
            //(float x2, float y2) = Resolution.PixelToScreen(mx + width / 2, my + h/2 + gap + gap / 2 - i * (h + gap));
            textRenderer.Draw(text, mx - width / 2, my - 10 , 3);
            textRenderer.End();



        }

        internal void Update()
        {
            hovered = (InputHandler.mouseXPixel >= x1 && InputHandler.mouseXPixel < x2
                && InputHandler.mouseYPixel >= y1 && InputHandler.mouseYPixel < y2);
            if (hovered)
            {
                if (InputHandler.pressedLeft)
                {
                    if (muwh)
                    {
                        pressed = true;
                    }
                }
                else
                {
                    if (pressed)
                    {
                        OnClick.Invoke();
                        pressed = false;
                    }
                    muwh = true;
                }
            }
            else
            {
                pressed = false;
                muwh = false;
            }

        }


    }
}
