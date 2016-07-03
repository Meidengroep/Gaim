using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gaim
{
    abstract class Bar
    {
        public Bar(Texture2D sprite, Vector2 position, float maxValue)
        {
            this.sprite = sprite;
            this.position = position;
            this.maxValue = maxValue;
            this.currentValue = 1;
            this.sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            this.fullRectangle = sourceRectangle;
        }

        protected Texture2D sprite;
        protected Vector2 position;
        protected float maxValue;
        protected float currentValue;
        protected Rectangle sourceRectangle;
        protected Rectangle fullRectangle;

        protected abstract void UpdateRectangle();

        public void Update(float currentValue)
        {
            this.currentValue = currentValue / maxValue;
            this.currentValue = MathHelper.Clamp(this.currentValue, 0, 1);
            UpdateRectangle();
        }

        public abstract void Draw(SpriteBatch sb);
    }

    class VerticalBar : Bar
    {
        public VerticalBar(Texture2D sprite, Vector2 position, int maxValue)
            : base(sprite, position, maxValue)
        {
        }

        //protected override void UpdateRectangle()
        //{
        //    sourceRectangle = new Rectangle((int)position.X, (int)(position.Y + (1f - this.currentValue) * sprite.Height)
        //        , sprite.Width, (int)(this.currentValue * sprite.Height));
        //}

        protected override void UpdateRectangle()
        {
            sourceRectangle = new Rectangle((int)0, (int)(0 + (1f - this.currentValue) * sprite.Height)
                , sprite.Width, (int)(this.currentValue * sprite.Height));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, sourceRectangle, Color.White, (float)Math.PI, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
        }
    }

    class HorizontalBar : Bar
    {
        public HorizontalBar(Texture2D sprite, Vector2 position, int maxValue)
            : base(sprite, position, maxValue)
        {
        }

        protected override void UpdateRectangle()
        {
            sourceRectangle = new Rectangle((int)0, (int)0, (int)(this.currentValue * sprite.Width), sprite.Height);
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, sourceRectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}
