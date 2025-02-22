
namespace SeeloewenCraft.game.ui.ui_lib
{
    public class ClickEvent : InputEvent
    {

        public bool pressedLeft { get; init; }
        public bool pressedRight { get; init; }

        public ButtonType button { get; init; }

        public bool pressed { get; init; }

        internal ClickEvent(ButtonType button, bool pressed, bool pressedLeft, bool pressedRight)
        {
            this.pressed = pressed;
            this.button = button;
            this.pressedLeft = pressedLeft;
            this.pressedRight = pressedRight;
        }


    }
}
