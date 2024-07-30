
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Entity
    {
        protected World world;

        public string type;

        private const int accGrav = 70000;
        protected int frictionGround = 10;
        protected int frictionAir = 10;
        protected int frictionWater = 25;
        private const int slowestGroundSpeed = 100;

        public long lifeTime;

        private static int nextID = 0;
        public int id;

        public Canvas texture;

        public int sizeX;
        public int sizeY;

        public int posX;
        public int posY;

        public int velX;
        public int velY;

        public bool onGround;
        public bool touchingWater;

        private void DoFrictionStep(int tps)
        {
            if (touchingWater)
            {
                double velTotal = Math.Sqrt(velX * velX + velY * velY); //tactical pythagoras
                if (velTotal != 0)
                {
                    if (velX > 0)
                    {
                        velX -= (int)(frictionWater * velX / tps);
                    }
                    else if (velX < 0)
                    {
                        velX -= (int)(frictionWater * velX / tps);
                    }
                    if (velY > 0)
                    {

                        velY -= (int)(frictionWater * velY / tps);
                    }
                    else if (velY < 0)
                    {
                        velY -= (int)(frictionWater * velY / tps);
                    }
                }
            }
            else
            {
                if (onGround)
                {
                    if (velX > 0)
                    {
                        //this reduces the velocity by f_ground * v_x * dt until v_x reaches a threshold with value slowest_ground_speed, when it gets set to zero
                        int v_reduction = frictionGround * velX / tps;
                        velX -= Math.Max(Math.Min(slowestGroundSpeed, velX), v_reduction);
                    }
                    else if (velX < 0)
                    {
                        int v_reduction = -frictionGround * velX / tps;
                        velX += Math.Max(Math.Min(slowestGroundSpeed, -velX), v_reduction);
                    }
                }
                else
                {
                    int v_reduction = frictionAir * velX / tps;
                    velX -= v_reduction;
                }
            }
        }

        public virtual void DoPhysicsStep(int tps)
        {
            lifeTime += 1000 / tps;

            (onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);

            (touchingWater, int forceWaterX) = DoWaterTouchCheck();

            DoFrictionStep(tps);

            if (!onGround)
            {
                velY += accGrav / tps;
            }

            if (touchingWater)
            {
                velX += forceWaterX * 20000 / tps;
            }
            Move(tps);
        }

        protected void Move(int tps)
        {

            int dX = velX / tps;
            int dY = velY / tps;

            if (velX > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + dX, posY + sizeY);
                if (collided)
                {
                    velX = 0;
                    posX += maxMovement;
                    //MoveHorizontal(maxMovement / 20);
                }
                else
                {
                    posX += velX / tps;
                    //MoveHorizontal(dX / 20);
                }
            }
            if (velX < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.LEFT, posX, posY, posX + dX, posY + sizeY);
                if (collided)
                {
                    velX = 0;
                    posX -= maxMovement;
                    //MoveHorizontal(-maxMovement / 20);
                }
                else
                {
                    posX += velX / tps;
                    //MoveHorizontal(dX / 20);
                }
            }

            if (velY > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + dY);
                if (collided)
                {
                    velY = 0;
                    posY += maxMovement;
                    //MoveVertical(-maxMovement / 20);
                }
                else
                {
                    posY += velY / tps;
                    //MoveVertical(-dY / 20);
                }
            }
            if (velY < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.UP, posX, posY, posX + sizeX, posY + dY);
                if (collided)
                {
                    velY = 0;
                    posY -= maxMovement;
                    //MoveVertical(maxMovement / 20);
                }
                else
                {
                    posY += velY / tps;
                    //MoveVertical(-dY / 20);
                }
            }
        }


        public int ConvertToBlockX(int i)
        {
            return i >= 0
                ? i / 1000
                : (i - 999) / 1000;
        }
        public int GetChunkIndex()
        {
            return posX >= 0
                ? posX / 8000
                : (posX - 7999) / 8000;
        }

        protected (bool, int) DoWaterTouchCheck()
        {
            if (posY < 0 || posY > 75000) return (false, 0);

            bool touched = false;
            int totalForce = 0;

            for (int x = ConvertToBlockX(posX); x <= ConvertToBlockX(posX + sizeX - 1); x++)
            {
                for (int y = posY / 1000; y <= (posY + sizeY - 1) / 1000; y++)
                {

                    int startX = posX - x * 1000;
                    int endX = posX + sizeX - 1 - x * 1000;
                    int startY = posY - y * 1000;
                    int endY = posY + sizeY - 1 - y * 1000;
                    (bool touch, int xForce) = world.GetBlock(x, y).CheckWaterTouch(startX, startY, endX, endY);
                    if (touch)
                    {
                        touched = true;
                        if (xForce != 0)
                        {
                            if (xForce == 1)
                            {
                                totalForce = Math.Min(totalForce + 1, 1);
                            }
                            if (xForce == -1)
                            {
                                totalForce = Math.Max(totalForce - 1, -1);
                            }
                        }
                    }
                }
            }
            return (touched, totalForce);
        }

        protected (bool, int) DoCollisionCheck(Direction direction, int startX, int startY, int endX, int endY)
        {
            if (endY < 0 || endY > 75000 || posY < 0 || posY > 75000) return (false, 0);

            if (direction.IsRight())
            {
                for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int y = startY / 1000; y <= endY / 1000; y++)
                    {
                        Block b = world.GetBlock(x, y);
                        (bool newCollision, int newMaxMovement) = b.CheckCollision(Direction.RIGHT, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }
                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }

            if (direction.IsLeft())
            {
                for (int x = ConvertToBlockX(startX); x >= ConvertToBlockX(endX); x--)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int y = startY / 1000; y <= endY / 1000; y++)
                    {
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.LEFT, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }

                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }


            if (direction.IsDown())
            {
                for (int y = startY / 1000; y <= endY / 1000; y++)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                    {
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.DOWN, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }

                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }

            if (direction.IsUp())
            {
                for (int y = startY / 1000; y >= endY / 1000; y--)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                    {
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.UP, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }

                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }
            return (false, 0);
        }



        protected Entity(JsonToken token, int sizeX, int sizeY, World world, Color color)
            : this(sizeX, sizeY,
                token.GetInt("/posX"),
                token.GetInt("/posY"),
                token.GetInt("/velX"),
                token.GetInt("/velY"),
                world, color)
        {
            lifeTime = token.GetInt("/life_time");
        }

        public Entity(int sizeX, int sizeY, int posX, int posY, int velX, int velY, World world, Color color)
        {
            lifeTime = 0;
            this.id = nextID;
            nextID++;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.posX = posX;
            this.posY = posY;
            this.velX = velX;
            this.velY = velY;
            this.world = world;

            texture = new Canvas();
            texture.Margin = new Thickness(0, 0, 0, 0);
            texture.Width = sizeX / 20;
            texture.Height = sizeY / 20;
            texture.Background = new SolidColorBrush(color);

        }

        public static Entity LoadFromJson(JsonToken token, World world)
        {
            Entity entity = null;
            switch (token.GetString("/type"))
            {
                case "ItemEntity":
                    entity = new ItemEntity(token, world);
                    break;
                default:
                    throw new Exception();
            }

            nextID = Math.Max(nextID, entity.id);

            return entity;
        }

        public void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(type);
            writer.WritePropertyName("id");
            writer.WriteValue(id);
            writer.WritePropertyName("posX");
            writer.WriteValue(posX);
            writer.WritePropertyName("posY");
            writer.WriteValue(posY);
            writer.WritePropertyName("velX");
            writer.WriteValue(velX);
            writer.WritePropertyName("velY");
            writer.WriteValue(velY);
            writer.WritePropertyName("life_time");
            writer.WriteValue(lifeTime);

            SaveSpecialInfo(writer);
            writer.WriteEndObject();
        }

        protected virtual void SaveSpecialInfo(JsonWriter writer)
        {
            writer.WritePropertyName("type");
            writer.WriteValue("Entity");
        }



        public override bool Equals(object obj)
        {
            return (obj is Entity e && e.id == this.id);
        }

        public override int GetHashCode()
        {
            return (int)id;
        }
    }
}
