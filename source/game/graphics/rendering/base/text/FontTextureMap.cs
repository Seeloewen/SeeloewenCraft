
using SeeloewenCraft.game.util;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace SeeloewenCraft.game.graphics
{
    internal class FontTextureMap
    {

        const int WIDTH = 1024, HEIGHT = 1024;

        Texture texture;

        Dictionary<char, (float s1, float t1, float s2, float t2)> mappings;

        internal static (FontTextureMap, Dictionary<char, int>) ParseFontMap()
        {
            FontTextureMap textureMap = new();
            var widthMappings = new Dictionary<char, int>();

            textureMap.mappings = new Dictionary<char, (float, float, float, float)>();

            JsonToken token = JsonUtil.ReadResource(TextureManager.fontMappings).GetToken("/characters");


            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(TextureManager.fontMap);
            Bitmap bitmap = new Bitmap(stream);
            TextureImage texImage = new TextureImage(bitmap);

            float width = texImage.width;
            float height = texImage.height;

            TextureImage map = new TextureImage(WIDTH, HEIGHT);

            for (int i = 0; i < token.GetArrayLength(); i++)
            {
                JsonToken charToken = token.GetToken($"/{i}");

                char c = char.Parse(charToken.GetString("/character"));
                int x = charToken.GetInt("/offsetX");
                int y = charToken.GetInt("/offsetY");
                int w = charToken.GetInt("/width");

                float s1 = x / width;
                float t1 = y / height;
                float s2 = s1 + w / width;
                float t2 = t1 + 8 / height;

                textureMap.mappings.Add(c, (s1, t1, s2, t2));

                widthMappings.Add(c, w);
            }

            textureMap.texture = new Texture(texImage, false);

            return (textureMap, widthMappings);
        }

        internal (float s1, float t1, float s2, float t2) GetMapping(char c)
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
