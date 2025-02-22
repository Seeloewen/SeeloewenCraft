

namespace SeeloewenCraft.game.ui.ui_lib
{
    public class CBorder : Component
    {

        int size;
        Color color;



        internal CBorder(int size, Color color) : base()
        {
            this.size = size;
            this.color = color;
        }


        protected override void OnAdd(Component parent)
        {
            bounds = parent.getBounds();
        }


        protected override void OnRender()
        {
            (int x1, int y1) = Resolution.ScreenToPixel(bounds.x1, bounds.y1);
            (int x2, int y2) = Resolution.ScreenToPixel(bounds.x2, bounds.y2);
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
