using OpenTK.Windowing.GraphicsLibraryFramework;
using SeeloewenCraft.gl_rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Navigation;
namespace SeeloewenCraft
{
    public static class InputHandler
    {

        static public HashSet<System.Windows.Input.Key> pressedKeys = new HashSet<System.Windows.Input.Key>();



        public static int mouseXPixel { get; private set; }
        public static int mouseYPixel { get; private set; }

        public static float mouseXScreen { get => ((float)mouseXPixel) /(Resolution.WIDTH/2) - 1; }
        public static float mouseYScreen { get => ((float)mouseYPixel) / (Resolution.HEIGHT / 2) + 1; }



        public static bool pressedLeft { get; private set; }
        public static bool pressedRight { get; private set; }



        public unsafe static void Init(Window* window)
        {
            GLFW.SetCursorPosCallback(window, (_, x, y) =>
            {
                mouseXPixel = (int)x;
                mouseYPixel = (int)y;
                Log.Write($"mouse move {x} {y}", LogType.GENERAL, LogLevel.INFO);
            });

            GLFW.SetMouseButtonCallback(window, (_, button, action, _) =>
            {
                switch (button)
                {
                    case MouseButton.Left: 
                        pressedLeft = action == InputAction.Press; 
                        break;
                    case MouseButton.Right: 
                        pressedRight = action == InputAction.Press; 
                        break;
                }
            Log.Write($"click mouse {button} {action}", LogType.GENERAL, LogLevel.INFO);
            });

            GLFW.SetWindowCloseCallback(window, (window) =>
            {
                GLFW.SetWindowShouldClose(window, true);

                Log.Write("close window", LogType.GENERAL, LogLevel.INFO);
            });

            GLFW.SetKeyCallback(window, (_, k, _, a, _) =>
            {
                if(KeyBinds.bindings.TryGetValue(k, out var v)) {
                    KeyBinds.pressed[v] = a != InputAction.Release;
                }
                Log.Write($"key {k} {a}", LogType.GENERAL, LogLevel.INFO);
            });

           

        }


        /*public static void HandleMouseMove(Point p)
        {
            currentMouseX = (float)p.X / 640 - 1;
            currentMouseY = (float)p.Y / -360 + 1;
        }


        public static void HandleMouseClick(MouseButton button, MouseButtonState action)
        {

            Log.Write($"button={button}, action={action}, x={currentMouseX}, y={currentMouseY}", LogType.RENDERING, LogLevel.INFO);
            if (button == MouseButton.Left)
            {
                pressedLeft = action == MouseButtonState.Pressed;
            }
            if (button == MouseButton.Right)
            {
                pressedRight = action == MouseButtonState.Pressed;
            }


        }*/










    }
}
