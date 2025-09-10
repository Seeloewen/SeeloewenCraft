namespace SeeloewenCraft.game.core.events
{
    public abstract class GameEvent
    {
        protected readonly double maxTicks; //in milliseconds
        protected readonly int maxRuns; //-1 means infinite
        protected readonly int maxStacks; //amount of runs before reset

        protected double ticks;
        protected int timesRun;
        protected int timesStacked;

        public GameEvent(int maxTicks, int maxRuns = -1, int maxStacks = 1)
        {
            this.maxTicks = maxTicks;
            this.maxStacks = maxStacks;
            this.maxRuns = maxRuns;
        }

        internal void Update(double dt)
        {
            //Update progress and run the event when due
            ticks += dt;

            if (IsReady())
            {
                Run();

                timesRun++;
                timesStacked++;

                if (timesStacked >= maxStacks)
                {
                    //If the max amounts of stacks is reached, reset progress and stacks
                    ticks = 0;
                    timesStacked = 0;
                }

                if (timesRun >= maxRuns && maxRuns != -1) //If the event has reached max runs
                {
                    GameEventHandler.Unregister(this);
                }
            }
        }

        protected virtual bool IsReady() => ticks > maxTicks; //Should be updated accordingly when using stacks

        protected abstract void Run();
    }
}
