using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeeloewenCraft { 
    public class WorldRenderer
    {
        
        wndGame wndGame;

        public double offsetX = 10; //block coordinates of center of frame
        public double offsetY = 20;

        public double playerPosX;
        public double playerPosY;

        List<Chunk> chunks;
        List<Entity> entities;

        public WorldRenderer(wndGame wndGame)
        {
            this.wndGame = wndGame;

            entities = new List<Entity>();
            chunks = new List<Chunk>();

        }

        public void Render()
        {
            offsetX = playerPosX;
            offsetY = playerPosY;

            foreach(Chunk chunk in chunks)
            {
                Canvas.SetLeft(chunk.grdChunk,(int) (600 - 50 * offsetX + 400 * chunk.index));
            }
            wndGame.svWorld.ScrollToVerticalOffset((int)((offsetY-6) * 50));

            Thickness currentMarginPlayer = wndGame.world.player.texture.Margin;
            currentMarginPlayer.Top = playerPosY * 50;
            currentMarginPlayer.Left = playerPosX * 50 - (offsetX-12) * 50;
            wndGame.world.player.texture.Margin = currentMarginPlayer;

            foreach(Entity entity in entities)
            {
                Thickness currentMargin = entity.texture.Margin;
                currentMargin.Top = entity.posY /20;
                currentMargin.Left = entity.posX/20 - (offsetX - 12) * 50;
                entity.texture.Margin = currentMargin;
            }


            if (Canvas.GetLeft(wndGame.world.loadedChunkList[3].grdChunk) <= 40)
            {
                //Save the chunk that has moved to far and remove it. Add a new one at the opposite site.
                Chunk removeChunk = wndGame.world.GetLoadedChunk(wndGame.world.loadedChunkList[0].index);
                wndGame.world.loadedChunkList.Remove(removeChunk);
                RemoveChunk(removeChunk);
                Chunk addChunk = wndGame.world.GetChunk(wndGame.world.loadedChunkList[5].index + 1);
                wndGame.world.LoadChunk(addChunk);
                try
                {
                    AddChunk(wndGame.world.loadedChunkList[6]);
                }
                catch { }


                //Sort the chunklist again
                wndGame.world.loadedChunkList = wndGame.world.loadedChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                Render();
            }
            else if (Canvas.GetLeft(wndGame.world.loadedChunkList[3].grdChunk) >= 800)
            {
                //Move the chunk on the right all the way to the left
                Chunk chunk = wndGame.world.GetLoadedChunk(wndGame.world.loadedChunkList[6].index);
                wndGame.world.UnloadChunk(chunk);
                RemoveChunk(chunk);
                wndGame.world.LoadChunk(wndGame.world.GetChunk(wndGame.world.loadedChunkList[0].index - 1));
                try
                {
                    AddChunk(wndGame.world.loadedChunkList[6]);
                }
                catch { }

                //Sort the list again
                wndGame.world.loadedChunkList = wndGame.world.loadedChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                Render();
            }

        }

        public (double, double) GetMouseOffset()
        {
            Point p = Mouse.GetPosition(wndGame.cvsWorld);


            double x = 20*(p.X + (offsetX-12)*50);
            double y = p.Y * 20;

            return ((double)x, (double)y);
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void Remove(Entity entitiy)
        {
            entities.Remove(entitiy);
        }


        public void AddChunk(Chunk chunk)
        {
            chunks.Add(chunk);
            wndGame.cvsWorld.Children.Add(chunk.grdChunk);
        }

        public void RemoveChunk(int id)
        {
            foreach(Chunk chunk in chunks)
            {
                if(chunk.index == id)
                {
                    chunks.Remove(chunk);
                    wndGame.cvsWorld.Children.Remove(chunk.grdChunk);
                }
            }
        }

        public void RemoveChunk(Chunk chunk)
        {
            chunks.Remove(chunk);
            wndGame.cvsWorld.Children.Remove(chunk.grdChunk);
        }
        
        
        
    }
}
