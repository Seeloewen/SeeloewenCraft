
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
        }

        public string GetTexturePath(string blockID)
        {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\{blockMappings[blockID]}";
        }

        public string GetMisssingTexturePath() {
            return $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets\\Missing_Texture.png";
        }

    }
}
