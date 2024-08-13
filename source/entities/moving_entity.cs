using System;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class MovingEntity : Entity
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);

        public const int MAX_STEPUP_HEIGHT = 505;

        public const double MAX_HP = 10.0;
        public double hp;

        protected int accWalking = 70000;
        private const int jumpStartSpeed = 15000;

        private int fallMaxHeight = 0;

        public bool pressedUp;
        public bool pressedRight;
        public bool pressedLeft;

        bool touchingRight;
        bool touchingLeft;



        public MovingEntity(int accWalking, int sizeX, int sizeY, int posX, int posY, int velX, int velY, World world, Brush image)
            : base(sizeX, sizeY, posX, posY, velX, velY, world, image)
        {
            this.accWalking = accWalking;
            hp = MAX_HP;

            texture.MouseLeftButtonDown += Texture_MouseLeftButtonDown;
        }

        protected override void DoFallDamage()
        {
            int fallHeight = posY - fallMaxHeight;
            if (fallHeight > 3950)
            {
                Damage((fallHeight - 2000) / 3000.0);
                Log.Write($"new hp after fall damage applied: {hp}", "Info");
            }
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
            if (this is not Player)
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
        }

        public override void OnUpdate(int tps) //temporary because player death is not handled properly
        {
            if (hp != 0)
            {
                base.OnUpdate(tps);
            }
        }

        protected override void OnUpdateEnd(int tps)
        {
            if (velY <= 0)
            {
                fallMaxHeight = posY;
            }

            base.OnUpdateEnd(tps);
        }

        public virtual void Drop(string id)
        {
            world.AddEntity(new ItemEntity(ItemRegister.GenerateItem(id, world), //item type
                posX + sizeX / 2 - ItemEntity.itemSizeX / 2, //posX
                posY + sizeY * 2 / 3 - ItemEntity.itemSizeY / 2, //posY
                rnd.Next(-6000, 6000), rnd.Next(-15000, -10000), //velX and velY
                world));
        }

        public virtual void Die()
        {
            world.toDieEntities.Add(this);
        }

        public virtual void SetHP(double hp)
        {
            this.hp = Math.Min(hp, MAX_HP);
            if (hp <= 0)
            {
                Die();
            }
        }

        public virtual void Heal(double amount)
        {
            SetHP(hp + amount);
        }

        public virtual void Damage(double damage)
        {
            SetHP(hp - damage);
        }

        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);
            (touchingLeft, _) = DoCollisionCheck(Direction.LEFT, posX, posY, posX - 1, posY + sizeY);
            (touchingRight, _) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + 1, posY + sizeY);

            if (touchingStatus[TOUCHING_CACTUS])
            {
                Damage((1000 / tps) * 0.001);
            }
        }

        public override void DoPhysicsStep(int tps)
        {
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

            //handle jump key
            if (pressedUp)
            {
                if (onGround)
                {
                    velY = -jumpStartSpeed;
                }

                if (touchingStatus[TOUCHING_WATER])
                {
                    velY -= 200000 / tps;
                }
            }

            base.DoPhysicsStep(tps);
        }
    }
}
