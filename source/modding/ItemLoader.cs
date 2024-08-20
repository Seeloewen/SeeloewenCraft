using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SeeloewenCraft
{

    //Warning: Currently not maintained and most likely not compatible/working

    class ItemLoader
    {

        public static Dictionary<string, ItemType> itemTypes;

        public static void Load(string[] modFolders)
        {
            itemTypes = new Dictionary<string, ItemType>();

            foreach (string modFolder in modFolders)
            {
                JsonToken rootToken = JsonUtil.ReadFile($"{modFolder}\\item_types.json").GetToken("/item_types");

                for (int i = 0; i < rootToken.GetArrayLength(); i++)
                {
                    JsonToken token = rootToken.GetToken($"/{i}");
                    itemTypes.Add(token.GetString("/type"), new ItemType(token, modFolder));
                }
            }
        }
    }
    public class ItemType
    {

        public ImageBrush texture;

        public ItemType(JsonToken token, string pathModFolder)
        {
            Uri imageUri = new Uri($"{pathModFolder}\\item_textures\\{token.GetString($"/texture")}", UriKind.Absolute);
            BitmapFrame source = BitmapFrame.Create(imageUri);
            texture = new ImageBrush { ImageSource = source };
        }
    }



    public class ModdedItem : Item
    {
        public string type;

        public ModdedItem(string type, Block block) : base()
        {
            this.type = type;
            name = "Modded Item";
            id = "sc:modded_item";
            SetTexture();
        }

        /*public override Block GenerateBlock(bool isInBackground)
        {
            return new ModdedBlock(type,  isInBackground);
        }*/

        public override void RightClickAction(Block block, object sender)
        {
            throw new NotImplementedException();
        }

        public override void SetTexture()
        {
            image = ItemLoader.itemTypes[type].texture;
        }
    }


}
