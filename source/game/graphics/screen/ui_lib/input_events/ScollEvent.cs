namespace SeeloewenCraft.game.graphics.ui_lib;

public class ScrollEvent : InputEvent
{
    public int offset { get; init; }

    internal ScrollEvent(int offset)
    {
        this.offset = offset;
    }

}