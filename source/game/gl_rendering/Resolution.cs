using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeeloewenCraft.gl_rendering
{
    internal class Resolution
    {

        public const int WIDTH = 1280;
        public const int HEIGHT = 720;
        public const float RATIO = 16 / 9f;


        static public (int, int) ScreenToPixel(float x, float y)
        {
            return ((int)((x+1)*0.5*WIDTH),(int)((y+1)*0.5*HEIGHT));
        }

        static public (float x, float y) PixelToScreen(int x, int y)
        {
            return ((float)x / (WIDTH / 2) - 1, (float)y / -(HEIGHT / 2) + 1);
        }

        static public (float x, float y) PixelToScreen(double x, double y)
        {
            return ((float)x / (WIDTH / 2) - 1, (float)y / -(HEIGHT / 2) + 1);
        }

    }
}
