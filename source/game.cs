using System;
using System.Collections.Generic;
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
        public const int WORLD_VERSION = 7; //Up to date as of Alpha 1.2.2 (Recent changes: ground block checking)
        public const string GAME_VERSION = "Alpha 1.2.2";
        public const string VERSION_DATE = "01.01.2025";
        public const int TEXTUREPACK_VERSION = 2;

        //Variables
        public static List<string> unstackableItems = new List<string>();
        public static string selectedTexturepack;
        public static Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Microsecond);
        public static int playerId;


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
