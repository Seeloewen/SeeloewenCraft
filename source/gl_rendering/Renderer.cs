using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {

        Shader shader;
        VertexBuffer buffer;

        public Renderer() {
            shader = new Shader();

            float[] vertices = {
                     0.0f,  0.5f, 1.0f, 0.0f, 0.0f,
                    -0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                     0.5f, -0.5f, 0.0f, 0.0f, 1.0f
            };


            buffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(3), vertices);
       
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();
            buffer.Bind();
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

    }
}
