
using System;

namespace SeeloewenCraft.game.ui
{
    internal class EscapeMenuScreen
    {

        static Button_old[] buttons;


        internal static void Init()
        {
            buttons = new Button_old[4];
            int mx = Resolution.WIDTH / 2;
            int my = Resolution.HEIGHT / 2;

            string[] texts = { "RESUME", "SETTINGS", "TEXTUREPACKS", "EXIT GAME" };
            Action[] actions =
            {
                () => Log.Write("Button clicked: RESUME", LogType.GENERAL, LogLevel.INFO),
                () => Log.Write("Button clicked: SETTINGS", LogType.GENERAL, LogLevel.INFO),
                () => Log.Write("Button clicked: TEXTUREPACKS", LogType.GENERAL, LogLevel.INFO),
                () => Log.Write("Button clicked: EXIT GAME", LogType.GENERAL, LogLevel.INFO),
            };


            int h = 44;
            int gap = 16;
            int w = 300;

            int top = my - 2 * h - gap - gap / 2;

            for (int i = 0; i < 4; i++)
            {
                buttons[i] = new Button_old(actions[i], texts[i],
                    mx - w / 2,
                    top + i * (h + gap),
                    mx + w / 2,
                    top + h + i * (h + gap)
                );
            }

        }

        internal static void Update()
        {
            foreach (Button_old button in buttons)
            {
                button.Update();
            }
        }

        internal static void Render()
        {
            PrimitiveRenderer.Begin();
            PrimitiveRenderer.DrawRectangle(-1f, -1f, 1f, 1f, 0f, 0f, 0f, .5f);
            PrimitiveRenderer.End();

            foreach (Button_old b in buttons)
            {
                b.Render();
            }


        }


    }
}
