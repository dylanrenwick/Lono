using System.Linq;

using Lono.Data;
using Lono.Core.Components;
using Lono.Core.UI.Components;

namespace Lono
{
    public static class LonoUI
    {
        public static Entity GetParentWindow(Scene scene, UIComponent ui)
        {
            Entity parent;

            do
            {
                parent = scene.GetEntity(ui.Parent);
                if (parent == null || !parent.HasComponent("ui_ui")) return null;

                ui = parent.GetComponents<UIComponent>("ui_ui").First();
            } while (!parent.HasComponent("ui_window"));

            return parent;
        }

        public static Vector2 GetAbsolutePos(Scene scene, Entity uiEntity)
        {
            Entity parentEntity = null;
            Vector2 totalOffset = new Vector2(0, 0);

            do
            {
                var ui = uiEntity.GetComponents<UIComponent>("ui_ui").First();
                var transform = uiEntity.GetComponents<TransformComponent>("core_transform").First();
                UIComponent.AnchorType anchorType = ui.Anchor;

                Vector2 parentSize;
                if (!string.IsNullOrWhiteSpace(ui.Parent))
                {
                    parentEntity = scene.GetEntity(ui.Parent);
                    parentSize = GetAbsoluteSize(scene, parentEntity);
                }
                else
                {
                    parentEntity = null;
                    parentSize = new Vector2(scene.ScreenWidth, scene.ScreenHeight);
                }

                switch (anchorType)
                {
                    case UIComponent.AnchorType.TopRight:
                        totalOffset += parentSize * new Vector2(1, 0) + transform.Position * new Vector2(-1, 1);
                        break;
                    case UIComponent.AnchorType.BottomRight:
                        totalOffset += parentSize * new Vector2(1, 1) + transform.Position * new Vector2(-1, -1);
                        break;
                    case UIComponent.AnchorType.BottomLeft:
                        totalOffset += parentSize * new Vector2(0, 1) + transform.Position * new Vector2(1, -1);
                        break;
                    default:
                        totalOffset += transform.Position;
                        break;
                }

                uiEntity = parentEntity;
            } while (parentEntity != null);

            return totalOffset;
        }

        public static Vector2 GetAbsoluteSize(Scene scene, Entity uiEntity)
        {
            Vector2 totalSize = new Vector2(0, 0);

            var ui = uiEntity.GetComponents<UIComponent>("ui_ui").First();

            Vector2 parentSize;
            if (!string.IsNullOrWhiteSpace(ui.Parent))
            {
                Entity parentEntity = scene.GetEntity(ui.Parent);
                parentSize = GetAbsoluteSize(scene, parentEntity);
            }
            else
            {
                parentSize = new Vector2(scene.ScreenWidth, scene.ScreenHeight);
            }

            switch (ui.WidthType)
            {
                case UIComponent.SizeType.Absolute:
                    totalSize.X = ui.Size.X;
                    break;
                case UIComponent.SizeType.Relative:
                    totalSize.X = parentSize.X * ui.Size.X;
                    break;
                default:
                    break;
            }
            switch (ui.HeightType)
            {
                case UIComponent.SizeType.Absolute:
                    totalSize.Y = ui.Size.Y;
                    break;
                case UIComponent.SizeType.Relative:
                    totalSize.Y = parentSize.Y * ui.Size.Y;
                    break;
                default:
                    break;
            }

            return totalSize;
        }
    }
}
