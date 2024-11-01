using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {


        WorldRenderer worldRenderer;
        PlayerRenderer playerRenderer;
        PrimitiveRenderer primitiveRenderer;
        public Screen screen;

        public GameCamera cam;

        public Renderer()
        {
            TextureManager textureManager = new TextureManager();

            worldRenderer = new WorldRenderer(textureManager);
            playerRenderer = new PlayerRenderer(textureManager);
            primitiveRenderer = new PrimitiveRenderer();

            cam = new GameCamera();

            GL.ClearColor(0.74f, 0.96f, 0.97f, 1f);
        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            worldRenderer.Render(cam);
            playerRenderer.Render(cam, Game.world.player.playerRenderInfo);

            //InventoryRenderer.Render(primitiveRenderer);
            primitiveRenderer.Begin();
            screen.Render(primitiveRenderer);
            primitiveRenderer.End();

        }

    }
}
