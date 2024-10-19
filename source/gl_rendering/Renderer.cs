using OpenTK.Graphics.OpenGL;
using SeeloewenCraft.util;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {


        WorldRenderer worldRenderer;

        public GameCamera cam;

        public Renderer()
        {
            TextureManager textureManager = new TextureManager();

            worldRenderer = new WorldRenderer(textureManager);

            cam = new GameCamera();
        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            worldRenderer.Render(cam);


        }

    }
}
