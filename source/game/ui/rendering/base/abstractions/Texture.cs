
using System.Drawing;

using OpenTK.Graphics.OpenGL4;
using SeeloewenCraft.game.ui;

namespace SeeloewenCraft.gl_rendering
{
    internal class Texture
    {

        private int id;



        internal Texture(TextureImage textureImage) {
            int width = textureImage.width;
            int height = textureImage.height;
            byte[] data = textureImage.rawData;


            byte[] data = new byte[width * height * 4];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    int index = (x + y * width) * 4;
                    data[index] = c.R;
                    data[index + 1] = c.G;
                    data[index + 2] = c.B;
                    data[index + 3] = c.A;
                }
            }

            id = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, antiAlias ? (int)TextureMinFilter.Linear : (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, antiAlias ? (int)TextureMagFilter.Linear : (int)TextureMagFilter.Nearest);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        }

        internal Texture(Bitmap bitmap, bool antiAlias) {


            int width = bitmap.Width;
            int height = bitmap.Height;



            byte[] data = new byte[width * height * 4];
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    int index = (x + y * width) * 4;
                    data[index] = c.R;
                    data[index+1] = c.G;
                    data[index+2] = c.B;
                    data[index+3] = c.A;
                }
            }

            id = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, antiAlias ? (int) TextureMinFilter.Linear : (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, antiAlias ? (int)TextureMagFilter.Linear : (int)TextureMagFilter.Nearest);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        }


        ~Texture()
        {
            //GL.DeleteTexture(id);
        }


        internal void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

    }
}
