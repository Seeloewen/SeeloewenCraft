using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.util
{
    internal static class Raycast
    {
        private static bool IsSolid(int x, int y) => World.Get().GetBlock(PositionData.FromGlobalX(x, y)).isSolid;

        public static List<Point> Cast(int x1, int y1, int x2, int y2)
        {
            List<Point> points = new List<Point>();

            //If cast from right to left, swap variables as algorithm only works for left to right
            if (x2 < x1)
            {
                int a = x1;
                x1 = x2;
                x2 = a;
            }

            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = 2 * dy - dx;

            Point p = new Point(x1, y1);
            points.Add(p);

            while(p.x < x2)
            {
                p = new Point(p.x + 1, p.y);
                if(d >0)
                {
                    p = new Point(p.x, p.y + 1);
                    d = d - 2 * dx;
                }
                d = d + 2 * dy;
                points.Add(p);
            }

            return points;
        }

        public static bool CanSee(int x1, int y1, int x2, int y2)
        {
            List<Point> points = Cast(x1, y1, x2, y2);
            foreach(Point p in points)
            {
                if (IsSolid(p.x, p.y)) return false;
            }
            return true;
        }
    }
}
