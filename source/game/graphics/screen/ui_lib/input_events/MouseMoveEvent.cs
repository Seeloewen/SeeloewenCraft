

namespace SeeloewenCraft.game.graphics.ui_lib
{

    /// <summary>
    /// Represents a mouse move event, fired when mouse position is changed
    /// </summary>
    public class MouseMoveEvent : InputEvent
    {

        /// <summary>
        /// New x position of mouse
        /// </summary>
        public int x { get; init; }

        /// <summary>
        /// New y position of mouse
        /// </summary>
        public int y { get; init; }

        internal MouseMoveEvent(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
