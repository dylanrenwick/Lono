using System;
using System.Linq;

using Lono.Data;
using Lono.Core.Components;
using Lono.Box2D.Components;

using Box2DX.Dynamics;

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

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            physicsWorld.Step((float)deltaTime.TotalMilliseconds, velocityIterations, positionIterations);

            foreach (string entityID in entities)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue;

                Box2DBodyComponent bodyComponent = entity.GetComponents<Box2DBodyComponent>("b2d_body").First();
                TransformComponent transform = entity.GetComponents<TransformComponent>("core_transform").First();
                if (bodyComponent.RigidBody == null)
                {
                    bodyComponent.RigidBody = CreateBody(transform.Position);
                }
                transform.Position = Util.Box2DVecToLonoVec(bodyComponent.RigidBody.GetPosition());
            }
        }

        public Box2DSystem(World world, int velIters = 8, int posIters = 3)
        {
            physicsWorld = world;

            velocityIterations = velIters;
            positionIterations = posIters;
        }

        private Body CreateBody(Vector2 pos)
        {
            BodyDef bodyDef = new BodyDef()
            {
                Position = Util.LonoVecToBox2DVec(pos),
            };
            return CreateBody(bodyDef);
        }
        private Body CreateBody(BodyDef bodyDef)
        {
            Body body = physicsWorld.CreateBody(bodyDef);
            FixtureDef fixDef = new FixtureDef();
            return body;
        }

        private 
    }
}
