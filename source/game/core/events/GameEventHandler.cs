using System.Collections.Generic;

namespace SeeloewenCraft.game.core.events
{
    public class GameEventHandler
    {
        private List<GameEvent> registeredEvents = new List<GameEvent>();

        public void Register(GameEvent e)
        {
            registeredEvents.Add(e);
        }

        internal void Unregister(GameEvent e)
        {
            registeredEvents.Remove(e);
        }

        public void Tick(double dt)
        {
            dt = dt * 1000;

            //Go through all events backwards (considering they might get removed after execution)
            for (int i = registeredEvents.Count - 1; i >= 0; i--)
            {
                GameEvent e = registeredEvents[i];
                e.Update(dt);
            }
        }
    }
}
