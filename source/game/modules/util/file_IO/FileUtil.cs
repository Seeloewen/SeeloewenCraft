using System.IO;
using System.Reflection;


namespace SeeloewenCraft.game.util
{
    public static class FileUtil
    {
        public static string StrFromResource(string resourcePath)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"SeeloewenCraft.Resources.{resourcePath}");
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            return text;
        }
    }
}
