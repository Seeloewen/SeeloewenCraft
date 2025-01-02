using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;

namespace SeeloewenCraft
{
    internal class SplashTextHandler
    {
        wndMenu wndMenu;
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
            string splashText = texts[Game.rnd.Next(texts.Length)];
            Log.Write($"Rolled splash text {splashText}", LogType.GENERAL, LogLevel.INFO);
            return splashText;
        }


    }
}
