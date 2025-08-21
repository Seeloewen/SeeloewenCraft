using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    class GameOverlay : CRectangle
    {
        public GameOverlay() : base(new Color(0f, 0f, 0f, 0f), new Rectangle(-1f, -1f, 1f, 1f))
        {
            //AddChild(new CText("Game Menu", 8, new TextLayout(400, TextHAlignment.CENTER, 400, TextVAlignment.CENTER)));
        }
    }
}
