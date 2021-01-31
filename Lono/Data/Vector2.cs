using System;
using System.Collections.Generic;
using System.Text;

namespace Lono.Data
{
    public struct Vector2
    {
        public static Vector2 Zero => new Vector2(0, 0);

        public double X { get; set; }
        public double Y { get; set; }

        public Vector2 Sign => new Vector2(Math.Sign(X), Math.Sign(Y));
        public Vector2 Abs => new Vector2(Math.Abs(X), Math.Abs(Y));
        public Vector2 Normalized
        {
            get
            {
                double mag = Magnitude;
                return new Vector2(X / mag, Y / mag);
            }
        }

        public double SqrMagnitude => X * X + Y * Y;
        public double Magnitude =>Math.Sqrt(SqrMagnitude);

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector2 && (Vector2)obj == this) || base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"X: {X}, Y: :{Y}";
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }

        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }
        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }
    }
}
