using System.Collections.Generic;


namespace SeeloewenCraft.game.ui
{
    internal class TextureMap
    {

        Dictionary<string, (float s1, float t1, float s2, float t2)> mappings;


        internal TextureMap(string section)
        {
            var textureIDs = TextureManager.GetMappings(section);

            TextureImage map = new TextureImage(1024, 1024);
            int rootX = 0, rootY = 0;
            foreach (var id in textureIDs)
            {
                TextureImage image = TextureManager.LoadTexture(section, id);
                for (int rY = 0; rY < image.height; rY++) //TODO possible optimization by copying whole array row
                {
                    int wY = rootY + rY;
                    for (int rX = 0; rX < image.width; rX++)
                    {
                        int wX = rootX + rX;
                        for (int c = 0; c < 4; c++/*nice*/)
                        {
                            map.SetByte(image.GetByte(rX, rY, c), wX, wY, c);
                        }
                    }
                }

            }

            #region Initializing

            #endregion


        }
    }
