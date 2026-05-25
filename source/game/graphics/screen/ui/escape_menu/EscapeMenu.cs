using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics.ui_lib;
using SeeloewenCraft.game.notifications;
using SeeloewenCraft.game.util.logging;
using SeeloewenCraft.launcher;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal class EscapeMenu : CRectangle
    {

        const int h = 44;
        const int gap = 16;
        const int w = 300;

        string[] texts =
        {
            "Return to game",
            "Notifications",
            "Settings",
            "Save Game",
            "Exit World"
        };


        internal EscapeMenu() : base(new Color(0.0f, 0.0f, 0.0f, 0.5f), new Rectangle(-1f, -1f, 1f, 1f))
        {
            (int mx, int my) = Resolution.ScreenToPixel(0f, 0f);
            int top = my - 2 * h - gap - gap / 2;

            int xb1 = mx - w / 2 - 25;
            int yb1 = top - 25;
            int xb2 = xb1 + w + 50;
            int yb2 = yb1 + 5 * (h + gap) + 35;
            CRectangle backplate = new CRectangle(new Color(0.5f, 0.5f, 0.5f, 0.9f), new Rectangle(xb1, yb1, xb2, yb2));

            backplate.AddChild(new CBorder(3, new Color(0.2f)));
            AddChild(backplate);

            TextLayout tLayout = new TextLayout(GetBounds().GetCenter().x, TextHAlignment.CENTER, top - 70, TextVAlignment.CENTER);
            AddChild(new CText("Game Menu", 4, tLayout));

            for (int i = 0; i < 5; i++)
            {
                int x1 = mx - w / 2;
                int y1 = top + i * (h + gap);
                int x2 = x1 + w;
                int y2 = y1 + h;

                Rectangle bounds = new(x1, y1, x2, y2);

                string s = texts[i];

                Action onPress = actions[i];

                AddChild(new CButton(onPress, texts[i], 3,
                "sc:button_1",
                "general",
                bounds)
                );

            }
        }

        public Action[] actions =
        {
            () => Screen.showEscapeMenu = false,
            () => {
                    Screen.showEscapeMenu = false;
                    Log.WriteD("Missing notification menu");
                  },
            () => {
                    Screen.showEscapeMenu = false;
                    Game.wndMenu.wndSettings = new wndSettings(false);
                    Game.wndMenu.wndSettings.ShowDialog(Game.wndMenu);
                  },
            () => {
                    Screen.showEscapeMenu = false;
                    World.Get().Save();
                    NotificationHandler.Notify("sc:save", "Successfully saved the world!", "general");
                  },
            () => Game.shouldClose = true,

        };
    }
}
