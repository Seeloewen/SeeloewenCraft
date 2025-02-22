

namespace SeeloewenCraft.game.ui.ui_lib
{
    public class MouseMoveEvent : InputEvent
    {

        public int x { get; init; }
        public int y { get; init; }

        internal MouseMoveEvent(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
