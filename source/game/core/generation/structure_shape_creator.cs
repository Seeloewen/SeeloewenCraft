using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class StructureShapeCreator
    {      
        static int rndOffset;

        //Used to get specific shapes of structure components for structures
        public List<StructureComponent> GetCircle(int radius)
        {
            //Midpoint circle algorithm, I'm absolutely not sure what I'm even doing here //Seeloewen
            int x_center = radius;
            int y_center = radius;
            int x = radius;
            int y = 0;
            int radius_error = 1 - x;

            List<StructureComponent> points = new List<StructureComponent>();

            while (x >= y)
            {
                AddSymmetricPoints(points, x_center, y_center, x, y);
                y++;
                if (radius_error < 0)
                {
                    radius_error += 2 * y + 1;
                }
                else
                {
                    x--;
                    radius_error += 2 * (y - x + 1);
                }
            }

            FillCircle(points, radius);
            return points;
        }

        private static void AddSymmetricPoints(List<StructureComponent> points, int xc, int yc, int x, int y)
        {
            //Add points
            points.Add(new StructureComponent(xc + x, yc + y, null));
            points.Add(new StructureComponent(xc + y, yc + x, null));
            points.Add(new StructureComponent(xc - y, yc + x, null));
            points.Add(new StructureComponent(xc - x, yc + y, null));
            points.Add(new StructureComponent(xc - x, yc - y, null));
            points.Add(new StructureComponent(xc - y, yc - x, null));
            points.Add(new StructureComponent(xc + y, yc - x, null));
            points.Add(new StructureComponent(xc + x, yc - y, null));
        }

        private static void FillCircle(List<StructureComponent> points, int radius)
        {
            //Fill the inside of the circle
            for (int y = 0; y <= radius * 2; y++)
            {
                for (int x = 0; x <= radius * 2; x++)
                {
                    if (IsInsideCircle(radius, x, y))
                    {
                        StructureComponent comp = new StructureComponent(x, y, null);
                        if (!points.Contains(comp))
                        {
                            points.Add(comp);
                        }
                    }
                }
            }
        }

        private static bool IsInsideCircle(int radius, int x, int y)
        {
            //Check if a point is inside the circle
            int centerX = radius;
            int centerY = radius;
            int dx = x - centerX;
            int dy = y - centerY;
            return (dx * dx + dy * dy) <= (radius * radius);
        }


        public List<StructureComponent> GetRectangle(int width, int height)
        {
            //Creates a rectangle based on the given parameters
            List<StructureComponent> components = new List<StructureComponent>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    components.Add(new StructureComponent(x, y, null));
                }
            }

            return components;
        }

        public List<StructureComponent> GetSquare(int length)
        {
            //Creates a square based on the specified length
            List<StructureComponent> components = new List<StructureComponent>();

            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    components.Add(new StructureComponent(x, y, null));
                }
            }

            return components;
        }

        public List<StructureComponent> GetCustomCircle(int radiusX, int radiusY)
        {
            //Creates a circle-like structure that is not actually round, with fixed X and Y radius. This will probably result in either the X or Y side having a flat cut-off
            List<StructureComponent> components = new List<StructureComponent>();

            int y;

            //Create upper left quarter of circle
            y = radiusY;
            for (int x = radiusX; x > 0; x--)
            {
                for (int j = y; j > 0; j--)
                {
                    components.Add(new StructureComponent(x, j, null));
                }
                if (GetRandomNumber(0, 1) == 0 && y >= 0) y--;
            }

            y = radiusY;
            //Create upper right quarter of circle
            for (int x = radiusX; x < radiusX * 2; x++)
            {
                for (int j = y; j > 0; j--)
                {
                    components.Add(new StructureComponent(x, j, null));
                }
                if (GetRandomNumber(0, 1) == 0 && y >= 0) y--;
            }

            //Create lower left quarter of circle
            y = radiusY;
            for (int x = radiusX; x > 0; x--)
            {
                for (int j = 0 - y; j < 0; j++)
                {
                    components.Add(new StructureComponent(x, j + 1, null));

                }
                if (GetRandomNumber(0, 1) == 0 && y >= 0) y--;
            }

            y = radiusY;
            //Create lower right quarter of circle
            for (int x = radiusX; x < radiusX * 2; x++)
            {
                for (int j = 0 - y; j < 0; j++)
                {
                    components.Add(new StructureComponent(x, j + 1, null));

                }
                if (GetRandomNumber(0, 1) == 0 && y >= 0) y--;
            }

            return components;

        }

        public int GetRandomNumber(int lowerBound, int upperBound)
        {
            rndOffset++;
            Random rnd = new Random(DateTime.Now.Millisecond + rndOffset);
            return rnd.Next(lowerBound, upperBound + 1);
        }
    }
}

