
namespace SeeloewenCraft.game.ui.ui_lib
{
    /// <summary>
    /// Represents a mouse click event, fired when a button state changed
    /// </summary>
    public class ClickEvent : InputEvent
    {

        /// <summary>
        /// Flag if the left button is pressed
        /// </summary>
        public bool pressedLeft { get; init; }

        /// <summary>
        /// Flag if the right button is pressed
        /// </summary>
        public bool pressedRight { get; init; }

        /// <summary>
        /// Changed button
        /// </summary>
        public ButtonType button { get; init; }

        /// <summary>
        /// Pressed status of changed button
        /// </summary>
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
