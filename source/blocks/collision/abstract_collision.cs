using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public abstract class Collision
    {

        public abstract (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY);


    }
}
