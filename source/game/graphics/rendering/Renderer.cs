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

            skyColor = SkyColors.DAY_COLOR;

            GL.Enable(EnableCap.StencilTest);
        }


        public static void Render()
        {
            GL.ClearColor(skyColor.r, skyColor.g, skyColor.b, skyColor.a);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            WorldRenderer.Render();

            //HitboxRenderer.Render();
            
            ItemEntityRenderer.Render();
            PlayerRenderer.Render();
            
            WorldRenderer.RenderShadow();

            Screen.Render();

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
