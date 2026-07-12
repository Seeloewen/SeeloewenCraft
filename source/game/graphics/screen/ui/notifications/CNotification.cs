using SeeloewenCraft.game.graphics.ui_lib;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.graphics
{
    internal class CNotification : CRectangle
    {
        private readonly int yAnchor;

        private Notification notification;

        private readonly CTexture cIcon;
        private readonly CBorder cBorder;
        private readonly CText cNotificationText;
        private readonly CButton cCloseButton;

        private readonly bool isFixedSize = false;

        internal CNotification(int yAnchor) : base(new Color(0.69f), new Rectangle(0, 0, 0, 0))
        {
            this.yAnchor = yAnchor; //height - 45 bounds und height - 33 text sowie height -37 texture

            cBorder = new CBorder(3, new Color(0.2f));
            AddChild(cBorder);

            cNotificationText = new CText("", 2, new TextLayout(Resolution.WIDTH - 60, TextHAlignment.RIGHT, yAnchor + 12, TextVAlignment.TOP));
            AddChild(cNotificationText);

            cIcon = new CTexture("items", "", new Rectangle(0, yAnchor + 8, 24, yAnchor + 32));
            AddChild(cIcon);

            cCloseButton = new CButton(cCloseButton_OnClick, "X", 3, "sc:stone_block", "blocks", new Rectangle(0, yAnchor, 40, yAnchor + 40));
            AddChild(cCloseButton);
        }

        //For use in guis where fixed sizes are needed
        internal CNotification(int x, int y, int width, int height) : base(new Color(0.69f), new Rectangle(x, y, x + width, y + height))
        {
            isFixedSize = true;

            SetBounds(new Rectangle(x, y, x + width, y + height));

            cBorder = new CBorder(3, new Color(0.2f));
            AddChild(cBorder);

            cNotificationText = new CText("", 2, new TextLayout(x + 50, TextHAlignment.LEFT, y + 12, TextVAlignment.TOP));
            AddChild(cNotificationText);

            cIcon = new CTexture("items", "", new Rectangle(x + 10, y + 8, x + 34, y + 32));
            AddChild(cIcon);

            cCloseButton = new CButton(cCloseButton2_OnClick, "X", 3, "sc:stone_block", "blocks", new Rectangle(x + width - 40, y, x+width, y + 40));
            AddChild(cCloseButton);
        }

        internal void SetNotification(Notification n)
        {
            int textWidth = TextRenderer.GetWidth(n.content, 2);
            int rightAnchor = Resolution.WIDTH - 10;
            int leftAnchor = Resolution.WIDTH - textWidth - 100;

            notification = n;
            cNotificationText.SetText(n.content);

            //Only update bounds and positions if not at a fixed pos
            if (!isFixedSize)
            {
                SetBounds(new Rectangle(leftAnchor, yAnchor, rightAnchor, yAnchor + 40));
                cBorder.SetBounds(bounds);

                cIcon.MoveTo(leftAnchor + 7, cIcon.GetBounds().y1P);
                cCloseButton.MoveTo(rightAnchor - 40, cCloseButton.GetBounds().y1P);
            }

            cIcon.SetId(n.iconId);
            cIcon.SetMap(n.iconTexMap);
        }

        protected override void OnUpdate(double dt)
        {
            cCloseButton.visible = hovered;
        }

        private void cCloseButton_OnClick()
        {
            notification.lifetime = 0;
        }

        private void cCloseButton2_OnClick()
        {
            NotificationManager.Get().Eliminate(notification);
        }
    }
}
