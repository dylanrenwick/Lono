using System;
using System.Linq;

using Lono.Data;
using Lono.Core.Components;

namespace Lono.Core.Systems
{
    public class PhysicsSystem : Lono.Data.System
    {
        public override bool IsInterestedInEntity(Entity entity)
        {
            return entity.HasComponent("core_physicsbody") && entity.HasComponent("core_transform");
        }

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            foreach (string entityID in entities)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue; // Entity was removed

                PhysicsBodyComponent physicsBody = entity.GetComponents<PhysicsBodyComponent>("core_physicsbody").First();
                TransformComponent transform = entity.GetComponents<TransformComponent>("core_transform").First();
                if (!physicsBody.IsMoving) continue; // Motor isn't going anywhere

                Vector2 velocity = physicsBody.Velocity;
                transform.Position += velocity;
            }
        }
    }
}
