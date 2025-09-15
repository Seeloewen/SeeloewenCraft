using SeeloewenCraft.game.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SeeloewenCraft.game.graphics
{
    internal record TexturePath(bool disk, string path) { }
    
    public class TexturePackException : Exception {
        internal TexturePackException(string msg) : base(msg) { }
    }
    
    internal static class TextureManager
    {
        public static Dictionary<string, TextureMap> textureMaps;

        private static Dictionary<string, Dictionary<string, TexturePath>> texturePaths;
        
        static string internalTexturePackPath = "SeeloewenCraft.Resources.assets.";



        public static string fontMap { get; private set; }
        public static string fontMappings { get; private set; }



        static void LoadTexturePaths(string[] texturePackPaths)
        {
            LoadInternalTexturePaths();
            foreach (string texturePackPath in texturePackPaths)
            {
                LoadExternalTexturePaths(texturePackPath);
            }
        }

        static void LoadInternalTexturePaths()
        {
            texturePaths = new Dictionary<string, Dictionary<string, TexturePath>>();
            
            JsonToken fileToken = JsonUtil.ReadResource("assets.content.json");

            JsonToken sectionsToken = fileToken.GetToken("/sections");

            for (int i = 0; i < sectionsToken.GetArrayLength(); i++)
            {
                var sectionToken = sectionsToken.GetToken($"/{i}");

                string sectionName = sectionToken.GetString("/section_name");
                texturePaths[sectionName] = new Dictionary<string, TexturePath>();

                var textureArrayToken = sectionToken.GetToken("/textures");
                for (int j = 0; j < textureArrayToken.GetArrayLength(); j++)
                {
                    JsonToken textureToken = textureArrayToken.GetToken($"/{j}");
                    string id = textureToken.GetString("/id");
                    string file = textureToken.GetString("/file");
                    texturePaths[sectionName][id] = new TexturePath(false, $"SeeloewenCraft.Resources.assets.{file}");
                }
            }
        }

        static void LoadExternalTexturePaths(string path)
        {
            JsonToken fileToken;
            try
            {
                fileToken = JsonUtil.ReadFile($"{path}\\content.json");
            }
            catch (Exception e)
            {
                throw new TexturePackException("Error reading content.json in the texture pack folder");
            }

            JsonToken sectionsToken;
            try
            {
                sectionsToken = fileToken.GetToken("/sections");
            }
            catch
            {
                throw new TexturePackException("Error finding /sections array in content.json");
            }

            int sectionsLength;
            try
            {
                sectionsLength = sectionsToken.GetArrayLength();
            }
            catch
            {
                throw new TexturePackException("Error: /sections must be an array");
            }

            for (int s = 0; s < sectionsLength; s++)
            {
                var sectionToken = sectionsToken.GetToken($"/{s}");
                Dictionary<string, TexturePath> texturePathsSection;
                try
                {
                    string sectionName = sectionToken.GetString("/section_name");
                    if (!texturePaths.TryGetValue(sectionName, out texturePathsSection))
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    throw new TexturePackException($"Error: invalid /section_name in section ${s}");
                }

                JsonToken texturesToken;
                int texturesCount;
                try
                {
                    texturesToken = sectionToken.GetToken("/textures");
                    texturesCount = texturesToken.GetArrayLength();
                }
                catch
                {
                    throw new TexturePackException($"Error: /textures in section {sectionToken.GetString("/section_name")} must be an array");
                }

                for (int t = 0; t < texturesCount; t++)
                {
                    string id;
                    string filePath;
                    try
                    {
                        JsonToken textureToken = texturesToken.GetToken($"/{t}");
                        id = textureToken.GetString("/id");
                        filePath = textureToken.GetString("/file");
                        texturePathsSection[id] = new TexturePath(true, $"{path}\\{filePath}");
                    }
                    catch
                    {
                        throw new TexturePackException($"Error: invalid texture object {t} in section {sectionToken.GetString("/section_name")}");
                    }
                }

            }



        }


        static void CreateTextureMaps()
        {
            textureMaps = new Dictionary<string, TextureMap>(); 
            foreach(var section in texturePaths.Keys.ToList())
            {
                textureMaps[section] = new TextureMap(texturePaths[section]);
            }
        }

         
        
        
        static public void Load()
        {

            LoadTexturePaths(new string[] { });

            CreateTextureMaps();
            
            
            //TODO seperate Font stuff
            JsonToken fileToken = JsonUtil.ReadResource("assets.content.json");
            JsonToken fontToken = fileToken.GetToken("/font");

            fontMap = $"SeeloewenCraft.Resources.assets.{fontToken.GetString("/font_map")}";
            fontMappings = $"assets.{fontToken.GetString("/mappings")}";



        }



        static internal TextureImage LoadTexture(TexturePath path)
        {
            TextureImage texture;
            try
            {
                Bitmap bitmap;
                if (path.disk)
                {
                    bitmap = new Bitmap(path.path);
                }
                else
                {
                    using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path.path);
                    bitmap = new Bitmap(stream);
                }
                texture = new TextureImage(bitmap);
            }
            catch
            {
                texture = GetMissingTexture();
            }

            return texture;
        }

        static TextureImage GetMissingTexture()
        {
                using Stream stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"SeeloewenCraft.Resources.assets.Missing_Texture.png");
                return new TextureImage(new Bitmap(stream));
        }





    }
}
