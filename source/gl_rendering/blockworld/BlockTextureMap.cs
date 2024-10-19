
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace SeeloewenCraft.gl_rendering
{
    internal class BlockTextureMap
    {

        Texture texture;

        Dictionary<string, (float s1, float t1, float s2, float t2)> mappings = new Dictionary<string, (float s1, float t1, float s2, float t2)>();

        internal BlockTextureMap(TextureManager manager)
        {
            List<string> blocks = manager.blockTextures;
            Bitmap map = new Bitmap(2048, 2048);
            Debug.Assert(blocks.Count < 1024);
            for (int i = 0; i < blocks.Count; i++)
            {
                Bitmap block;
                try
                {
                    block = new Bitmap(manager.GetTexturePath(blocks[i]));
                }
                catch
                {
                    block = new Bitmap(manager.GetMisssingTexturePath());
                }
                for (int rX = 0; rX < 64; rX++)
                {
                    for (int rY = 0; rY < 64; rY++)
                    {
                        int wX = rX + (i % 32) * 64;
                        int wY = rY + (i / 32) * 64;
                        map.SetPixel(wX, wY, block.GetPixel(rX, rY));
                    }
                }
                float s1 = (i % 32) / 32.0f;
                float t1 = (i / 32) / 32.0f;
                float s2 = ((i % 32) + 1) / 32.0f;
                float t2 = ((i / 32 + 1)) / 32.0f;
                mappings.Add(blocks[i], (s1, t1, s2, t2));

            }

            texture = new Texture(map);
        }

        internal (float s1, float t1, float s2, float t2) GetTexture(string blockID)
        {
            (float s1, float t1, float s2, float t2) a;
            if (mappings.TryGetValue(blockID, out a))
            {
                return mappings[blockID];
            } else
            {
                return mappings["missing_texture"];
            }
            
        }

        internal void Bind()
        {
            texture.Bind();
        }


    }
}
