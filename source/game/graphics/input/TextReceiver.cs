namespace SeeloewenCraft.game.graphics;

public interface TextReceiver
{
    public void HandleChar(string text);
    public void HandleEnter();
    public void HandleEscape();
    public void HandleBackspace();

}