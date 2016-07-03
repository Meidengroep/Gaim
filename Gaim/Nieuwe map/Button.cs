using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Gaim
{
    class Button
    {
        public Button(Texture2D offSprite, Texture2D onSprite, Vector2 position)
        {
            this.offSprite = offSprite;
            this.onSprite = onSprite;
            this.currentSprite = onSprite;
            this.position = position;
            this.buttonBox = new Rectangle((int)position.X, (int)position.Y, onSprite.Width, onSprite.Height);
        }

        protected Texture2D offSprite, onSprite, currentSprite;
        protected Vector2 position;
        protected Point mousePosition;
        protected Rectangle buttonBox;
        protected bool on;
        public bool prevMouse = true;

        public bool On
        {
            get { return this.on; }
            set { this.on = value; }
        }

        public bool IsSelected()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Released)
                prevMouse = false;
            mousePosition = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            if (!prevMouse && buttonBox.Contains(mousePosition))
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    prevMouse = true;
                    return true;
                }
                else return false;
            }
            else
            {
                currentSprite = offSprite;
                return false;
            }
        }

        public void IsTurnedOn()
        {
            currentSprite = onSprite;
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Released) //&& mouse.RightButton == ButtonState.Released)
                prevMouse = false;
            mousePosition = new Point(mouse.X, mouse.Y);
            if (!prevMouse && buttonBox.Contains(mousePosition))
            {
                currentSprite = onSprite;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    on = true;
                    prevMouse = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(currentSprite, position, Color.White);
        }
    }
}
