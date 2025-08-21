using System.IO;
using static System.Environment;

namespace SeeloewenCraft.game.core.legacy
{
    class ModLoader
    {

        public static void LoadMods()
        {
            string[] modFolders = Directory.GetDirectories($"{GetFolderPath(SpecialFolder.ApplicationData)}\\SeeloewenCraft\\mods");
            BlockLoader.Load(modFolders);
            ItemLoader.Load(modFolders);


        }





    }
}
