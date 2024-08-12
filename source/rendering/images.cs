using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Documents;

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

        public static ImageSource GetImageSource(string resourceName)
        {
            Uri imageUri;

            //Try to get the image file if a texturepack is loaded
            imageUri = GetTexturepackUri(resourceName);

            //If no texturepack is loaded or it's not found in the texturepack, fallback to default ones
            if (imageUri == null)
            {
                imageUri = GetInternalUri(resourceName);
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

        public static Uri GetTexturepackUri(string resourceName)
        {
            //Go through all possible folders in the texturepack and search for the image
            if (File.Exists($"{textureDirectory}/{resourceName}")) return new Uri($"{textureDirectory}/{resourceName}", UriKind.Absolute);
            else if (File.Exists($"{textureDirectory}/Blocks/{resourceName}")) return new Uri($"{textureDirectory}/Blocks/{resourceName}", UriKind.Absolute);
            else if (File.Exists($"{textureDirectory}/Blocks/Chiseled/{resourceName}")) return new Uri($"{textureDirectory}/Blocks/Chiseled/{resourceName}", UriKind.Absolute);
            else if (File.Exists($"{textureDirectory}/Blocks/Connected/{resourceName}")) return new Uri($"{textureDirectory}/Blocks/Connected/{resourceName}", UriKind.Absolute);
            else if (File.Exists($"{textureDirectory}/Item/{resourceName}")) return new Uri($"{textureDirectory}/Items/{resourceName}", UriKind.Absolute);
            else if (File.Exists($"{textureDirectory}/Gui/{resourceName}")) return new Uri($"{textureDirectory}/Gui/{resourceName}", UriKind.Absolute);
            else if (File.Exists($"{textureDirectory}/Entities/{resourceName}")) return new Uri($"{textureDirectory}/Entities/{resourceName}", UriKind.Absolute);

            return null;
        }

        public static Uri GetInternalUri(string resourceName)
        {
            //Go through all possible folders internally and search for the image
            Uri imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/Chiseled/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/Connected/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Items/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Gui/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Entities/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            //If default one doesn't exist, use missing texture image
            return new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Missing_Texture.png", UriKind.Absolute);

        }

        private static void CreateResources()
        {
            Background = new ImageBrush { ImageSource = GetImageSource("Background.png") };
            GrassBlock = new ImageBrush { ImageSource = GetImageSource("Grass_Block.png") };
            StoneBlock = new ImageBrush { ImageSource = GetImageSource("Stone_Block.png") };
            Dirt = new ImageBrush { ImageSource = GetImageSource("Dirt.png") };
            Air = new ImageBrush { ImageSource = GetImageSource("Air.png") };
            Bedrock = new ImageBrush { ImageSource = GetImageSource("Bedrock.png") };
            CoalOre = new ImageBrush { ImageSource = GetImageSource("Coal_Ore.png") };
            DiamondOre = new ImageBrush { ImageSource = GetImageSource("Diamond_Ore.png") };
            IronOre = new ImageBrush { ImageSource = GetImageSource("Iron_Ore.png") };
            OakLog = new ImageBrush { ImageSource = GetImageSource("Oak_Log.png") };
            OakLeaves = new ImageBrush { ImageSource = GetImageSource("Oak_Leaves.png") };
            Chest = new ImageBrush { ImageSource = GetImageSource("Chest.png") };
            MagmaBlock = new ImageBrush { ImageSource = GetImageSource("Magma_Block.png") };
            MissingTexture = new ImageBrush { ImageSource = GetImageSource("Missing_Texture.png") };
            Stone_Hammer = new ImageBrush { ImageSource = GetImageSource("Stone_Hammer.png") };
            SpruceLog = new ImageBrush { ImageSource = GetImageSource("Spruce_Log.png") };
            SpruceLeaves = new ImageBrush { ImageSource = GetImageSource("Spruce_Leaves.png") };
            Torch = new ImageBrush { ImageSource = GetImageSource("Torch.png") };
            PottedCactus_Base = new ImageBrush { ImageSource = GetImageSource("Potted_Cactus_Base.png") };
            PottedCactus_Top = new ImageBrush { ImageSource = GetImageSource("Potted_Cactus_Top.png") };
            PottedCactus = new ImageBrush { ImageSource = GetImageSource("Potted_Cactus.png") };
            Water_1_Right = new ImageBrush { ImageSource = GetImageSource("Water_1_Right.png") };
            Water_1_Left = new ImageBrush { ImageSource = GetImageSource("Water_1_Left.png") };
            Water_2_Right = new ImageBrush { ImageSource = GetImageSource("Water_2_Right.png") };
            Water_2_Left = new ImageBrush { ImageSource = GetImageSource("Water_2_Left.png") };
            Water_3_Right = new ImageBrush { ImageSource = GetImageSource("Water_3_Right.png") };
            Water_3_Left = new ImageBrush { ImageSource = GetImageSource("Water_3_Left.png") };
            Water_4_Right = new ImageBrush { ImageSource = GetImageSource("Water_4_Right.png") };
            Water_4_Left = new ImageBrush { ImageSource = GetImageSource("Water_4_Left.png") };
            Water_5_Right = new ImageBrush { ImageSource = GetImageSource("Water_5_Right.png") };
            Water_5_Left = new ImageBrush { ImageSource = GetImageSource("Water_5_Left.png") };
            Water_6 = new ImageBrush { ImageSource = GetImageSource("Water_6.png") };
            Gui = new ImageBrush { ImageSource = GetImageSource("Gui.png") };
            Heart_Full = new ImageBrush { ImageSource = GetImageSource("Heart_Full.png") };
            Heart_Half = new ImageBrush { ImageSource = GetImageSource("Heart_Half.png") };
            Heart_Empty = new ImageBrush { ImageSource = GetImageSource("Heart_Empty.png") };
            CraftingTable = new ImageBrush { ImageSource = GetImageSource("Crafting_Table.png") };
            CobbleStoneBlock = new ImageBrush { ImageSource = GetImageSource("Cobblestone.png") };
            CobbleStoneBlock_BottomRight = new ImageBrush { ImageSource = GetImageSource("Cobblestone_BottomRight.png") };
            CobbleStoneBlock_BottomLeft = new ImageBrush { ImageSource = GetImageSource("Cobblestone_BottomLeft.png") };
            CobbleStoneBlock_TopRight = new ImageBrush { ImageSource = GetImageSource("Cobblestone_TopRight.png") };
            CobbleStoneBlock_TopLeft = new ImageBrush { ImageSource = GetImageSource("Cobblestone_TopLeft.png") };
            CobbleStoneBlock_SlabBottom = new ImageBrush { ImageSource = GetImageSource("Cobblestone_SlabBottom.png") };
            CobbleStoneBlock_SlabTop = new ImageBrush { ImageSource = GetImageSource("Cobblestone_SlabTop.png") };
            CobbleStoneBlock_SlabLeft = new ImageBrush { ImageSource = GetImageSource("Cobblestone_SlabLeft.png") };
            CobbleStoneBlock_SlabRight = new ImageBrush { ImageSource = GetImageSource("Cobblestone_SlabRight.png") };
            CobbleStoneBlock_StairBottomRight = new ImageBrush { ImageSource = GetImageSource("Cobblestone_StairBottomRight.png") };
            CobbleStoneBlock_StairBottomLeft = new ImageBrush { ImageSource = GetImageSource("Cobblestone_StairBottomLeft.png") };
            CobbleStoneBlock_StairTopRight = new ImageBrush { ImageSource = GetImageSource("Cobblestone_StairTopRight.png") };
            CobbleStoneBlock_StairTopLeft = new ImageBrush { ImageSource = GetImageSource("Cobblestone_StairTopLeft.png") };
            CobbleStoneBlock_Center = new ImageBrush { ImageSource = GetImageSource("Cobblestone_Center.png") };
            Chiseler = new ImageBrush { ImageSource = GetImageSource("Chiseler.png") };
            Unchiseler = new ImageBrush { ImageSource = GetImageSource("Unchiseler.png") };
            Break_1 = new ImageBrush { ImageSource = GetImageSource("Break_1.png") };
            Break_2 = new ImageBrush { ImageSource = GetImageSource("Break_2.png") };
            Break_3 = new ImageBrush { ImageSource = GetImageSource("Break_3.png") };
            Break_4 = new ImageBrush { ImageSource = GetImageSource("Break_4.png") };
            Break_5 = new ImageBrush { ImageSource = GetImageSource("Break_5.png") };
            Slime_Green = new ImageBrush { ImageSource = GetImageSource("Slime_Green.png") };
            Slime_Blue = new ImageBrush { ImageSource = GetImageSource("Slime_Blue.png") };
            Slime_Red = new ImageBrush { ImageSource = GetImageSource("Slime_Red.png") };
            Slime_Magenta = new ImageBrush { ImageSource = GetImageSource("Slime_Magenta.png") };
            Slime_Yellow = new ImageBrush { ImageSource = GetImageSource("Slime_Yellow.png") };
            Hammer_1 = new ImageBrush { ImageSource = GetImageSource("Hammer_1.png") };
            Hammer_2 = new ImageBrush { ImageSource = GetImageSource("Hammer_2.png") };
            Hammer_3 = new ImageBrush { ImageSource = GetImageSource("Hammer_3.png") };
            Hammer_4 = new ImageBrush { ImageSource = GetImageSource("Hammer_4.png") };
            Hammer_5 = new ImageBrush { ImageSource = GetImageSource("Hammer_5.png") };
            Bone = new ImageBrush { ImageSource = GetImageSource("Bone.png") };
            Arrow = new ImageBrush { ImageSource = GetImageSource("Arrow.png") };
            Snowball = new ImageBrush { ImageSource = GetImageSource("Snowball.png") };
        }

        //-- Images --//

        public static ImageBrush Background;
        public static ImageBrush GrassBlock;
        public static ImageBrush StoneBlock;
        public static ImageBrush Dirt;
        public static ImageBrush Air;
        public static ImageBrush Bedrock;
        public static ImageBrush CoalOre;
        public static ImageBrush DiamondOre;
        public static ImageBrush IronOre;
        public static ImageBrush OakLog;
        public static ImageBrush OakLeaves;
        public static ImageBrush Chest;
        public static ImageBrush MagmaBlock;
        public static ImageBrush MissingTexture;
        public static ImageBrush Stone_Hammer;
        public static ImageBrush SpruceLog;
        public static ImageBrush SpruceLeaves;
        public static ImageBrush Torch;
        public static ImageBrush PottedCactus_Top;
        public static ImageBrush PottedCactus_Base;
        public static ImageBrush PottedCactus;
        public static ImageBrush Water_1_Right;
        public static ImageBrush Water_1_Left;
        public static ImageBrush Water_2_Right;
        public static ImageBrush Water_2_Left;
        public static ImageBrush Water_3_Right;
        public static ImageBrush Water_3_Left;
        public static ImageBrush Water_4_Right;
        public static ImageBrush Water_4_Left;
        public static ImageBrush Water_5_Right;
        public static ImageBrush Water_5_Left;
        public static ImageBrush Water_6;
        public static ImageBrush Gui;
        public static ImageBrush Heart_Full;
        public static ImageBrush Heart_Half;
        public static ImageBrush Heart_Empty;
        public static ImageBrush CraftingTable;
        public static ImageBrush CobbleStoneBlock;
        public static ImageBrush CobbleStoneBlock_BottomRight;
        public static ImageBrush CobbleStoneBlock_BottomLeft;
        public static ImageBrush CobbleStoneBlock_TopRight;
        public static ImageBrush CobbleStoneBlock_TopLeft;
        public static ImageBrush CobbleStoneBlock_SlabBottom;
        public static ImageBrush CobbleStoneBlock_SlabTop;
        public static ImageBrush CobbleStoneBlock_SlabLeft;
        public static ImageBrush CobbleStoneBlock_SlabRight;
        public static ImageBrush CobbleStoneBlock_StairBottomRight;
        public static ImageBrush CobbleStoneBlock_StairBottomLeft;
        public static ImageBrush CobbleStoneBlock_StairTopRight;
        public static ImageBrush CobbleStoneBlock_StairTopLeft;
        public static ImageBrush CobbleStoneBlock_Center;
        public static ImageBrush Chiseler;
        public static ImageBrush Unchiseler;
        public static ImageBrush Break_1; 
        public static ImageBrush Break_2;
        public static ImageBrush Break_3;
        public static ImageBrush Break_4;
        public static ImageBrush Break_5;
        public static ImageBrush Slime_Green;
        public static ImageBrush Slime_Blue;
        public static ImageBrush Slime_Magenta;
        public static ImageBrush Slime_Yellow;
        public static ImageBrush Slime_Red;
        public static ImageBrush Hammer_1;
        public static ImageBrush Hammer_2;
        public static ImageBrush Hammer_3;
        public static ImageBrush Hammer_4;
        public static ImageBrush Hammer_5;
        public static ImageBrush Bone;
        public static ImageBrush Arrow;
        public static ImageBrush Snowball;
    }
}
