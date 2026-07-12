using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.events
{
    internal class AutoSaveEvent : GameEvent
    {
        public AutoSaveEvent(int interval) : base(interval) { }

        protected override void Run()
        {
            World.Get().Save();

            if (Settings.showAutoSaveNotification)
            {
                NotificationManager.Get().Notify("sc:save", "Successfully Auto-Saved the world!", "general");
            }
        }
    }
}
