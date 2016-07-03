using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

enum BulletType
{
    SingleShot = 0,
    Flamethrower = 1,
    Laser = 2,
    StraightShot = 3,
    ABomb = 4,
    Empty = 5,
    Minigun = 6,
}
namespace Gaim
{
    #region Abstract Bullet
    abstract class Bullet : ICloneable
    {
        public Bullet(Vector2 position, Texture2D sprite, List<Texture2D> particleList)
        {
            this.position = position;
            this.sprite = sprite;
            if(sprite != null)
                this.origin = new Vector2(sprite.Width / 2, sprite.Height);
            this.middle = new Vector2(sprite.Width / 2, sprite.Height / 2);
            this.color = Color.White;
        }

        #region Membervariabelen
        protected Vector2 velocity, position, middle;
        protected float rotation;
        protected int damage;
        protected Texture2D sprite;
        protected ParticleEngine particleEngine;
        protected Vector2 origin;
        protected int ttl = 200;          // Time To Live
        protected BulletType bulletType;
        protected Color color;
        #endregion

        #region Properties
        public virtual Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public virtual Vector2 MiddlePosition
        {
            get { return this.position + this.middle; }
        }

        public Vector2 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        public float Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        public BulletType BulletType
        {
            get { return this.bulletType; }
        }

        public int Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        public Vector2 Origin
        {
            get { return this.origin; }
        }

        public int Ttl
        {
            get { return this.ttl; }
            set { this.ttl = value; }
        }

        public Texture2D Sprite
        {
            get { return this.sprite; }
        }

        public virtual Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, 
                    sprite.Width, sprite.Height);
            }
        }

        public ParticleEngine ParticleEngine
        {
            get { return this.particleEngine; }
            set { this.particleEngine = value; }
        }
        #endregion

        protected float CalcRotation
        {
            get { return (float)(Math.Atan(this.velocity.X / this.velocity.Y) * -1); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual Bullet DeepClone()
        {
            return (Bullet)this.Clone();
        }

        protected abstract void Behaviour(GameTime gt);

        public virtual void Update(GameTime gt, Player player)
        {
            this.Behaviour(gt);
            this.position += this.velocity;
            if (particleEngine != null)
            {
                particleEngine.EmitterLocation = this.position;
                particleEngine.Update(true, rotation);
            }
            this.ttl -= 1;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, rotation, this.origin, 1, SpriteEffects.None, 0);
            if (particleEngine != null)
                particleEngine.Draw(spriteBatch);
        }
    }
    #endregion

    #region Basic
    class SingleShot : Bullet
    {
        public SingleShot(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(0, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    #region L2
    class SingleShot2Left : SingleShot
    {
        public SingleShot2Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(0, -9);
            this.rotation = this.CalcRotation;
        }
        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width - 3, 15); }
        }
    }

    class SingleShot2Right : SingleShot
    {
        public SingleShot2Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(0, -9);
            this.rotation = this.CalcRotation;
        }
        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width + 3, 15); }
        }
    }
    #endregion

    #region L3
    class SingleShot3Left: Bullet
    {
        public SingleShot3Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(-2, -8);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width * 2 - 3, 15); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    class SingleShot3Right : Bullet
    {
        public SingleShot3Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(2, -8);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width * 2 + 3, 15); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion

    #region L4
    class SingleShot4Left : Bullet
    {
        public SingleShot4Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(0, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width * 2, 15); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    class SingleShot4Right : Bullet
    {
        public SingleShot4Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(0, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width * 2, 15); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion

    #region L5
    class SingleShot5Left : Bullet
    {
        public SingleShot5Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(-1, -8);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width * 2, 15); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    class SingleShot5Right : Bullet
    {
        public SingleShot5Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(1, -8);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width * 2, 15); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
    #endregion

    #region Flamethrower
    class Flamethrower : Bullet
    {
        public Flamethrower(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.particleList = particleList;
            this.bulletType = BulletType.Flamethrower;
            Random r = new Random();
            this.ttl = 15 + (int)(r.NextDouble() * 10);
            this.damage = 10;
            this.velocity = new Vector2(0, -10);
            this.particleEngine = new ParticleEngine(particleList, Vector2.Zero, 3, 2f, Color.White, 5, 1, ParticleType.standaard);
            this.rotation = this.CalcRotation;
        }

        protected List<Texture2D> particleList;

        public override Bullet DeepClone()
        {
            return new Flamethrower(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(0, 80); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    #region L2
    class Flamethrower2Right : Flamethrower
    {
        public Flamethrower2Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(0, -10);
        }

        public override Bullet DeepClone()
        {
            return new Flamethrower2Right(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width + 10, 80); }
        }
    }

    class Flamethrower2Left : Flamethrower 
    {
        public Flamethrower2Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(0, -10);
        }

        public override Bullet DeepClone()
        {
            return new Flamethrower2Left(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width - 10, 80); }
        }
    }
    #endregion
    #region L3
    class Flamethrower3 : Bullet
    {
        public Flamethrower3(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(0, -10);
            this.particleList = particleList;
            this.bulletType = BulletType.Flamethrower;
            Random r = new Random();
            this.ttl = 30 + (int)(r.NextDouble() * 10);
            this.damage = 10;
            this.velocity = new Vector2(0, -10);
            this.particleEngine = new ParticleEngine(particleList, Vector2.Zero, 3, 1f, Color.White, 5, 1, ParticleType.standaard);
            this.rotation = this.CalcRotation;
            this.velocity = new Vector2(0, -10);
            this.color = Color.White;
        }

        protected List<Texture2D> particleList;
        protected Vector2 orbit;
        protected Vector2 playerMid;

        public override Bullet DeepClone()
        {
            return new Flamethrower3(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(0, 0); }
        }
        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(position.X + orbit.X + playerMid.X), (int)(position.Y + orbit.Y + playerMid.Y),
                    sprite.Width, sprite.Height);
            }
        }

        protected override void Behaviour(GameTime gt)
        {
            orbit.X = 50 * (float)Math.Sin(((double)gt.TotalRealTime.TotalMilliseconds) * 2);
            orbit.Y = 50 * (float)Math.Cos(((double)gt.TotalRealTime.TotalMilliseconds) * 2);
        }

        public override void Update(GameTime gt, Player player)
        {
            this.playerMid = new Vector2(0, player.Middle.Y);
            this.position = player.MiddlePosition - playerMid;
            this.Behaviour(gt);
            if (particleEngine != null)
            {
                particleEngine.EmitterLocation = this.position + orbit + playerMid;
                particleEngine.Update(true, rotation);
            }
            this.ttl -= 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position + orbit + playerMid, null, color, rotation, this.origin, 1, SpriteEffects.None, 0);
            if (particleEngine != null)
                particleEngine.Draw(spriteBatch);
        }
    }
    #endregion
    #region L4
    class Flamethrower4Right : Flamethrower
    {
        public Flamethrower4Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(3, -7);
        }

        public override Bullet DeepClone()
        {
            return new Flamethrower4Right(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width, 60); }
        }
    }

    class Flamethrower4Left : Flamethrower
    {
        public Flamethrower4Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(-3, -7);
        }

        public override Bullet DeepClone()
        {
            return new Flamethrower4Left(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width, 60); }
        }
    }
    #endregion
    #region L5
    class Flamethrower5 : Bullet
    {
        public Flamethrower5(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.velocity = new Vector2(0, -10);
            this.particleList = particleList;
            this.bulletType = BulletType.Flamethrower;
            Random r = new Random();
            this.ttl = 30 + (int)(r.NextDouble() * 10);
            this.damage = 10;
            this.velocity = new Vector2(0, -10);
            this.particleEngine = new ParticleEngine(particleList, Vector2.Zero, 3, 1f, Color.White, 5, 1, ParticleType.standaard);
            this.rotation = this.CalcRotation;
            this.velocity = new Vector2(0, -10);
            this.offSet = Vector2.Zero;
            this.color = Color.White;
        }

        protected List<Texture2D> particleList;
        protected Vector2 orbit;
        protected Vector2 playerMid;
        protected Vector2 offSet;

        public override Bullet DeepClone()
        {
            return new Flamethrower5(this.position, this.sprite, particleList);
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(0, 0); }
        }
        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(offSet.X + position.X + orbit.X + playerMid.X), (int)(offSet.Y + position.Y + orbit.Y + playerMid.Y),
                    sprite.Width, sprite.Height);
            }
        }

        protected override void Behaviour(GameTime gt)
        {
            orbit.X = 50 * (float)Math.Sin(((double)gt.TotalRealTime.TotalMilliseconds) * 2);
            orbit.Y = 50 * (float)Math.Cos(((double)gt.TotalRealTime.TotalMilliseconds) * 2);
        }

        public override void Update(GameTime gt, Player player)
        {
            offSet += velocity;
            this.playerMid = new Vector2(0, player.Middle.Y);
            this.position = player.MiddlePosition - playerMid;
            this.Behaviour(gt);
            if (particleEngine != null)
            {
                particleEngine.EmitterLocation = this.position + orbit + playerMid + offSet;
                particleEngine.Update(true, rotation);
            }
            this.ttl -= 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position + orbit + playerMid + offSet, null, color, rotation, this.origin, 1, SpriteEffects.None, 0);
            if (particleEngine != null)
                particleEngine.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion

    #region Rear Missile Launcher
    #region L1
    class RearMissile1Left : Bullet
    {
        public RearMissile1Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(-4, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    class RearMissile1Right : Bullet
    {
        public RearMissile1Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(4, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
    #region L2
    class RearMissile2Left : Bullet
    {
        public RearMissile2Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(-4, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
            this.helixVelocity = 0.5f;
        }
        bool omKeer;
        float helixVelocity;

        protected override void Behaviour(GameTime gt)
        {
            if (!omKeer)
                velocity += new Vector2(helixVelocity, 0);
            else velocity += new Vector2(-helixVelocity, 0);
            if (velocity.X >= 6)
                omKeer = true;
            if (velocity.X <= -6)
                omKeer = false;

            this.rotation = CalcRotation;
        }
    }
    class RearMissile2Right : Bullet
    {
        public RearMissile2Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(4, -9);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
            this.helixVelocity = 0.5f;
        }
        bool omKeer;
        float helixVelocity;

        protected override void Behaviour(GameTime gt)
        {
            if (!omKeer)
                velocity += new Vector2(-helixVelocity, 0);
            else velocity += new Vector2(helixVelocity, 0);
            if (velocity.X <= -6)
                omKeer = true;
            if (velocity.X >= 6)
                omKeer = false;
            this.rotation = CalcRotation;
        }
    }
    #endregion
    #region L3
    class RearMissile3Left : Bullet
    {
        public RearMissile3Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(-2, -7);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    class RearMissile3Right : Bullet
    {
        public RearMissile3Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 10;
            this.velocity = new Vector2(2, -7);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
    #endregion

    #region Laser
    class Laser : Bullet
    {
        public Laser(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Laser;
            this.damage = 10;
            this.velocity = new Vector2(0, -45);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    #region L2
    class Laser2Left : Bullet
    {
        public Laser2Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Laser;
            this.damage = 10;
            this.velocity = new Vector2(0, -45);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width - 3, 0); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    class Laser2Right : Bullet
    {
        public Laser2Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Laser;
            this.damage = 10;
            this.velocity = new Vector2(0, -45);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width + 3, 0); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
    #region L3
    #endregion
    #region L4
    class Laser4Left : Bullet
    {
        public Laser4Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Laser;
            this.damage = 10;
            this.velocity = new Vector2(0, -45);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width - 3, sprite.Height / 2); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    class Laser4Right : Bullet
    {
        public Laser4Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Laser;
            this.damage = 10;
            this.velocity = new Vector2(0, -45);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width + 3, sprite.Height / 2); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
    #region L5
    #endregion
    #endregion

    #region Minigun
    class Minigun : Bullet
    {
        public Minigun(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Minigun;
            this.damage = 10;
            this.velocity = new Vector2(0, -15);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #region L2
    class Minigun2Right : Bullet
    {
        public Minigun2Right(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Minigun;
            this.damage = 10;
            this.velocity = new Vector2(0, -15);
            this.particleEngine = null;
            this.rotation = this.CalcRotation; ;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(sprite.Width / 2 + 3, 5); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }

    class Minigun2Left : Bullet
    {
        public Minigun2Left(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.Minigun;
            this.damage = 10;
            this.velocity = new Vector2(0, -15);
            this.particleEngine = null;
            this.rotation = this.CalcRotation; ;
        }

        public override Vector2 Position
        {
            get { return this.position; }
            set { this.position = value + new Vector2(-sprite.Width / 2 - 3, 5); }
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
    #region L3
    #endregion
    #region L4
    #endregion
    #region L5
    #endregion
    #endregion

    #region SidekickBullets
    class ABomb : Bullet
    {
        public ABomb(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.ABomb;
            this.damage = 50;
            this.velocity = new Vector2(0, -12);
            this.particleEngine = new ParticleEngine(particleList, Vector2.Zero, 1, 2, Color.White, 0, 1, ParticleType.standaard);
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }

        public override void Update(GameTime gt, Player player)
        {
            this.Behaviour(gt);
            this.position += this.velocity;
            if (particleEngine != null)
            {
                particleEngine.EmitterLocation = this.position + new Vector2(0, 0);
                particleEngine.Update(true, rotation);
            }
            this.ttl -= 1;
        }
    }
    #endregion

    #region EnemyBullets
    class StraightShot : Bullet
    {
        public StraightShot(Vector2 positie, Texture2D sprite, List<Texture2D> particleList)
            : base(positie, sprite, particleList)
        {
            this.bulletType = BulletType.SingleShot;
            this.damage = 50;
            this.velocity = new Vector2(0, 14);
            this.particleEngine = null;
            this.rotation = this.CalcRotation;
        }

        protected override void Behaviour(GameTime gt)
        {
        }
    }
    #endregion
}