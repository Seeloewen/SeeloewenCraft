using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.notifications
{
    public static class NotificationHandler
    {
        private static List<Notification> notifications = new List<Notification>();
        public static void Notify(string iconId, string message, string iconTexMap = "items")
        {
            Notification noti = new Notification(iconId, message, iconTexMap);
            notifications.Add(noti);
        }

        public static List<Notification> GetNotifications() => notifications;

        public static void Update(double dt)
        {
            for (int i = notifications.Count - 1; i >= 0; i--)
            {
                Notification n = notifications[i];

                n.Update(dt);
                if (n.lifetime <= 0) notifications.Remove(n); //Hide notification when lifetime exceeds 5s
            }


        }
    }
}
