
namespace SeeloewenCraft
{

    public class EmtpyCollision : Collision
    {
        public override (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {
            return (false, 0);
        }

    }


    public class EntireBlockCollision : Collision
    {
        
        public override (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {
            
            if (direction == Direction.RIGHT)
            {
                if (startY < 1000 && endY > 0)
                {
                    if (startX >= 0 && startX < 1000)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startX <= 0 && endX > 0)
                    {
                        //if movement starts before block and ends in or after block
                        //return collision:true and max movement: distance from startX to 0
                        return (true, 0 - startX);
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
                if (startY < 1000 && endY > 0)
                {
                    if (startX > 0 && startX <= 1000)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startX > 1000 && endX < 1000)
                    {
                        //if movement starts before block and ends in or after block 
                        //return collision:true and max movement: distance from startX to 1000
                        return (true, startX - 1000);
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
                if (startX < 1000 && endX > 0)
                {
                    if (startY >= 0 && startY < 1000)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startY <= 0 && endY > 0)
                    {
                        //if movement starts before block and ends in or after block
                        //return collision:true and max movement: distance from startY to 0
                        return (true, 0 - startY);
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
                if (startX < 1000 && endX > 0)
                {
                    if (startY > 0 && startY <= 1000)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startY > 1000 && endY < 1000)
                    {
                        //if movement starts before block and ends in or after block 
                        //return collision:true and max movement: distance from startY to 1000
                        return (true, startY - 1000);
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
