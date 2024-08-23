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
        public static Client client;
        public static Server server;

        //Constants
        public const int WORLD_VERSION = 5;
        public const string GAME_VERSION = "Alpha M-Dev3";
        public const string VERSION_DATE = "24.08.2024";
        public const int TEXTUREPACK_VERSION = 2;

        //Variables
        public static string selectedTexturepack;
        public static bool isServer = false;
        public static bool isClient = false;

        //Methods
        public static bool IsMultiplayer()
        {
            return isServer || isClient;
        }
    }
}
