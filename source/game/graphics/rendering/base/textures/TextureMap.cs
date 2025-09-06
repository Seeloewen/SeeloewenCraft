using System.Collections.Generic;


namespace SeeloewenCraft.game.graphics
{
    internal class TextureMap
    {
        const int HEIGHT = 2048, WIDTH = 2048;

        Dictionary<string, (float s1, float t1, float s2, float t2)> mappings;

        protected Texture texture;

        protected TextureMap()
        {
            
        }
        
        internal TextureMap(Dictionary<string, TexturePath> section)
        {

            mappings = new Dictionary<string, (float s1, float t1, float s2, float t2)>();

            TextureImage map = new TextureImage(WIDTH, HEIGHT);
            int offsetX = 0, offsetY = 0, maxY = 0;
            foreach (var texture in section)
            {
                TextureImage image = TextureManager.LoadTexture(texture.Value);

                if (image.width + offsetX > WIDTH)
                {
                    offsetY += maxY;
                    offsetX = 0;
                }

                if (image.height > maxY) maxY = image.height;

                for (int rY = 0; rY < image.height; rY++) //TODO possible optimization by copying whole array row
                {
                    int wY = offsetY + rY;
                    for (int rX = 0; rX < image.width; rX++)
                    {
                        int wX = offsetX + rX;
                        for (int c = 0; c < 4; c++/*nice*/)
                        {
                            map.SetByte(image.GetByte(rX, rY, c), wX, wY, c);
                        }
                    }
                }


                float s1 = ((float)offsetX + 0.5f) / WIDTH;
                float t1 = ((float)offsetY + 0.5f) / HEIGHT;
                float s2 = s1 + (((float)image.width - 1.0f) / WIDTH);
                float t2 = t1 + (((float)image.height - 1.0f) / HEIGHT);
                mappings.Add(texture.Key, (s1, t1, s2, t2));

                offsetX += image.width;
            }

            texture = new Texture(map, false);
        }

        internal (float s1, float t1, float s2, float t2) GetMapping(string id)
        {
            if (mappings.TryGetValue(id, out var v))
            {
                return v;
            }
            else
            {
                return (0f, 0f, 1f, 1f);
            }
        }

        internal void Bind()
        {
            texture.Bind();
        }

    }
}
