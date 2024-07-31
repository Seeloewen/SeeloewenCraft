using System;
using System.Windows.Media;

namespace SeeloewenCraft
{
    class Zombie : MovingEntity
    {

        private Random rnd;

        private int timeSinceMove;


        public Zombie(int posX, int posY, int velX, int velY, World world)
            : base(20000, 900, 1800, posX, posY, velX, velY, world, Colors.LimeGreen)
        {
            rnd = new Random(DateTime.Now.Millisecond);
        }

        public override void DoPhysicsStep(int tps)
        {
            if (timeSinceMove > 1000)
            {
                int dir = rnd.Next(-1, 2);

                switch (dir)
                {
                    case 1:
                        pressedLeft = false;
                        pressedRight = true;
                        break;
                    case -1:
                        pressedLeft = true;
                        pressedRight = false;
                        break;
                    case 0:
                        pressedLeft = false;
                        pressedRight = false;
                        break;
                }
                timeSinceMove = 0;
            }

            timeSinceMove += 1000/tps;

            base.DoPhysicsStep(tps);
        }

    }
}
