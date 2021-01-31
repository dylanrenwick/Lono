using System;
using System.Linq;
using System.Collections.Generic;

using Lono.Data;
using Lono.Core.Components;
using Lono.Box2D.Components;

using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace Lono.Box2D.Systems
{
    public class Box2DSystem : Lono.Data.System
    {
        private World physicsWorld;

        private int velocityIterations;
        private int positionIterations;

        public override bool IsInterestedInEntity(Entity entity)
        {
            return entity.HasComponent("b2d_body");
        }

        public override void AddEntity(Entity entity)
        {
            base.AddEntity(entity);

            Box2DBodyComponent bodyComponent = entity.GetComponents<Box2DBodyComponent>("b2d_body").First();
            TransformComponent transform = entity.GetComponents<TransformComponent>("core_transform").First();
            if (bodyComponent.RigidBody == null)
            {
                bodyComponent.RigidBody = CreateBody(transform.Position);
            }

            List<Box2DShapeComponent> shapes = entity.GetComponents<Box2DShapeComponent>("b2d_shape");
            foreach (Box2DShapeComponent shape in shapes)
            {
                AddShapeToBody(bodyComponent.RigidBody, shape.CollisionShape);
            }
        }

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            physicsWorld.Step((float)deltaTime.TotalMilliseconds, velocityIterations, positionIterations);

            foreach (string entityID in entities)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue;

                Box2DBodyComponent bodyComponent = entity.GetComponents<Box2DBodyComponent>("b2d_body").First();
                TransformComponent transform = entity.GetComponents<TransformComponent>("core_transform").First();

                transform.Position = Util.NetVecToLonoVec(bodyComponent.RigidBody.GetPosition());
            }
        }

        public Box2DSystem(World? world, int velIters = 8, int posIters = 3)
        {
            physicsWorld = world ?? new World();

            velocityIterations = velIters;
            positionIterations = posIters;
        }

        private Body CreateBody(Vector2 pos)
        {
            BodyDef bodyDef = new BodyDef()
            {
                position = Util.LonoVecToNetVec(pos),
                type = BodyType.Dynamic
            };
            return CreateBody(bodyDef);
        }
        private Body CreateBody(BodyDef bodyDef)
        {
            return physicsWorld.CreateBody(bodyDef);
        }

        private Fixture AddShapeToBody(Body body, Shape shape)
        {
            FixtureDef fixDef = new FixtureDef()
            {
                shape = shape
            };
            return body.CreateFixture(fixDef);
        }
    }
}
