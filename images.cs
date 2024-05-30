using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace SeeloewenCraft
{
    public class Images //This class contains all the references to the image resources

    {
        //-- Custom Methods --//

        public static string textureDirectory;
        wndGame wndGame;

        public Images(wndGame wndGame)
        {
            this.wndGame = wndGame;

            //Check which texurepack is selected and set the directory based on that
            if (wndGame.wndMenu.selectedTexturepack == "default")
            {
                textureDirectory = "pack://application:,,,/SeeloewenCraft;component/Resources";
                wndGame.log.Write($"Set texture directory to pack://application:,,,/SeeloewenCraft;component/Resources (internal resources)", "Info");
            }
            else
            {
                textureDirectory = wndGame.wndMenu.selectedTexturepack;
                wndGame.log.Write($"Set texture directory to {wndGame.wndMenu.selectedTexturepack}", "Info");
            }

            //Actually load the resources
            CreateResources();
        }

        public static ImageSource GetImageSource(string resourceName)
        {
            Uri imageUri;

            //Check if the image file exists and use default or missing texture image otherwise
            if (File.Exists($"{textureDirectory}/{resourceName}"))
            {
                imageUri = new Uri($"{textureDirectory}/{resourceName}", UriKind.Absolute);
            }
            else
            {
                try
                {
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/{resourceName}", UriKind.Absolute);

                }
                catch
                {
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/MissingTexture.png", UriKind.Absolute);
                }
            }

            //Get an image from an imagesource from a uri
            return BitmapFrame.Create(imageUri);
        }

        private void CreateResources()
        {
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
        }

        //-- Images --//

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

    }
}
