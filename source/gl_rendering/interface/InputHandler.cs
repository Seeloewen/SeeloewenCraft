
using System.Windows;
using System.Windows.Input;

namespace SeeloewenCraft.gl_rendering
{
    public static class InputHandler
    {



        public static float currentMouseX;
        public static float currentMouseY;

        public static bool mouseClick;



        public static void HandleMouseMove(Point p) {
            currentMouseX = (float)p.X / 640 - 1;
            currentMouseY = (float)p.Y / -360 + 1;
        }


        public static void HandleMouseClick(MouseButton button, MouseButtonState action)
        {

            Log.Write($"button={button}, action={action}, x={currentMouseX}, y={currentMouseY}", LogType.RENDERING, LogLevel.INFO);
            if(button == MouseButton.Left) { 
                mouseClick = action == MouseButtonState.Pressed;
            }


        }



    }


}

