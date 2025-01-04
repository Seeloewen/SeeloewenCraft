using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SeeloewenCraft.util;

namespace SeeloewenCraft.gl_rendering
{
    public static class Renderer
    {


        static WorldRenderer worldRenderer;
        static PlayerRenderer playerRenderer;
        static PrimitiveRenderer primitiveRenderer;
        static ItemRenderer itemRenderer;
        static EntityRenderer entityRenderer;
        static TextRenderer textRenderer;


        static public void Init()
        {
            TextureManager textureManager = new TextureManager();

            worldRenderer = new WorldRenderer(textureManager);
            playerRenderer = new PlayerRenderer(textureManager);
            primitiveRenderer = new PrimitiveRenderer();
            itemRenderer = new ItemRenderer(textureManager);
            entityRenderer = new EntityRenderer(textureManager, itemRenderer);
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

            Screen.Render(primitiveRenderer, textRenderer, itemRenderer);







        }

    }
}
