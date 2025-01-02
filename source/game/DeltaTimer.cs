

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SeeloewenCraft
{
    class DeltaTimer
    {

        const double mTickrate = 1.0;

        static double timeSinceM = 0;

        static double lastTime = 0;

        static int frames = 0;

        public static void Tick()
        {
            double t = GLFW.GetTime();
            double dt = t - lastTime;
            //Log.Write($"{1 / dt}", LogType.GENERAL, LogLevel.INFO);
            lastTime = t;
            timeSinceM += dt;
            frames++;
            if(timeSinceM > 1.0)
            {
                Log.Write($"{frames}", LogType.GENERAL, LogLevel.INFO);
                frames = 0;
                timeSinceM %= 1.0;
            }

        }


    }
}
