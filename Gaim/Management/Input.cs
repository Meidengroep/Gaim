using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Gaim
{
    static class Input
    {
        #region Members

        static KeyboardState currentKeyboard = Keyboard.GetState();
        static KeyboardState prevKeyboard;
        static MouseState currentMouse = Mouse.GetState();
        static MouseState prevMouse;

        #endregion

        public static void Update()
        {
            prevKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();
            prevMouse = currentMouse;
            currentMouse = Mouse.GetState();
        }

        #region Mouse

        public static bool MouseLeftClick
        {
            get { return (currentMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released); }
        }

        public static bool MouseLeftPressed
        {
            get { return (currentMouse.LeftButton == ButtonState.Pressed); }
        }

        public static bool MouseRightClick
        {
            get { return (currentMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released); }
        }

        public static bool MouseRightPressed
        {
            get { return (currentMouse.RightButton == ButtonState.Pressed); }
        }

        public static int MouseScrollDistance
        {
            get { return (currentMouse.ScrollWheelValue - prevMouse.ScrollWheelValue); }
        }

        public static Point MousePoint
        {
            get { return new Point(currentMouse.X, currentMouse.Y); }
        }

        public static Vector2 MousePosition
        {
            get { return new Vector2(currentMouse.X, currentMouse.Y); }
        }

        #endregion

        #region Keyboard

        public static bool KeyboardButtonPressed(Keys key)
        {
            return (currentKeyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key));
        }

        public static bool KeyboardButtonPressing(Keys key)
        {
            return (currentKeyboard.IsKeyDown(key));
        }

        #endregion
    }
}