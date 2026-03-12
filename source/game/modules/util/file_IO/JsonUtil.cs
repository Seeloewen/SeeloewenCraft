using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.util.logging;
using System;
using System.IO;
using System.Windows.Navigation;
using Windows.Perception.Spatial;

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

                return valueToken.ToObject<T>();
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
            return new JObject(File.ReadAllText(path));
        }

        public static JArray ArrayFromFile(string path)
        {
            return new JArray(File.ReadAllText(path));
        }
    }
}