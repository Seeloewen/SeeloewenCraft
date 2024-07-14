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
            return new JsonToken(JToken.Parse(File.ReadAllText(path)));
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
            return ((JArray)value).Count;
        }

        public bool ContainsKey(string address)
        {
            JObject objectToken = (JObject)value;
            return objectToken.ContainsKey(address);
        }


        public JsonToken GetToken(string address)
        {
            return new JsonToken(new JsonPointer(address).Evaluate(value));
        }

        public int GetInt(string address)
        {
            return (int)new JsonPointer(address).Evaluate(value);
        }

        public bool GetBool(string address)
        {
            return (bool)new JsonPointer(address).Evaluate(value);
        }

        public string GetString(string address)
        {
            return (string)new JsonPointer(address).Evaluate(value);
        }

        public double GetDouble(string address)
        {
            return (double)new JsonPointer(address).Evaluate(value);
        }
    }

}
