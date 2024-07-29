using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Windows.Resources;
using System.Windows;

namespace SeeloewenCraft
{
    public class JsonWriter : Newtonsoft.Json.JsonTextWriter
    {
        StringBuilder sb;

        private JsonWriter(StringBuilder sb, StringWriter sw) : base(sw)
        {
            this.sb = sb;
            Formatting = Newtonsoft.Json.Formatting.Indented;
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
            string text = File.ReadAllText(path);
            JToken token = JToken.Parse(text);
            return new JsonToken(token);
        }

        public static JsonToken ReadResource(string name)
        {
            //Read all texts from file
            Uri uri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/{name}", UriKind.Absolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using StreamReader reader = new(info.Stream);
            return new JsonToken(JToken.Parse(reader.ReadToEnd()));
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
            }
        }

    }

}
