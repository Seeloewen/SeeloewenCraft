using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeeloewenCraft
{
    public class images //This class contains all the references to the image resources

    {
        //-- Custom Methods --//

        public static ImageSource GetImageSource(string assemblyName, string resourceName)
        {
            //Get an image from an imagesource from a uri
            Uri imageUri = new Uri(string.Format("pack://application:,,,/{0};component/Resources/{1}", assemblyName, resourceName), UriKind.Absolute);
            return BitmapFrame.Create(imageUri);
        }

        //-- Images --//

        public ImageBrush GrassBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "GrassBlock.png") };
        public ImageBrush StoneBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "StoneBlock.png") };
        public ImageBrush DirtBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "DirtBlock.png") };
        public ImageBrush AirBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "AirBlock.png") };
        public ImageBrush BedrockBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "BedrockBlock.png") };
        public ImageBrush CoalOreBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "CoalOreBlock.png") };
        public ImageBrush DiamondOreBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "DiamondOreBlock.png") };
        public ImageBrush IronOreBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "IronOreBlock.png") };
        public ImageBrush OakLogBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "OakLogBlock.png") };
        public ImageBrush OakLeavesBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "OakLeavesBlock.png") };
        public ImageBrush ChestBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "ChestBlock.png") };
        public ImageBrush MagmaBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "MagmaBlock.png") };
        public ImageBrush MissingTexture = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "MissingTexture.png") };
        public ImageBrush Hammer = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "Hammer.png") };
        public ImageBrush SpruceLogBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "SpruceLogBlock.png") };
        public ImageBrush SpruceLeavesBlock = new ImageBrush { ImageSource = GetImageSource("SeeloewenCraft", "SpruceLeavesBlock.png") };

    }
}
