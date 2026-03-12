using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Core;

namespace SeeloewenCraft.game.graphics
{
    internal record TexturePath(bool disk, string path) { }

    public class TexturePackException : Exception
    {
        internal TexturePackException(string msg) : base(msg) { }
    }

    internal static class TextureManager
    {
        public static Dictionary<string, TextureMap> textureMaps;

        private static Dictionary<string, Dictionary<string, TexturePath>> texturePaths;



        public static string fontMap { get; private set; }
        public static string fontMappings { get; private set; }



        static void LoadTexturePaths(string[] texturePackPaths)
        {
            LoadInternalTexturePaths();

            //Load first item in array last -> prioritized
            for (int i = texturePackPaths.Length - 1; i >= 0; i--)
            {
                string texturePackPath = texturePackPaths[i];

                LoadExternalTexturePaths(texturePackPath);
            }
        }

        static void LoadInternalTexturePaths()
        {
            texturePaths = new Dictionary<string, Dictionary<string, TexturePath>>();

            JObject baseObj = JObject.Parse(FileUtil.StrFromResource("assets.content.json"));
            JArray sectionsArr = baseObj.Get<JArray>("sections");

            foreach (JObject sectionObj in sectionsArr)
            {
                string sectionName = sectionObj.Get<string>("section_name");
                texturePaths[sectionName] = new Dictionary<string, TexturePath>();

                JArray textureArr = sectionObj.Get<JArray>("textures");

                foreach(JObject textureObj in textureArr)
                {
                    string id = textureObj.Get<string>("id");
                    string file = textureObj.Get<string>("file");
                    texturePaths[sectionName][id] = new TexturePath(false, $"assets.{file}");
                }
            }
        }

        static void LoadExternalTexturePaths(string path)
        {
            JObject fileObj;
            try
            {
                fileObj = JsonUtil.ObjectFromFile($"{path}\\content.json");
            }
            catch
            {
                throw new TexturePackException("Error reading content.json in the texture pack folder");
            }

            JArray sectionsArr;
            try
            {
                sectionsArr = fileObj.Get<JArray>("sections");
            }
            catch
            {
                throw new TexturePackException("Error finding sections array in content.json");
            }

            int s = 0;
            foreach(JObject sectionObj in sectionsArr)
            {
                Dictionary<string, TexturePath> texturePathsSection;
                try
                {
                    string sectionName = sectionObj.Get<string>("section_name");
                    if (!texturePaths.TryGetValue(sectionName, out texturePathsSection))
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    throw new TexturePackException($"Error: invalid section_name in section ${s}");
                }

                JArray texturesArr;
                try
                {
                    texturesArr = sectionObj.Get<JArray>("textures");
                }
                catch
                {
                    throw new TexturePackException($"Error: textures array in section {sectionObj.Get<string>("section_name")} must be an array");
                }

                int t = 0;
                foreach(JObject textureObj in texturesArr)
                {
                    string id;
                    string filePath;
                    try
                    {
                        id = textureObj.Get<string>("id");
                        filePath = textureObj.Get<string>("file");
                        texturePathsSection[id] = new TexturePath(true, $"{path}\\{filePath}");
                    }
                    catch
                    {
                        throw new TexturePackException($"Error: invalid texture object {t} in section {sectionObj.Get<string>("section_name")}");
                    }

                    t++;
                }

                s++;
            }
        }


        static void CreateTextureMaps()
        {
            textureMaps = new Dictionary<string, TextureMap>();
            foreach (var section in texturePaths.Keys.ToList())
            {
                textureMaps[section] = new TextureMap(texturePaths[section]);
            }
        }




        static public void Load()
        {

            LoadTexturePaths(Settings.texturepacks.ToArray());

            CreateTextureMaps();

            //TODO seperate Font stuff
            JObject fileObj = JObject.Parse(FileUtil.StrFromResource("assets.content.json"));
            JObject fontObj = fileObj.Get<JObject>("font");

            fontMap = $"assets.{fontObj.Get<string>("font_map")}";
            fontMappings = $"assets.{fontObj.Get<string>("mappings")}";
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
                    using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.{path.path}");
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
