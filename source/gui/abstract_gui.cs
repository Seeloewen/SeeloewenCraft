using System.Windows.Controls;
using System.Windows;

namespace SeeloewenCraft
{
    public abstract class Gui
    {
        public Inventory inventory;
        public Canvas cvsGui;
        public TextBlock tblHeader;
        public string id;
        public bool isOpen = false;

        //-- Constructor --//

        public Gui( int height, int width, int top, int left, string id)
        {
            //Create references
            this.id = id;

            //Setup Canvas
            cvsGui = new Canvas();
            cvsGui.Background = Images.Gui.GetTexture();
            cvsGui.Width = width;
            cvsGui.Height = height;
            cvsGui.Visibility = Visibility.Hidden;
            Canvas.SetTop(cvsGui, top);
            Canvas.SetLeft(cvsGui, left);
            Panel.SetZIndex(cvsGui, 4);
            Game.world.wndGame.cvsGame.Children.Add(cvsGui);

            //Setup Header
            tblHeader = new TextBlock();
            tblHeader.FontSize = 20;
            tblHeader.Text = id;
            tblHeader.FontWeight = FontWeights.SemiBold;
            Canvas.SetLeft(tblHeader, 25);
            Canvas.SetTop(tblHeader, 2);
            cvsGui.Children.Add(tblHeader);
        }

        public virtual void Show()
        {
            cvsGui.Visibility = Visibility.Visible;
            isOpen = true;
            Game.world.guiList.Add(this);
        }

        public void Hide()
        {
            cvsGui.Visibility = Visibility.Hidden;
            isOpen = false;
            Game.world.guiList.Remove(this);

        }

        public void SetTop(int newTop)
        {
            Canvas.SetTop(cvsGui, newTop);
        }

        public void SetLeft(int newLeft)
        {
            Canvas.SetLeft(cvsGui, newLeft);
        }
    }
}
