
namespace SeeloewenCraft
{
    public abstract class Collision
    {

        public abstract (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY);
    }
}
