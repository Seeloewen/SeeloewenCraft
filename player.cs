using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace SeeloewenCraft
{
    public class Player
    {
        public Canvas cvsPlayerHitbox = new Canvas();
        public Canvas cvsGravityHitbox = new Canvas();
        public Canvas cvsPlayer = new Canvas();
        wndGame wndGame;
        public Inventory inventory;
        private System.Windows.Forms.Timer tmrGravity = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer tmrJump = new System.Windows.Forms.Timer();
        private int jumpCount;
        private int halfedJumpCount;
        public bool isJumping;

        public Player(wndGame wndGame, int x, int y)
        {
            //Set the attributes
            this.wndGame = wndGame;

            //Generate the player
            GeneratePlayer(x, y);
        }

        public void GeneratePlayer(int x, int y)
        {
            //Setup the character canvas that is shown but does not count in movement checks
            cvsPlayer.Margin = new Thickness(x, y + 1, 0, 0);
            cvsPlayer.Width = 50;
            cvsPlayer.Height = 99;
            cvsPlayer.Background = new SolidColorBrush(Colors.Red);

            //Setup the character hitbox
            cvsPlayerHitbox.Margin = new Thickness(x, y + 1, 0, 0);
            cvsPlayerHitbox.Width = 40;
            cvsPlayerHitbox.Height = 89;
            cvsPlayerHitbox.Background = new SolidColorBrush(Colors.Purple);
            cvsPlayerHitbox.Visibility = Visibility.Hidden;

            //Setup the gravity hitbox
            cvsGravityHitbox.Margin = new Thickness(x + 2, y + 99, 0, 0);
            cvsGravityHitbox.Width = 46;
            cvsGravityHitbox.Height = 1;
            cvsGravityHitbox.Background = new SolidColorBrush(Colors.Green);
            cvsGravityHitbox.Visibility = Visibility.Hidden;

            //Setup the gravity timer
            tmrGravity.Interval = 16; // Approximately 60 FPS
            tmrGravity.Tick += tmrGravity_Tick;
            tmrGravity.Start();

            //Setup the jump timer
            tmrJump.Interval = 16; // Approximately 60 FPS
            tmrJump.Tick += tmrJump_Tick;
        }    

        public async void MoveRight(int amount)
        {
            //Move all chunks 5 to the left
            foreach (Chunk chunk in wndGame.chunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) - amount);
            }

            //Sort the chunklist to account for chunk movement
            wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

            //Check if the chunk has moved too far
            if (Canvas.GetLeft(wndGame.chunkList[2].grdChunk) == 0)
            {
                //Move the most left chunk all the way to the right
                wndGame.GetChunk(wndGame.chunkList[0].index).SaveChunk();
                wndGame.chunkList.Remove(wndGame.GetChunk(wndGame.chunkList[0].index));
                wndGame.chunkList.Add(new Chunk(wndGame, wndGame.chunkList[3].index + 1));
                wndGame.cvsWorld.Children.Add(wndGame.chunkList[4].grdChunk);
                Canvas.SetLeft(wndGame.chunkList[4].grdChunk, 1200);

                //Sort the chunklist again
                wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }

            //Wait to allow UI updates
            await Task.Delay(1);

            //Check each block for collision
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (PresentationSource.FromVisual(block.bdrBlock) != null)
                    {
                        if (BoundingBoxCollision(wndGame.GetRectangle(cvsPlayerHitbox), wndGame.GetRectangle(block.bdrBlock)) && block.isSolid == true)
                        {
                            //Move all chunks 5 to the right to basically reset action done earlier
                            foreach (Chunk chunk2 in wndGame.chunkList)
                            {
                                Canvas.SetLeft(chunk2.grdChunk, Canvas.GetLeft(chunk2.grdChunk) + amount);
                            }

                            //Sort the list to account for chunk movement
                            wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

                            //If the canvas moved too far
                            if (Canvas.GetLeft(wndGame.chunkList[2].grdChunk) == 0)
                            {
                                //Move the most right chunk all the way to the left
                                wndGame.GetChunk(wndGame.chunkList[4].index).SaveChunk();
                                wndGame.chunkList.Remove(wndGame.GetChunk(wndGame.chunkList[4].index));
                                wndGame.chunkList.Add(new Chunk(wndGame, wndGame.chunkList[3].index + 1));
                                wndGame.cvsWorld.Children.Add(wndGame.chunkList[4].grdChunk);
                                Canvas.SetLeft(wndGame.chunkList[4].grdChunk, 1200);

                                //Sort the list again
                                wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                            }
                            break;
                        }
                    }
                }
            }
        }

        public async void MoveLeft(int amount)
        {
            //Move all chunks 5 to the right
            foreach (Chunk chunk in wndGame.chunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) + amount);
            }

            //Sort the list to account for chunk movement
            wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

            //If the chunk has moved too far
            if (Canvas.GetLeft(wndGame.chunkList[2].grdChunk) == 800)
            {
                //Move the chunk on the right all the way to the left
                wndGame.GetChunk(wndGame.chunkList[4].index).SaveChunk();
                wndGame.chunkList.Remove(wndGame.GetChunk(wndGame.chunkList[4].index));
                wndGame.chunkList.Add(new Chunk(wndGame, wndGame.chunkList[0].index - 1));
                wndGame.cvsWorld.Children.Add(wndGame.chunkList[4].grdChunk);
                Canvas.SetLeft(wndGame.chunkList[4].grdChunk, -400);

                //Sort the list again
                wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }

            //Wait to allow UI update
            await Task.Delay(1);

            //Check each block for collision
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (PresentationSource.FromVisual(block.bdrBlock) != null)
                    {
                        if (BoundingBoxCollision(wndGame.GetRectangle(cvsPlayerHitbox), wndGame.GetRectangle(block.bdrBlock)) && block.isSolid == true)
                        {
                            //Move all chunks to the left
                            foreach (Chunk chunk2 in wndGame.chunkList)
                            {
                                Canvas.SetLeft(chunk2.grdChunk, Canvas.GetLeft(chunk2.grdChunk) - amount);
                            }

                            //Sort the list to account for the chunk movement
                            wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

                            //If the chunk moved too far
                            if (Canvas.GetLeft(wndGame.chunkList[2].grdChunk) == 800)
                            {
                                //Move the chunk on the right all the way to the left
                                wndGame.GetChunk(wndGame.chunkList[0].index).SaveChunk();
                                wndGame.chunkList.Remove(wndGame.GetChunk(wndGame.chunkList[0].index));
                                wndGame.chunkList.Add(new Chunk(wndGame, wndGame.chunkList[0].index - 1));
                                wndGame.cvsWorld.Children.Add(wndGame.chunkList[4].grdChunk);
                                Canvas.SetLeft(wndGame.chunkList[4].grdChunk, -400);

                                //Sort the list again
                                wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
                            }

                            break;
                        }
                    }
                }
            }
        }

        public async void MoveUp(double amount)
        {
            //Change the margin of all character components
            Thickness currentMargin = cvsPlayerHitbox.Margin;
            currentMargin.Top -= amount;
            cvsPlayerHitbox.Margin = currentMargin;
            Thickness currentMarginPlayer = cvsPlayer.Margin;
            currentMarginPlayer.Top -= amount;
            cvsPlayer.Margin = currentMarginPlayer;
            Thickness gravityMargin = cvsGravityHitbox.Margin;
            gravityMargin.Top -= amount;
            cvsGravityHitbox.Margin = gravityMargin;

            //Scroll along to match the movement
            wndGame.relativeSvPos -= 10;
            wndGame.svWorld.ScrollToVerticalOffset(wndGame.relativeSvPos);

            //Wait to allow UI update
            await Task.Delay(1);

            //Check each block for collision
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (PresentationSource.FromVisual(block.bdrBlock) != null)
                    {
                        if (BoundingBoxCollision(wndGame.GetRectangle(cvsPlayerHitbox), wndGame.GetRectangle(block.bdrBlock)) && block.isSolid == true)
                        {
                            //Reset previous movement
                            currentMargin.Top += amount;
                            cvsPlayerHitbox.Margin = currentMargin;
                            currentMarginPlayer.Top += amount;
                            cvsPlayer.Margin = currentMarginPlayer;
                            gravityMargin.Top += amount;
                            cvsGravityHitbox.Margin = gravityMargin;

                            //Scroll along to match movement
                            wndGame.relativeSvPos += 10;
                            wndGame.svWorld.ScrollToVerticalOffset(wndGame.relativeSvPos);

                            break;
                        }
                    }
                }
            }
        }

        public void MoveDown(double amount)
        {
            //Move all player components down
            Thickness currentMargin = cvsPlayerHitbox.Margin;
            currentMargin.Top += amount;
            cvsPlayerHitbox.Margin = currentMargin;
            Thickness currentMarginPlayer = cvsPlayer.Margin;
            currentMarginPlayer.Top += amount;
            cvsPlayer.Margin = currentMarginPlayer;
            Thickness gravityMargin = cvsGravityHitbox.Margin;
            gravityMargin.Top += amount;
            cvsGravityHitbox.Margin = gravityMargin;

            //Scroll along to match movement
            wndGame.relativeSvPos += 10;
            wndGame.svWorld.ScrollToVerticalOffset(wndGame.relativeSvPos);

            //Check blocks for collision
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (PresentationSource.FromVisual(block.bdrBlock) != null)
                    {
                        if (BoundingBoxCollision(wndGame.GetRectangle(cvsPlayerHitbox), wndGame.GetRectangle(block.bdrBlock)) && block.isSolid == true)
                        {
                            //Revert the previous movement
                            currentMargin.Top -= amount;
                            cvsPlayerHitbox.Margin = currentMargin;
                            currentMarginPlayer.Top -= amount;
                            cvsPlayer.Margin = currentMarginPlayer;
                            gravityMargin.Top -= amount;
                            cvsGravityHitbox.Margin = gravityMargin;

                            //Scroll along to account for movement
                            wndGame.relativeSvPos -= 10;
                            wndGame.svWorld.ScrollToVerticalOffset(wndGame.relativeSvPos);

                            break;
                        }
                    }
                }
            }
        }

        public void Jump()
        {
            //Stop the gravity
            tmrGravity.Stop();

            //Begin jumping
            isJumping = true;
            jumpCount = 15;
            halfedJumpCount = jumpCount / 2;
            tmrJump.Start();
        }

        public List<Chunk> getCurrentChunks()
        {
            //Create a list of the chunks the player is currently in by checking collision
            List<Chunk> chunkList = new List<Chunk>();
            foreach (Chunk chunk in wndGame.chunkList)
            {
                if (wndGame.GetRectangle(cvsGravityHitbox).IntersectsWith(wndGame.GetRectangle(chunk.grdChunk)))
                {
                    chunkList.Add(chunk);
                }
            }
            return chunkList;
        }

        private bool BoundingBoxCollision(Rect rectA, Rect rectB)
        {
            //Check for a literal collision (not even corners touching, but only overlappinng) by comparing the coordinates of the two rectangles
            if ((rectB.X + 45 >= rectA.X && rectB.X <= rectA.X + 45) && (rectB.Y + 45 >= rectA.Y && rectB.Y <= rectA.Y + 95))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoGravity()
        {
            //Create a list of current ground blocks by checking which blocks the gravity hitbox collides with
            List<Block> currentGroundBlocks = new List<Block>();
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (PresentationSource.FromVisual(block.bdrBlock) != null)
                    {
                        if (wndGame.GetRectangle(cvsGravityHitbox).IntersectsWith(wndGame.GetRectangle(block.bdrBlock)))
                        {
                            currentGroundBlocks.Add(block);
                        }
                    }
                }
            }

            //Check for each ground block if it's solid to determine whether it's non-solid
            int nonSolidGroundBlocks = 0;
            foreach (Block block in currentGroundBlocks)
            {
                if (block.isSolid == false)
                {
                    nonSolidGroundBlocks++;
                }
            }


            //If the amount of ground blocks matches the amount of non-solid blocks, it's safe to move down
            if (nonSolidGroundBlocks == currentGroundBlocks.Count)
            {
                MoveDown(10);
            }
        }


        public bool isOnFloor()
        {
            //Create a list of current ground blocks by checking collision
            List<Block> currentGroundBlocks = new List<Block>();
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (PresentationSource.FromVisual(block.bdrBlock) != null)
                    {
                        if (wndGame.GetRectangle(cvsGravityHitbox).IntersectsWith(wndGame.GetRectangle(block.bdrBlock)))
                        {
                            currentGroundBlocks.Add(block);
                        }
                    }
                }
            }

            //Check for each groundblock if it's solid. If at least one is solid, the ground is safe to stand one
            foreach (Block block in currentGroundBlocks)
            {
                if (block.isSolid == true)
                {
                    return true;
                }
            }
            return false;

        }

        private void tmrJump_Tick(object sender, EventArgs e)
        {
            //Check the jump count
            if (jumpCount > halfedJumpCount)
            {
                //If it's greater than half, move up
                MoveUp(10);
                jumpCount--;

            }
            else if (jumpCount <= halfedJumpCount && jumpCount > 0)
            {
                //If it's less than helf, start doing gravity
                DoGravity();
                jumpCount--;
            }
            else if (jumpCount <= 0)
            {
                //If the jumpcount is 0, stop the jump and restart the gravity timer
                tmrJump.Stop();
                tmrGravity.Start();
                isJumping = false;
            }
        }

        private void tmrGravity_Tick(object sender, EventArgs e)
        {
            //Do the gravity thing
            DoGravity();
        }
    }
}
