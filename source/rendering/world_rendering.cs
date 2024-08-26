using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SeeloewenCraft.entity;

namespace SeeloewenCraft
{
    public class WorldRenderer
    {
        public double offsetX = 10; //block coordinates of center of frame
        public double offsetY = 20;

        public double playerPosX;
        public double playerPosY;

        List<Chunk> chunks;
        List<Entity> entities;

        public WorldRenderer()
        {
            entities = new List<Entity>();
            chunks = new List<Chunk>();
        }

        public void Render()
        {
            offsetX = playerPosX;
            offsetY = playerPosY;

            foreach (Chunk chunk in chunks)
            {
                Canvas.SetLeft(chunk.grdChunk, (int)(600 - 50 * offsetX + 400 * chunk.index));
            }
            Game.world.wndGame.svWorld.ScrollToVerticalOffset((int)((offsetY - 6) * 50));

            Thickness currentMarginPlayer = Game.world.player.texture.Margin;
            currentMarginPlayer.Top = playerPosY * 50;
            currentMarginPlayer.Left = playerPosX * 50 - (offsetX - 12) * 50;
            Game.world.player.texture.Margin = currentMarginPlayer;

            foreach (Entity entity in entities)
            {
                Thickness currentMargin = entity.texture.Margin;
                currentMargin.Top = entity.posY / 20;
                currentMargin.Left = entity.posX / 20 - (offsetX - 12) * 50;
                entity.texture.Margin = currentMargin;
            }


            if (Canvas.GetLeft(Game.world.loadedChunkList[3].grdChunk) <= 0)
            {
                //Save the chunk that has moved to far and remove it. Add a new one at the opposite site.
                Game.world.MoveLoadedChunks(Direction.RIGHT);
            }
            else if (Canvas.GetLeft(Game.world.loadedChunkList[3].grdChunk) >= 800)
            {
                //Move the chunk on the right all the way to the left
                Game.world.MoveLoadedChunks(Direction.LEFT);
            }

        }

        public (double, double) GetMouseOffset()
        {
            Point p = Mouse.GetPosition(Game.world.wndGame.cvsWorld);


            double x = 20 * (p.X + (offsetX - 12) * 50);
            double y = p.Y * 20;

            return ((double)x, (double)y);
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
            Game.world.wndGame.cvsWorld.Children.Add(entity.texture);
            Panel.SetZIndex(entity.texture, 1);
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
            Game.world.wndGame.cvsWorld.Children.Remove(entity.texture);
        }


        public void AddChunk(Chunk chunk)
        {
            chunks.Add(chunk);
            Game.world.wndGame.cvsWorld.Children.Add(chunk.grdChunk);
        }

        public void RemoveChunk(int id)
        {
            foreach (Chunk chunk in chunks)
            {
                if (chunk.index == id)
                {
                    chunks.Remove(chunk);
                    Game.world.wndGame.cvsWorld.Children.Remove(chunk.grdChunk);
                }
            }
        }

        public void RemoveChunk(Chunk chunk)
        {
            chunks.Remove(chunk);
            Game.world.wndGame.cvsWorld.Children.Remove(chunk.grdChunk);
        }
    }
}
