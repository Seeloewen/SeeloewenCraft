
namespace SeeloewenCraft.game.ui.ui_lib
{
    /// <summary>
    /// Abstract base class for all input events that can be consumed
    /// </summary>
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
