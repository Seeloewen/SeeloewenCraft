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
        wndMenu wndMenu;
        Random rnd = new Random(DateTime.Now.Millisecond);
        string[] texts;

        public SplashTextHandler(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;

            //Read all texts from file
            Uri uri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/splash_texts.txt", UriKind.Absolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using StreamReader reader = new(info.Stream);
            string text = reader.ReadToEnd();
            texts = text.Split(';');
        }

        public string GetText()
        {
            string splashText = texts[rnd.Next(texts.Length)];
            wndMenu.log.Write($"Rolled splash text {splashText}", "Info");
            return splashText;
        }


    }
}
