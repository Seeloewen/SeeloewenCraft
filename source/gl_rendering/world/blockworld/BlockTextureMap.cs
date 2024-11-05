
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace SeeloewenCraft.gl_rendering
{
    internal class BlockTextureMap : TextureMap
    {

        internal BlockTextureMap(TextureManager manager) : base(2048, 2048)
        {
            List<string> blocks = manager.blockTextures;
            Debug.Assert(blocks.Count < 1024);
            for (int i = 0; i < blocks.Count; i++)
            {
                Bitmap block;
                try
                {
                    block = new Bitmap(manager.GetBlockTexturePath(blocks[i]));
                }
                catch
                {
                    block = new Bitmap(manager.GetMisssingTexturePath());
                }
                
                int offsetX = (i % 32) * 64;
                int offsetY = (i / 32) * 64;

                AddMapping(blocks[i], block, offsetX, offsetY);

            }

            Finalize();
        }





    }
}
