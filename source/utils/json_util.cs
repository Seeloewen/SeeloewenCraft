using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
