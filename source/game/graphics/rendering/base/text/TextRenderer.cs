
using OpenTK.Graphics.OpenGL4;
using SeeloewenCraft.entity;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics
{
    internal static class TextRenderer
    {

        static FontTextureMap textureMap;
        static Shader shader;
        static VertexBuffer vertexBuffer;

        static Dictionary<char, int> widthMappings;

        internal static void Init()
        {
            (textureMap, widthMappings) = FontTextureMap.ParseFontMap();
            
            shader = new Shader("shader.texture");
            vertexBuffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2).AddAttribute(1), 1024);
        }


        static float[] vertices;
        static int index;
        static bool drawing;


        static internal void Test()
        {
            Begin();
            

            Draw("Hallo meine Kameraden geil man jawollja komm in die Gruppe", 1, 1, 2);

            End();
        }

        internal static void Draw(string s, int x, int y, int size)
        {

            (float x1, float y1) = Resolution.PixelToScreen(x, y);
            float sizeY = 2f / Resolution.HEIGHT;

            Draw(s, x1, y1 - size * 8 * sizeY, size * 8 * sizeY);
        }

        public static int GetWidth(string s, int size)
        {
            int width = 0;
            foreach(char c in s) {
                int charWidth;
                if (!widthMappings.TryGetValue(c, out charWidth))
                {
                    charWidth = 5;
                } 
                width += charWidth;
                width += 1;
            }
            if(width!=0) width--;
            return width * size;
        }

        static private void Draw(string s, float x, float y, float h)
        {
            float w = (h / 8) * (1 / Resolution.RATIO);

            foreach (char c in s)
            {
                x += Draw(c, x, y, h);
                x += w;
            }
        }

        //returns width
        static private float Draw(char c, float x, float y, float h)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(c);

            int charWidth;
            if (!widthMappings.TryGetValue(c, out charWidth))
            {
                charWidth = 5;
            }
            float w = charWidth * (h / 8) / Resolution.RATIO;
            Draw(x, y, x + w, y + h, s1, t2, s2, t1);

            return w;
        }

        static private void Draw(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2)
        {
            Debug.Assert(drawing);
            if (index + 4 * 6 >= 1024)
            {
                End();
                Begin();
            }
            Put(x1, y1, s1, t1, 1.0f);
            Put(x1, y2, s1, t2, 1.0f);
            Put(x2, y1, s2, t1, 1.0f);
            Put(x1, y2, s1, t2, 1.0f);
            Put(x2, y1, s2, t1, 1.0f);
            Put(x2, y2, s2, t2, 1.0f);
        }

        static private void Put(float x, float y, float s, float t, float g)
        {
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = s;
            vertices[index++] = t;
            vertices[index++] = g;
        }

        static internal void Begin()
        {
            Debug.Assert(!drawing);
            drawing = true;
            index = 0;
            vertices = new float[1024];
        }

        static internal void End()
        {
            Debug.Assert(drawing);
            drawing = false;
            vertexBuffer.SetVertices(vertices);
            vertexBuffer.Bind();
            shader.Use();
            textureMap.Bind();

            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }


    }
}
