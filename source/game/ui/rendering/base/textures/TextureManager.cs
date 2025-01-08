using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace SeeloewenCraft.game.ui
{
    internal static class TextureManager
    {

        static string texturePackPath = $"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\assets";

        static JsonToken token;

        static internal JsonToken GetMappingToken(string section)
        {
            return token.GetToken(section);
        }



    }
}
