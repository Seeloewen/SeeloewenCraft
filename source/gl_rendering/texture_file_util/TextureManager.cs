
using System.Collections.Generic;
using System.Windows.Documents;
using static System.Environment;

namespace SeeloewenCraft.gl_rendering
{
    internal class TextureManager
    {

        string texturePackPath = $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets";

        Dictionary<string, string> blockMappings = new Dictionary<string, string>();
        public List<string> blockTextures = new List<string>();

        Dictionary<string, string> playerMappings = new Dictionary<string, string>();

        internal TextureManager() {
            ParseContent();
        }

        internal void ParseContent() {
            JsonToken token = JsonUtil.ReadFile($"{texturePackPath}\\content.json");

            for (int i = 0; i < token.GetToken("/blocks").GetArrayLength(); i++) {
                JsonToken mappingToken = token.GetToken($"/blocks/{i}");
                blockMappings.Add(mappingToken.GetString("/id"), mappingToken.GetString("/file"));
                blockTextures.Add(mappingToken.GetString("/id"));
            }

            playerMappings.Add("head", token.GetString("/skin/head"));
            playerMappings.Add("body", token.GetString("/skin/body"));
            playerMappings.Add("arm_right", token.GetString("/skin/arm_right"));
            playerMappings.Add("arm_left", token.GetString("/skin/arm_left"));
            playerMappings.Add("leg_left", token.GetString("/skin/leg_left"));
            playerMappings.Add("leg_right", token.GetString("/skin/leg_right"));

        }

        public string GetPlayerTexture(string part)
        {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\{playerMappings[part]}";
        }

        public string GetBlockTexturePath(string blockID)
        {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\{blockMappings[blockID]}";
        }

        public string GetMisssingTexturePath() {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\Missing_Texture.png";
        }

    }
}
