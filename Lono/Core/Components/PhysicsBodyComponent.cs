using Lono.Data;

namespace Lono.Core.Components
{
    public class PhysicsBodyComponent : Component
    {
        public override string GetTypeIdentifier() => "core_physicsbody";
        public override string[] GetDependencies() => new string[] { "core_transform" };
        public override bool GetIsUnique() => true;

        public bool IsMoving => Velocity != Vector2.Zero;

        public Vector2 Velocity { get; set; }
    }
}
