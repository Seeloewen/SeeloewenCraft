

namespace SeeloewenCraft.game.ui.ui_lib
{
    public abstract class InputEvent
    {

        public bool consumed { get; private set; }

        protected InputEvent() { 
            consumed = false;
        }

        public void consume()
        {
            consumed = true;
        }

    }
}
