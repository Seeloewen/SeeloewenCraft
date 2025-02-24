

using OpenTK.Graphics.OpenGL4;

namespace SeeloewenCraft.game.ui
{
    public static class Renderer
    {


        public static void Init()
        {
            PrimitiveRenderer.Init();
            TextureRenderer.Init();
            TextRenderer.Init();


            WorldRenderer.Init();
            ItemRenderer.Init();
            PlayerRenderer.Init();
            GeneralTextureMap.Init();
        }

        public static void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            /*Rectangle r = new Rectangle(100, 200, 500, 400);
            ColorI c = new ColorI(0.4f, 0.4f, 0.4f);
            PrimitiveRenderer.Begin();
            PrimitiveRenderer.DrawRectangle(r, c);
            PrimitiveRenderer.End();
            */

            WorldRenderer.Render();
            ItemEntityRenderer.Render();
            PlayerRenderer.Render();


            Screen.Render();

        } 


    }
}
