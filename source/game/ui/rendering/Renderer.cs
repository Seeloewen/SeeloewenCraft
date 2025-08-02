

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
            
            GL.Enable(EnableCap.StencilTest);
        }

        public static void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit |  ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            /*
            Rectangle r = new Rectangle(100, 200, 500, 400);
            Color c = new Color(0.4f, 0.4f, 0.4f);
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
