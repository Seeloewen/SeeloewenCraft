namespace SeeloewenCraft.game.graphics;

internal interface IBatch
{
    internal void Fill(float[] vertices, int startIndex);

    static abstract internal int GetSize();

    static abstract internal VBLayout GetVBLayout();
}