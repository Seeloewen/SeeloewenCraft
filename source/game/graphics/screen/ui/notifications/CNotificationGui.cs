using SeeloewenCraft.game.graphics.ui_lib;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.graphics
{
    internal class CNotificationGui : CGui
    {
        internal CBorder cBorder;
        internal CText cHeader;
        internal CScrollPane cNotifications;

        internal CNotificationGui() : base(NotificationManager.Get(), new Color(0.82f), new Rectangle(0, 0, 0, 0))
        {
            int width = 500;
            int height = 700;

            SetBounds(new Rectangle(GuiSizes.mx - width / 2, GuiSizes.my - height / 2, GuiSizes.mx + width / 2, GuiSizes.my + height / 2));

            cHeader = new CText("Notifications", 2, new TextLayout(GetBounds().x1P + 20, TextHAlignment.LEFT, GetBounds().y1P + 15, TextVAlignment.TOP));
            AddChild(cHeader);

            cNotifications = new CScrollPane(new Color(0.9f), new Rectangle(GetBounds().x1P + 20, GetBounds().y1P + 40, GetBounds().x2P - 20, GetBounds().y2P - 20), 500);
            AddChild(cNotifications);

            cBorder = new CBorder(5, new Color(0.5f));
            AddChild(cBorder);
        }

        protected override void OnUpdate(double dt)
        {
            cNotifications.ClearScrollables();

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
