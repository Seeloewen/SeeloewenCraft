
using OpenTK.Graphics.OpenGL4;
using SeeloewenCraft.game.ui;
using System.Diagnostics;
using System.Windows.Forms;


namespace SeeloewenCraft.gl_rendering
{
    internal class PrimitiveRenderer
    {

        Shader shader;
        VertexBuffer buffer;

        float[] vertices;
        int index;
        bool drawing;

        internal PrimitiveRenderer()
        {
            shader = new Shader("shader/primitive");
            buffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(4), 1024);


        }


        internal void DrawRectangle(float x1, float y1, float x2, float y2, float r, float g, float b, float a)
        {
            if (index + 3 * 5 >= 1024)
            {
                End();
                Begin();
            }
            Put(x1, y1, r, g, b, a);
            Put(x1, y2, r, g, b, a);
            Put(x2, y1, r, g, b, a);
            Put(x1, y2, r, g, b, a);
            Put(x2, y1, r, g, b, a);
            Put(x2, y2, r, g, b, a);
        }

        internal void DrawRectangle(float x1, float y1, float x2, float y2, float r, float g, float b)
        {
            DrawRectangle(x1, y1, x2, y2, r, g, b, 1.0f);
        }

        internal void DrawTriangle(Vector2f v1,  Vector2f v2, Vector2f v3, float r, float g, float b)
        {
            if(index + 3*5 >= 1024)
            {
                End();
                Begin();
            }
            Put(v1.x, v1.y, r, g, b, 1.0f);
            Put(v2.x, v2.y, r, g, b, 1.0f);
            Put(v3.x, v3.y, r, g, b, 1.0f);
        }

        internal void Put(float x, float y, float r, float g, float b, float a)
        {
            Debug.Assert(drawing && index + 6 < 1024);
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = r;
            vertices[index++] = g;
            vertices[index++] = b;
            vertices[index++] = a;
        }

        internal void Begin()
        {
            drawing = true;
            vertices = new float[1024];
            index = 0;
        }

        internal void End()
        {
            drawing = false;
            buffer.SetVertices(vertices);
            buffer.Bind();
            shader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }


        internal void Test()
        {
            Begin();
            DrawRectangle(-0.5f, -0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 1.0f);
            DrawTriangle(new Vector2f(0.0f, 0.5f), new Vector2f(-0.5, -0.5), new Vector2f(0.5, -0.5), 1.0f, 1.0f, 1.0f);
            End();
        }


    }
}
