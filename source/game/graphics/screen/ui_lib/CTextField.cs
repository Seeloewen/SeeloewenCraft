using System.Linq;

namespace SeeloewenCraft.game.graphics.ui_lib;

public class CTextField : CText, TextReceiver
{
    int maxSize;
    private bool p_editMode;

    public bool editMode
    {
        get => p_editMode;
        set {
            if (value != editMode)
            {
                p_editMode = value;
                if (value)
                {
                    InputHandler.EnterTextMode(this);
                }
                else
                {
                    InputHandler.ExitTextMode();
                }
            }
        }
}


public CTextField(int fontSize, TextLayout textLayout, int maxSize) :
base("", fontSize, textLayout)
{
    this.maxSize = maxSize;
}

protected override void OnClickEvent(ClickEvent mouseClickEvent)
{
    if (mouseClickEvent.pressedLeft && mouseClickEvent.pressed) editMode = true;

}

public virtual void HandleBackspace()
{
    if (text.Length > 0) text = text.Remove(text.Length - 1, 1);
}

public virtual void HandleEnter()
{
    text = "";
    editMode = false;
}

public virtual void HandleEscape()
{
    text = "";
    editMode = false;
}

public void HandleChar(string c)
{
    if (c.Length + text.Length <= maxSize)
    {
        text += c;
    }
}
}