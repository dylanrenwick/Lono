﻿using System;
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
    public class Box2DSystem : Box2DPhysicsSystem
    {
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
                bodyComponent.RigidBody = physicsWorld.CreateBody(transform.Position);
                bodyComponent.RigidBody.SetUserData(bodyComponent.ID);
            }

            List<Box2DShapeComponent> shapes = entity.GetComponents<Box2DShapeComponent>("b2d_shape");
            foreach (Box2DShapeComponent shape in shapes)
            {
                bodyComponent.RigidBody.AddShape(shape.CollisionShape);
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

        public Box2DSystem(World? world = null, int velIters = 8, int posIters = 3)
            :base(world)
        {
            velocityIterations = velIters;
            positionIterations = posIters;
        }
    }
}
