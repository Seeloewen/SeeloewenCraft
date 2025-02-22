

namespace SeeloewenCraft.game.ui.ui_lib
{
    public class CText : Component
    {

        string text;
        int size;

        public CText(string text, int size) : base(new Rectangle(0,0,0,0))
        {
            this.text = text;
            this.size = size;
        }

        protected override void OnAdd(Component parent)
        {
            bounds = parent.getBounds();
        }

        protected override void OnRender()
        {
            int width = TextRenderer.GetWidth(text, size);
            int height = size * 7;

            (int x1, int y1) = Resolution.ScreenToPixel(bounds.x1S, bounds.y1S);
            (int x2, int y2) = Resolution.ScreenToPixel(bounds.x2S, bounds.y2S);

            int centerX = (x1 + x2) / 2;
            int centerY = (y1 + y2) / 2;

            int startX = centerX - width/2;
            int startY = centerY - height/2;

            TextRenderer.Draw(text, startX, startY, size);

        }




    }
}
