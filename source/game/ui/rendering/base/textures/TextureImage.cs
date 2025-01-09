
using System.Configuration;
using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Activation;

namespace SeeloewenCraft.game.ui
{
    internal struct TextureImage
    {
        internal byte[] rawData;
        internal int width;
        internal int height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal byte GetByte(int x, int y, int c)
        {
            return rawData[(x + y * width) * 4 + c];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetByte(byte value, int x, int y, int c)
        {
            rawData[(x + y * width) * 4 + c] = value;
        }

        internal TextureImage(int width, int height)
        {
            rawData = new byte[width * height * 4];
            this.width = width;
            this.height = height;
        }

        internal TextureImage(Bitmap bitmap)
        {
            width = bitmap.Width;
            height = bitmap.Height;

            rawData = new byte[4 * width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    int index = (x + y * width) * 4;
                    rawData[index] = c.R;
                    rawData[index + 1] = c.G;
                    rawData[index + 2] = c.B;
                    rawData[index + 3] = c.A;
                }
            }
        }
    }
}
