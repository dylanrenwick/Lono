using Lono.Data;

using Box2D.NetStandard.Dynamics.Bodies;

namespace Lono.Box2D.Components
{
    public class Box2DBodyComponent : Component
    {
        public override string GetTypeIdentifier() => "b2d_body";
        public override bool GetIsUnique() => true;
        public override string[] GetDependencies() => new string[1] { "core_transform" };

        public Body RigidBody { get; set; }

        public Vector2 Position
        {
            get => Util.NetVecToLonoVec(RigidBody.GetPosition());
            set { RigidBody.SetTransform(Util.LonoVecToNetVec(value), Angle); }
        }
        
        public float Angle
        {
            get => RigidBody.GetAngle();
            set { RigidBody.SetTransform(Util.LonoVecToNetVec(Position), value); }
        }
    }
}
