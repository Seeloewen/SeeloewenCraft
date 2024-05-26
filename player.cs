using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace SeeloewenCraft
{
    public class Player
    {
        public Canvas cvsPlayer = new Canvas();
        wndGame wndGame;
        public Inventory inventory;

        //-- Variables for physics --/

        //constants:
        private const double a_ground = 50; // a: acceleration
        private const double a_air = 50;
        private const double a_gravity = 70;
        private const double f_ground = 7; //f: friction
        private const double jump_start_speed = 15;
        private const double slowest_ground_speed = 3;

        //variables
        private double v_x = 0; //v: velocity
        private double v_y = 0;
        private int d_x = 0; // pixels to be moved
        private int d_y = 0;
        private double d_offset_x = 0;
        private double d_offset_y = 0;
        private double posX;
        private double posY;


        //Hitbox points

        //onGround points
        private Point pointGround1 = new Point(0, 95);
        private Point pointGround2 = new Point(44, 95);

        //touchingRight points
        private Point pointRight1 = new Point(45, 0);
        private Point pointRight2 = new Point(45, 48);
        private Point pointRight3 = new Point(45, 94);

        //touchingLeft points
        private Point pointLeft1 = new Point(-1, 0);
        private Point pointLeft2 = new Point(-1, 48);
        private Point pointLeft3 = new Point(-1, 94);

        //touchingTop points
        private Point pointTop1 = new Point(0, -1);
        private Point pointTop2 = new Point(44, -1);

        //-- Constructor --//

        public Player(wndGame wndGame, int x, int y)
        {
            //Set the attributes
            this.wndGame = wndGame;

            //Generate the player
            GeneratePlayer(x, y);
        }

        //-- Custom Methods --//

        public void GeneratePlayer(int x, int y)
        {
            //Setup the character canvas that is shown but does not count in movement checks
            cvsPlayer.Margin = new Thickness(x + 2, y + 5, 0, 0);
            cvsPlayer.Width = 45;
            cvsPlayer.Height = 95;
            cvsPlayer.Background = new SolidColorBrush(Colors.Red);
        }

        //physics
        public void physicsStep(bool pressedLeft, bool pressedRight, bool pressedUp, double dt)
        {
            // -- determine which sides of the player are touched by solid blocks --

            //reset
            bool onGround = false;
            bool touchingRight = false;
            bool touchingLeft = false;
            bool touchingTop = false;

            //flag to only calculate the player coordinates once
            bool coordsDetermined = false;

            //iterate over every solid block
            foreach (Chunk chunk in getCurrentChunks())
            {
                foreach (Block block in chunk.blockList)
                {
                    if (!block.isSolid) continue;

                    //Convert positions to screen coordinates
                    Point playerScreenPoint = wndGame.player.cvsPlayer.PointToScreen(new Point(0, 0));
                    Point blockScreenPoint = block.blockContainer.bdrBlock.PointToScreen(new Point(0, 0));

                    //Convert to coordinates considering scrolling
                    Point playerPosition = wndGame.svWorld.TranslatePoint(playerScreenPoint, wndGame.cvsWorld);
                    Point blockPosition = wndGame.svWorld.TranslatePoint(blockScreenPoint, wndGame.cvsWorld);

                    //calculate relative position of block from player (top-left corner)
                    Point blockOriginPoint = new Point(blockPosition.X - playerPosition.X, blockPosition.Y - playerPosition.Y);

                    //set flag to true if a point is inside of the block
                    onGround = onGround
                        || InBlock(blockOriginPoint, pointGround1)
                        || InBlock(blockOriginPoint, pointGround2);

                    touchingRight = touchingRight
                        || InBlock(blockOriginPoint, pointRight1)
                        || InBlock(blockOriginPoint, pointRight2)
                        || InBlock(blockOriginPoint, pointRight3);

                    touchingLeft = touchingLeft
                        || InBlock(blockOriginPoint, pointLeft1)
                        || InBlock(blockOriginPoint, pointLeft2)
                        || InBlock(blockOriginPoint, pointLeft3);

                    touchingTop = touchingTop
                        || InBlock(blockOriginPoint, pointTop1)
                        || InBlock(blockOriginPoint, pointTop2);


                    //determine coordinates of player through relative position to block (done only once)
                    if (!coordsDetermined)
                    {
                        posY = block.yPos - blockOriginPoint.Y / 50;
                        posX = block.xPos - blockOriginPoint.X / 50 + 8 * chunk.index;
                        coordsDetermined = true;
                    }
                }
            }

            // -- change velocity depending on inputs --
            if (pressedRight && !touchingRight)
            {
                v_x += onGround     //use different acceleration values depending on onGround
                    ? a_ground * dt
                    : a_air * dt;
            }
            if (pressedLeft && !touchingLeft)
            {
                v_x -= onGround
                    ? a_ground * dt
                    : a_air * dt;
            }

            //jump
            if (pressedUp && onGround)
            {
                v_y = jump_start_speed;
                onGround = false;
            }

            // -- friction --
            if (true) //normally: onGround
            {
                if (v_x > 0)
                {
                    //this reduces the velocity by f_ground * v_x * dt until v_x reaches a threshold with value slowest_ground_speed, when it gets set to zero
                    double v_reduction = f_ground * v_x * dt;
                    v_x -= Math.Max(Math.Min(slowest_ground_speed * dt, v_x), v_reduction);
                }
                else if (v_x < 0)
                {
                    double v_reduction = -f_ground * v_x * dt;
                    v_x += Math.Max(Math.Min(slowest_ground_speed * dt, -v_x), v_reduction);
                }
            }

            if(!onGround)
            {
                v_y -= a_gravity * dt; //no air resistance yet
            }

            // -- check if moving into blocks --
            if ((touchingRight && v_x > 0) || (touchingLeft && v_x < 0))
            {
                //set horizontal movement to zero
                v_x = 0;
                d_x = 0;
                d_offset_x = 0;
            }
            if ((onGround && v_y < 0) || (touchingTop && v_y > 0))
            {
                //set vertical movement to zero
                v_y = 0;
                d_y = 0;
                d_offset_y = 0;
            }

            //convert velocity to pixels
            d_offset_x += v_x * dt * 50;
            d_offset_y += v_y * dt * 50;

            //calculate actual pixels to be moved
            d_x = (int)Math.Floor(d_offset_x);
            d_y = (int)Math.Floor(d_offset_y);

            //store remaining pixels in offset
            d_offset_x -= d_x;
            d_offset_y -= d_y;

            // -- start of collision checking -- 
            // calculate that actual pixels only move to block border so the touching code above can calculate collsions
            // remaining amount gets added to offset to move in next physics step

            //right
            if (d_x > 1)
            {
                double startX = posX + 0.90;
                double endX = posX + 0.90 + (double)d_x / 50.0;
                double borderX = Math.Floor(endX);

                if (borderX > startX)
                {
                    d_x = Convert.ToInt32((borderX - startX) * 50);
                    d_offset_x += ((endX - borderX) * 50);
                }
            }
            //left
            else if (d_x < -1)
            {
                double startX = posX;
                double endX = posX + (double)d_x / 50.0;
                double borderX = Math.Ceiling(endX);

                if (borderX < startX)
                {
                    d_x = Convert.ToInt32((borderX - startX) * 50);
                    d_offset_x += ((endX - borderX) * 50);
                }
            }

            // top
            if (d_y > 1)
            {
                double startY = posY;
                double endY = posY - (double)d_y / 50.0;
                double borderY = Math.Ceiling(endY);

                if (borderY < startY)
                {
                    d_y = -Convert.ToInt32((borderY - startY) * 50);
                    d_offset_y += ((borderY - endY) * 50);
                }
            }
            // bottom
            else if (d_y < -1)
            {
                double startY = posY + 1.90;
                double endY = posY + 1.90 - (double)d_y / 50.0;
                double borderY = Math.Floor(endY);

                if (borderY > startY)
                {
                    d_y = -Convert.ToInt32((borderY - startY) * 50);
                    d_offset_y += ((endY - borderY) * 50);
                }
            }

            //move with amount of acual pixels
            MoveHorizontal(d_x);
            MoveVertical(d_y);
        }

        //block origin means top left corner of the block relative to the top left corner of player
        private bool InBlock(Point blockOrigin, Point point)
        {
            return blockOrigin.Y <= point.Y
                && blockOrigin.Y + 50 > point.Y
                && blockOrigin.X <= point.X
                && blockOrigin.X + 50 > point.X;
        }

        public void MoveHorizontal(int amount)
        {
            //Move all chunks the specified amount to the left
            foreach (Chunk chunk in wndGame.chunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) - amount);
            }

            //Sort the list to account for chunk movement
            wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

            //Check if the chunk has moved too far
            if (Canvas.GetLeft(wndGame.chunkList[2].grdChunk) <= 0)
            {
                double offset = -Canvas.GetLeft(wndGame.chunkList[2].grdChunk);
                //Save the chunk that has moved to far and remove it. Add a new one at the opposite site.
                wndGame.GetChunk(wndGame.chunkList[0].index).bgwSaveChunk.RunWorkerAsync();
                wndGame.chunkList.Remove(wndGame.GetChunk(wndGame.chunkList[0].index));
                wndGame.chunkList.Add(new Chunk(wndGame, wndGame.chunkList[3].index + 1));
                wndGame.cvsWorld.Children.Add(wndGame.chunkList[4].grdChunk);
                Canvas.SetLeft(wndGame.chunkList[4].grdChunk, 1200 - offset);

                //Sort the chunklist again
                wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }
            else if (Canvas.GetLeft(wndGame.chunkList[2].grdChunk) >= 800)
            {
                double offset = Canvas.GetLeft(wndGame.chunkList[2].grdChunk) - 800;
                //Move the chunk on the right all the way to the left
                wndGame.GetChunk(wndGame.chunkList[4].index).bgwSaveChunk.RunWorkerAsync();
                wndGame.chunkList.Remove(wndGame.GetChunk(wndGame.chunkList[4].index));
                wndGame.chunkList.Add(new Chunk(wndGame, wndGame.chunkList[0].index - 1));
                wndGame.cvsWorld.Children.Add(wndGame.chunkList[4].grdChunk);
                Canvas.SetLeft(wndGame.chunkList[4].grdChunk, -400 + offset);

                //Sort the list again
                wndGame.chunkList = wndGame.chunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }
        }



        public async void MoveVertical(int amount)
        {

            Thickness currentMarginPlayer = cvsPlayer.Margin;
            currentMarginPlayer.Top -= amount;
            cvsPlayer.Margin = currentMarginPlayer;

            //Scroll along to match the movement
            wndGame.svWorld.ScrollToVerticalOffset(wndGame.player.cvsPlayer.Margin.Top - 400);

            //Wait to allow UI update
            await Task.Delay(1);



        }



        public List<Chunk> getCurrentChunks()
        {
            //Create a list of the chunks the player is currently in by checking collision
            List<Chunk> chunkList = new List<Chunk>();
            foreach (Chunk chunk in wndGame.chunkList)
            {
                if (wndGame.GetRectangle(cvsPlayer).IntersectsWith(wndGame.GetRectangle(chunk.grdChunk)))
                {
                    chunkList.Add(chunk);
                }
            }

            return chunkList;
        }
    }
}
