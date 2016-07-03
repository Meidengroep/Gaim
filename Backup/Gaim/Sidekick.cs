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
    abstract class Sidekick
    {
        public Sidekick(Vector2 position, ContentManager cm, Vector2 playerOffset)
        {
            this.kogels = new List<Bullet>();
            this.position = position;
            this.playerOffset = playerOffset;
            this.followSpeed = 0.1f;
        }

        protected Texture2D sprite;
        public List<Bullet> kogels;
        protected Vector2 position, velocity, origin, playerOffset;
        protected int delay, engineUsage;
        protected float followSpeed;

        public int Delay
        {
            get { return this.delay; }
        }

        public Vector2 Position
        {
            get { return this.position; }
        }

        public int EngineUsage
        {
            get { return this.engineUsage; }
        }

        public void Update(Vector2 playerPosition)
        {

            playerPosition += this.playerOffset;
            Random r = new Random();
            this.velocity = (playerPosition - this.position) * followSpeed;
            position += velocity;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
        }
    }

    class ABombSidekick : Sidekick
    {
        public ABombSidekick(Vector2 position, ContentManager cm, Vector2 playerOffset)
            : base(position, cm, playerOffset)
        {
            List<Texture2D> particlelist = new List<Texture2D>();
            particlelist.Add(cm.Load<Texture2D>("Particles/FireSmoke/firesmoke1"));
            particlelist.Add(cm.Load<Texture2D>("Particles/FireSmoke/firesmoke2"));
            particlelist.Add(cm.Load<Texture2D>("Particles/FireSmoke/firesmoke3"));
            this.kogels.Add(new ABomb(Vector2.Zero, cm.Load<Texture2D>("Bullets/Rocket"), particlelist));
            this.sprite = cm.Load<Texture2D>("Sidekicks/Tard");
            this.origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            this.delay = 50;
            this.engineUsage = 200;
        }
    }
}
