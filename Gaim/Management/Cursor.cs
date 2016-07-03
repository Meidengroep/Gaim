using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Gaim
{
    class Cursor
    {
        public Cursor(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        Texture2D sprite;
        Vector2 positie;

        public void Update()
        {
            positie = Input.MousePosition;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, positie, Color.White);
        }
    }
}
