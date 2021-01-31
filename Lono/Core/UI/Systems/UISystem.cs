using System;
using System.Linq;

using Lono.Data;
using Lono.Core.Components;
using Lono.Core.UI.Components;

namespace Lono.Core.UI.Systems
{
    public class UISystem : Lono.Data.System
    {
        public override bool IsInterestedInEntity(Entity entity)
        {
            return entity.HasComponent("ui_ui") && string.IsNullOrWhiteSpace(entity.GetComponents<UIComponent>("ui_ui").First().Parent);
        }

        public override void Update(TimeSpan deltaTime, IRenderWrapper renderer, IInputWrapper input)
        {
            foreach (string entityID in entities)
            {
                Entity entity = Scene.GetEntity(entityID);
                if (entity == null) continue;

                var ui = entity.GetComponents<UIComponent>("ui_ui").First();

                TraverseChildren(entity, ui, (e, uiC) =>
                {
                    var transform = e.GetComponents<TransformComponent>("core_transform").First();
                    HandleUIComponentInput(input, uiC, e, transform);
                    RenderUIComponent(renderer, uiC, e, transform);
                });
            }
        }

        public void TraverseChildren(Entity entity, UIComponent ui, Action<Entity, UIComponent> pred, int maxDepth = 0, int currentDepth = 0)
        {
            pred(entity, ui);

            foreach (string entityID in ui.Children)
            {
                Entity childEntity = Scene.GetEntity(entityID);
                if (childEntity == null) continue;

                var childUI = childEntity.GetComponents<UIComponent>("ui_ui").First();
                pred(childEntity, childUI);
                if (maxDepth == 0 || currentDepth < maxDepth) TraverseChildren(childEntity, childUI, pred, maxDepth, currentDepth + 1);
            }
        }

        private void RenderUIComponent(IRenderWrapper renderer, UIComponent ui, Entity entity, TransformComponent transform)
        {
            if (entity.HasComponent("ui_panel"))
            {
                var panel = entity.GetComponents<UIPanelComponent>("ui_panel").First();
                RenderUIPanel(renderer, ui, panel, entity, transform);
            }
            if (entity.HasComponent("ui_text"))
            {
                var text = entity.GetComponents<UITextComponent>("ui_text").First();
                RenderUIText(renderer, ui, text, entity, transform);
            }
            if (entity.HasComponent("ui_window"))
            {
                var window = entity.GetComponents<UIWindowComponent>("ui_window").First();
                RenderUIWindow(renderer, ui, window, entity, transform);
            }
        }
        private void RenderUIPanel(IRenderWrapper renderer, UIComponent ui, UIPanelComponent panel, Entity entity, TransformComponent transform)
        {
            Vector2 pos = LonoUI.GetAbsolutePos(Scene, entity);
            Vector2 size = LonoUI.GetAbsoluteSize(Scene, entity);
            renderer.FillRect(pos, size, panel.BackgroundColor);
        }
        private void RenderUIWindow(IRenderWrapper renderer, UIComponent ui, UIWindowComponent window, Entity entity, TransformComponent transform)
        {
            Vector2 position = LonoUI.GetAbsolutePos(Scene, entity);
            Vector2 size = LonoUI.GetAbsoluteSize(Scene, entity);

            if (window.DisplayTitle)
            {
                renderer.DrawText(window.Title, position + new Vector2(10, 10));
            }

            if (window.IsResizable)
            {
                var glyphVerts = window.ResizeGlyphVerts;
                var origin = position + size;

                for (int i = 0; i + 1 < glyphVerts.Count; i += 2)
                {
                    renderer.DrawLine(origin - glyphVerts[i], origin - glyphVerts[i + 1], new Color() { R = 255, G = 255, B = 255, A = 255 });
                }
            }
        }
        private void RenderUIText(IRenderWrapper renderer, UIComponent ui, UITextComponent text, Entity entity, TransformComponent transform)
        {
            Vector2 position = LonoUI.GetAbsolutePos(Scene, entity);
            Vector2 size = LonoUI.GetAbsoluteSize(Scene, entity);

            renderer.DrawText(text.Text, position, size);
        }

        private void HandleUIComponentInput(IInputWrapper input, UIComponent ui, Entity entity, TransformComponent transform)
        {
            if (entity.HasComponent("ui_window"))
            {
                var window = entity.GetComponents<UIWindowComponent>("ui_window").First();
                HandleUIWindowInput(input, ui, window, entity, transform);
            }
            if (entity.HasComponent("ui_button"))
            {
                var button = entity.GetComponents<UIButtonComponent>("ui_button").First();
                HandleUIButtonInput(input, ui, button, entity, transform);
            }
        }
        private void HandleUIWindowInput(IInputWrapper input, UIComponent ui, UIWindowComponent window, Entity entity, TransformComponent transform)
        {
            if (!window.IsResizable && !window.IsMoveable) return;

            Vector2 size = LonoUI.GetAbsoluteSize(Scene, entity);
            bool mouseDown = input.GetButtonDown(0);

            if (!mouseDown)
            {
                window.BeingDragged = false;
                window.BeingResized = false;
            }

            if (window.BeingDragged)
            {
                transform.Position += input.MouseDelta;
            }
            else if (window.BeingResized)
            {
                var newSize = size + input.MouseDelta;
                if (newSize.X < window.MinimumSize.X) newSize.X = window.MinimumSize.X;
                if (newSize.Y < window.MinimumSize.Y) newSize.Y = window.MinimumSize.Y;
                if (window.MaximumSize.SqrMagnitude > 0)
                {
                    if (newSize.X > window.MaximumSize.X) newSize.X = window.MaximumSize.X;
                    if (newSize.Y > window.MaximumSize.Y) newSize.Y = window.MaximumSize.Y;
                }
                ui.Size = newSize;
            }
            else if (input.GetButtonPressed(0))
            {
                Vector2 pos = LonoUI.GetAbsolutePos(Scene, entity);

                if (Physics.PointCollidesRect(pos, new Vector2(size.X, window.TitleBarHeight), input.MousePos))
                {
                    window.BeingDragged = true;
                }
                else
                {
                    Vector2 resizeSize = new Vector2(20, 20);
                    Vector2 resizePos = pos + size - resizeSize;

                    if (Physics.PointCollidesRect(resizePos, resizeSize, input.MousePos))
                    {
                        window.BeingResized = true;
                    }
                }
            }

            if (window.LockToParent)
            {
                HandleUIWindowConstraints(ui, window, transform, size);
            }
        }
        private void HandleUIButtonInput(IInputWrapper input, UIComponent ui, UIButtonComponent button, Entity entity, TransformComponent transform)
        {
            Vector2 buttonPos = LonoUI.GetAbsolutePos(Scene, entity);

            bool mousePressed = input.GetButtonPressed(0);
            Vector2 size = LonoUI.GetAbsoluteSize(Scene, entity);
            bool mouseInButton = Physics.PointCollidesRect(buttonPos, size, input.MousePos);

            if (mousePressed)
            {
                button.ButtonPressed = mouseInButton;
                button.ButtonDown = mouseInButton;
            }
            else
            {
                button.ButtonPressed = false;
                button.ButtonDown = false;
            }
        }

        private void HandleUIWindowConstraints(UIComponent ui, UIWindowComponent window, TransformComponent transform, Vector2 size)
        {
            if (transform.Position.X < 0) transform.Position = new Vector2(0, transform.Position.Y);
            if (transform.Position.Y < 0) transform.Position = new Vector2(transform.Position.X, 0);

            Vector2 parentSize = new Vector2(0, 0);
            bool hasParent = false;
            if (!string.IsNullOrWhiteSpace(ui.Parent))
            {
                Entity parent = Scene.GetEntity(ui.Parent);
                if (parent != null)
                {
                    parentSize = LonoUI.GetAbsoluteSize(Scene, parent);
                    hasParent = true;
                }
            }
            if (!hasParent) parentSize = new Vector2(Scene.ScreenWidth, Scene.ScreenHeight);

            if (window.BeingDragged)
            {
                if (transform.Position.X + size.X > parentSize.X) transform.Position = new Vector2(parentSize.X - size.X, transform.Position.Y);
                if (transform.Position.Y + size.Y > parentSize.Y) transform.Position = new Vector2(transform.Position.X, parentSize.Y - size.Y);
            }
            else if (window.BeingResized)
            {
                if (transform.Position.X + size.X > parentSize.X && ui.WidthType == UIComponent.SizeType.Absolute)
                    ui.Size = new Vector2(parentSize.X - transform.Position.X, ui.Size.Y);
                if (transform.Position.Y + size.Y > parentSize.Y && ui.HeightType == UIComponent.SizeType.Absolute)
                    ui.Size = new Vector2(ui.Size.X, parentSize.Y - transform.Position.Y);
            }
        }
    }
}
