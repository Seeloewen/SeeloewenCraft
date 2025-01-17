

using OpenTK.Windowing.GraphicsLibraryFramework;
using SeeloewenCraft.game.ui;
using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    class DeltaTimer
    {

        static List<double> frameTimes;


        static double lastCounterUpdate = 0;
        static double lastTime = 0;
        static double lastBlockUpdate = 0;


        public static double Tick(out bool blockUpdate)
        {
            double dt = 0, t = 0;
            

            while (dt < 0.0001)
            {
                t = GLFW.GetTime();
                dt = t - lastTime;
            }

            if (t > lastBlockUpdate + 1 / 20d)
            {
                blockUpdate = true;
                lastBlockUpdate += 1 / 20d; //TODO think this through(ithink this isnt correct)
            } else
            {
                blockUpdate = false;
            }
            lastTime = t;
            AddFrameTime(dt);

            if (lastCounterUpdate + 1 < lastTime)
            {
                UpdateCounter();
            }

            dt = Math.Min(dt, 1d / 20);
            return dt;
        }
        public static void Start()
        {
            lastTime = GLFW.GetTime();
            lastCounterUpdate = lastTime;
            frameTimes = new List<double>();
        }

        static void UpdateCounter()
        {
            double min = 100, sum = 0, max = 0;
            int c = 0;
            foreach (double t in frameTimes)
            {
                if (t < min && min != 0) min = t;
                if (t > max) max = t;
                sum += t;
                c++;//nice
            }
            double avg = sum / c;
            DebugMenu.UpdateLine(DebugMenu.Section.WORLD, "fps", $"{c} ({min * 1000:F2};{avg * 1000:F2};{max * 1000:F2})");
            frameTimes.Clear();
            lastCounterUpdate = lastTime;
        }
        static void AddFrameTime(double dt)
        {
            frameTimes.Add(dt);
        }


    }
}
