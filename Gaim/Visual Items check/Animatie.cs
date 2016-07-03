using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Gaim
{
    class Animatie
    {
        public Animatie(Texture2D sprite, bool isHalve)
        {
            this.sprite = sprite;
            this.frameHeight = sprite.Height;
            this.frameWidth = frameHeight;
            this.maxFrame = sprite.Width / frameWidth;
            this.isHalve = isHalve;
        }

        protected Vector2 position;
        protected Texture2D sprite;
        protected int currentFrame, maxFrame;
        protected int frameHeight;
        protected int frameWidth;
        protected int animationTime = 100;
        protected TimeSpan timer;
        protected bool isHalve;

        protected Rectangle SourceRectangle()
        {
            return new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        }

        public Rectangle BoundingBox
        {
            get
            {
                if (isHalve)
                    return new Rectangle((int)position.X + frameWidth / 4, (int)position.Y, frameWidth / 2, frameHeight);
                else return new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            }
        }

        public void Update(Vector2 position, GameTime gt)
        {
            this.timer += gt.ElapsedGameTime;
            this.position = position;
            if (isHalve)
                this.position.X -= frameWidth / 4;
            if (timer.TotalMilliseconds >= animationTime)
            {
                timer = TimeSpan.Zero;
                currentFrame++;
                if (currentFrame >= maxFrame)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, SourceRectangle(), Color.White);
        }
    }
}
