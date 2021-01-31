using Lono.Core.Assets;
using Lono.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lono.Core.Components
{
    public class SpriteComponent : Component
    {
        public override string GetTypeIdentifier() => "core_sprite";
        public override string[] GetDependencies() => new string[] { "core_transform" };

        public SpriteAsset Sprite { get; set; }
    }
}
