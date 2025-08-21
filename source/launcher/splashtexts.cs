using SeeloewenCraft.game;
using SeeloewenCraft.game.util.logging;
using System.IO;
using System.Reflection;

namespace SeeloewenCraft.launcher
{
    internal class SplashTextHandler
    {
        wndMenu wndMenu;
        string[] texts;

        public SplashTextHandler(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;

            //Read all texts from file

            string resourceName = "YourNamespace.Images.MyBitmap.bmp";

            using Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("SeeloewenCraft.Resources.splash_texts.txt");
            using StreamReader reader = new(stream);
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
