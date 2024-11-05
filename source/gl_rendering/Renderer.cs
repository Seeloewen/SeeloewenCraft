using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {


        WorldRenderer worldRenderer;
        //ItemEntityRenderer itemEntityRenderer;
        //PlayerRenderer playerRenderer;
        PrimitiveRenderer primitiveRenderer;
        EntityRenderer entityRenderer;
        public Screen screen;

        public GameCamera cam;

        public Renderer()
        {
            TextureManager textureManager = new TextureManager();

            worldRenderer = new WorldRenderer(textureManager);
            //itemEntityRenderer = new ItemEntityRenderer(textureManager);
            //playerRenderer = new PlayerRenderer(textureManager);
            primitiveRenderer = new PrimitiveRenderer();
            entityRenderer = new EntityRenderer(textureManager);

            cam = new GameCamera();

            GL.ClearColor(0.74f, 0.96f, 0.97f, 1f);
        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            worldRenderer.Render(cam);

            entityRenderer.Render(cam);

            //playerRenderer.Render(Game.world.player.playerRenderInfo, cam);

            //InventoryRenderer.Render(primitiveRenderer);
            primitiveRenderer.Begin();
            screen.Render(primitiveRenderer);
            primitiveRenderer.End();

        }

    }
}
