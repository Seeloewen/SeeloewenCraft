
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics
{
    
    
    struct TextVertex : IBatch
    {
        private float x, y, z;
        private float s, t;
        private float r, g, b;

        internal TextVertex(float x, float y, float z, float s, float t, float r, float g, float b)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.s = s;
            this.t = t;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public void Fill(float[] vertices, int i)
        {
            vertices[i++] = this.x;
            vertices[i++] = this.y;
            vertices[i++] = this.z;
            vertices[i++] = this.s;
            vertices[i++] = this.t;
            vertices[i++] = this.r;
            vertices[i++] = this.g;
            vertices[i++] = this.b;
        }
        

        public static int GetSize() => 8;

        public static VBLayout GetVBLayout() => new VBLayout().AddAttribute(3).AddAttribute(2).AddAttribute(3);
        
    }    
    internal static class TextRenderer
    {

        static FontTextureMap textureMap;
        static Dictionary<char, int> widthMappings;

        private static BatchRenderer<TextVertex> renderer;
        
        internal static void Init()
        {
            (textureMap, widthMappings) = FontTextureMap.ParseFontMap();

            renderer = new BatchRenderer<TextVertex>(new Shader("shader.text"));
            renderer.SetTexture(textureMap);
        }

        internal static void Draw(string s, int x, int y, int size)
        {
            Draw(s, x, y, size, new Color(0f));
        }
        
        internal static void Draw(string s, int x, int y, int size, Color color)
        {

            (float x1, float y1) = Resolution.PixelToScreen(x, y);
            float sizeY = 2f / Resolution.HEIGHT;

            Draw(s, x1, y1 - size * 8 * sizeY, size * 8 * sizeY, color);
        }

        public static int GetWidth(string s, int size)
        {
            int width = 0;
            foreach (char c in s)
            {
                int charWidth;
                if (!widthMappings.TryGetValue(c, out charWidth))
                {
                    charWidth = 5;
                }
                width += charWidth;
                width += 1;
            }
            if (width != 0) width--;
            return width * size;
        }

        
        static private void Draw(string s, float x, float y, float h, Color color)
        {
            float w = (h / 8) * (1 / Resolution.RATIO);

            foreach (char c in s)
            {
                x += Draw(c, x, y, h, color);
                x += w;
            }
        }

        //returns width
        static private float Draw(char c, float x, float y, float h, Color color)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(c);

            int charWidth;
            if (!widthMappings.TryGetValue(c, out charWidth))
            {
                charWidth = 5;
            }
            float w = charWidth * (h / 8) / Resolution.RATIO;
            Draw(x, y, x + w, y + h, s1, t2, s2, t1, color);

            return w;
        }

        static private void Draw(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2, Color color, float z = 0f)
        {
            TextVertex v0 = new TextVertex(x1, y1, z, s1, t1, color.r, color.g, color.b);
            TextVertex v1 = new TextVertex(x1, y2, z, s1, t2, color.r, color.g, color.b);
            TextVertex v2 = new TextVertex(x2, y2, z, s2, t2, color.r, color.g, color.b);
            TextVertex v3 = new TextVertex(x2, y1, z, s2, t1, color.r, color.g, color.b);
            
            renderer.DrawRect(v0, v1, v2, v3);
        }
        static internal void Begin()
        {
            renderer.Begin();
        }

        static internal void End()
        {
            renderer.End();
        }
        
        internal static void Flush()
        {
            renderer.Flush();
        }


    }
}
