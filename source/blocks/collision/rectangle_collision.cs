
namespace SeeloewenCraft
{
    class RectangleCollision : Collision
    {

        int left;
        int right;
        int top;
        int bottom;

        public RectangleCollision(int left, int right, int top, int bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }

        public override (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {

            if (direction == Direction.RIGHT)
            {
                if (startY < bottom && endY > top)
                {
                    if (startX >= left && startX < right)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (left <= 500 && endX > left)
                    {
                        //if movement starts before block and ends in or after block
                        //return collision:true and max movement: distance from startX to 0
                        return (true, left - startX);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }

            if (direction == Direction.LEFT)
            {
                if (startY < bottom && endY > top)
                {
                    if (startX > left && startX <= right)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startX > right && endX < right)
                    {
                        //if movement starts before block and ends in or after block 
                        //return collision:true and max movement: distance from startX to 1000
                        return (true, startX - right);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }


            if (direction == Direction.DOWN)
            {
                if (startX < right && endX > left)
                {
                    if (startY >= top && startY < bottom)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startY <= top && endY > top)
                    {
                        //if movement starts before block and ends in or after block
                        //return collision:true and max movement: distance from startY to 0
                        return (true, top - startY);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }

            if (direction == Direction.UP)
            {
                if (startX < right && endX > left)
                {
                    if (startY > 0 && startY <= 500)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startY > bottom && endY < bottom)
                    {
                        //if movement starts before block and ends in or after block 
                        //return collision:true and max movement: distance from startY to 1000
                        return (true, startY - bottom);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }

            //if no direction , maybe throw exception
            return (false, 0);

        }
    }
}
