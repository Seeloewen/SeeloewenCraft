

namespace SeeloewenCraft.game.ui.ui_lib
{
    public class CRectangle : Component
    {

        Color color;

        public CRectangle(Color color, Rectangle bounds) : base(bounds)
        {
            this.color = color;
        }

        protected override void OnRender()
        {
            PrimitiveRenderer.DrawRectangle(bounds, color);
        }






    }
}
