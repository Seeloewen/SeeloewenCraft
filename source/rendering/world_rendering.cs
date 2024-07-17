using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SeeloewenCraft { 
    public class WorldRenderer
    {
        
        wndGame wndGame;

        public double offsetX = 10; //block coordinates of center of frame
        public double offsetY = 20;

        List<Chunk> chunks;

        public WorldRenderer(wndGame wndGame)
        {
            this.wndGame = wndGame;

            chunks = new List<Chunk>();

        }

        public void Render()
        {
            foreach(Chunk chunk in chunks)
            {
                Canvas.SetLeft(chunk.grdChunk,(int) (600 - 50 * offsetX + 400 * chunk.index));
            }
            wndGame.svWorld.ScrollToVerticalOffset((int)((offsetY-7) * 50));

            Thickness currentMarginPlayer = wndGame.world.player.cvsPlayer.Margin;
            currentMarginPlayer.Top = offsetY * 50 - 55;
            wndGame.world.player.cvsPlayer.Margin = currentMarginPlayer;


            if (Canvas.GetLeft(wndGame.world.currentChunkList[2].grdChunk) <= 0)
            {
                //Save the chunk that has moved to far and remove it. Add a new one at the opposite site.
                Chunk removeChunk = wndGame.world.GetFromCurrentChunks(wndGame.world.currentChunkList[0].index);
                wndGame.world.currentChunkList.Remove(removeChunk);
                RemoveChunk(removeChunk);
                Chunk addChunk = wndGame.world.GetChunk(wndGame.world.currentChunkList[3].index + 1);
                wndGame.world.currentChunkList.Add(addChunk);
                try
                {
                    AddChunk(wndGame.world.currentChunkList[4]);
                }
                catch { }


                //Sort the chunklist again
                wndGame.world.currentChunkList = wndGame.world.currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                Render();
            }
            else if (Canvas.GetLeft(wndGame.world.currentChunkList[2].grdChunk) >= 800)
            {
                //Move the chunk on the right all the way to the left
                Chunk chunk = wndGame.world.GetFromCurrentChunks(wndGame.world.currentChunkList[4].index);
                wndGame.world.currentChunkList.Remove(chunk);
                RemoveChunk(chunk);
                wndGame.world.currentChunkList.Add(wndGame.world.GetChunk(wndGame.world.currentChunkList[0].index - 1));
                try
                {
                    AddChunk(wndGame.world.currentChunkList[4]);
                }
                catch { }

                //Sort the list again
                wndGame.world.currentChunkList = wndGame.world.currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                Render();
            }


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
