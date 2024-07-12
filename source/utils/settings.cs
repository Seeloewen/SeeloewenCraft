using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public class Settings
    {
        //Settings
        public bool saveLogOnExit = false;
        public bool saveWorldOnClose = true;
        public bool enableHammer = true;
        public bool enableCaveGeneration = true;
        public bool enableLighting = true;
        public bool showNotifications = true;
        public string texturepack;

        //Keybinds
        public Key cMoveRight = Key.D;
        public Key cMoveLeft = Key.A;
        public Key cShowInv = Key.E;
        public Key cToggleDebug = Key.F3;
        public Key cJump = Key.Space;
        public Key cNotifications = Key.N;
    }
}
