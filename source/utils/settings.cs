using System;
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
        public static string resolution = "1280x720";
        public static string videoMode = "Windowed";
        public static int customResX = 1280;
        public static int customResY = 720;

        public static string texturepack;

        //Keybinds
        public static Key cMoveRight = Key.D;
        public static Key cMoveLeft = Key.A;
        public static Key cShowInv = Key.E;
        public static Key cToggleDebug = Key.F3;
        public static Key cJump = Key.Space;
        public static Key cNotifications = Key.N;

        public static void Save(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("settings");

            //Save all the settings 
            writer.WriteStartObject();

            writer.WritePropertyName("save_log_on_exit");
            writer.WriteValue(saveLogOnExit);

            writer.WritePropertyName("save_world_on_close");
            writer.WriteValue(saveWorldOnClose);

            writer.WritePropertyName("enable_hammer");
            writer.WriteValue(enableHammer);

            writer.WritePropertyName("enable_cave_generation");
            writer.WriteValue(enableCaveGeneration);

            writer.WritePropertyName("enable_lighting");
            writer.WriteValue(enableLighting);

            writer.WritePropertyName("show_notifications");
            writer.WriteValue(showNotifications);

            writer.WritePropertyName("enable_health");
            writer.WriteValue(enableHealth);

            writer.WritePropertyName("resolution");
            writer.WriteValue(resolution);

            writer.WritePropertyName("video_mode");
            writer.WriteValue(videoMode);

            writer.WritePropertyName("custom_res_x");
            writer.WriteValue(customResX);

            writer.WritePropertyName("custom_res_y");
            writer.WriteValue(customResY);

            writer.WritePropertyName("texturepack");
            writer.WriteValue(texturepack);

            writer.WriteEndObject();

            writer.WritePropertyName("keybinds");

            //Save all the keybinds
            writer.WriteStartObject();

            writer.WritePropertyName("move_right");
            writer.WriteValue(cMoveRight);

            writer.WritePropertyName("move_left");
            writer.WriteValue(cMoveLeft);

            writer.WritePropertyName("jump");
            writer.WriteValue(cJump);

            writer.WritePropertyName("show_inventory");
            writer.WriteValue(cShowInv);

            writer.WritePropertyName("toggle_debug_menu");
            writer.WriteValue(cToggleDebug);

            writer.WritePropertyName("show_notification_list");
            writer.WriteValue(cNotifications);

            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        public static void Load(JsonToken fileToken, bool overwriteResolution)
        {
            //Get the settings from the JSON file
            try
            {
                JsonToken settingsToken = fileToken.GetToken("/settings");
                JsonToken keybindsToken = fileToken.GetToken("/keybinds");

                saveLogOnExit = settingsToken.GetBool("/save_log_on_exit");
                saveWorldOnClose = settingsToken.GetBool("/save_world_on_close");
                enableHammer = settingsToken.GetBool("/enable_hammer");
                enableCaveGeneration = settingsToken.GetBool("/enable_cave_generation");
                enableLighting = settingsToken.GetBool("/enable_lighting");
                enableHealth = settingsToken.GetBool("/enable_health");
                showNotifications = settingsToken.GetBool("/show_notifications");
                texturepack = settingsToken.GetString("/texturepack");
                resolution = settingsToken.GetString("/resolution");
                videoMode = settingsToken.GetString("/video_mode");
                if (overwriteResolution)
                {
                    customResX = settingsToken.GetInt("/custom_res_x");
                    customResY = settingsToken.GetInt("/custom_res_y");
                }

                cMoveRight = KeyConverter.StringToKey(keybindsToken.GetString("/move_right"));
                cMoveLeft = KeyConverter.StringToKey(keybindsToken.GetString("/move_left"));
                cJump = KeyConverter.StringToKey(keybindsToken.GetString("/jump"));
                cShowInv = KeyConverter.StringToKey(keybindsToken.GetString("/show_inventory"));
                cToggleDebug = KeyConverter.StringToKey(keybindsToken.GetString("/toggle_debug_menu"));
                cNotifications = KeyConverter.StringToKey(keybindsToken.GetString("/show_notification_list"));
            }
            catch (Exception ex)
            {
                Console.Write($"Error loading settings from json: {ex.Message}");
            }
        }
    }
}
