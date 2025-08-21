using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SeeloewenCraft.game.util
{
    public class JsonWriter : Newtonsoft.Json.JsonTextWriter
    {
        StringBuilder sb;

        private JsonWriter(StringBuilder sb, StringWriter sw) : base(sw)
        {
            this.sb = sb;
            Formatting = Newtonsoft.Json.Formatting.Indented;
        }

        public string ToString()
        {
            return sb.ToString();
        }

        public void WriteToFile(string path)
        {
            File.WriteAllText(path, sb.ToString());
        }

        public static JsonWriter Create()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            return new JsonWriter(sb, sw);
        }

        public override void Flush()
        {
            base.Flush();
        }
    }


    public class JsonUtil
    {
        public static JsonToken ReadFile(string path)
        {
            try
            {
                string text = File.ReadAllText(path);
                JToken token = JToken.Parse(text);
                return new JsonToken(token);
            }
            catch (Exception ex)
            {
                throw new JsonUtilException("reading file to json token failed", ex);
            }
        }

        public static JsonToken ReadString(string content)
        {
            try
            {
                JToken token = JToken.Parse(content);
                return new JsonToken(token);
            }
            catch (Exception ex)
            {
                throw new JsonUtilException("reading string to json token failed", ex);
            }
        }

        public static JsonToken ReadResource(string name)
        {
            try
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.{name}");
                using StreamReader reader = new(stream);
                return new JsonToken(JToken.Parse(reader.ReadToEnd()));
            }
            catch (Exception ex)
            {
                throw new JsonUtilException("reading resource file to json token failed", ex);
            }
        }

    }

    public class JsonToken
    {
        public JToken value;

        public JsonToken(JToken value)
        {
            this.value = value;
        }

        public int GetArrayLength()
        {
            try
            {
                return ((JArray)value).Count;
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed getting array length of token", e);
            }
        }

        public bool ContainsKey(string address)
        {
            try
            {
                JObject objectToken = (JObject)value;
                return objectToken.ContainsKey(address);
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed checking if key exists", e);
            }
        }


        public JsonToken GetToken(string address)
        {
            try
            {
                return new JsonToken(new JsonPointer(address).Evaluate(value));
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed getting json token from json token", e);
            }
        }

        public int GetInt(string address)
        {
            try
            {
                return (int)new JsonPointer(address).Evaluate(value);
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed getting int from json token", e);
            }
        }

        public bool GetBool(string address)
        {
            try
            {
                return (bool)new JsonPointer(address).Evaluate(value);
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed getting bool from json token", e);
            }
        }

        public string GetString(string address)
        {
            try
            {
                return (string)new JsonPointer(address).Evaluate(value);
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed getting string from json token", e);
            }
        }

        public double GetDouble(string address)
        {
            try
            {
                return (double)new JsonPointer(address).Evaluate(value);
            }
            catch (Exception e)
            {
                throw new JsonUtilException("failed getting double from json token", e);
            }
        }

    }

}