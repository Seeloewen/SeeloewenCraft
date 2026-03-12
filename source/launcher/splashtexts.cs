using SeeloewenCraft.game;
using SeeloewenCraft.game.util.logging;
using System.IO;
using System.Reflection;

namespace SeeloewenCraft.launcher
{
    internal class SplashTextHandler
    {
        string[] texts;

        public SplashTextHandler()
        {
            //Read all texts from file TODO: Replace with file io util method
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
