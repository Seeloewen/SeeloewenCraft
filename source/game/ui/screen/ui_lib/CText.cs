


namespace SeeloewenCraft.game.ui.ui_lib
{

    public enum TextHAlignment { LEFT, CENTER, RIGHT }
    public enum TextVAlignment { TOP, CENTER, BOTTOM }


    public class TextLayout
    {

        int xAnchor;
        TextHAlignment textHAlignment;
        int yAnchor;
        TextVAlignment textVAlignment;

        public TextLayout(int xAnchor, TextHAlignment textHAlignment, int yAnchor, TextVAlignment textVAlignment)
        {
            this.xAnchor = xAnchor;
            this.textHAlignment = textHAlignment;
            this.yAnchor = yAnchor;
            this.textVAlignment = textVAlignment;
        }

        internal Rectangle CalcBounds(string text, int size)
        {
            int width = TextRenderer.GetWidth(text, size);
            int height = size * 7;

            int x1 = 0, x2 = 0;
            switch (textHAlignment)
            {
                case TextHAlignment.LEFT:
                    x1 = xAnchor;
                    x2 = xAnchor + width;
                    break;
                case TextHAlignment.CENTER:
                    x1 = xAnchor - width / 2;
                    x2 = xAnchor + width / 2;
                    break;
                case TextHAlignment.RIGHT:
                    x1 = xAnchor - width;
                    x2 = xAnchor;
                    break;
            }
            int y1 = 0, y2 = 0;
            switch (textVAlignment)
            {
                case TextVAlignment.TOP:
                    y1 = yAnchor;
                    y2 = yAnchor + height;
                    break;
                case TextVAlignment.CENTER:
                    y1 = yAnchor - height / 2;
                    y2 = yAnchor + height / 2;
                    break;
                case TextVAlignment.BOTTOM:
                    y1 = yAnchor - height;
                    y2 = yAnchor;
                    break;
            }

            return new Rectangle(x1, y1, x2, y2);
        }


    }


    public class CText : Component
    {

        string text;
        int size;

        TextLayout layout;

        public CText(string text, int size, TextLayout layout) : base(layout.CalcBounds(text, size))
        {
            this.text = text;
            this.size = size;
            this.layout = layout;
        }

        public void setText(string text)
        {
            this.text = text;
            bounds = layout.CalcBounds(text, size);
        }



        protected override void OnRender()
        {
            int width = TextRenderer.GetWidth(text, size);
            int height = size * 7;

            (int x1, int y1) = Resolution.ScreenToPixel(bounds.x1S, bounds.y1S);
            (int x2, int y2) = Resolution.ScreenToPixel(bounds.x2S, bounds.y2S);

            int centerX = (x1 + x2) / 2;
            int centerY = (y1 + y2) / 2;

            int startX = centerX - width / 2;
            int startY = centerY - height / 2;

            TextRenderer.Draw(text, startX, startY, size);

        }




    }
}
