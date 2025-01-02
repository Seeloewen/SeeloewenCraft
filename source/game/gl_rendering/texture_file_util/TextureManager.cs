using System.Collections.Generic;
using static System.Environment;

namespace SeeloewenCraft.gl_rendering
{
    internal class TextureManager
    {

        string texturePackPath = $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets";

        public string fontPath = $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\font\\block font - final3.png";
        public Dictionary<char, (int x, int y, int w)> charMappings = new Dictionary<char, (int x, int y, int w)>();
        public List<char> mappedChars = new List<char>();

        Dictionary<string, string> blockMappings = new Dictionary<string, string>();
        public List<string> blockTextures = new List<string>();

        Dictionary<string, string> itemMappings = new Dictionary<string, string>();
        public List<string> itemTextures = new List<string>();

        Dictionary<string, string> playerMappings = new Dictionary<string, string>();

        internal TextureManager() {
            ParseContent();
        }

        internal void ParseContent() {
            JsonToken token = JsonUtil.ReadFile($"{texturePackPath}\\content.json");

            for (int i = 0; i < token.GetToken("/blocks").GetArrayLength(); i++) {
                JsonToken mappingToken = token.GetToken($"/blocks/{i}");
                string id = mappingToken.GetString("/id");
                string path = mappingToken.GetString("/file");
                blockMappings.Add(id, path);
                blockTextures.Add(id);
            }

            for (int i = 0; i < token.GetToken("/items").GetArrayLength(); i++)
            {
                JsonToken mappingToken = token.GetToken($"/items/{i}");
                string id = mappingToken.GetString("/id");
                string path = mappingToken.GetString("/file");
                itemMappings.Add(id, path);
                itemTextures.Add(id);
            }

            playerMappings.Add("head", token.GetString("/skin/head"));
            playerMappings.Add("body", token.GetString("/skin/body"));
            playerMappings.Add("arm_right", token.GetString("/skin/arm_right"));
            playerMappings.Add("arm_left", token.GetString("/skin/arm_left"));
            playerMappings.Add("leg_left", token.GetString("/skin/leg_left"));
            playerMappings.Add("leg_right", token.GetString("/skin/leg_right"));



            token = JsonUtil.ReadFile($"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\font\\font-map.json");
            for (int i = 0; i < token.GetToken("/characters").GetArrayLength(); i++)
            {
                JsonToken mappingToken = token.GetToken($"/characters/{i}");
                char c = char.Parse(mappingToken.GetString("/character"));
                int x = mappingToken.GetInt("/offsetX");
                int y = mappingToken.GetInt("/offsetY");
                int w = mappingToken.GetInt("/width");
                charMappings.Add(c, (x, y, w));
                mappedChars.Add(c);
            }


        }

        public string GetPlayerTexture(string part)
        {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\{playerMappings[part]}";
        }

        public string GetBlockTexturePath(string blockID)
        {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\{blockMappings[blockID]}";
        }
        public string GetItemTexturePath(string itemID)
        {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\{itemMappings[itemID]}";
        }

        public string GetMisssingTexturePath() {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\Missing_Texture.png";
        }

    }
}
