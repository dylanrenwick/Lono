using Lono.Data;

namespace Lono.Core.UI.Components
{
    public class UITextComponent : Component
    {
        public override string GetTypeIdentifier() => "ui_text";
        public override string[] GetDependencies() => new string[] { "ui_ui" };

        public string Text { get; set; }
    }
}
