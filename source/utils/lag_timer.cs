using System;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SeeloewenCraft.util
{
    class LagTimer
    {
        static int idToBeTested = 0;

        private int id;
        private Stopwatch timer;
        World world;

        public LagTimer(int id, World world)
        {
            this.id = id;
            timer = new Stopwatch();
            this.world = world;
        }


        public void start()
        {
            if (id != idToBeTested) return;

            world.log.Write("*************************", "Info");
            timer.Start();

        }

        public void ping(string label)
        {
            if (id != idToBeTested) return;

            timer.Stop();
            long elapsedMicroseconds = timer.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            world.log.Write($"{label}: {elapsedMicroseconds}", "Info");
            timer.Restart();
        }

        public void stop()
        {
            if (id != idToBeTested) return;

            timer.Stop();
        }








    }
}
