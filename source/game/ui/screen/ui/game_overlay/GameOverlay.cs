using SeeloewenCraft.game.ui.ui_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.ui
{
    class GameOverlay : CRectangle
    {
        public GameOverlay() : base(new Color(0f, 0f, 0f, 0f), new Rectangle(-1f, -1f, 1f, 1f))
        {
            AddChild(new CText("Game Menu", 8, new TextLayout(400, TextHAlignment.CENTER, 400, TextVAlignment.CENTER)));
        }
    }
}
