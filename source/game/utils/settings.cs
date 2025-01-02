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
        public static string nickname;

        //Log types
        public static bool logGeneral = true;
        public static bool logWorldGeneration = true;
        public static bool logStructureGeneration = false;
        public static bool logNetwork = true;
        public static bool logEntities = true;
        public static bool logRendering = true;

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

            writer.WritePropertyName("nickname");
            writer.WriteValue(nickname);

            writer.WriteEndObject();

            writer.WritePropertyName("keybinds");

            //Save all the keybinds
            KeyBinds.Save(writer);

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
            nickname = Game.playerId.ToString(); //Default value, will be overwritten if a name is saved

            //Get the settings from the JSON file
            try
            {
                JsonToken settingsToken = fileToken.GetToken("/settings");
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
                nickname = settingsToken.GetString("/nickname");

                logGeneral = logTypesToken.GetBool("/general");
                logWorldGeneration = logTypesToken.GetBool("/world_generation");
                logStructureGeneration = logTypesToken.GetBool("/structure_generation");
                logNetwork = logTypesToken.GetBool("/network");
                logEntities = logTypesToken.GetBool("/entities");
                logRendering = logTypesToken.GetBool("/rendering");


                KeyBinds.Load(fileToken);

            }
            catch (Exception ex)
            {
                Console.Write($"Error loading settings from json: {ex.Message}");
            }
        }
    }
}
