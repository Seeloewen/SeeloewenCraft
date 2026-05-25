using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Media.Imaging;

namespace SeeloewenCraft.game.graphics
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
        
        internal unsafe TextureImage(Bitmap bitmap)
        {
            width = bitmap.PixelSize.Width;
            height = bitmap.PixelSize.Height;

            int stride = width * 4;
            int bufferSize = stride * height;

            byte[] bgra = new byte[bufferSize];
            rawData = new byte[bufferSize];

            fixed (byte* ptr = bgra)
            {
                var rect = new PixelRect(0, 0, width, height);
                bitmap.CopyPixels(rect, (nint)ptr, bufferSize, stride);
            }

            for (int i = 0; i < bufferSize; i += 4)
            {
                byte b = bgra[i + 0];
                byte g = bgra[i + 1];
                byte r = bgra[i + 2];
                byte a = bgra[i + 3];

                rawData[i + 0] = r;
                rawData[i + 1] = g;
                rawData[i + 2] = b;
                rawData[i + 3] = a;
            }
        }
    }
}
