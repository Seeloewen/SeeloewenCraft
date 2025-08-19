using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SeeloewenCraft.game.graphics;
using Renderer = SeeloewenCraft.game.graphics.Renderer;
using TextureManager = SeeloewenCraft.game.graphics.TextureManager;

namespace SeeloewenCraft
{
    public static class Game
    {
        //References
        public static World world;
        public static Client client;
        public static Server server;
        public static wndMenu wndMenu;

        //Constants
        public const int WORLD_VERSION = 6; //Up to date as of Alpha 1.2.1 (Recent changes: Seeding)
        public const string GAME_VERSION = "Beta 1.0.0-dev";
        public const string VERSION_DATE = "19.08.2025";
        public const int TEXTUREPACK_VERSION = 2;

        //Variables
        public static List<string> unstackableItems = new List<string>();
        public static string selectedTexturepack;
        public static Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Microsecond);
        public static int playerId;
        public static bool generated;



        static unsafe void GameLoop(Window* window)
        {
            DeltaTimer.Start();
            while (!shouldClose && !GLFW.WindowShouldClose(window))
            {
                double dt = DeltaTimer.Tick(out bool blockUpdate);

                
                Screen.Update();

                world.doGameTick(dt * 0.7, blockUpdate);

                Renderer.Render();

                GLFW.SwapBuffers(window);
                
                InputHandler.Reset();
                GLFW.PollEvents();

            }
        }
        
        
        #region GLFW

        public static bool shouldClose = false;

        public static void Close()
        {
            shouldClose = true;
        }

        public unsafe static void Create(string worldName, int seed, bool isNew, int worldVersion, string gameVersion, MultiplayerType multiplayerType, wndMenu wndMenu)
        {
            Game.wndMenu = wndMenu;

            Window* window = InitWindow();

            InputHandler.Init(window);

            Resolution.Init(window);

            TextureManager.Init();

            world = new World(null, worldName, seed, isNew, worldVersion, gameVersion, multiplayerType);

            Renderer.Init();

            Screen.Init();

            GameLoop(window);


            GLFW.DestroyWindow(window);
            GLFW.Terminate();
        }


        static unsafe Window* InitWindow()
        {
            if (!GLFW.Init())
            {
                FatalError("Failed to init GLFW");
            }

            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 3);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            GLFW.WindowHint(WindowHintBool.Resizable, true);

            Window* window = GLFW.CreateWindow(Resolution.WIDTH, Resolution.HEIGHT, "Game Window (ohio)", null, null);
            if (window == null)
            {
                FatalError("Failed to create window");
            }

            GLFW.SetErrorCallback((error, description) =>
            {
                Log.Write($"GLFW Error ({error}): {description}", LogType.GENERAL, LogLevel.ERROR);
            });

            GLFW.MakeContextCurrent(window);




            GL.LoadBindings(new GLFWBindingsContext());

            GL.Viewport(0, 0, 1280, 720);

            GL.ClearColor(188f/255, 244f/255, 247f/255, 1f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GLFW.SwapInterval(0);


            return window;
        }

        static bool IsExtensionSupported(string extensionName)
        {
            int numExtensions = GL.GetInteger(GetPName.NumExtensions);
            for (int i = 0; i < numExtensions; i++)
            {
                string ext = GL.GetString(StringNameIndexed.Extensions, i);
                if (ext == extensionName) return true;
            }
            return false;
        }

        #endregion

        #region general

        //Methods
        public static bool IsMultiplayer()
        {
            return IsServer() || IsClient();
        }

        public static bool IsServer()
        {
            return server != null;
        }

        public static bool IsClient()
        {
            return client != null && client.isConnected;
        }

        public static void FatalError(string message)
        {
            ShowException(new Exception(message));
        }

        public static void ShowException(Exception ex)
        {
            System.Windows.MessageBox.Show($"Oh no! The game has encountered an exception: {ex.Message} \n\n{ex.StackTrace}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        #endregion

    }

    // i hope i never have to understand this 💀
    public class GLFWBindingsContext : IBindingsContext
    {
        public IntPtr GetProcAddress(string procName)
        {
            // Use GLFW's GetProcAddress to load OpenGL functions
            IntPtr address = GLFW.GetProcAddress(procName);
            if (address == IntPtr.Zero)
            {
                Log.Write($"Failed to load OpenGL function: {procName}", LogType.GENERAL, LogLevel.WARNING);
                //throw new InvalidOperationException($"Failed to load OpenGL function: {procName}");
            }
            return address;
        }
    }

}
