using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.util
{
    internal static class Extensions
    {
        public static IList Swap<T>(this IList list, T entry1, T entry2)
        {
            //Get the indices of the items and swap the entries
            int i1 = -1;
            int i2 = -1;
            foreach (T item in list)
            {
                if (item.Equals(entry1)) i1 = list.IndexOf(item);
                if (item.Equals(entry2)) i2 = list.IndexOf(item);
            }

            T temp = (T)list[i1];
            list[i1] = list[i2];
            list[i2] = temp;

            return list;
        }

        //Should only be used when Find() is not available
        public static T Get<T>(this IList list, Predicate<T> predicate)
        {
            foreach (T item in list)
            {
                if (predicate(item)) return item;
            }

            return default;
        }

        public static async Task<string?> OpenFolderAsync(this IStorageProvider storageProvider)
        {
            var result = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Choose a folder...",
                AllowMultiple = false
            });

            return result.FirstOrDefault()?.Path.LocalPath;
        }

        public static async Task<IStorageFile> SaveFileAsync(this IStorageProvider storageProvider, string fileName = "", IReadOnlyList<FilePickerFileType> types = null)
        {
            FilePickerSaveOptions opt = new FilePickerSaveOptions()
            {
                Title = "Choose a location to save the file...",
                SuggestedFileName = fileName,
                FileTypeChoices = types
            };

            return await storageProvider.SaveFilePickerAsync(opt);
        }
    }
}
