using Lono.Data;

namespace Lono.Core.Components
{
    public class RectColliderComponent : ColliderComponent
    {
        public override string GetTypeIdentifier() => "core_rectcollider";

        public Vector2 Size { get; set; }
    }
}
