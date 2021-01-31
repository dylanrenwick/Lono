using System;
using System.Linq;

using Lono.Data;
using Lono.Core.UI.Components;

namespace Lono.Core.UI.Systems
{
    public class UICloseWindowSystem : Lono.Data.System
    {
        public override bool IsInterestedInEntity(Entity entity)
        {
            return entity.HasComponent("ui_closewindow");
        }

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            foreach (string entityID in entities)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue;

                var closeWindow = entity.GetComponents<UICloseWindowComponent>("ui_closewindow").First();
                var button = entity.GetComponents<UIButtonComponent>("ui_button").First();
                if (!button.ButtonPressed) continue;

                Entity windowEntity;

                if (!string.IsNullOrWhiteSpace(closeWindow.WindowID))
                {
                    windowEntity = Scene.GetEntity(closeWindow.WindowID);
                }
                else
                {
                    var ui = entity.GetComponents<UIComponent>("ui_ui").First();

                    windowEntity = LonoUI.GetParentWindow(Scene, ui);
                }

                if (windowEntity != null)
                {
                    var windowUI = windowEntity.GetComponents<UIComponent>("ui_ui").First();
                    windowUI.Enabled = false;
                }
            }
        }
    }
}
