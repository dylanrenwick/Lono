using System;
using System.Collections.Generic;
using System.Text;

using Lono.Data;
using Microsoft.Xna.Framework.Input;

namespace Lono.MonoGame
{
    public class MonoGameInputWrapper : IInputWrapper
    {
        public Vector2 MousePos => new Vector2(mouseState.X, mouseState.Y);
        public Vector2 MouseDelta => MousePos - new Vector2(lastMouseState.X, lastMouseState.Y);

        private KeyboardState keyboardState;
        private KeyboardState lastKeyboardState;

        private MouseState mouseState;
        private MouseState lastMouseState;

        public bool GetKeyDown(VirtualKey key)
        {
            return keyboardState.IsKeyDown(VirtualKeyToMonoGameKey(key));
        }
        public bool GetKeyPressed(VirtualKey key)
        {
            Keys mgKey = VirtualKeyToMonoGameKey(key);
            return keyboardState.IsKeyDown(mgKey) && !lastKeyboardState.IsKeyDown(mgKey);
        }

        public bool GetButtonDown(int button)
        {
            switch (button)
            {
                case 0:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case 1:
                    return mouseState.RightButton == ButtonState.Pressed;
                case 2:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                default: return false;
            }
        }
        public bool GetButtonPressed(int button)
        {
            switch (button)
            {
                case 0:
                    return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed;
                case 1:
                    return mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton != ButtonState.Pressed;
                case 2:
                    return mouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton != ButtonState.Pressed;
                default: return false;
            }
        }

        public void Update()
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        private Keys VirtualKeyToMonoGameKey(VirtualKey key)
        {
            string valName = key.ToString();
            if (Enum.TryParse(valName, true, out Keys result))
            {
                return result;
            }

            switch (key)
            {
                case VirtualKey.Return: return Keys.Enter;
                case VirtualKey.Shift: return Keys.LeftShift;
                case VirtualKey.Control: return Keys.LeftControl;
                case VirtualKey.ScrollLock: return Keys.Scroll;
                case VirtualKey.MediaPrevTrack: return Keys.MediaPreviousTrack;
                case VirtualKey.LaunchMediaSelect: return Keys.SelectMedia;
                case VirtualKey.Menu:
                case VirtualKey.LeftMenu: return Keys.LeftAlt;
                case VirtualKey.RightMenu: return Keys.RightAlt;
                default: throw new Exception();
            }
        }
    }
}
