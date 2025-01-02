

using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace SeeloewenCraft
{
    class DeltaTimer
    {
            

        static double lastTime = 0;

        public static double Tick()
        {
            double t = GLFW.GetTime();
            double dt = t - lastTime;
            //Log.Write($"{1 / dt}", LogType.GENERAL, LogLevel.INFO);
            lastTime = t;
            dt = Math.Max(Math.Min(dt, 1.0), 0.0001);
            return dt;
        }


    }
}
