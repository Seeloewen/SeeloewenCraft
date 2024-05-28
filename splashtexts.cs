using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace SeeloewenCraft
{
    internal class SplashTextHandler
    {

        Random rnd = new Random(DateTime.Now.Millisecond);
        string[] texts;

        public SplashTextHandler()
        {
            Uri uri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/splash_texts.txt", UriKind.Absolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);


            using StreamReader reader = new(info.Stream);

            // Read the stream as a string.
            string text = reader.ReadToEnd();

            //string text = File.ReadAllText("/Resources/Splashtexts.txt");
            texts = text.Split(';');
        }

        public string GetText()
        {
            return texts[rnd.Next(texts.Length)];
        }


    }
}
