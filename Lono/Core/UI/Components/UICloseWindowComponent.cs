using System;
using System.Collections.Generic;
using System.Text;

using Lono.Data;

namespace Lono.Core.UI.Components
{
    public class UICloseWindowComponent : Component
    {
        public override string GetTypeIdentifier() => "ui_closewindow";
        public override string[] GetDependencies() => new string[] { "ui_button" };

        public string WindowID { get; set; }
    }
}
