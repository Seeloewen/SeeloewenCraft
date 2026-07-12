using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics.ui_lib;
using SeeloewenCraft.game.notifications;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics
{
    class GameOverlay : CRectangle
    {
        private readonly CNotification[] cNotifications;
        private readonly CHealthBar cHealthBar = new CHealthBar();

        public GameOverlay() : base(new Color(0f, 0f, 0f, 0f), new Rectangle(-1f, -1f, 1f, 1f))
        {
            cNotifications = new CNotification[9];
            for (int i = 0; i < cNotifications.Length; i++)
            {
                CNotification cNotification = new CNotification(Resolution.HEIGHT - (i + 1) * 45);
                cNotifications[i] = cNotification;
                AddChild(cNotification);
            }

            AddChild(cHealthBar);
        }

        protected override void OnUpdate(double dt)
        {
            List<Notification> notifications = NotificationManager.Get().GetActiveNotifications();

            for(int i = 0; i < 9; i++)
            {
                CNotification cn = cNotifications[i];

                if (notifications.Count <= i) //If there are less notifications than slots, hide the other slots
                {
                    cn.visible = false;
                    continue;
                }

                Notification n = notifications[i];
                cn.SetNotification(n);
                cn.visible = true;
            }

            cHealthBar.visible = World.Get().gamemode == Gamemode.Survival;
        }
    }
}
