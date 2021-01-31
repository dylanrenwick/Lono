using Lono.Data;

using Box2DX.Dynamics;

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
            get => Util.Box2DVecToLonoVec(RigidBody.GetPosition());
            set { RigidBody.SetPosition(Util.LonoVecToBox2DVec(value)); }
        }
        
        public float Angle
        {
            get => RigidBody.GetAngle();
            set { RigidBody.SetAngle(value); }
        }
    }
}
