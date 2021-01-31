using Lono.Data;
using Lono.Core.Components;

namespace Lono
{
    public static class Physics
    {
        public static bool CircleCollidesCircle(Vector2 aPos, Vector2 bPos, CircleColliderComponent a, CircleColliderComponent b)
        {
            return (aPos - bPos).Magnitude < a.Radius + b.Radius;
        }
        public static bool CircleCollidesRect(Vector2 aPos, Vector2 bPos, CircleColliderComponent a, RectColliderComponent b)
        {
            if (PointCollidesRect(bPos, b.Size, aPos)) return true;

            double testX = aPos.X;
            double testY = aPos.Y;

            if (aPos.X < bPos.X) testX = bPos.X;
            else if (aPos.X > bPos.X + b.Size.X) testX = bPos.X + b.Size.X;

            if (aPos.Y < bPos.Y) testY = bPos.Y;
            else if (aPos.Y > bPos.Y + b.Size.Y) testY = bPos.Y + b.Size.Y;

            Vector2 testVec = new Vector2(testX, testY);
            Vector2 distVec = aPos - testVec;

            return distVec.Magnitude < a.Radius;
        }
        public static bool RectCollidesRect(Vector2 aPos, Vector2 bPos, RectColliderComponent a, RectColliderComponent b)
        {
            return aPos.X + a.Size.X < bPos.X ||
                aPos.X >= bPos.X + b.Size.X ||
                aPos.Y + a.Size.Y < bPos.Y ||
                aPos.Y >= bPos.Y + b.Size.Y;
        }

        public static bool PointCollidesRect(Vector2 rectPos, Vector2 rectSize, Vector2 point)
        {
            return point.X >= rectPos.X && point.X < rectPos.X + rectSize.X && point.Y >= rectPos.Y && point.Y < rectPos.Y + rectSize.Y;
        }
        public static bool PointCollidesCircle(Vector2 circleOrigin, double circleRadius, Vector2 point)
        {
            return (circleOrigin - point).Abs.SqrMagnitude < (circleRadius * circleRadius);
        }
    }
}
