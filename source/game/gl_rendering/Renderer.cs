using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SeeloewenCraft.util;

namespace SeeloewenCraft.gl_rendering
{
    public static class Renderer
    {


        static WorldRenderer worldRenderer;
        static ItemEntityRenderer itemEntityRenderer;
        static PlayerRenderer playerRenderer;
        static PrimitiveRenderer primitiveRenderer;
        static EntityRenderer entityRenderer;
        static TextRenderer textRenderer;


        static public void Init()
        {
            TextureManager textureManager = new TextureManager();

            worldRenderer = new WorldRenderer(textureManager);
            itemEntityRenderer = new ItemEntityRenderer(textureManager);
            playerRenderer = new PlayerRenderer(textureManager);
            primitiveRenderer = new PrimitiveRenderer();
            entityRenderer = new EntityRenderer(textureManager);
            textRenderer = new TextRenderer(textureManager);

            GL.ClearColor(0.74f, 0.96f, 0.97f, 1f);
        }

        static public void Render()
        {

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            worldRenderer.Render();

            entityRenderer.Render();

            playerRenderer.Render(Game.world.player.playerRenderInfo);

            //InventoryRenderer.Render(primitiveRenderer);

            Screen.Render(primitiveRenderer, textRenderer);







        }

    }
}
