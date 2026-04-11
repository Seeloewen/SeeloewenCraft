using System;

namespace SeeloewenCraft.game.graphics.ui_lib;

public class CTextField : CText, TextReceiver
{
    int maxSize;
    private bool p_editMode;

    internal Action onBackspace;
    internal Action onEnter;
    internal Action onEscape;

    public bool editMode
    {
        get => p_editMode;
        set
        {
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

        onBackspace = OnBackspace;
        onEnter = OnEnter;
        onEscape = OnEscape;
    }

    protected override void OnClickEvent(ClickEvent mouseClickEvent)
    {
        if (mouseClickEvent.pressedLeft && mouseClickEvent.pressed) editMode = true;

    }

    public virtual void HandleEnter() => onEnter.Invoke();
    public virtual void HandleEscape() => onEscape.Invoke();
    public virtual void HandleBackspace() => onBackspace.Invoke();

    private void OnBackspace()
    {
        if (text.Length > 0) SetText(text.Remove(text.Length - 1, 1));
    }

    private void OnEnter()
    {
        SetText("");
        editMode = false;
    }

    private void OnEscape()
    {
        SetText("");
        editMode = false;
    }

    public void HandleChar(string c)
    {
        if (c.Length + text.Length <= maxSize)
        {
            SetText(text + c);
        }
    }
}