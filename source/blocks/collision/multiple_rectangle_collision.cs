using System;
using System.Collections.Generic;

namespace SeeloewenCraft
{
    internal class MultipleRectangleCollision : Collision
    {
        List<Collision> collisions;


        public MultipleRectangleCollision(int[] left, int[] right, int[] top, int[] bottom)
        {
            collisions = new List<Collision>();
            for (int i = 0; i < left.Length; i++)
            {
                collisions.Add(new RectangleCollision(left[i], right[i], top[i], bottom[i]));
            }
        }



        public override (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {
            bool collsionDetected = false;
            int m = int.MaxValue;
            foreach (Collision collision in collisions)
            {
                (bool c, int s) = collision.CheckCollision(direction, startX, endX, startY, endY);
                if (c)
                {
                    collsionDetected = true;
                    m = Math.Min(s, m);
                }
            }
            if (collsionDetected)
            {
                return (true, m);
            }
            else
            {
                return (false, 0);
            }
        }


    }
}
