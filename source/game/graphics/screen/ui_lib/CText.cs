


namespace SeeloewenCraft.game.graphics.ui_lib
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

        internal int xAnchor;
        TextHAlignment textHAlignment;
        internal int yAnchor;
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

        protected string text;
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
        public void SetText(string text)
        {
            this.text = text;
            bounds = layout.CalcBounds(text, size);
        }

        /// <summary>
        /// Returns the drawn text
        /// </summary>
        public string GetText() => text;

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

        /// <summary>
        /// Moves the upper left corner of the bounding box of this component to the specified coordinates
        /// Also moves the children of this component by the necessary amount
        /// </summary>
        /// <param name="x">X coordinate of new upper left corner</param>
        /// <param name="y">Y coordinate of new upper left corner</param>
        internal override void MoveTo(int x, int y)
        {
            int stepX = x - bounds.x1P;
            int stepY = y - bounds.y1P;

            layout.xAnchor = x;
            layout.yAnchor = y;

            Rectangle newBounds = layout.CalcBounds(text, size);
            SetBounds(newBounds);

            ForEachChild(c => c.MoveBy(stepX, stepY));

        }

        /// <summary>
        /// Moves the upper left corner of the bounding box of this component by the specified amount
        /// Also moves the children of this component by the specified amount
        /// </summary>
        /// <param name="x">Amount of steps on positive x-axis</param>
        /// <param name="y">Amount of steps on positive y-axis</param>
        internal override void MoveBy(int x, int y)
        {
            layout.xAnchor = layout.xAnchor + x;
            layout.yAnchor = layout.yAnchor + y;

            Rectangle newBounds = layout.CalcBounds(text, size);
            SetBounds(newBounds);

            ForEachChild(c => c.MoveBy(x, y));

        }


    }
}
