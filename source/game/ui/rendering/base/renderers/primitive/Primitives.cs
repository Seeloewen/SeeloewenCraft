

using SeeloewenCraft.game.ui;

namespace SeeloewenCraft.game.ui
{
    
    public struct Rectangle
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;

        public Rectangle(int xLeft, int yTop, int xRight, int yBottom)
        {
            (x1, y1) = Resolution.PixelToScreen(xLeft, yTop);
            (x2, y2) = Resolution.PixelToScreen(xRight, yBottom);
        }
        
        public Rectangle(float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public bool isInBounds(int xPixel, int yPixel)
        {
            (float x, float y) = Resolution.PixelToScreen(xPixel, yPixel);
            return x >= x1 && x < x2 
                && ((y >= y1 && y < y2)
                || (y >= y2 && y < y1));
        }

    }

    public struct Color
    {
        public float r, g, b, a;

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1.0f;
        }
        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

}
