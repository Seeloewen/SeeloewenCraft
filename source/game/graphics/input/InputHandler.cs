using OpenTK.Windowing.GraphicsLibraryFramework;
using SeeloewenCraft.game.util;
using System;

namespace SeeloewenCraft.game.graphics
{
    public static class InputHandler
    {
        public static int mouseXPixel { get; private set; }
        public static int mouseYPixel { get; private set; }

        public static float mouseXScreen
        {
            get => ((float)mouseXPixel) / (Resolution.WIDTH / 2) - 1;
        }

        public static float mouseYScreen
        {
            get => ((float)mouseYPixel) / (-Resolution.HEIGHT / 2) + 1;
        }


        public static int scrollAmount { get; private set; }

        public static bool pressedLeft { get; private set; }
        public static bool pressedRight { get; private set; }


        private static bool textMode = false;
        private static TextReceiver textReceiver;


        public static void EnterTextMode(TextReceiver textReceiver)
        {
            InputHandler.textReceiver = textReceiver;
            textMode = true;
        }

        public static void ExitTextMode()
        {
            textMode = false;
            InputHandler.textReceiver = null;
        }


        public unsafe static void Init(Window* window)
        {
            GLFW.SetCursorPosCallback(window, (_, x, y) =>
            {
                mouseXPixel = (int)x;
                mouseYPixel = (int)y;
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
            });

            GLFW.SetScrollCallback(window, (_, _, y) => { scrollAmount += Convert.ToInt32(y); });

            GLFW.SetWindowCloseCallback(window, (window) => { GLFW.SetWindowShouldClose(window, true); });

            GLFW.SetCharCallback(window, (_, ch) =>
            {
                if (textMode)
                {
                    textReceiver.HandleChar(Char.ConvertFromUtf32((int)ch));
                }
            });

            GLFW.SetKeyCallback(window, (GLFWCallbacks.KeyCallback)((_, k, _, a, _) =>
            {
                if (textMode)
                {
                    if (a == InputAction.Press)
                    {
                        switch (k)
                        {
                            case Keys.Escape:
                                textReceiver.HandleEscape();
                                break;
                            case Keys.Enter:
                                textReceiver.HandleEnter();
                                break;
                            case Keys.Backspace:
                                textReceiver.HandleBackspace();
                                break;
                        }
                    }
                }
                else
                {
                    if (k == Keys.F2 && a == InputAction.Press) BreakPoint.breakNext = true;
                    if (KeyBinds.bindings.TryGetValue(k, out var v))
                    {
                        if (a == InputAction.Press)
                        {
                            KeyBinds.pressed[v] = true;
                            KeyBinds.pressedFirst[v] = true;
                        }
                        else if (a == InputAction.Release)
                        {
                            KeyBinds.pressed[v] = false;
                            KeyBinds.pressedFirst[v] = false;
                        }
                    }
                }
            }));
        }

        static public void Reset()
        {
            scrollAmount = 0;
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