using System.Collections.Generic;

using Lono.Data;

namespace Lono.Core.Components
{
    public abstract class ColliderComponent : Component
    {
        public override string[] GetDependencies() => new string[] { "core_transform" };

        public List<string> CollidingEntities { get; set; } = new List<string>();
        public List<string> NewCollidingEntities { get; set; } = new List<string>();

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public bool IsPhysical { get; set; } = true;
    }
}
