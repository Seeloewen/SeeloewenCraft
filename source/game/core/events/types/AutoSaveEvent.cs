using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.events
{
    internal class AutoSaveEvent : GameEvent
    {
        public AutoSaveEvent(int interval) : base(interval) { }

        protected override void Run()
        {
            Game.world.Save();

            if (Settings.showAutoSaveNotification)
            {
                NotificationHandler.Notify("sc:save", "Successfully Auto-Saved the world!", "general");
            }
        }
    }
}
