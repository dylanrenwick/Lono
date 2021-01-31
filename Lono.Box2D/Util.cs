using Lono.Data;

using Box2DX.Common;

namespace Lono.Box2D
{
    internal static class Util
    {
        public static Vector2 Box2DVecToLonoVec(Vec2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vec2 LonoVecToBox2DVec(Vector2 vec)
        {
            return new Vec2((float)vec.X, (float)vec.Y);
        }
    }
}
