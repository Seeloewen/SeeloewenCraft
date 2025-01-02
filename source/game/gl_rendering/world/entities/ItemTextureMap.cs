

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace SeeloewenCraft.gl_rendering
{
    internal class ItemTextureMap : TextureMap
    {

        internal ItemTextureMap(TextureManager manager) : base(2048, 2048)
        {
            List<string> items = manager.itemTextures;
            Debug.Assert(items.Count < 1024);
            for (int i = 0; i < items.Count; i++)
            {
                Bitmap block;
                try
                {
                    block = new Bitmap(manager.GetItemTexturePath(items[i]));
                }
                catch
                {
                    block = new Bitmap(manager.GetMisssingTexturePath());
                }

                int offsetX = (i % 32) * 64;
                int offsetY = (i / 32) * 64;

                AddMapping(items[i], block, offsetX, offsetY);

            }

            Finalize();
        }

    }
}
