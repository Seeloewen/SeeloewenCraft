using System.Windows.Media;

namespace SeeloewenCraft
{
    public class MovingEntity : Entity
    {
        protected double hp = 10.0;

        protected int accWalking = 70000;
        private const int jumpStartSpeed = 15000;

        public bool pressedUp;
        public bool pressedRight;
        public bool pressedLeft;

        public MovingEntity(int accWalking, int sizeX, int sizeY, int posX, int posY, int velX, int velY, World world, Brush image)
            : base(sizeX, sizeY, posX, posY, velX, velY, world, image)
        {
            this.accWalking = accWalking;

            texture.MouseLeftButtonDown += Texture_MouseLeftButtonDown;
        }

        //hit by player
        private void Texture_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int playerX = world.player.posX;
            int playerY = world.player.posY;

            if (playerX - (posX + sizeX) < Player.HIT_RANGE
                && playerY - (posY + sizeY) < Player.HIT_RANGE
                && (playerX + world.player.sizeX) - posX < Player.HIT_RANGE
                && (playerY +  world.player.sizeY) - posY < Player.HIT_RANGE)
            {
                Damage(Player.HIT_DAMAGE);
            }


        }

        public virtual void Die()
        {
            world.RemoveEntity(this);
        }

        public virtual void SetHP(double hp)
        {
            this.hp = hp;
            if(hp <= 0)
            {
                Die();
            }
        }

        public virtual void Damage(double damage)
        {
            SetHP(hp - damage);
        }

        public override void DoPhysicsStep(int tps)
        {
            // -- determine which sides of the player are touched by solid blocks --

            //reset
            (onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);
            (touchingWater, int forceWaterX) = DoWaterTouchCheck();
            (bool touchingLeft, _) = DoCollisionCheck(Direction.LEFT, posX, posY, posX - 1, posY + sizeY);
            (bool touchingRight, _) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + 1, posY + sizeY);


            // -- change velocity depending on inputs --
            if (pressedRight && !touchingRight)
            {
                velX += accWalking / tps;
            }
            if (pressedLeft && !touchingLeft)
            {
                velX -= accWalking / tps;
            }

            //jump
            if (pressedUp && onGround)
            {
                velY = -jumpStartSpeed;
            }

            if (touchingWater)
            {
                if (pressedUp)
                {
                    velY -= 200000 / tps;
                }
            }

            base.DoPhysicsStep(tps);
        }
    }
}
