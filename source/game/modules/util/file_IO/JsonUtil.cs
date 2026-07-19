using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.util.logging;
using System;
using System.IO;

namespace SeeloewenCraft.game.util
{
    public static class JsonUtil
    {
        public static T Get<T>(this JToken obj, string key)
        {
            try
            {
                JToken valueToken = obj[key];
                if (valueToken == null) throw new Exception($"Could not find key {key} in JObject");

                if (typeof(T) == typeof(JObject) || typeof(T) == typeof(JArray))
                {
                    return valueToken.ToObject<T>();
                }
                else
                {
                    return valueToken.Value<T>();
                }

            }
            catch (Exception e)
            {
                Log.Write($"Error while handling JToken: {e.Message}\n{e.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
            }

            return default;
        }

        public static void ToFile(this JToken obj, string path)
        {
            File.WriteAllText(path, obj.ToString());
        }

        public static JObject ObjectFromFile(string path)
        {
            return JObject.Parse(File.ReadAllText(path));
        }

        public static JArray ArrayFromFile(string path)
        {
            return JArray.Parse(File.ReadAllText(path));
        }
    }
}