using SeeloewenCraft.game.ui.ui_lib;


namespace SeeloewenCraft.game.ui
{
    internal class InvSlotUI : Component
    {

        bool hovered;
        bool pressed;

        Color color = new Color(.5f, .5f, .5f);
        Color hoveredColor = new Color(.6f, .6f, .6f);
        Color pressedColor = new Color(.7f, .7f, .7f);


        CTexture cTexture;
        CText cAmountLabel;

        int x;
        int y;

        string itemID;
        int amount;


        internal InvSlotUI(int x, int y, TextureMap map) : base(calcBounds(x, y))
        {
            this.x = x;
            this.y = y;

            Rectangle texBounds = new Rectangle(bounds.x1P + 10, bounds.y1P + 10, bounds.x2P - 10, bounds.y2P - 10);

            cTexture = new CTexture(map, itemID, texBounds);
            AddChild(cTexture);

            TextLayout layout = new TextLayout(bounds.x2P - 8, TextHAlignment.RIGHT, bounds.y2P - 8, TextVAlignment.BOTTOM);
            cAmountLabel = new CText("69", 2, layout);
            AddChild(cAmountLabel);

        }

        protected override void OnUpdate()
        {
            itemID = Game.world.player.inventory.GetSlot(x, y).itemId;
            amount = Game.world.player.inventory.GetSlot(x, y).Amount;
            cTexture.setID(itemID);
            cAmountLabel.setText(amount == 0 ? "" : $"{amount}");

        }

        


        protected override void OnMouseEnter()
        {
            hovered = true;
            pressed = false;
        }

        protected override void OnMouseLeave()
        {
            hovered = false;
            pressed = false;
        }

        protected override void OnClickEvent(ClickEvent mouseClickEvent)
        {
            if (mouseClickEvent.button == ButtonType.LEFT)
            {
                if (mouseClickEvent.pressed)
                {
                    pressed = true;
                }
                else
                {
                    pressed = false;
                }


            }


        }



        protected override void OnRender()
        {

            if (pressed)
            {
                PrimitiveRenderer.DrawRectangle(bounds, pressedColor);
            }
            else if (hovered)
            {
                PrimitiveRenderer.DrawRectangle(bounds, hoveredColor);
            }
            else
            {
                PrimitiveRenderer.DrawRectangle(bounds, color);
            }

        }



        private static Rectangle calcBounds(int x, int y)
        {
            int startX = IS.mx - (9 * IS.slotSize + 8 * IS.edgeSize) / 2;
            int startY = IS.my - (4 * IS.slotSize + 4 * IS.edgeSize) / 2;

            int x1 = startX + (IS.edgeSize + IS.slotSize) * x;
            int y1 = startY + (IS.edgeSize + IS.slotSize) * y;
            if (y == 3) y1 += IS.edgeSize;
            int x2 = x1 + IS.slotSize;
            int y2 = y1 + IS.slotSize;

            return new Rectangle(x1, y1, x2, y2);
        }

    }
}
