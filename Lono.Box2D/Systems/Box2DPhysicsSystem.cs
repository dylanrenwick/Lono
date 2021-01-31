using System;
using System.Numerics;

using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Fixtures;

using Lono.Data;
using Lono.Box2D.Components;

using Vector2 = System.Numerics.Vector2;

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

        public Entity GetEntityFromFixture(Fixture fix)
        {
            string entityID = fix.Body.GetUserData<string>();
            return Scene.GetEntity(entityID);
        }
    }
}
