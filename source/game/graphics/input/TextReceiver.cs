namespace SeeloewenCraft.game.graphics;

public interface TextReceiver
{
    public void HandleChar(string text);
    public void HandleEscape();
    public void HandleEnter();
    public void HandleBackspace();

}