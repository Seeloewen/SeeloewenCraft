using Newtonsoft.Json.Linq;
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

        public static JObject Save()
        {
            //Construct settings object from different setting types and sources
            JArray texturepackArr = new JArray();
            texturepacks.ForEach(t => texturepackArr.Add(t));

            JObject settingsObj = new JObject()
            {
                {"save_log_on_exit", saveLogOnExit },
                {"save_world_on_close", saveWorldOnClose },
                {"show_notifications", showNotifications },
                {"enable_mobs", enableMobs },
                {"enable_auto_save", enableAutoSave },
                {"show_auto_save_notification", showAutoSaveNotification },
                {"auto_save_interval", autoSaveInterval },
                {"nickname", nickname },
            };

            JObject logSettingsObj = new JObject()
            {
                { "general", logGeneral},
                { "world_generation", logWorldGeneration },
                { "structure_generation", logStructureGeneration },
                { "network", logNetwork},
                { "entities", logEntities },
                { "rendering", logRendering }
            };

            JObject keybindsObj = KeyBinds.ToJson();

            JObject globalSettingsObj = new JObject()
            {
                {"general_settings", settingsObj },
                {"log_settings", logSettingsObj },
                {"keybinds", keybindsObj },
                {"texturepacks", texturepackArr }
            };

            return globalSettingsObj;
        }

        public static void Load(JObject obj)
        {
            JObject settingsObj = obj.Get<JObject>("general_settings");
            JObject logSettingsObj = obj.Get<JObject>("log_settings");
            JArray texturepacksArray = obj.Get<JArray>("texturepacks");
            JObject keybindsObj = obj.Get<JObject>("keybinds");

            //Get the settings from the JSON file
            try
            {
                //General settings
                saveLogOnExit = settingsObj.Get<bool>("save_log_on_exit");
                saveWorldOnClose = settingsObj.Get<bool>("save_world_on_close");
                showNotifications = settingsObj.Get<bool>("show_notifications");
                enableMobs = settingsObj.Get<bool>("enable_mobs");
                enableAutoSave = settingsObj.Get<bool>("enable_auto_save");
                showAutoSaveNotification = settingsObj.Get<bool>("show_auto_save_notification");
                autoSaveInterval = settingsObj.Get<int>("auto_save_interval");
                nickname = settingsObj.Get<string>("nickname");

                //Log settings
                logGeneral = logSettingsObj.Get<bool>("general");
                logWorldGeneration = logSettingsObj.Get<bool>("world_generation");
                logStructureGeneration = logSettingsObj.Get<bool>("structure_generation");
                logNetwork = logSettingsObj.Get<bool>("network");
                logEntities = logSettingsObj.Get<bool>("entities");
                logRendering = logSettingsObj.Get<bool>("rendering");

                //Keybinds
                KeyBinds.FromJson(keybindsObj);

                //Texture packs
                texturepacks.Clear();
                foreach (JToken tp in texturepacksArray)
                {
                    string texturepackPath = tp.ToString();

                    if (!Directory.Exists(texturepackPath)) //If the texturepack doesn't exist, remove it from the list
                    {
                        Log.Write($"Could not load texturepack {texturepackPath} as it doesn't seem to exist", LogType.RENDERING, LogLevel.ERROR);
                        continue;
                    }

                    texturepacks.Add(texturepackPath);
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error loading settings from json: {ex.Message}");
            }

            nickname ??= Game.playerId.ToString();
        }
    }
}
