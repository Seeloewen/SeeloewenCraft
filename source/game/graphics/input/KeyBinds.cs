using Newtonsoft.Json.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SeeloewenCraft.game.graphics
{
    public static class KeyBinds
    {

        public const int MOVE_RIGHT = 0;
        public const int MOVE_LEFT = 1;
        public const int SHOW_INV = 2;
        public const int TOGGLE_DEBUG = 3;
        public const int JUMP = 4;
        public const int NOTIFICATIONS = 5;
        public const int THROW_ITEM = 6;
        public const int SNEAK = 7;
        public const int SPRINT = 8;
        public const int OPEN_MENU = 9;
        public const int CREATIVE_MENU = 10;
        const int KEYBINDS_COUNT = 11;


        public static bool[] pressed = new bool[KEYBINDS_COUNT];

        public static bool[] pressedFirst = new bool[KEYBINDS_COUNT];

        public static bool CheckPressedFirst(int keybind)
        {
            if (pressed[keybind])
            {
                pressed[keybind] = false;
                return true;
            }
            return false;
        }


        public static Dictionary<Keys, int> bindings = new Dictionary<Keys, int>()
        {
            {Keys.D, MOVE_RIGHT },
            {Keys.A, MOVE_LEFT },
            {Keys.E, SHOW_INV },
            {Keys.F3, TOGGLE_DEBUG },
            {Keys.Space, JUMP },
            {Keys.N, NOTIFICATIONS },
            {Keys.Q, THROW_ITEM },
            {Keys.LeftShift, SNEAK },
            {Keys.LeftControl, SPRINT },
            {Keys.Escape, OPEN_MENU },
            {Keys.C, CREATIVE_MENU}
        };

        public static void Bind(Keys k, int action)
        {
            //Make sure no other keybind is connected to the specified action
            foreach (var keybind in bindings)
            {
                if (keybind.Value == action)
                {
                    bindings.Remove(keybind.Key);
                    break; //If everything goes right, there should ever only be one key per action
                }
            }

            bindings[k] = action;
        }

        public static JObject ToJson()
        {
            JObject obj = new JObject();
            foreach (var binding in bindings)
            {
                obj.Add(binding.Value.ToString(), binding.Key.ToString());
            }

            return obj;
        }

        public static void FromJson(JObject obj)
        {
            bindings = new Dictionary<Keys, int>();
            foreach (JProperty property in obj.Properties())
            {
                Enum.TryParse(property.Value.ToString(), out Keys key);
                int id = int.Parse(property.Name);
                bindings[key] = id;
            }
        }

        public static Keys ToGLFWKey(Key wpfKey)
        {
            return wpfKey switch
            {
                Key.LeftCtrl => Keys.LeftControl,
                Key.RightCtrl => Keys.RightControl,
                _ => Keys.Unknown
            };
        }

        public static Keys ToGLFWKey(string s)
        {
            if (Enum.TryParse(s, out Keys key))
            {
                return key;
            }

            return default;
        }
    }
}
