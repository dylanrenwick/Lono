using System.Collections.Generic;

using Lono.Data;

namespace Lono.Core.UI.Components
{
    public class UIComponent : Component
    {
        public override string GetTypeIdentifier() => "ui_ui";
        public override string[] GetDependencies() => new string[] { "core_transform" };
        public override bool GetIsUnique() => true;

        public bool Enabled { get; set; } = true;

        public Vector2 Size { get; set; }
        public SizeType WidthType { get; set; } = SizeType.Absolute;
        public SizeType HeightType { get; set; } = SizeType.Absolute;

        public string Parent { get; set; }
        public List<string> Children { get; set; } = new List<string>();

        public AnchorType Anchor { get; set; } = AnchorType.TopLeft;

        public enum AnchorType
        {
            TopLeft = 0,
            TopRight = 2,
            BottomRight = 4,
            BottomLeft = 6,
        }

        public enum SizeType
        {
            Absolute = 0,
            Relative = 1
        }
    }
}
