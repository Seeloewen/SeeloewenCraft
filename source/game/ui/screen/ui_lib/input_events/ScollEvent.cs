using System.Windows.Controls.Primitives;

namespace SeeloewenCraft.game.ui.ui_lib;

public class ScrollEvent : InputEvent
{
    public int offset { get; init; }

    internal ScrollEvent(int offset)
    {
        this.offset = offset;
    }
    
}