using System;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public static class Settings
    {
        //Settings
        public static bool saveLogOnExit = false;
        public static bool saveWorldOnClose = true;
        public static bool showNotifications = true;
        public static bool enableMobs = false;
        public static bool enableAutoSave = true;
        public static bool showAutoSaveNotification = true;
        public static string resolution = "1280x720";
        public static string videoMode = "Windowed";
        public static int customResX = 1280;
        public static int customResY = 720;
        public static int autoSaveInterval = 10;
        public static string texturepack;

        //Log types
        public static bool logGeneral = true;
        public static bool logWorldGeneration = true;
        public static bool logStructureGeneration = false;
        public static bool logNetwork = true;
        public static bool logEntities = true;
        public static bool logRendering = true;

        //Keybinds
        public static Key cMoveRight = Key.D;
        public static Key cMoveLeft = Key.A;
        public static Key cShowInv = Key.E;
        public static Key cToggleDebug = Key.F3;
        public static Key cJump = Key.Space;
        public static Key cNotifications = Key.N;
        public static Key cThrowItem = Key.Q;
        public static Key cSneak = Key.LeftShift;
        public static Key cSprint = Key.LeftCtrl;

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

            writer.WritePropertyName("show_notifications");
            writer.WriteValue(showNotifications);

            writer.WritePropertyName("enable_mobs");
            writer.WriteValue(enableMobs);

            writer.WritePropertyName("enable_auto_save");
            writer.WriteValue(enableAutoSave);

            writer.WritePropertyName("show_auto_save_notification");
            writer.WriteValue(showAutoSaveNotification);

            writer.WritePropertyName("resolution");
            writer.WriteValue(resolution);

            writer.WritePropertyName("video_mode");
            writer.WriteValue(videoMode);

            writer.WritePropertyName("custom_res_x");
            writer.WriteValue(customResX);

            writer.WritePropertyName("custom_res_y");
            writer.WriteValue(customResY);

            writer.WritePropertyName("auto_save_interval");
            writer.WriteValue(autoSaveInterval);

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

            writer.WritePropertyName("throw_item");
            writer.WriteValue(cThrowItem);

            writer.WritePropertyName("sneak");
            writer.WriteValue(cSneak);

            writer.WritePropertyName("sprint");
            writer.WriteValue(cSprint);

            writer.WriteEndObject();

            //Save all the keybinds
            writer.WritePropertyName("log_types");

            writer.WriteStartObject();

            writer.WritePropertyName("general");
            writer.WriteValue(logGeneral);

            writer.WritePropertyName("world_generation");
            writer.WriteValue(logWorldGeneration);

            writer.WritePropertyName("structure_generation");
            writer.WriteValue(logStructureGeneration);

            writer.WritePropertyName("network");
            writer.WriteValue(logNetwork);

            writer.WritePropertyName("entities");
            writer.WriteValue(logEntities);

            writer.WritePropertyName("rendering");
            writer.WriteValue(logRendering);

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
                JsonToken logTypesToken = fileToken.GetToken("/log_types");

                saveLogOnExit = settingsToken.GetBool("/save_log_on_exit");
                saveWorldOnClose = settingsToken.GetBool("/save_world_on_close");
                enableMobs = settingsToken.GetBool("/enable_mobs");
                enableAutoSave = settingsToken.GetBool("/enable_auto_save");
                showAutoSaveNotification = settingsToken.GetBool("/show_auto_save_notification");
                showNotifications = settingsToken.GetBool("/show_notifications");
                texturepack = settingsToken.GetString("/texturepack");
                resolution = settingsToken.GetString("/resolution");
                videoMode = settingsToken.GetString("/video_mode");
                if (overwriteResolution)
                {
                    customResX = settingsToken.GetInt("/custom_res_x");
                    customResY = settingsToken.GetInt("/custom_res_y");
                }
                autoSaveInterval = settingsToken.GetInt("/auto_save_interval");

                logGeneral = logTypesToken.GetBool("/general");
                logWorldGeneration = logTypesToken.GetBool("/world_generation");
                logStructureGeneration = logTypesToken.GetBool("/structure_generation");
                logNetwork = logTypesToken.GetBool("/network");
                logEntities = logTypesToken.GetBool("/entities");
                logRendering = logTypesToken.GetBool("/rendering");

                cMoveRight = KeyConverter.StringToKey(keybindsToken.GetString("/move_right"));
                cMoveLeft = KeyConverter.StringToKey(keybindsToken.GetString("/move_left"));
                cJump = KeyConverter.StringToKey(keybindsToken.GetString("/jump"));
                cShowInv = KeyConverter.StringToKey(keybindsToken.GetString("/show_inventory"));
                cToggleDebug = KeyConverter.StringToKey(keybindsToken.GetString("/toggle_debug_menu"));
                cNotifications = KeyConverter.StringToKey(keybindsToken.GetString("/show_notification_list"));
                cThrowItem = KeyConverter.StringToKey(keybindsToken.GetString("/throw_item"));
                cSneak = KeyConverter.StringToKey(keybindsToken.GetString("/sneak"));
                cSprint = KeyConverter.StringToKey(keybindsToken.GetString("/sprint"));

            }
            catch (Exception ex)
            {
                Console.Write($"Error loading settings from json: {ex.Message}");
            }
        }
    }
}
