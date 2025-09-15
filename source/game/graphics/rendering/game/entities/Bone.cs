namespace SeeloewenCraft.game.graphics;

public struct Bone
{
    internal float offsetX, offsetY;
    internal float rot;

    internal string id;
    private int x1, x2, y1, y2;
    
    internal Bone[] children;
}