using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SeeloewenCraft.gl_rendering
{
    public class Renderer
    {

        Shader shader;

        public Renderer() {
            shader = new Shader();

            float[] vertices = {
                     0.4f,  0.5f,// 1.0f, 0.0f, 0.0f,
                    -0.5f, -0.5f,// 0.0f, 1.0f, 0.0f,
                     0.5f, -0.5f,// 0.0f, 0.0f, 1.0f
            };

            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.EnableVertexArrayAttrib(vao, 0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            //GL.EnableVertexArrayAttrib(vao, 1);
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));

            GL.ClearColor(Color4.Blue);
        }

        public void render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        }

    }
}
