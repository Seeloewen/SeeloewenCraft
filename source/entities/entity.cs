
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Entity
    {
        protected World world;

        private const int accGrav = 70000;
        protected int frictionGround = 10;
        protected int frictionAir = 10;
        private const int slowestGroundSpeed = 400;

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


        public static Entity LoadFromJson(JsonToken token, World world)
        {
            Entity entity = null;
            switch(token.GetString("/type"))
            {
                case "Entity":
                    entity = new Entity(token, world);
                    break;
                case "ItemEntity":
                    entity = new ItemEntity(token, world);
                    break;
            }
            entity.id = token.GetInt("/id");
            entity.posX = token.GetInt("/posX");
            entity.posY = token.GetInt("/posY");
            entity.velX = token.GetInt("/velX");
            entity.velY = token.GetInt("/velY");
            entity.lifeTime = token.GetInt("/life_time");

            nextID = Math.Max(nextID, entity.id);

            return entity;
        }

        public void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();
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


        public virtual void DoPhysicsStep(int tps)
        {
            lifeTime += 1000 / tps;

            (bool onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);

            // -- friction --
            if (onGround)
            {
                if (velX > 0)
                {
                    //this reduces the velocity by f_ground * v_x * dt until v_x reaches a threshold with value slowest_ground_speed, when it gets set to zero
                    int v_reduction = frictionGround * velX / tps;
                    velX -= Math.Max(Math.Min(slowestGroundSpeed / tps, velX), v_reduction);
                }
                else if (velX < 0)
                {
                    int v_reduction = -frictionGround * velX / tps;
                    velX += Math.Max(Math.Min(slowestGroundSpeed / tps, -velX), v_reduction);
                }
            }
            else
            {
                int v_reduction = frictionAir * velX / tps;
                velX -= v_reduction;
            }

            if (!onGround)
            {
                velY += accGrav / tps;
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

        protected int ConvertToBlockX(int i)
        {
            return i >= 0
                ? i / 1000
                : (i - 999) / 1000;
        }

        protected virtual (bool, int) DoCollisionCheck(Direction direction, int startX, int startY, int endX, int endY)
        {
            if (endY < 0 || endY > 75000) return (false, 0);

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

        protected Entity(JsonToken token, World world)
        {
            this.world = world;
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
