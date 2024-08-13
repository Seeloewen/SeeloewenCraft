using System;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class MovingEntity : Entity
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);

        public const int MAX_STEPUP_HEIGHT = 505;

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

        protected override bool CheckUpStep(Direction direction, int remaining, int tps)
        {
            if (!onGround) return false;

            (_, int open) = direction.IsRight()
                ? DoCollisionCheck(Direction.DOWN, posX + sizeX, posY, posX + sizeX + 1, posY + sizeY)
                : DoCollisionCheck(Direction.DOWN, posX - 1, posY, posX, posY + sizeY);

            int amount = sizeY - open;
            if (amount <= MAX_STEPUP_HEIGHT)
            {
                bool collided;
                int maxMovement;
                if (direction.IsRight())
                {
                    (collided, maxMovement) = DoCollisionCheck(Direction.UP, posX + sizeX, posY, posX + sizeX + 1, posY - amount);
                }
                else
                {
                    (collided, maxMovement) = DoCollisionCheck(Direction.UP, posX - 1, posY, posX, posY - amount);
                }
                if (!collided) maxMovement = amount;
                if (maxMovement < amount)
                {
                    return false;
                }
                else
                {
                    Move(0, -maxMovement, tps);
                    Move(direction.IsRight() ? remaining : -remaining, 0, tps);
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        //hit by player
        private void Texture_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int playerX = world.player.posX;
            int playerY = world.player.posY;

            if (playerX - (posX + sizeX) < Player.HIT_RANGE
                && playerY - (posY + sizeY) < Player.HIT_RANGE
                && (playerX + world.player.sizeX) - posX < Player.HIT_RANGE
                && (playerY + world.player.sizeY) - posY < Player.HIT_RANGE)
            {
                Damage(Player.HIT_DAMAGE);
            }


        }

        public virtual void Drop(string id)
        {
            world.AddEntity(new ItemEntity(ItemRegister.GenerateItem(id, world), posX + sizeX / 2 - ItemEntity.itemSizeX / 2, posY + sizeY * 2 / 3 - ItemEntity.itemSizeY / 2, rnd.Next(-6000, 6000), rnd.Next(-15000, -10000), world));
        }

        public virtual void Die()
        {
            world.RemoveEntity(this);
        }

        public virtual void SetHP(double hp)
        {
            this.hp = hp;
            if (hp <= 0)
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
            if (pressedRight)
            {
                if (!touchingRight || CheckUpStep(Direction.RIGHT, 1, tps))
                {
                    velX += accWalking / tps;
                }
            }
            if (pressedLeft)
            {
                if (!touchingLeft || CheckUpStep(Direction.LEFT, -1, tps))
                {
                    velX -= accWalking / tps;
                }
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
