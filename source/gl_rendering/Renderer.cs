using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {


        WorldRenderer worldRenderer;
        PlayerRenderer playerRenderer;

        public GameCamera cam;

        public Renderer()
        {
            TextureManager textureManager = new TextureManager();

            worldRenderer = new WorldRenderer(textureManager);
            playerRenderer = new PlayerRenderer(textureManager);


            cam = new GameCamera();

            GL.ClearColor(.1f,.1f,.1f,1f);
        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            worldRenderer.Render(cam);
            playerRenderer.Render(cam, Game.world.player.playerRenderInfo);

        }

    }
}
