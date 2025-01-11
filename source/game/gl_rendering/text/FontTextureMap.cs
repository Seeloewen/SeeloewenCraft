
using SeeloewenCraft.game.ui;
using System.Collections.Generic;
using System.Drawing;

namespace SeeloewenCraft.gl_rendering
{
    internal class FontTextureMap
    {

        Texture texture;

        Dictionary<char, (float s1, float t1, float s2, float t2)> mappings;

        internal FontTextureMap(TextureManager textureManager)
        {
            mappings = new Dictionary<char, (float, float, float, float)>();

            Bitmap bitmap = new Bitmap(textureManager.fontPath);

            float width = bitmap.Width;
            float height = bitmap.Height;

            foreach(char c in textureManager.mappedChars)
            {
                (int x, int y, int w) = textureManager.charMappings[c];

                float s1 = (x+0.0f) / width;
                float t1 = (y+0.0f) / height;
                float s2 = s1 + (w - 0f) / width;
                float t2 = t1 + (8 - 0f) / height;

                mappings.Add(c, (s1, t1, s2, t2));
            }

            texture = new Texture(bitmap, false);
        }

        internal (float s1, float t1,float s2,float t2) GetMapping(char c)
        {
            (float s1, float t1, float s2, float t2) a;
            if (!mappings.TryGetValue(c, out a))
            {
                return mappings['?'];
            }
            return a;
        }


        internal void Bind()
        {
            texture.Bind();
        }

    }
}
