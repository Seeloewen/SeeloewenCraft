using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.events;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeeloewenCraft.game.core.settings
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
        public static string nickname;

        //Log types
        public static bool logGeneral = true;
        public static bool logWorldGeneration = true;
        public static bool logStructureGeneration = false;
        public static bool logNetwork = true;
        public static bool logEntities = true;
        public static bool logRendering = true;

        public static List<string> texturepacks = new List<string>();

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

            writer.WritePropertyName("texturepacks");
            writer.WriteStartArray();
            foreach (string tp in texturepacks)
            {
                writer.WriteValue(tp);
            }
            writer.WriteEndArray();

            writer.WritePropertyName("nickname");
            writer.WriteValue(nickname);

            writer.WriteEndObject();

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

                texturepacks.Clear();
                JArray texturepackArray = settingsToken.GetToken("/texturepacks").value as JArray;
                foreach(JToken tp in texturepackArray)
                {
                    string texturepackPath = tp.ToString();

                    if (!Directory.Exists(texturepackPath)) //If the texturepack doesn't exist, remove it from the list
                    {
                        Log.Write($"Could not load texturepack {texturepackPath} as it doesn't seem to exist", LogType.RENDERING, LogLevel.ERROR);
                        continue;
                    }

                    texturepacks.Add(texturepackPath);
                }

                KeyBinds.Load(fileToken);

            }
            catch (Exception ex)
            {
                Console.Write($"Error loading settings from json: {ex.Message}");
            }
        }
    }
}
