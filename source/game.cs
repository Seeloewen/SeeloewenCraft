using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeloewenCraft
{
    public static class Game
    {
        //References
        public static World world;
        public static Client client;
        public static Server server;

        //Constants
        public const int WORLD_VERSION = 6; //Up to date as of Alpha 1.2.1-dev (Recent changes: Seeding)
        public const string GAME_VERSION = "Alpha 1.2.1-Dev";
        public const string VERSION_DATE = "07.10.2024";
        public const int TEXTUREPACK_VERSION = 2;

        //Variables
        public static List<string> unstackableItems = new List<string>();
        public static string selectedTexturepack;
        public static Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Microsecond);

        //Methods
        public static bool IsMultiplayer()
        {
            return IsServer() || IsClient();
        }

        public static bool IsServer()
        {
            return server != null;
        }

        public static bool IsClient()
        {
            return client != null && client.isConnected;
        }

        public static void ShowException(Exception ex)
        {
            MessageBox.Show($"Oh no! The game has encountered an exception: {ex.Message} \n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
