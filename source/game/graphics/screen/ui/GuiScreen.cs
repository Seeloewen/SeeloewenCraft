using SeeloewenCraft.game.graphics.ui_lib;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    internal class GuiScreen : CRectangle
    {
        List<CGui> guis { get => Screen.guiHandler.guis; }
        internal CMouseFollower mouseFollowerSlot = new CMouseFollower();

        internal GuiScreen() : base(new Color(0f, 0f, 0f, 0.3f), new Rectangle(-1f, -1f, 1f, 1f)) //Goes over the entire screen as an invisible background to catch click events
        {
            if (guis.Count == 0) return;

            if (guis.Count == 1)
            {
                int x = width / 2 - guis[0].width / 2;
                int y = height / 2 - guis[0].height / 2;

                if (guis[0].GetBounds().x1P != x || guis[0].GetBounds().y1P != y)
                {
                    guis[0].MoveTo(x, y);
                }
            }
            else
            {
                int x1 = width / 2 - guis[0].width / 2;
                int y1 = height / 2 - (guis[0].height + guis[1].height) / 2;

                int x2 = width / 2 - guis[1].width / 2;
                int y2 = y1 + guis[0].height;

                guis[0].MoveTo(x1, y1);
                guis[1].MoveTo(x2, y2);
            }

            //Add all guis
            for (int i = 0; i < guis.Count; i++)
            {
                CGui gui = guis[i];
                AddChild(gui);

                if (i >= 1) break;
            }

            AddChild(mouseFollowerSlot);

            guis.ForEach(gui => gui.PostInit());
        }

        protected override void OnUpdate()
        {
            mouseFollowerSlot.Reset();

            foreach (CGui gui in guis)
            {
                if (gui == null) return;

                gui.Update();
            }
        }

        protected override void OnRender()
        {
            base.OnRender();
        }
    }
}
