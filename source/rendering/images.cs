using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace SeeloewenCraft
{
    public static class Images //This class contains all the references to the image resources
    {
        //-- Custom Methods --//

        public static string textureDirectory;

        public static void Init(World world)
        {
            //Check which texurepack is selected and set the directory based on that
            if (world.wndMenu.selectedTexturepack == "default")
            {
                textureDirectory = "pack://application:,,,/SeeloewenCraft;component/Resources";
                Log.Write($"Set texture directory to 'pack://application:,,,/SeeloewenCraft;component/Resources' (internal resources)", "Info");
            }
            else
            {
                textureDirectory = world.wndMenu.selectedTexturepack;
                Log.Write($"Set texture directory to '{world.wndMenu.selectedTexturepack}'", "Info");
            }

            //Actually load the resources
            CreateResources();
        }

        public static ImageSource GetImageSource(string resourceName, TextureType type)
        {
            Uri imageUri;

            //Try to get the image file if a texturepack is loaded
            imageUri = GetTexturepackUri(resourceName, type);

            //If no texturepack is loaded or it's not found in the texturepack, fallback to default ones
            if (imageUri == null)
            {
                imageUri = GetInternalUri(resourceName, type);
            }

            //Get an image from an imagesource from a uri
            return BitmapFrame.Create(imageUri);
        }

        public static bool ResourceExists(Uri resourceUri)
        {
            //Check if the resource exists
            try
            {
                var resourceInfo = Application.GetResourceStream(resourceUri);
                return resourceInfo != null;
            }
            catch
            {
                return false;
            }
        }

        public static Uri GetTexturepackUri(string resourceName, TextureType type)
        {
            Uri imageUri;

            //Get the uri from texturepack
            switch(type)
            {
                case TextureType.Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Item:
                    imageUri = new Uri($"{textureDirectory}/Items/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Gui:
                    imageUri = new Uri($"{textureDirectory}/Gui/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Entity:
                    imageUri = new Uri($"{textureDirectory}/Entities/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Chiseled_Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/Chiseled/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Connected_Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/Connected/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.General:
                    imageUri = new Uri($"{textureDirectory}/{resourceName}", UriKind.Absolute);
                    break;
                default:
                    imageUri = new Uri($"{textureDirectory}/{resourceName}", UriKind.Absolute);
                    break;
            }

            if(File.Exists(imageUri.LocalPath))
            {
                return imageUri;
            }
            else
            {
                return null;
            }
        }

        public static Uri GetInternalUri(string resourceName, TextureType type)
        {
            Uri imageUri;

            //Get the internal resource
            switch (type)
            {
                case TextureType.Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Item:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Items/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Gui:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Gui/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Entity:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Entities/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Chiseled_Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/Chiseled/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Connected_Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/Connected/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.General:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/{resourceName}", UriKind.Absolute);
                    break;
                default:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/{resourceName}", UriKind.Absolute);
                    break;
            }

            //Check if the resource exists and return it
            if (ResourceExists(imageUri))
            {
                return imageUri;
            }
            else
            {
                //If default one doesn't exist, use missing texture image
                return new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Missing_Texture.png", UriKind.Absolute);
            }
        }

        private static void CreateResources()
        {
            Background = new SealImage(TextureType.Gui, "Background.png");
            GrassBlock = new SealImage(TextureType.Block, "Grass_Block.png");
            StoneBlock = new SealImage(TextureType.Block, "Stone_Block.png");
            Dirt = new SealImage(TextureType.Block, "Dirt.png");
            Air = new SealImage(TextureType.Block, "Air.png");
            Bedrock = new SealImage(TextureType.Block, "Bedrock.png");
            CoalOre = new SealImage(TextureType.Block, "Coal_Ore.png");
            DiamondOre = new SealImage(TextureType.Block, "Diamond_Ore.png");
            IronOre = new SealImage(TextureType.Block, "Iron_Ore.png");
            OakLog = new SealImage(TextureType.Block, "Oak_Log.png");
            OakLeaves = new SealImage(TextureType.Block, "Oak_Leaves.png");
            Chest = new SealImage(TextureType.Block, "Chest.png");
            MagmaBlock = new SealImage(TextureType.Block, "Magma_Block.png");
            MissingTexture = new SealImage(TextureType.General, "Missing_Texture.png");
            StoneHammer = new SealImage(TextureType.Item, "Stone_Hammer.png");
            SpruceLog = new SealImage(TextureType.Block, "Spruce_Log.png");
            SpruceLeaves = new SealImage(TextureType.Block, "Spruce_Leaves.png");
            Torch = new SealImage(TextureType.Block, "Torch.png");
            PottedCactus_Base = new SealImage(TextureType.Connected_Block, "Potted_Cactus_Base.png");
            PottedCactus_Top = new SealImage(TextureType.Connected_Block, "Potted_Cactus_Top.png");
            PottedCactus = new SealImage(TextureType.Connected_Block, "Potted_Cactus.png");
            Water_1_Right = new SealImage(TextureType.Block, "Water_1_Right.png");
            Water_1_Left = new SealImage(TextureType.Block, "Water_1_Left.png");
            Water_2_Right = new SealImage(TextureType.Block, "Water_2_Right.png");
            Water_2_Left = new SealImage(TextureType.Block, "Water_2_Left.png");
            Water_3_Right = new SealImage(TextureType.Block, "Water_3_Right.png");
            Water_3_Left = new SealImage(TextureType.Block, "Water_3_Left.png");
            Water_4_Right = new SealImage(TextureType.Block, "Water_4_Right.png");
            Water_4_Left = new SealImage(TextureType.Block, "Water_4_Left.png");
            Water_5_Right = new SealImage(TextureType.Block, "Water_5_Right.png");
            Water_5_Left = new SealImage(TextureType.Block, "Water_5_Left.png");
            Water_6 = new SealImage(TextureType.Block, "Water_6.png");
            Gui = new SealImage(TextureType.Gui, "Gui.png");
            Heart_Full = new SealImage(TextureType.Gui, "Heart_Full.png");
            Heart_Half = new SealImage(TextureType.Gui, "Heart_Half.png");
            Heart_Empty = new SealImage(TextureType.Gui, "Heart_Empty.png");
            CraftingTable = new SealImage(TextureType.Block, "Crafting_Table.png");
            CobbleStoneBlock = new SealImage(TextureType.Block, "Cobblestone.png");
            CobbleStoneBlock_BottomRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_BottomRight.png");
            CobbleStoneBlock_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_BottomLeft.png");
            CobbleStoneBlock_TopRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_TopRight.png");
            CobbleStoneBlock_TopLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_TopLeft.png");
            CobbleStoneBlock_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabBottom.png");
            CobbleStoneBlock_SlabTop = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabTop.png");
            CobbleStoneBlock_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabLeft.png");
            CobbleStoneBlock_SlabRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabRight.png");
            CobbleStoneBlock_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairBottomRight.png");
            CobbleStoneBlock_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairBottomLeft.png");
            CobbleStoneBlock_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairTopRight.png");
            CobbleStoneBlock_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairTopLeft.png");
            CobbleStoneBlock_Center = new SealImage(TextureType.Chiseled_Block, "Cobblestone_Center.png");
            Chiseler = new SealImage(TextureType.Block, "Chiseler.png");
            Unchiseler = new SealImage(TextureType.Block, "Unchiseler.png");
            Break_1 = new SealImage(TextureType.General, "Break_1.png");
            Break_2 = new SealImage(TextureType.General, "Break_2.png");
            Break_3 = new SealImage(TextureType.General, "Break_3.png");
            Break_4 = new SealImage(TextureType.General, "Break_4.png");
            Break_5 = new SealImage(TextureType.General, "Break_5.png");
            Slime_Green = new SealImage(TextureType.Entity, "Slime_Green.png");
            Slime_Blue = new SealImage(TextureType.Entity, "Slime_Blue.png");
            Slime_Red = new SealImage(TextureType.Entity, "Slime_Red.png");
            Slime_Magenta = new SealImage(TextureType.Entity, "Slime_Magenta.png");
            Slime_Yellow = new SealImage(TextureType.Entity, "Slime_Yellow.png");
            Hammer_1 = new SealImage(TextureType.General, "Hammer_1.png");
            Hammer_2 = new SealImage(TextureType.General, "Hammer_2.png");
            Hammer_3 = new SealImage(TextureType.General, "Hammer_3.png");
            Hammer_4 = new SealImage(TextureType.General, "Hammer_4.png");
            Hammer_5 = new SealImage(TextureType.General, "Hammer_5.png");
            Bone = new SealImage(TextureType.Item, "Bone.png");
            Arrow = new SealImage(TextureType.Item, "Arrow.png");
            Snowball = new SealImage(TextureType.Item, "Snowball.png");
        }

        //-- Images --//

        public static SealImage Background;
        public static SealImage GrassBlock;
        public static SealImage StoneBlock;
        public static SealImage Dirt;
        public static SealImage Air;
        public static SealImage Bedrock;
        public static SealImage CoalOre;
        public static SealImage DiamondOre;
        public static SealImage IronOre;
        public static SealImage OakLog;
        public static SealImage OakLeaves;
        public static SealImage Chest;
        public static SealImage MagmaBlock;
        public static SealImage MissingTexture;
        public static SealImage StoneHammer;
        public static SealImage SpruceLog;
        public static SealImage SpruceLeaves;
        public static SealImage Torch;
        public static SealImage PottedCactus_Top;
        public static SealImage PottedCactus_Base;
        public static SealImage PottedCactus;
        public static SealImage Water_1_Right;
        public static SealImage Water_1_Left;
        public static SealImage Water_2_Right;
        public static SealImage Water_2_Left;
        public static SealImage Water_3_Right;
        public static SealImage Water_3_Left;
        public static SealImage Water_4_Right;
        public static SealImage Water_4_Left;
        public static SealImage Water_5_Right;
        public static SealImage Water_5_Left;
        public static SealImage Water_6;
        public static SealImage Gui;
        public static SealImage Heart_Full;
        public static SealImage Heart_Half;
        public static SealImage Heart_Empty;
        public static SealImage CraftingTable;
        public static SealImage CobbleStoneBlock;
        public static SealImage CobbleStoneBlock_BottomRight;
        public static SealImage CobbleStoneBlock_BottomLeft;
        public static SealImage CobbleStoneBlock_TopRight;
        public static SealImage CobbleStoneBlock_TopLeft;
        public static SealImage CobbleStoneBlock_SlabBottom;
        public static SealImage CobbleStoneBlock_SlabTop;
        public static SealImage CobbleStoneBlock_SlabLeft;
        public static SealImage CobbleStoneBlock_SlabRight;
        public static SealImage CobbleStoneBlock_StairBottomRight;
        public static SealImage CobbleStoneBlock_StairBottomLeft;
        public static SealImage CobbleStoneBlock_StairTopRight;
        public static SealImage CobbleStoneBlock_StairTopLeft;
        public static SealImage CobbleStoneBlock_Center;
        public static SealImage Chiseler;
        public static SealImage Unchiseler;
        public static SealImage Break_1;
        public static SealImage Break_2;
        public static SealImage Break_3;
        public static SealImage Break_4;
        public static SealImage Break_5;
        public static SealImage Slime_Green;
        public static SealImage Slime_Blue;
        public static SealImage Slime_Magenta;
        public static SealImage Slime_Yellow;
        public static SealImage Slime_Red;
        public static SealImage Hammer_1;
        public static SealImage Hammer_2;
        public static SealImage Hammer_3;
        public static SealImage Hammer_4;
        public static SealImage Hammer_5;
        public static SealImage Bone;
        public static SealImage Arrow;
        public static SealImage Snowball;
    }

    public class SealImage
    {
        public ImageBrush texture = new ImageBrush();
        public TextureType type;
        public string name;

        public SealImage(TextureType type, string name)
        {
            this.type = type;
            this.name = name;

            texture.ImageSource = Images.GetImageSource(name, type);
        }

        public ImageBrush GetTexture()
        {
            return texture;
        }
    }

    public enum TextureType
    {
        Block,
        Item,
        Gui,
        Entity,
        Chiseled_Block,
        Connected_Block,
        General
    }
}


