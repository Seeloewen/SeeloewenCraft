using System.Windows.Input;

namespace SeeloewenCraft
{
    public static class Settings
    {
        //Settings
        public static bool saveLogOnExit = false;
        public static bool saveWorldOnClose = true;
        public static bool enableHammer = true;
        public static bool enableCaveGeneration = true;
        public static bool enableLighting = true;
        public static bool showNotifications = false;
        public static bool enableHealth = false;
        public static string texturepack;

        //Keybinds
        public static Key cMoveRight = Key.D;
        public static Key cMoveLeft = Key.A;
        public static Key cShowInv = Key.E;
        public static Key cToggleDebug = Key.F3;
        public static Key cJump = Key.Space;
        public static Key cNotifications = Key.N;
    }
}
