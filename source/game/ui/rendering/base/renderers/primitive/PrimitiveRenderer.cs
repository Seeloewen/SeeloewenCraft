using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace SeeloewenCraft.game.ui
{
    class PrimitiveRenderer
    {

        private static Shader shader;
        private static VertexBuffer buffer;

        static float[] vertices;
        static int index;
        static bool drawing;

        public static void Init()
        {
            shader = new Shader("shader/primitive");
            buffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(4), 1024);
        }

        internal static void DrawRectangle(Rectangle rectangle, Color color)
        {
            float x1 = rectangle.x1;
            float y1 = rectangle.y1;
            float x2 = rectangle.x2;
            float y2 = rectangle.y2;
            float r = color.r;
            float g = color.g;
            float b = color.b;
            float a = color.a;
            DrawRectangle(x1, y1, x2, y2, r, g, b, a);
        }

        internal static void DrawRectangle(float x1, float y1, float x2, float y2, float r, float g, float b, float a)
        {
            if (index + 6 * 6 >= 1024)
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

        internal static void DrawRectangle(float x1, float y1, float x2, float y2, float r, float g, float b)
        {
            DrawRectangle(x1, y1, x2, y2, r, g, b, 1.0f);
        }

        internal static void DrawRectangle(int x1, int y1, int x2, int y2, float r, float g, float b)
        {
            (float s1, float t1) = Resolution.PixelToScreen(x1, y1);
            (float s2, float t2) = Resolution.PixelToScreen(x2, y2);
            DrawRectangle(s1, t1, s2, t2, r, g, b, 1.0f);
        }

        internal static void DrawRectangle(int x1, int y1, int x2, int y2, float r, float g, float b, float a)
        {
            (float s1, float t1) = Resolution.PixelToScreen(x1, y1);
            (float s2, float t2) = Resolution.PixelToScreen(x2, y2);
            DrawRectangle(s1, t1, s2, t2, r, g, b, a);
        }

        internal static void DrawTriangle(Vector2f v1, Vector2f v2, Vector2f v3, float r, float g, float b)
        {
            if (index + 3 * 6 >= 1024)
            {
                End();
                Begin();
            }
            Put(v1.x, v1.y, r, g, b, 1.0f);
            Put(v2.x, v2.y, r, g, b, 1.0f);
            Put(v3.x, v3.y, r, g, b, 1.0f);
        }

        static void Put(float x, float y, float r, float g, float b, float a)
        {
            Debug.Assert(drawing && index + 6 < 1024);
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = r;
            vertices[index++] = g;
            vertices[index++] = b;
            vertices[index++] = a;
        }

        internal static  void Begin()
        {
            Debug.Assert(!drawing);
            drawing = true;
            vertices = new float[1024];
            index = 0;
        }

        internal static void End()
        {
            Debug.Assert(drawing);
            drawing = false;
            buffer.SetVertices(vertices);
            buffer.Bind();
            shader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }


        internal void Test()
        {
            Begin();

            End();

            Rectangle r = new Rectangle(20, 10, 30, 40);

        }








    }
}
