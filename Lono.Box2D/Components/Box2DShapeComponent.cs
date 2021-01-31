using System;
using System.Collections.Generic;
using System.Text;

using Lono.Data;

using Box2D.NetStandard.Collision.Shapes;

namespace Lono.Box2D.Components
{
    public class Box2DShapeComponent : Component
    {
        public override string GetTypeIdentifier() => "b2d_shape";

        public Shape CollisionShape { get; set; }
    }
}
