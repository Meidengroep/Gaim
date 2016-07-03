using Microsoft.Xna.Framework.Graphics;
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
            this.currentSprite = offSprite;
            this.position = position;
            this.buttonBox = new Rectangle((int)position.X, (int)position.Y, onSprite.Width, onSprite.Height);
        }

        protected Texture2D offSprite, onSprite, currentSprite;
        protected Vector2 position;
        protected Rectangle buttonBox;
        protected bool on;

        public bool On
        {
            get { return this.on; }
            set { this.on = value; }
        }

        public bool IsSelected()
        {
            if (buttonBox.Contains(Input.MousePoint))
            {
                if (Input.MouseLeftClick)
                {
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
            if (buttonBox.Contains(Input.MousePoint))
            {
                currentSprite = onSprite;
                if (Input.MouseLeftClick)
                {
                    on = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(currentSprite, position, Color.White);
        }
    }
}
