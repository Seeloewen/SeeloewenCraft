using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Resources;
using System.Windows;

namespace SeeloewenCraft
{
    public class Images //This class contains all the references to the image resources

    {
        //-- Custom Methods --//

        public static string textureDirectory;
        World world;

        public Images(World world)
        {
            this.world = world;

            //Check which texurepack is selected and set the directory based on that
            if (world.wndMenu.selectedTexturepack == "default")
            {
                textureDirectory = "pack://application:,,,/SeeloewenCraft;component/Resources";
                world.log.Write($"Set texture directory to pack://application:,,,/SeeloewenCraft;component/Resources (internal resources)", "Info");
            }
            else
            {
                textureDirectory = world.wndMenu.selectedTexturepack;
                world.log.Write($"Set texture directory to {world.wndMenu.selectedTexturepack}", "Info");
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

            //If default one doesn't exist, use missing texture image
            return new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/MissingTexture.png", UriKind.Absolute);

        }

        private void CreateResources()
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
            QuarterOakPlankBlock = new ImageBrush { ImageSource = GetImageSource("QuarterOakPlankBlock.png") };
        }

        //-- Images --//

        public ImageBrush Background;
        public ImageBrush GrassBlock;
        public ImageBrush StoneBlock;
        public ImageBrush DirtBlock;
        public ImageBrush AirBlock;
        public ImageBrush BedrockBlock;
        public ImageBrush CoalOreBlock;
        public ImageBrush DiamondOreBlock;
        public ImageBrush IronOreBlock;
        public ImageBrush OakLogBlock;
        public ImageBrush OakLeavesBlock;
        public ImageBrush ChestBlock;
        public ImageBrush MagmaBlock;
        public ImageBrush MissingTexture;
        public ImageBrush Hammer;
        public ImageBrush SpruceLogBlock;
        public ImageBrush SpruceLeavesBlock;
        public ImageBrush Torch;
        public ImageBrush Plant2_Top;
        public ImageBrush Plant2_Base;
        public ImageBrush Plant2;
        public ImageBrush Water_1_Right;
        public ImageBrush Water_1_Left;
        public ImageBrush Water_2_Right;
        public ImageBrush Water_2_Left;
        public ImageBrush Water_3_Right;
        public ImageBrush Water_3_Left;
        public ImageBrush Water_4_Right;
        public ImageBrush Water_4_Left;
        public ImageBrush Water_5_Right;
        public ImageBrush Water_5_Left;
        public ImageBrush Water_6;
        public ImageBrush Gui;
        public ImageBrush HealthFull;
        public ImageBrush HealthHalf;
        public ImageBrush HealthEmpty;
        public ImageBrush AlphaCrafter;
        public ImageBrush QuarterOakPlankBlock;
    }
}
