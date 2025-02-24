namespace SeeloewenCraft.game.ui.ui_lib
{

    /// <summary>
    /// Basic rectangle component
    /// </summary>
    public class CRectangle : Component
    {
        private Color color;

        /// <summary>
        /// Creates a rectangle with one color
        /// </summary>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="bounds">Bounding box of the rectangle</param>
        public CRectangle(Color color, Rectangle bounds) : base(bounds)
        {
            this.color = color;
        }

        /// <summary>
        /// Sets the color
        /// </summary>
        /// <param name="color">New color of the rectangle</param>
        public void SetColor(Color color)
        {
            this.color = color;
        }

        /// <summary>
        /// Returns the color
        /// </summary>
        public Color GetColor()
        {
            return color;
        }

        /// <summary>
        /// Renders the rectangle
        /// </summary>
        protected override void OnRender()
        {
            PrimitiveRenderer.DrawRectangle(bounds, color);
        }
    }
}
