using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SeeloewenCraft
{
    public class GameLoop
    {
        //References
        public HighPrecisionTimer.MultimediaTimer tmrGameLoop = new HighPrecisionTimer.MultimediaTimer();
        public List<GameLoopEvent> loopEvents;
        public int tickrate;
        World world;

        //Gameloops
        WaterUpdateEvent waterUpdateEvent;
        DayNightCycle dayNightCycle;

        //-- Constructor --//

        public GameLoop(World world, int tickrate)
        {
            //Setup references
            this.world = world;
            this.tickrate = tickrate;
            loopEvents = new List<GameLoopEvent>();

            //Setup timer
            tmrGameLoop.Interval = tickrate;
            tmrGameLoop.Elapsed += tmrGameLoop_Tick;

            //Setup all game loops
            waterUpdateEvent = new WaterUpdateEvent(world, this);
            dayNightCycle = new DayNightCycle(world, this);
        }

        //-- Custom Methods --//

        public void DoUpdate(int tickrate)
        {
            foreach (GameLoopEvent gameEvent in loopEvents)
            {
                //Increase the tick for each game event
                gameEvent.tick += tickrate;

                if (gameEvent.IsReady())
                {
                    //If the event is ready, do the event and if it's only a single event, reset the counter
                    Application.Current.Dispatcher.Invoke(new Action(() => { gameEvent.DoEvent(); }));                          
                    if (gameEvent.singleEvent)
                    {
                        gameEvent.Reset();
                    }
                }
            }
        }

        public void Start()
        {
            //Start the game loop timer
            tmrGameLoop.Start();
        }

        public void Stop()
        {
            //Stop the game loop timer
            tmrGameLoop.Stop();
        }

        //-- Event Handlers --//
        public void tmrGameLoop_Tick(object sender, EventArgs e)
        {
            //Update the tick for all game events and do possible events
            DoUpdate(tickrate);
        }
    }

    //Abstract Game Loop
    public abstract class GameLoopEvent
    {

        public World world;
        public int tick;
        public int maxTick;
        public bool singleEvent = true;

        //-- Constructor --//

        public GameLoopEvent(World world, GameLoop gameLoop)
        {
            //Add the event to the loop events list so it can be accessed
            this.world = world;
            gameLoop.loopEvents.Add(this);
        }

        //-- Custom Methods --//

        public void Reset()
        {
            //Reset the tick to 0
            tick = 0;
        }

        //Do the event
        public abstract void DoEvent();

        public virtual bool IsReady()
        {
            //Check if the tick is past the max tick, which means that it should do the event
            return tick >= maxTick;
        }
    }
}
