

using SeeloewenCraft.game.graphics;

namespace SeeloewenCraft.game.graphics
{

    public struct Rectangle
    {
        public float x1S;
        public float y1S;
        public float x2S;
        public float y2S;
        public int x1P
        {
            get
            {
                (int x, _) = Resolution.ScreenToPixel(x1S, y1S);
                return x;
            }
        }

        public int y1P
        {
            get
            {
                (_, int y) = Resolution.ScreenToPixel(x1S, y1S);
                return y;
            }
        }

        public int x2P
        {
            get
            {
                (int x, _) = Resolution.ScreenToPixel(x2S, y2S);
                return x;
            }
        }

        public int y2P
        {
            get
            {
                (_, int y) = Resolution.ScreenToPixel(x2S, y2S);
                return y;
            }
        }

        public Rectangle(int xLeft, int yTop, int xRight, int yBottom)
        {
            (x1S, y1S) = Resolution.PixelToScreen(xLeft, yTop);
            (x2S, y2S) = Resolution.PixelToScreen(xRight, yBottom);
        }

        public Rectangle(float x1, float y1, float x2, float y2)
        {
            this.x1S = x1;
            this.y1S = y1;
            this.x2S = x2;
            this.y2S = y2;
        }

        public bool IsInBounds(int xPixel, int yPixel)
        {
            (float x, float y) = Resolution.PixelToScreen(xPixel, yPixel);
            return x >= x1S && x < x2S
                && ((y >= y1S && y < y2S)
                || (y >= y2S && y < y1S));
        }

        public (int x, int y) GetCenter()
        {
            return ((x1P + x2P) / 2, (y1P + y2P) / 2);
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

        public Color(float g) : this(g, g, g) { }

    }

}
