
using System;
using System.Windows;

namespace SeeloewenCraft.game.graphics
{
    public record Vector2f(float x, float y)
    {

        public Vector2f(double x, double y) : this((float)x, (float)y) { }

        public Vector2f Rotate(float angle)
        {
            return new Vector2f(x * Math.Cos(angle) - y * Math.Sin(angle), x * Math.Sin(angle) + y * Math.Cos(angle));
        }

        public Vector2f Scale(float sX, float sY)
        {
            return new Vector2f(x*sX, y*sY);
        }

        public Vector2f Scale(float s)
        {
            return new Vector2f(x * s, y * s);
        }

        public Vector2f Add(double x, double y)
        {
            return Add(new Vector2f(x, y));
        }

        public Vector2f Add(float x, float y)
        {
            return Add(new Vector2f(x, y));
        }

        public Vector2f Add(Vector2f v)
        {
            return new Vector2f(x + v.x, y + v.y);
        }

        public static void Test()
        {
            var v = new Vector2f(1.0, 1.0);
            v = v.Rotate((float)Math.PI / 4);
            MessageBox.Show(v.ToString());
        }

        public override string ToString()
        {
            return $"({x};{y})";
        }

    }
}
