using Lono.Data;

namespace Lono.Core.Components
{
    public class TransformComponent : Component
    {
        public override string GetTypeIdentifier() => "core_transform";
        public override bool GetIsUnique() => true;

        public Vector2 Position { get; set; }
    }
}
