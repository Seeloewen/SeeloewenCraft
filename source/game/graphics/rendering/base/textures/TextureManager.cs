using SeeloewenCraft.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Environment;

namespace SeeloewenCraft.game.graphics
{
    internal static class TextureManager
    {


        static string texturePackPath = "SeeloewenCraft.Resources.assets.";
        
        static Dictionary<string, Dictionary<string, string>> mappings;


        public static string fontMap { get; private set; }
        public static string fontMappings { get; private set; }





        static public void Init() //TODO add texturepack support
        {

            mappings = new Dictionary<string, Dictionary<string, string>>();

            JsonToken fileToken = JsonUtil.ReadResource("assets.content.json");

            JsonToken fontToken = fileToken.GetToken("/font");

            fontMap = $"SeeloewenCraft.Resources.assets.{fontToken.GetString("/font_map")}";
            fontMappings = $"assets.{fontToken.GetString("/mappings")}";


            JsonToken token = fileToken.GetToken("/sections");

            for (int i = 0; i < token.GetArrayLength();i++)
            {
                var sectionToken = token.GetToken($"/{i}");

                string sectionName = sectionToken.GetString("/section_name");
                mappings[sectionName] = new Dictionary<string, string>();

                var textureArrayToken = sectionToken.GetToken("/textures");
                for(int j = 0; j < textureArrayToken.GetArrayLength(); j++)
                {
                    JsonToken textureToken = textureArrayToken.GetToken($"/{j}");
                    string id = textureToken.GetString("/id");
                    string file = textureToken.GetString("/file");
                    mappings[sectionName][id] = $"{texturePackPath}{file}"; 
                }

            }

        }


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

        //TODO problem: font map is handled seperatly
        static TextureImage LoadTexture(string file) {
            Bitmap bitmap;
            try
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{file}");
                bitmap = new Bitmap(stream);
            } catch
            {
                using Stream stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"SeeloewenCraft.Resources.assets.Missing_Texture.png");
                bitmap = new Bitmap(stream); //TODO 💀
            }

            return new TextureImage(bitmap);
        }






    }
}
