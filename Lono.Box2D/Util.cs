using Vector2 = Lono.Data.Vector2;
using Vec2 = System.Numerics.Vector2;

namespace Lono.Box2D
{
    internal static class Util
    {
        public static Vector2 NetVecToLonoVec(Vec2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vec2 LonoVecToNetVec(Vector2 vec)
        {
            return new Vec2((float)vec.X, (float)vec.Y);
        }
    }
}
