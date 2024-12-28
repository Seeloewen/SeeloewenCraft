
using OpenTK.Graphics.OpenGL;
using SeeloewenCraft.entity;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.gl_rendering
{
    internal class TextRenderer
    {

        FontTextureMap textureMap;
        Shader shader;
        VertexBuffer vertexBuffer;

        static Dictionary<char, int> widthMappings;

        internal TextRenderer(TextureManager manager)
        {
            textureMap = new FontTextureMap(manager);
            widthMappings = new Dictionary<char, int>();
            foreach (char c in manager.mappedChars)
            {
                (_, _, int w) = manager.charMappings[c];
                widthMappings.Add(c, w);
            }
            shader = new Shader("shader/blockworld");
            vertexBuffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2).AddAttribute(1), 1024);
        }


        float[] vertices;
        int index;
        bool drawing;


        internal void Test()
        {
            Begin();
            

            Draw("Hallo meine Kameraden geil man jawollja komm in die Gruppe", 1, 1, 2);

            End();
        }

        internal void Draw(string s, int x, int y, int size)
        {
            float x1 = (x - 1280f + 0.5f) / 1280f;

            float y1 = ((y+74) - 720f + 0.5f) / 720f;
            float sizeY = 1 / 360f;

            Draw(s, x1, y1, size * 8 * sizeY);
        }

        public static int GetWidth(string s, int size)
        {
            int width = -1;
            foreach(char c in s) {
                width += 1;
                int charWidth;
                if (!widthMappings.TryGetValue(c, out charWidth))
                {
                    charWidth = 5;
                } 
                width += charWidth;
            }
            return width * size;
        }

        private void Draw(string s, float x, float y, float h)
        {
            float w = (h / 8) * (9 / 16f);

            foreach (char c in s)
            {
                x += Draw(c, x, y, h);
                x += w;
            }
        }

        //returns width
        private float Draw(char c, float x, float y, float h)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(c);

            int charWidth;
            if (!widthMappings.TryGetValue(c, out charWidth))
            {
                charWidth = 5;
            }
            float w = charWidth * (h / 8) * 9/16f;
            Draw(x, y, x + w, y + h, s1, t2, s2, t1);

            return w;
        }

        private void Draw(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2)
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

        private void Put(float x, float y, float s, float t, float g)
        {
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = s;
            vertices[index++] = t;
            vertices[index++] = g;
        }

        internal void Begin()
        {
            drawing = true;
            index = 0;
            vertices = new float[1024];
        }

        internal void End()
        {
            drawing = false;
            vertexBuffer.SetVertices(vertices);
            vertexBuffer.Bind();
            shader.Use();
            textureMap.Bind();

            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }


    }
}
