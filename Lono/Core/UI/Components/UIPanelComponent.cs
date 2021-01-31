using System;
using System.Collections.Generic;
using System.Text;

using Lono.Data;

namespace Lono.Core.UI.Components
{
    public class UIPanelComponent : Component
    {
        public override string GetTypeIdentifier() => "ui_panel";
        public override string[] GetDependencies() => new string[] { "core_transform", "ui_ui" };
        public override bool GetIsUnique() => true;

        public Color BackgroundColor { get; set; }
    }
}
