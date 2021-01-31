using System;
using System.Linq;
using System.Collections.Generic;

using Lono.Data;
using Lono.Core.Components;

namespace Lono.Core.Systems
{
    public class CollisionSystem : Lono.Data.System
    {
        protected List<string> circles = new List<string>();
        protected List<string> rects = new List<string>();

        private List<string> temp = new List<string>();

        public override bool IsInterestedInEntity(Entity entity)
        {
            return entity.HasComponent("core_transform") && 
            (
                entity.HasComponent("core_circlecollider") ||
                entity.HasComponent("core_rectcollider")
            );
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.HasComponent("core_circlecollider")) circles.Add(entity.ID);
            if (entity.HasComponent("core_rectcollider")) rects.Add(entity.ID);
        }

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            foreach (string entityID in circles)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue; // Entity was removed

                TransformComponent transform = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (var circle in entity.GetComponents<CircleColliderComponent>("core_circlecollider"))
                {
                    if (circle.Radius == 0 || !circle.IsPhysical) continue;
                    ProcessCircleCollider(transform, circle, entityID);
                }
            }
            foreach (string entityID in rects)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue; // Entity was removed

                TransformComponent transform = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (var rect in entity.GetComponents<RectColliderComponent>("core_rectcollider"))
                {
                    if (rect.Size.SqrMagnitude == 0 || !rect.IsPhysical) continue;
                    ProcessRectCollider(transform, rect, entityID);
                }
            }
        }

        private void ProcessCircleCollider(TransformComponent transform, CircleColliderComponent collider, string entityID)
        {
            temp.Clear();

            foreach (string entityIDB in circles)
            {
                if (entityIDB == entityID) continue;
                Entity entity = Scene.GetEntity(entityIDB);
                if (entity == null) continue; // Entity was removed

                TransformComponent transformB = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (var circle in entity.GetComponents<CircleColliderComponent>("core_circlecollider"))
                {
                    if (circle.Radius == 0 || collider.ID == circle.ID) continue;
                    if (Physics.CircleCollidesCircle(transform.Position + collider.Offset, transformB.Position + circle.Offset, collider, circle)) temp.Add(entityIDB);
                }
            }

            foreach (string entityIDB in rects)
            {
                if (entityIDB == entityID) continue;
                Entity entity = Scene.GetEntity(entityIDB);
                if (entity == null) continue; // Entity was removed

                TransformComponent transformB = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (var rect in entity.GetComponents<RectColliderComponent>("core_rectcollider"))
                {
                    if (rect.Size.SqrMagnitude == 0) continue;
                    if (Physics.CircleCollidesRect(transform.Position + collider.Offset, transformB.Position + rect.Offset, collider, rect)) collider.CollidingEntities.Add(entityIDB);
                }
            }

            collider.NewCollidingEntities = temp.Where(c => !collider.CollidingEntities.Contains(c)).ToList();
            collider.CollidingEntities = new List<string>(temp);
        }
        private void ProcessRectCollider(TransformComponent transform, RectColliderComponent collider, string entityID)
        {
            temp.Clear();

            foreach (string entityIDB in circles)
            {
                if (entityIDB == entityID) continue;
                Entity entity = Scene.GetEntity(entityIDB);
                if (entity == null) continue; // Entity was removed

                TransformComponent transformB = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (var circle in entity.GetComponents<CircleColliderComponent>("core_circlecollider"))
                {
                    if (circle.Radius == 0) continue;
                    if (Physics.CircleCollidesRect(transformB.Position + circle.Offset, transform.Position + collider.Offset, circle, collider)) temp.Add(entityIDB);
                }
            }

            foreach (string entityIDB in rects)
            {
                if (entityIDB == entityID) continue;
                Entity entity = Scene.GetEntity(entityIDB);
                if (entity == null) continue; // Entity was removed

                TransformComponent transformB = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (var rect in entity.GetComponents<RectColliderComponent>("core_rectcollider"))
                {
                    if (rect.Size.SqrMagnitude == 0 || collider.ID == rect.ID) continue;
                    if (Physics.RectCollidesRect(transform.Position + collider.Offset, transformB.Position + rect.Offset, collider, rect)) temp.Add(entityIDB);
                }
            }

            collider.NewCollidingEntities = temp.Where(c => !collider.CollidingEntities.Contains(c)).ToList();
            collider.CollidingEntities = new List<string>(temp);
        }
    }
}
