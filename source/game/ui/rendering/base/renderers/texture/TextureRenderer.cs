

using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace SeeloewenCraft.game.ui
{
    internal static class TextureRenderer
    {

        static Shader shader;
        static VertexBuffer buffer;

        static TextureMap textureMap;

        static float[] vertices;
        static int index;
        static bool drawing;

        public static void Init()
        {
            shader = new Shader("shader.texture");
            buffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2).AddAttribute(1), 1024);
        }

        static internal void SetTexture(TextureMap textureMap)
        {
            //Debug.Assert(!drawing);
            if (drawing && TextureRenderer.textureMap != textureMap) //TODO possible optimization: override equals to section id
            {
                End();
                TextureRenderer.textureMap = textureMap;
                Begin();
            }
            else
            {
                TextureRenderer.textureMap = textureMap;
            }
        }


        static internal void Draw(string id, Rectangle bounds)
        {
            Draw(id, bounds.x1S, bounds.y1S, bounds.x2S, bounds.y2S);
        }

        static internal void Draw(string id, Rectangle bounds, float g)
        {
            Draw(id, bounds.x1S, bounds.y1S, bounds.x2S, bounds.y2S, g);
        }

        static internal void Draw(string id, float x1, float y1, float x2, float y2, float g)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(id);
            Draw(x1, y1, x2, y2, s1, t1, s2, t2, g);
        }

        static internal void Draw(string id, float x1, float y1, float x2, float y2)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(id);
            Draw(x1, y1, x2, y2, s1, t1, s2, t2, 1f);
        }

        static internal void Draw(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2, float g)
        {
            if (index + 6 * 6 >= 1024)
            {
                End();
                Begin();
            }
            Put(x1, y1, s1, t1, g);
            Put(x1, y2, s1, t2, g);
            Put(x2, y1, s2, t1, g);
            Put(x1, y2, s1, t2, g);
            Put(x2, y1, s2, t1, g);
            Put(x2, y2, s2, t2, g);
        }

        static void Put(float x, float y, float s, float t, float g)
        {
            Debug.Assert(textureMap != null && drawing && index + 5 < 1024);
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = s;
            vertices[index++] = t;
            vertices[index++] = g;
        }


        internal static void Draw(string id, Vector2f topLeft, Vector2f topRight, Vector2f botLeft, Vector2f botRight)
        {
            if (index + 6 * 6 >= 1024)
            {
                End();
                Begin();
            }
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(id);
            Put(topLeft, s1, t1);
            Put(botLeft, s1, t2);
            Put(topRight, s2, t1);
            Put(botLeft, s1, t2);
            Put(topRight, s2, t1);
            Put(botRight, s2, t2);
        }

        static void Put(Vector2f vector, float s, float t)
        {
            Put(vector.x, vector.y, s, t, 1f);
        }

        internal static void Begin()
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
            textureMap.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }

    }
}
