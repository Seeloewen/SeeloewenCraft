
using System.Windows;
using System.Windows.Input;

namespace SeeloewenCraft.gl_rendering
{
    public static class InputHandler
    {



        public static float currentMouseX;
        public static float currentMouseY;

        public static bool pressedLeft;
        public static bool pressedRight;



        public static void HandleMouseMove(Point p) {
            currentMouseX = (float)p.X / 640 - 1;
            currentMouseY = (float)p.Y / -360 + 1;
        }


        public static void HandleMouseClick(MouseButton button, MouseButtonState action)
        {

            Log.Write($"button={button}, action={action}, x={currentMouseX}, y={currentMouseY}", LogType.RENDERING, LogLevel.INFO);
            if(button == MouseButton.Left) { 
                pressedLeft = action == MouseButtonState.Pressed;
            }
            if(button == MouseButton.Right)
            {
                pressedRight = action == MouseButtonState.Pressed;
            }


        }



    }


}

