using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace SeeloewenCraft.gl_rendering
{
    internal class TextureMap
    {

        Dictionary<string, (float s1, float t1, float s2, float t2)> mappings = new Dictionary<string, (float s1, float t1, float s2, float t2)>();


        Bitmap map;
        bool finalized;

        Texture texture;

        protected TextureMap(int width, int height)
        {
            map = new Bitmap(width, height);
        }

        protected void AddMapping(string id, Bitmap image, int offsetX, int offsetY)
        {
            Debug.Assert(!finalized);
            for (int rX = 0; rX < image.Width; rX++)
            {
                for (int rY = 0; rY < image.Height; rY++)
                {
                    int wX = rX + offsetX;
                    int wY = rY + offsetY;
                    map.SetPixel(wX, wY, image.GetPixel(rX, rY));
                }
            }

            float s1 = ((float)offsetX+0.5f) / map.Width;
            float t1 = ((float)offsetY+0.5f) / map.Height;
            float s2 = s1 + (((float)image.Width-1.0f) / map.Width);
            float t2 = t1 + (((float)image.Height - 1.0f) / map.Height);
            mappings.Add(id, (s1, t1, s2, t2));
        }

        protected void Finalize()
        {
            finalized = true;
            texture = new Texture(map);
        }

        internal (float s1, float t1, float s2, float t2) GetTexture(string blockID)
        {
            Debug.Assert(finalized);
            (float s1, float t1, float s2, float t2) a;
            if (mappings.TryGetValue(blockID, out a))
            {
                return mappings[blockID];
            }
            else
            {
                return (0.0f, 0.0f, 1.0f, 1.0f);
            }

        }

        internal void Bind()
        {
            Debug.Assert(finalized);
            texture.Bind();
        }
    }
}
