using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeeloewenCraft.game.ui
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
        const int KEYBINDS_COUNT = 10;


        public static bool[] pressed = new bool[KEYBINDS_COUNT];

        public static bool[] pressedFirst = new bool[KEYBINDS_COUNT];

        public static bool checkPressedFirst(int keybind)
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
            {Keys.Escape, OPEN_MENU }
        };

        public static void Save(JsonWriter writer)
        {
            writer.WritePropertyName("keybinds");
            writer.WriteStartObject();

            Keys moveRight = bindings.FirstOrDefault(x => x.Value == 0).Key;
            Keys moveLeft = bindings.FirstOrDefault(x => x.Value == 1).Key;
            Keys showInv = bindings.FirstOrDefault(x => x.Value == 2).Key;
            Keys toggleDebug = bindings.FirstOrDefault(x => x.Value == 3).Key;
            Keys jump = bindings.FirstOrDefault(x => x.Value == 4).Key;
            Keys notifications = bindings.FirstOrDefault(x => x.Value == 5).Key;
            Keys throwItem = bindings.FirstOrDefault(x => x.Value == 6).Key;
            Keys sneak = bindings.FirstOrDefault(x => x.Value == 7).Key;
            Keys sprint = bindings.FirstOrDefault(x => x.Value == 8).Key;

            writer.WritePropertyName("move_right");
            writer.WriteValue(moveRight);

            writer.WritePropertyName("move_left");
            writer.WriteValue(moveLeft);

            writer.WritePropertyName("show_inv");
            writer.WriteValue(showInv);

            writer.WritePropertyName("toggle_debug");
            writer.WriteValue(toggleDebug);

            writer.WritePropertyName("jump");
            writer.WriteValue(jump);

            writer.WritePropertyName("show_notification_list");
            writer.WriteValue(notifications);

            writer.WritePropertyName("throw_item");
            writer.WriteValue(throwItem);

            writer.WritePropertyName("sneak");
            writer.WriteValue(sneak);

            writer.WritePropertyName("sprint");
            writer.WriteValue(sprint);

            writer.WriteEndObject();
        }

        public static void Load(JsonToken fileToken)
        {
            JsonToken token = fileToken.GetToken("/keybinds");

            Enum.TryParse(token.GetString("/move_right"), out Keys moveRight);
            Enum.TryParse(token.GetString("/move_left"), out Keys moveLeft);
            Enum.TryParse(token.GetString("/show_inv"), out Keys showInv);
            Enum.TryParse(token.GetString("/toggle_debug"), out Keys toggleDebug);
            Enum.TryParse(token.GetString("/jump"), out Keys jump);
            Enum.TryParse(token.GetString("/show_notification_list"), out Keys showNotificationList);
            Enum.TryParse(token.GetString("/throw_item"), out Keys throwItem);
            Enum.TryParse(token.GetString("/sneak"), out Keys sneak);
            Enum.TryParse(token.GetString("/sprint"), out Keys sprint);

            bindings = new Dictionary<Keys, int>();
            bindings[moveRight] = MOVE_RIGHT;
            bindings[moveLeft] = MOVE_LEFT;
            bindings[showInv] = SHOW_INV;
            bindings[toggleDebug] = TOGGLE_DEBUG;
            bindings[jump] = JUMP;
            bindings[showNotificationList] = NOTIFICATIONS;
            bindings[throwItem] = THROW_ITEM;
            bindings[sneak] = SNEAK;
            bindings[sprint] = SPRINT;
            bindings[Keys.Escape] = OPEN_MENU;

        }



    }
}
