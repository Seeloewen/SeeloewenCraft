

using OpenTK.Graphics.OpenGL4;
using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.graphics
{
    public static class Renderer
    {
        public static Color skyColor;

        public static void Init()
        {
            PrimitiveRenderer.Init();
            TextureRenderer.Init();
            TextRenderer.Init();

            WorldRenderer.Init();
            ItemRenderer.Init();
            PlayerRenderer.Init();
            GeneralTextureMap.Init();

            skyColor = SkyColors.DAY_COLOR;

            GL.Enable(EnableCap.StencilTest);
        }


        public static void Render()
        {
            GL.ClearColor(skyColor.r, skyColor.g, skyColor.b, skyColor.a);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            /*
            Rectangle r = new Rectangle(100, 200, 500, 400);
            Color c = new Color(0.4f, 0.4f, 0.4f);
            PrimitiveRenderer.Begin();
            PrimitiveRenderer.DrawRectangle(r, c);
            PrimitiveRenderer.End();
            */
;
            PushDebugGroup("world rendering");
            WorldRenderer.Render();
            PopDebugGroup();
            PushDebugGroup("item entity rendering");
            ItemEntityRenderer.Render();
            PopDebugGroup();
            PushDebugGroup("player rendering");
            PlayerRenderer.Render();
            PopDebugGroup();

            PushDebugGroup("screen rendering");
            Screen.Render();
            PopDebugGroup();

        }

        //TODO maybe remove for performance
        internal static void PushDebugGroup(string name)
        {
            GL.PushDebugGroup(DebugSourceExternal.DebugSourceApplication, 0, name.Length, name);
        }
        
        internal static void PopDebugGroup()
        {
            GL.PopDebugGroup();
        }
        
    }
}
