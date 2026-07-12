using SeeloewenCraft.game.graphics.ui_lib;
using SeeloewenCraft.game.notifications;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal class CNotificationGui : CGui
    {
        internal CBorder cBorder;
        internal CText cHeader;
        internal CScrollPane cNotifications;

        private int notificationAmount = -1; //Shouldn't be zero so it triggers an initial update

        internal CNotificationGui() : base(NotificationManager.Get(), new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            int width = 500;
            int height = 700;

            SetBounds(new Rectangle(GuiSizes.mx - width / 2, GuiSizes.my - height / 2, GuiSizes.mx + width / 2, GuiSizes.my + height / 2));

            cHeader = new CText("Notifications", 2, new TextLayout(GetBounds().x1P + 20, TextHAlignment.LEFT, GetBounds().y1P + 15, TextVAlignment.TOP));
            AddChild(cHeader);

            cNotifications = new CScrollPane(new Color(0.9f), new Rectangle(GetBounds().x1P + 20, GetBounds().y1P + 40, GetBounds().x2P - 20, GetBounds().y2P - 20), 0);
            AddChild(cNotifications);

            cBorder = new CBorder(5, new Color(0.5f));
            AddChild(cBorder);
        }

        protected override void OnUpdate(double dt)
        {
            NotificationManager nm = NotificationManager.Get();

            //Only clear and update the menu when a change in notification amount occurs
            //This is necessary because hover and click events won't be handled otherwise as they're constantly reset
            if (nm.GetNotificationAmount() == notificationAmount) return;

            cNotifications.ClearScrollables();
            notificationAmount = nm.GetNotificationAmount();
            cNotifications.maxI = Math.Max(notificationAmount * 50 - height + 80, 0);

            if (notificationAmount == 0)
            {
                CText text = new CText("No notifications yet", 2, new TextLayout(GetBounds().x1P + 150, TextHAlignment.LEFT, GetBounds().y1P + 60, TextVAlignment.TOP));
                cNotifications.AddScrollable(text);
            }
            else
            {
                int y = 60;
                foreach (Notification n in NotificationManager.Get().GetTotalNotifications())
                {
                    CNotification cNoti = new CNotification(GetBounds().x1P + 30, y, 440, 40);
                    cNoti.SetNotification(n);
                    y += 50;
                    cNotifications.AddScrollable(cNoti);
                }
            }
        }
    }
}
