using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public static class Game
    {
        //References
        public static World world;

        //Constants
        public const int WORLD_VERSION = 5;
        public const string GAME_VERSION = "Alpha 1.2.0-Dev6";
        public const string VERSION_DATE = "14.08.2024";
        public const int TEXTUREPACK_VERSION = 2;

        //Variables
        public static string selectedTexturepack;
        public static List<string> unstackableItems = new List<string>();
    }
}
