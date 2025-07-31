using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SeeloewenCraft.game.ui
{
    internal class Resolution
    {
        
        static public int WIDTH
        {
            get;
            private set;
        } = 1280;
        
        static public int HEIGHT
        {
            get;
            private set;
        } = 720;

        static public float RATIO
        {
            get => (float)WIDTH / (float)HEIGHT;
        }


        static public (int, int) ScreenToPixel(float x, float y)
        {
            return ((int)(0.5 + (x + 1) * 0.5 * WIDTH), (int)(0.5 - (y - 1) * 0.5 * HEIGHT));
        }

        static public (float x, float y) PixelToScreen(int x, int y)
        {
            return ((float)x / (WIDTH / 2) - 1, (float)y / -(HEIGHT / 2) + 1);
        }

        static public (float x, float y) PixelToScreen(double x, double y)
        {
            return ((float)x / (WIDTH / 2) - 1, (float)y / -(HEIGHT / 2) + 1);
        }

        static unsafe public void Init(Window* window)
        {
            GLFW.SetWindowSizeCallback(window, (window1, width, height) =>
            {
                WIDTH = width;
                HEIGHT = height;
                
                GL.Viewport(0, 0, WIDTH, HEIGHT);
                Screen.OnResize();
            });
            
        }
    }
}