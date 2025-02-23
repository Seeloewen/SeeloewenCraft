


namespace SeeloewenCraft.game.ui.ui_lib
{
    /// <summary>
    /// Represents the horizontal alignment of text in a CText component
    /// </summary>
    public enum TextHAlignment { LEFT, CENTER, RIGHT }

    /// <summary>
    /// Represents the vertical alignment of text in a CText component
    /// </summary>
    public enum TextVAlignment { TOP, CENTER, BOTTOM }

    /// <summary>
    /// Manages the position of text in a CText component. The CText component grows according to set alignment rules
    /// </summary>
    public class TextLayout
    {

        int xAnchor;
        TextHAlignment textHAlignment;
        int yAnchor;
        TextVAlignment textVAlignment;

        /// <summary>
        /// Creates a TextLayout. The text position will be set so the specified edge of the text(LEFT, TOP, CENTER, ...) will be on the according anchor position
        /// </summary>
        /// <param name="xAnchor">horizontal anchor position</param>
        /// <param name="textHAlignment">horizontal alignment edge</param>
        /// <param name="yAnchor">vertical anchor position</param>
        /// <param name="textVAlignment">vertical alignment edge</param>
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

    /// <summary>
    /// Basic text component that draws one line string
    /// </summary>
    public class CText : Component
    {

        string text;
        int size;

        TextLayout layout;

        /// <summary>
        /// Creates a basic text component
        /// </summary>
        /// <param name="text">Text to be drawn</param>
        /// <param name="size">Size of drawn text</param>
        /// <param name="layout">TextLayout of the text</param>
        public CText(string text, int size, TextLayout layout) : base(layout.CalcBounds(text, size))
        {
            this.text = text;
            this.size = size;
            this.layout = layout;
        }

        /// <summary>
        /// Changes the drawn text
        /// </summary>
        /// <param name="text">New text to be drawn</param>
        public void setText(string text)
        {
            this.text = text;
            bounds = layout.CalcBounds(text, size);
        }


        /// <summary>
        /// Renders the text
        /// </summary>
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
