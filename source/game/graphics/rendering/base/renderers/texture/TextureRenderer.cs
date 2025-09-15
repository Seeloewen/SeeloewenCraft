

using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace SeeloewenCraft.game.graphics
{

    struct TextureVertex : IBatch
    {
        private float x, y;
        private float s, t;
        private float g;
        private float z;

        internal TextureVertex(float x, float y, float s, float t, float g, float z)
        {
            this.x = x;
            this.y = y;
            this.s = s;
            this.t = t;
            this.g = g;
            this.z = z;
        }

        public void Fill(float[] vertices, int i)
        {
            vertices[i++] = this.x;
            vertices[i++] = this.y;
            vertices[i++] = this.z;
            vertices[i++] = this.s;
            vertices[i++] = this.t;
            vertices[i++] = this.g;
        }
        

        public static int GetSize() => 6;
        
        public static VBLayout GetVBLayout() => new VBLayout(new [] {new VBElement(3), new VBElement(2), new VBElement(1)});
        
        
    }
    
    internal static class TextureRenderer
    {
        static BatchRenderer<TextureVertex> renderer;

        private static TextureMap textureMap;
        
        public static void Init()
        {
            renderer = new BatchRenderer<TextureVertex>(new Shader("shader.texture"));
        }

        static internal void SetTexture(TextureMap textureMap)
        {
            TextureRenderer.textureMap = textureMap;
            renderer.SetTexture(textureMap);
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

        static internal void Draw(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2, float g, float z = 0)
        {
            TextureVertex v0 = new TextureVertex(x1, y1, s1, t1, g, z);
            TextureVertex v1 = new TextureVertex(x1, y2, s1, t2, g, z);
            TextureVertex v2 = new TextureVertex(x2, y2, s2, t2, g, z);
            TextureVertex v3 = new TextureVertex(x2, y1, s2, t1, g, z);
            
            renderer.DrawRect(v0, v1, v2, v3);
        }



        internal static void Draw(string id, Vector2f topLeft, Vector2f topRight, Vector2f botLeft, Vector2f botRight, float g = 1f, float z = 0)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetMapping(id);
            
            TextureVertex v0 = new TextureVertex(topLeft.x, topLeft.y, s1, t1, g, z);
            TextureVertex v1 = new TextureVertex(botLeft.x, botLeft.y, s1, t2, g, z);
            TextureVertex v2 = new TextureVertex(botRight.x, botRight.y, s2, t2, g, z);
            TextureVertex v3 = new TextureVertex(topRight.x, topRight.y, s2, t1, g, z);
            
            renderer.DrawRect(v0, v1, v2, v3);
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
