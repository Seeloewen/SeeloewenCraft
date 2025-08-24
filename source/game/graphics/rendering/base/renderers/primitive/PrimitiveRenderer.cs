using System;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics
{
    struct PrimitiveVertex : IBatch
    {
        private float x, y;
        private float r, g, b, a;

        internal PrimitiveVertex(float x, float y, float r, float g, float b, float a)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public void Fill(float[] vertices, int startIndex)
        {
            vertices[startIndex++] = x;   
            vertices[startIndex++] = y;   
            vertices[startIndex++] = r;   
            vertices[startIndex++] = g;   
            vertices[startIndex++] = b;  
            vertices[startIndex++] = a; 
        }

        public static int GetSize() => 6;

        public static VBLayout GetVBLayout() => new VBLayout().AddAttribute(2).AddAttribute(4);

    }

    class PrimitiveRenderer
    {

        static BatchRenderer<PrimitiveVertex> renderer;

        public static void Init()
        {
            renderer = new BatchRenderer<PrimitiveVertex>(new Shader("shader.primitive"));
        }

        internal static void DrawRectangle(Rectangle rectangle, Color color)
        {
            float x1 = rectangle.x1S;
            float y1 = rectangle.y1S;
            float x2 = rectangle.x2S;
            float y2 = rectangle.y2S;
            float r = color.r;
            float g = color.g;
            float b = color.b;
            float a = color.a;
            DrawRectangle(x1, y1, x2, y2, r, g, b, a);
        }

        internal static void DrawRectangle(float x1, float y1, float x2, float y2, float r, float g, float b, float a)
        {
            PrimitiveVertex v0 = new PrimitiveVertex(x1, y1, r, g, b, a);
            PrimitiveVertex v1 = new PrimitiveVertex(x1, y2, r, g, b, a);
            PrimitiveVertex v2 = new PrimitiveVertex(x2, y2, r, g, b, a);
            PrimitiveVertex v3 = new PrimitiveVertex(x2, y1, r, g, b, a);
            renderer.DrawRect(v0, v1, v2, v3);
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


        internal static void Begin()
        {
            renderer.Begin();
        }

        internal static void End()
        {
            renderer.End();
        }

        internal static void Flush()
        {
            renderer.Flush();
        }

    }

}
