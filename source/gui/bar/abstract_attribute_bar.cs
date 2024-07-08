using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;
using Newtonsoft.Json.Linq;

namespace SeeloewenCraft
{
    public abstract class AttributeBar
    {
        //References
        public World world;
        private Canvas cvsBar = new Canvas();
        private List<AttributeBarElement> elements = new List<AttributeBarElement>();
        public ImageBrush imgElementFull;
        public ImageBrush imgElementHalf;
        public ImageBrush imgElementEmpty;

        //Constants
        public string name;
        public string id;

        //Variables
        public double value;


        //-- Constructor --//

        public AttributeBar(World world, int top, int left)
        {
            //Create references
            this.world = world;

            //Create the bar
            cvsBar.Height = 35;
            cvsBar.Width = 410;
            cvsBar.Background = new SolidColorBrush(Colors.Transparent);
            world.wndGame.cvsGame.Children.Add(cvsBar);
            Canvas.SetLeft(cvsBar, left);
            Canvas.SetTop(cvsBar, top);

            //Setup the elements and images
            SetupTextures();
            SetupElements();
        }

        private void SetupElements()
        {
            for (int i = 0; i < 10; i++)
            {
                //Create the elements in the bar
                AttributeBarElement element = new AttributeBarElement(this, i);

                cvsBar.Children.Add(element.cvsElement);
                elements.Add(element);
            }
        }

        public abstract void SetupTextures();

        private void Show()
        {
            //Show the bar
            cvsBar.Visibility = Visibility.Visible;
        }

        private void Hide()
        {
            //Hide the bar
            cvsBar.Visibility = Visibility.Hidden;
        }

        public void RemoveValue(double value)
        {
            double newValue = this.value - value;
            if (newValue < 0) this.value = 0;
            SetValue(newValue);
        }

        public void AddValue(double value)
        {
            double newValue = this.value + value;
            if (newValue > 10) newValue = 10;
            SetValue(newValue);
        }

        public void SetValue(double value)
        {
            if (value % 0.5 == 0 && value >= 0 && value <= 10)
            {
                this.value = value;

                foreach (AttributeBarElement element in elements)
                {
                    if (value == 0)
                    {
                        element.SetValue(0);
                    }
                    else if (value == 0.5)
                    {
                        element.SetValue(0.5);
                        value -= 0.5;
                    }
                    else if (value > 0.5)
                    {

                        element.SetValue(1);
                        value -= 1;
                    }
                }
            }
            else
            {
                world.log.Write($"Could not update value for bar {name} because an invalid value was given.", "Error");
            }
        }
    }
}
