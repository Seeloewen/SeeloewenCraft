

using SeeloewenCraft.game.ui.ui_lib;
using System;

namespace SeeloewenCraft.game.ui
{
    internal class EscapeMenu : CRectangle
    {

        const int h = 44;
        const int gap = 16;
        const int w = 300;

        string[] texts =
        {
            "MOIN",
            "HELLO",
            "GOOD",
            "MORNING"
        };


        internal EscapeMenu() : base(new Color(0.0f, 0.0f, 0.0f, 0.5f), new Rectangle(-1f, -1f, 1f, 1f))
        {
            (int mx, int my) = Resolution.ScreenToPixel(0f, 0f);
            int top = my - 2 * h - gap - gap / 2;

            for (int i = 0; i < 4; i++)
            {
                int x1 = mx - w / 2;
                int y1 = top + i * (h + gap);
                int x2 = x1 + w;
                int y2 = y1 + h;

                Rectangle bounds = new(x1, y1, x2, y2 );

                string s = texts[i];

                Action onPress = () =>
                {
                    Log.WriteD($"escape button pressed {s}");
                };

                AddChild(new Button(onPress, texts[i],
                    new Color(0.6f, 0.6f, 0.6f),
                    new Color(0.7f,0.7f,0.7f),
                    new Color(0.8f,0.8f,0.8f),
                    bounds)
                );



            }

        }



    }
}
