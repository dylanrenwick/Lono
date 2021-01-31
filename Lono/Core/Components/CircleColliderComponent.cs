namespace Lono.Core.Components
{
    public class CircleColliderComponent : ColliderComponent
    {
        public override string GetTypeIdentifier() => "core_circlecollider";

        public double Radius { get; set; }
    }
}
