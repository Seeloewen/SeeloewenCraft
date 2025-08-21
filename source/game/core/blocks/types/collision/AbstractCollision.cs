using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.blocks
{
    public abstract class Collision
    {

        public abstract (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY);
    }
}
