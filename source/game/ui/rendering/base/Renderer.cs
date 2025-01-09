

namespace SeeloewenCraft.game.ui
{
    public static class Renderer
    {


        public static void Init()
        {
            PrimitiveRenderer.Init();
        }

        public static void Render()
        {

            Rectangle r = new Rectangle(100, 200, 500, 400);
            ColorI c = new ColorI(0.4f, 0.4f, 0.4f);
            PrimitiveRenderer.Begin();
            PrimitiveRenderer.DrawRectangle(r, c);
            PrimitiveRenderer.End();

        } 


    }
}
