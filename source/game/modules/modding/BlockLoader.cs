using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.util;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeeloewenCraft.game.core.legacy
{
    //Warning: Currently not maintained and most likely not compatible/working

    class BlockLoader
    {

        public static Dictionary<string, BlockType> blockTypes;

        public static void Load(string[] modFolders)
        {
            blockTypes = new Dictionary<string, BlockType>();

            foreach (string modFolder in modFolders)
            {
                JsonToken rootToken = JsonUtil.ReadFile($"{modFolder}\\block_types.json").GetToken("/block_types");

                for (int i = 0; i < rootToken.GetArrayLength(); i++)
                {
                    JsonToken token = rootToken.GetToken($"/{i}");
                    blockTypes.Add(token.GetString("/type"), new BlockType(token, modFolder));
                }
            }
        }


    }

    public class BlockType
    {

        public ImageBrush texture;

        public BlockType(JsonToken token, string pathModFolder)
        {
            Uri imageUri = new Uri($"{pathModFolder}\\block_textures\\{token.GetString($"/texture")}", UriKind.Absolute);
            BitmapFrame source = BitmapFrame.Create(imageUri);
            texture = new ImageBrush { ImageSource = source };
        }

    }

    class ModdedBlock : Block //Warning: Legacy code that has been outdated for ages
    {
        string type;

        public ModdedBlock() : base("Modded Block", "sc:modded_block", 500, null, Tool.None)
        {
            //this.type = type;
        }

        public override void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(id);

            writer.WritePropertyName("pos_x");
            writer.WriteValue(xPos);

            writer.WritePropertyName("pos_y");
            writer.WriteValue(yPos);

            writer.WritePropertyName("is_in_background");
            writer.WriteValue(isBackground);

            writer.WritePropertyName("type");
            writer.WriteValue(type);

            /*if (hasInventory)
            {
                writer.WritePropertyName("inventory");
                blockInventory.SaveToJson(writer);
            }*/

            writer.WriteEndObject();
        }


        /*public override void GenerateItem()
        {
            item = new ModdedItem(type,  this);
        }*/

        public override void RightClickAction()
        {
            throw new NotImplementedException();
        }

        public override void AddDebugMenu()
        {
            throw new NotImplementedException();
        }



    }


}
