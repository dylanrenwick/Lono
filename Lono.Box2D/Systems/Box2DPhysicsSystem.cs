using System;
using System.Numerics;

using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace Lono.Box2D.Systems
{
    public abstract class Box2DPhysicsSystem : Lono.Data.System
    {
        protected World physicsWorld;
        public World PhysicsWorld { get => physicsWorld; }

        public Box2DPhysicsSystem(World? world = null)
        {
            physicsWorld = world ?? new World();
        }

        public void PhysicsCast(Func<Fixture, bool> cb, float x, float y, float w = 0, float h = 0)
        {
            PhysicsCast(cb, new AABB(new Vector2(x, y), new Vector2(x + w, y + h)));
        }
        public void PhysicsCast(Func<Fixture, bool> cb, AABB aabb)
        {
            physicsWorld.QueryAABB(new World.QueryCallback(cb), aabb);
        }
    }
}
