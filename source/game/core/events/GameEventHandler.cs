using System.Collections.Generic;

namespace SeeloewenCraft.game.core.events
{
    public static class GameEventHandler
    {
        private static List<GameEvent> registeredEvents;

        public static void Init()
        {
            registeredEvents = new List<GameEvent> ();
        }

        public static void Register(GameEvent e)
        {
            registeredEvents.Add(e);
        }

        public static void Unregister(GameEvent e)
        {
            registeredEvents.Remove(e);
        }

        public static void Update(double dt)
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
