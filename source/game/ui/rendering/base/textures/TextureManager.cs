using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Environment;

namespace SeeloewenCraft.game.ui
{
    internal static class TextureManager
    {

        static string texturePackPath = $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets";

        static Dictionary<string, Dictionary<string, string>> mappings;

        static internal List<string> GetMappings(string section)
        {
            if (mappings.TryGetValue(section, out var m))
            {
                return m.Keys.ToList();
            }
            throw new Exception("Textures section not loaded");
        }

        static internal TextureImage LoadTexture(string section, string name)
        {
            if (!mappings.TryGetValue(section, out var map))
            {
                throw new Exception("Textures section not loaded");
            }
            if (!map.TryGetValue(name, out string file))
            {
                throw new Exception("Texture not loaded");
            }
            return LoadTexture(file); //TODO mechanism for loading file from resources when necessary
        }

        static TextureImage LoadTexture(string file) {
            Bitmap bitmap = new Bitmap(file);

            return new TextureImage(bitmap);
        }






    }
}
