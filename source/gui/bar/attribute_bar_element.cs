using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class AttributeBarElement
    {
        private AttributeBar bar;
        public double value;
        public int index;
        public Canvas cvsElement = new Canvas();

        public ImageBrush imgElementFull;
        public ImageBrush imgElementHalf;
        public ImageBrush imgElementEmpty;

        public AttributeBarElement(AttributeBar bar, int index)
        {
            //Create references
            this.index = index;
            this.bar = bar;
            imgElementFull = bar.imgElementFull;
           imgElementHalf = bar.imgElementHalf;
            imgElementEmpty = bar.imgElementEmpty;

            //Setup canvas
            cvsElement.Height = 35;
            cvsElement.Width = 35;
            Canvas.SetLeft(cvsElement, index * 42);

            //Setup default state
            SetValue(1);
        }

        public void SetValue(double value)
        {
            //Set the value and image of the element based on the given input
            switch (value)
            {
                case 1:
                    this.value = 1;
                    cvsElement.Background = imgElementFull;
                    break;
                case 0.5:
                   this. value = 0.5;
                    cvsElement.Background = imgElementHalf;
                    break;
                case 0:
                    this.value = 0;
                    cvsElement.Background = imgElementEmpty;
                    break;
            }
        }
    }
}
