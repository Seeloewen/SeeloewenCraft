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

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Items/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Gui/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Entities/{resourceName}", UriKind.Absolute);
            if (ResourceExists(imageUri)) return imageUri;

            //If default one doesn't exist, use missing texture image
            return new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/MissingTexture.png", UriKind.Absolute);

        }

        private static void CreateResources()
        {
            Background = new ImageBrush { ImageSource = GetImageSource("Background.png") };
            GrassBlock = new ImageBrush { ImageSource = GetImageSource("GrassBlock.png") };
            StoneBlock = new ImageBrush { ImageSource = GetImageSource("StoneBlock.png") };
            DirtBlock = new ImageBrush { ImageSource = GetImageSource("DirtBlock.png") };
            AirBlock = new ImageBrush { ImageSource = GetImageSource("AirBlock.png") };
            BedrockBlock = new ImageBrush { ImageSource = GetImageSource("BedrockBlock.png") };
            CoalOreBlock = new ImageBrush { ImageSource = GetImageSource("CoalOreBlock.png") };
            DiamondOreBlock = new ImageBrush { ImageSource = GetImageSource("DiamondOreBlock.png") };
            IronOreBlock = new ImageBrush { ImageSource = GetImageSource("IronOreBlock.png") };
            OakLogBlock = new ImageBrush { ImageSource = GetImageSource("OakLogBlock.png") };
            OakLeavesBlock = new ImageBrush { ImageSource = GetImageSource("OakLeavesBlock.png") };
            ChestBlock = new ImageBrush { ImageSource = GetImageSource("ChestBlock.png") };
            MagmaBlock = new ImageBrush { ImageSource = GetImageSource("MagmaBlock.png") };
            MissingTexture = new ImageBrush { ImageSource = GetImageSource("MissingTexture.png") };
            Hammer = new ImageBrush { ImageSource = GetImageSource("Hammer.png") };
            SpruceLogBlock = new ImageBrush { ImageSource = GetImageSource("SpruceLogBlock.png") };
            SpruceLeavesBlock = new ImageBrush { ImageSource = GetImageSource("SpruceLeavesBlock.png") };
            Torch = new ImageBrush { ImageSource = GetImageSource("Torch.png") };
            Plant2_Base = new ImageBrush { ImageSource = GetImageSource("Plant2_Base.png") };
            Plant2_Top = new ImageBrush { ImageSource = GetImageSource("Plant2_Top.png") };
            Plant2 = new ImageBrush { ImageSource = GetImageSource("Plant2.png") };
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
            HealthFull = new ImageBrush { ImageSource = GetImageSource("HealthFull.png") };
            HealthHalf = new ImageBrush { ImageSource = GetImageSource("HealthHalf.png") };
            HealthEmpty = new ImageBrush { ImageSource = GetImageSource("HealthEmpty.png") };
            AlphaCrafter = new ImageBrush { ImageSource = GetImageSource("AlphaCrafter.png") };
            CobbleStoneBlock = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock.png") };
            CobbleStoneBlock_BottomRight = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_BottomRight.png") };
            CobbleStoneBlock_BottomLeft = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_BottomLeft.png") };
            CobbleStoneBlock_TopRight = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_TopRight.png") };
            CobbleStoneBlock_TopLeft = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_TopLeft.png") };
            CobbleStoneBlock_SlabBottom = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_SlabBottom.png") };
            CobbleStoneBlock_SlabTop = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_SlabTop.png") };
            CobbleStoneBlock_SlabLeft = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_SlabLeft.png") };
            CobbleStoneBlock_SlabRight = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_SlabRight.png") };
            CobbleStoneBlock_StairBottomRight = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_StairBottomRight.png") };
            CobbleStoneBlock_StairBottomLeft = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_StairBottomLeft.png") };
            CobbleStoneBlock_StairTopRight = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_StairTopRight.png") };
            CobbleStoneBlock_StairTopLeft = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_StairTopLeft.png") };
            CobbleStoneBlock_Center = new ImageBrush { ImageSource = GetImageSource("CobbleStoneBlock_Center.png") };
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

        }

        //-- Images --//

        public static ImageBrush Background;
        public static ImageBrush GrassBlock;
        public static ImageBrush StoneBlock;
        public static ImageBrush DirtBlock;
        public static ImageBrush AirBlock;
        public static ImageBrush BedrockBlock;
        public static ImageBrush CoalOreBlock;
        public static ImageBrush DiamondOreBlock;
        public static ImageBrush IronOreBlock;
        public static ImageBrush OakLogBlock;
        public static ImageBrush OakLeavesBlock;
        public static ImageBrush ChestBlock;
        public static ImageBrush MagmaBlock;
        public static ImageBrush MissingTexture;
        public static ImageBrush Hammer;
        public static ImageBrush SpruceLogBlock;
        public static ImageBrush SpruceLeavesBlock;
        public static ImageBrush Torch;
        public static ImageBrush Plant2_Top;
        public static ImageBrush Plant2_Base;
        public static ImageBrush Plant2;
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
        public static ImageBrush HealthFull;
        public static ImageBrush HealthHalf;
        public static ImageBrush HealthEmpty;
        public static ImageBrush AlphaCrafter;
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
    }
}
