
namespace SeeloewenCraft
{


    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    static class DirectionMethods
    {

        public static bool IsUp(this Direction d)
        {
            return d == Direction.UP;
        }

        public static bool IsDown(this Direction d)
        {
            return d == Direction.DOWN;
        }
        public static bool IsRight(this Direction d)
        {
            return d == Direction.RIGHT;
        }

        public static bool IsLeft(this Direction d)
        {
            return d == Direction.LEFT;
        }

    }

}
