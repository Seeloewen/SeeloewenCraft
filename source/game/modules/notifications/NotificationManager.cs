using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics;
using System.Collections.Generic;

namespace SeeloewenCraft.game.notifications
{
    public class NotificationManager : IGuiData
    {
        public string guiId { get; set; } = "notification_handler";
        public string tags { get; set; }

        private List<Notification> activeNotifications = new List<Notification>();
        private List<Notification> notifications = new List<Notification>();

        public void Notify(string iconId, string message, string iconTexMap = "items")
        {
            Notification noti = new Notification(iconId, message, iconTexMap);
            activeNotifications.Add(noti);
            notifications.Add(noti);
        }

        public List<Notification> GetActiveNotifications() => activeNotifications;
        public List<Notification> GetTotalNotifications() => notifications;

        public void Update(double dt)
        {
            for (int i = activeNotifications.Count - 1; i >= 0; i--)
            {
                Notification n = activeNotifications[i];

                n.Update(dt);
                if (n.lifetime <= 0) activeNotifications.Remove(n); //Hide notification when lifetime exceeds 5s
            }
        }

        public void Eliminate(Notification n)
        {
            notifications.Remove(n);
            activeNotifications.Remove(n);
        }

        public static NotificationManager Get() => World.Get().notificationManager;
    }
}
