using System;
using System.Collections.Generic;
using System.Text;

using Lono.Data;

namespace Lono.Core.UI.Components
{
    public class UIButtonComponent : Component
    {
        public override string GetTypeIdentifier() => "ui_button";
        public override string[] GetDependencies() => new string[] { "ui_ui" };

        public bool ButtonPressed { get; set; }
        public bool ButtonDown { get; set; }
    }
}
