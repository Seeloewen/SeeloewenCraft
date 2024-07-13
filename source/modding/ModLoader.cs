using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace SeeloewenCraft
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
