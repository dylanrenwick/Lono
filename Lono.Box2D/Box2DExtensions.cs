using System;

using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;

using Vector2 = Lono.Data.Vector2;
using Vec2 = System.Numerics.Vector2;

namespace Lono.Box2D
{
    public static class Box2DExtensions
    {
        public static Body CreateBody(this World world, Vector2 pos)
        {
            BodyDef bodyDef = new BodyDef()
            {
                position = Util.LonoVecToNetVec(pos),
                type = BodyType.Dynamic
            };
            return world.CreateBody(bodyDef);
        }

        public static Fixture AddShape(this Body body, Shape shape)
        {
            FixtureDef fixDef = new FixtureDef()
            {
                shape = shape,
                density = 1.0f
            };
            return body.CreateFixture(fixDef);
        }
    }
}
