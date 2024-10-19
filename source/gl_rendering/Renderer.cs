using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Windows;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {

        Shader shader;
        VertexBuffer buffer;
        Texture texture;

        BlockRenderer blockRenderer;

        public Renderer()
        {
            shader = new Shader();

            float[] vertices = {
                     0.0f,  0.5f, 0.5f, 1.0f,
                    -0.5f, -0.5f, 0.0f, 0.0f,
                     0.5f, -0.5f, 1.0f, 0.0f
            };


            buffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2), vertices);


            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

            shader.SetUniform("texIage", 0);
            texture = new Texture();
            shader.SetUniform("texIage", 0);

            var textureMap = new BlockTextureMap();
            blockRenderer = new BlockRenderer(textureMap);

        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //shader.Use();
            //buffer.Bind();
            //texture.Bind();
            blockRenderer.Begin();
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    blockRenderer.DrawBlock(" ", x, y); ;
                }
            }
            blockRenderer.End();

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

    }
}
