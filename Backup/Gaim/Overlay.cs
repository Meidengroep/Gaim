using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gaim
{
    class Overlay
    {
        public Overlay(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        private Vector2 position;
        private Texture2D sprite;

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
        }
    }
}
