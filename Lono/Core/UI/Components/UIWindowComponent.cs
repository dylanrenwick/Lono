using System.Collections.Generic;
using System.Linq;

using Lono.Data;

namespace Lono.Core.UI.Components
{
    public class UIWindowComponent : Component
    {
        private static readonly Vector2[] defaultGlyphVerts = new Vector2[]
        {
            new Vector2(3, 7),
            new Vector2(7, 3),
            new Vector2(3, 12),
            new Vector2(12, 3),
            new Vector2(3, 17),
            new Vector2(17, 3)
        };

        public override string GetTypeIdentifier() => "ui_window";
        public override string[] GetDependencies() => new string[] { "core_transform", "ui_ui", "ui_panel" };
        public override bool GetIsUnique() => true;

        public bool IsResizable { get; set; }
        public bool IsMoveable { get; set; }
        public bool DisplayTitle { get; set; }
        public bool LockToParent { get; set; }

        public bool BeingDragged { get; set; }
        public bool BeingResized { get; set; }

        public string Title { get; set; }

        public double TitleBarHeight { get; set; }
        public List<Vector2> ResizeGlyphVerts { get; set; } = defaultGlyphVerts.ToList();

        public Vector2 MinimumSize { get; set; } = new Vector2(0, 0);
        public Vector2 MaximumSize { get; set; } = new Vector2(0, 0);
    }
}
