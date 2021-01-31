using System;
using System.Linq;

using Lono.Core.Components;
using Lono.Data;

namespace Lono.Core.Systems
{
    public class SpriteRenderSystem : Lono.Data.System
    {
        public override bool IsInterestedInEntity(Entity entity)
        {
            return entity.HasComponent("core_sprite") && entity.HasComponent("core_transform");
        }

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            foreach (string entityID in entities)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue;
                var sprites = entity.GetComponents<SpriteComponent>("core_sprite");
                var transform = entity.GetComponents<TransformComponent>("core_transform").First();
                foreach (SpriteComponent sprite in sprites)
                {
                    renderer.DrawSprite(sprite.Sprite, transform.Position);
                }
            }
        }
    }
}
