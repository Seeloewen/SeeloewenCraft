using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static System.Environment;

namespace SeeloewenCraft
{
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

    class ModdedBlock : Block
    {

        string type;

        public ModdedBlock(string type, World world, bool isInBackground) : base(world, isInBackground)
        {
            this.type = type;
            SetTexture();
            name = "Modded Block";
            id = "sc:modded_block";
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

        public override void SetTexture()
        {
            image = BlockLoader.blockTypes[type].texture;
        }

        public override void GenerateItem(World world)
        {
            item = new ModdedItem(type, world, this);
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }



    }


}
