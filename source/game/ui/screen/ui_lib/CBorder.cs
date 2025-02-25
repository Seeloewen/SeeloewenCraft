
namespace SeeloewenCraft.game.ui.ui_lib
{
    /// <summary>
    /// Line Border Component with a specified width and color
    /// </summary>
    public class CBorder : Component
    {

        int size;
        Color color;


        /// <summary>
        /// Creates a line border component
        /// </summary>
        /// <param name="size">Width of the border in pixels</param>
        /// <param name="color">Color of the border</param>
        public CBorder(int size, Color color) : base()
        {
            this.size = size;
            this.color = color;
        }


        protected override void OnAdd(Component parent)
        {
            bounds = parent.GetBounds();
        }


        protected override void OnRender()
        {
            (int x1, int y1) = Resolution.ScreenToPixel(bounds.x1S, bounds.y1S);
            (int x2, int y2) = Resolution.ScreenToPixel(bounds.x2S, bounds.y2S);
            Rectangle left = new Rectangle(x1, y1, x1 + size, y2);
            Rectangle right = new Rectangle(x2, y1, x2 - size, y2);
            Rectangle top = new Rectangle(x1, y1, x2, y1 + size);
            Rectangle bottom = new Rectangle(x1, y2, x2, y2 - size);

            PrimitiveRenderer.DrawRectangle(left, color);
            PrimitiveRenderer.DrawRectangle(right, color);
            PrimitiveRenderer.DrawRectangle(top, color);
            PrimitiveRenderer.DrawRectangle(bottom, color);
        }
    }
}
