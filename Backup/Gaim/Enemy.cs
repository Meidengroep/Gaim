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
    abstract class Enemy
    {
        public Enemy(Texture2D sprite, Vector2 position, Game g)
        {
            this.position = position;
            this.sprite = sprite;
            this.canShoot = false;
            this.kogels = null;
            this.middle = new Vector2(sprite.Width / 2, sprite.Height / 2);
            this.pickupDrop = PickupType.None;
        }

        protected Vector2 position, velocity, middle;
        protected Texture2D sprite;
        protected int health;
        public List<Bullet> kogels;
        protected bool canShoot;
        protected PickupType pickupDrop;

        #region Propertays
        public Vector2 Position
        {
            get { return this.position; }
        }

        public Vector2 MiddlePosition
        {
            get { return this.position + this.middle; }
        }

        public PickupType PickupType
        {
            get { return this.pickupDrop; }
        }

        public bool CanShoot
        {
            get { return this.canShoot; }
        }

        public abstract Rectangle BoundingBox
        { get; }

        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }
        #endregion

        protected abstract void MovementBehaviour();

        protected abstract void ShootingBehaviour(Player player, GameTime gt);

        public virtual void Update(Player player, GameTime gt)
        {
            MovementBehaviour();
            ShootingBehaviour(player, gt);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
        }
    }

    class TardBoss : Enemy
    {
        public TardBoss(Texture2D sprite, Vector2 position, Game g, Texture2D bulletSprite, Texture2D ballBulletSprite)
            : base(sprite, position, g)
        {
            this.maxXPos = 600 - sprite.Width;
            this.downwardVelocity = new Vector2(0, 5);
            this.goingRight = true;
            this.goingDown = true;
            this.health = 5000;
            this.bulletSprite = bulletSprite;
            this.canShoot = true;
            this.kogels = new List<Bullet>();
            this.mainDelay = 50;
            this.altDelay = 20;
            this.currentMainDelay = 0;
            this.currentAltDelay = 0;
            this.healthBar = new HorizontalBar(g.Content.Load<Texture2D>("Overlays/BossHealthBar"), new Vector2(100, 10), this.health);
            this.bullet1 = new StraightShot(position + new Vector2(sprite.Width / 2 - 15, sprite.Height - 10), ballBulletSprite, null);
            this.bullet2 = new StraightShot(position + new Vector2(sprite.Width / 2 + 15, sprite.Height - 10), ballBulletSprite, null);
        }

        protected float maxXPos, bulletRotation;
        protected bool goingRight, goingDown, mirrorBullet;
        protected Vector2 downwardVelocity, bulletVelocity;
        protected int currentMainDelay, currentAltDelay, mainDelay, altDelay;
        protected Texture2D bulletSprite;
        protected HorizontalBar healthBar;
        protected Bullet bullet1;
        protected Bullet bullet2;


        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        protected override void MovementBehaviour()
        {
            throw new NotImplementedException();
        }

        protected override void ShootingBehaviour(Player player, GameTime gt)
        {
            throw new NotImplementedException();
        }

        public void CalcBulletOrientation(Vector2 position, Player player)
        {
            float x = player.Position.X + player.Middle.X - position.X;
            float y = player.Position.Y + player.Middle.Y - position.Y;
            float diagonale = (float)Math.Sqrt((x * x) + (y * y));
            float scale = (10 / diagonale);
            this.bulletVelocity = new Vector2(x * scale, y * scale);
            CalcRotation();
        }

        public void CalcRotation()
        {
            if (velocity.X < 0)
                mirrorBullet = true;
            else mirrorBullet = false;
            bulletRotation = (float)Math.Atan(bulletVelocity.Y / bulletVelocity.X) + (float)Math.PI / 2;
        }

        public override void Update(Player player, GameTime gt)
        {
            if (goingDown)
                position += downwardVelocity;
            if (goingRight)
                position += new Vector2(5, 0);
            else position += new Vector2(-5, 0);

            if (this.position.X > maxXPos)
                goingRight = false;
            if (this.position.X < 0)
                goingRight = true;
            if (this.position.Y - 10 > 0)
                goingDown = false;

            foreach (Bullet kogel in kogels)
                kogel.Update(gt, player);
            if (currentMainDelay <= 0)
            {
                kogels.Add(new StraightShot(position + new Vector2(sprite.Width / 2 + 15, sprite.Height - 10), bulletSprite, null));
                kogels.Add(new StraightShot(position + new Vector2(sprite.Width / 2 - 15, sprite.Height - 10), bulletSprite, null));
                currentMainDelay = mainDelay;
            }
            if (currentAltDelay <= 0)
            {
                bullet1.Position = position + new Vector2(sprite.Width / 2 - 15, sprite.Height - 10);
                CalcBulletOrientation(bullet1.Position, player);
                bullet1.Rotation = bulletRotation;
                bullet1.Velocity = bulletVelocity;
                bullet2.Position = position + new Vector2(sprite.Width / 2 + 15, sprite.Height - 10);
                CalcBulletOrientation(bullet2.Position, player);
                bullet2.Rotation = bulletRotation;
                bullet2.Velocity = bulletVelocity;

                kogels.Add(bullet1.DeepClone());
                kogels.Add(bullet2.DeepClone());
                
                currentAltDelay = altDelay;
            }
            currentAltDelay--;
            currentMainDelay--;

            healthBar.Update(health);

            for (int i = 0; i < kogels.Count; i++)
                if (kogels[i].Position.Y > 850)
                {
                    kogels.RemoveAt(i);
                    i--;
                }
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Bullet kogel in kogels)
                kogel.Draw(sb);
            base.Draw(sb);
            healthBar.Draw(sb);
        }
    }

    class TardEnemy : Enemy
    {
        public TardEnemy(Texture2D sprite, Vector2 position, Game g)
            : base(sprite, position, g)
        {
            this.velocity = new Vector2(0, 5);
            this.health = 50;
            this.pickupDrop = PickupType.Score;
        }

        protected override void MovementBehaviour()
        {
            position += velocity;
        }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        protected override void ShootingBehaviour(Player player, GameTime gt)
        {

        }
    }

    class ShootingTard : Enemy
    {
        public ShootingTard(Texture2D sprite, Texture2D bulletSprite, Vector2 position, Game g)
            : base(sprite, position, g)
        {
            this.velocity = new Vector2(0, 6);
            this.kogels = new List<Bullet>();
            this.maxDelay = 50;
            this.delay = 0;
            this.health = 100;
            this.bulletSprite = bulletSprite;
            this.canShoot = true;
            this.pickupDrop = PickupType.Structure;
        }

        int delay, maxDelay;
        Texture2D bulletSprite;

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        protected override void MovementBehaviour()
        {
            position += velocity;
        }

        protected override void ShootingBehaviour(Player player, GameTime gt)
        {
            foreach (Bullet kogel in kogels)
                kogel.Update(gt, player);
            if (delay <= 0)
            {
                kogels.Add(new StraightShot(position + new Vector2(sprite.Width / 2, sprite.Height), bulletSprite, null));
                delay = maxDelay;
            }
            delay--;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            foreach (Bullet kogel in kogels)
                kogel.Draw(sb);
        }
    }

    class TargetPracticeEnemy : Enemy
    {
        public TargetPracticeEnemy(Texture2D sprite, Vector2 position, Game g)
            : base(sprite, position, g)
        {
            this.velocity = Vector2.Zero;
            this.health = 50000000;
            this.pickupDrop = PickupType.None;
        }

        protected override void MovementBehaviour()
        {
        }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        protected override void ShootingBehaviour(Player player, GameTime gt)
        {
        }
    }

    class FromLeftAimingEnemy : Enemy
    {
        public FromLeftAimingEnemy(Texture2D sprite, Vector2 position, Game g)
            : base(sprite, position, g)
        {
            this.velocity = new Vector2(5, -1);
            this.health = 50;
            this.pickupDrop = PickupType.Score;
        }

        bool lockOn = false, goingRight;
        float maxXPos, minXPos;
        protected int timer;

        protected override void MovementBehaviour()
        {
            position += velocity;
        }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        protected override void ShootingBehaviour(Player player, GameTime gt)
        {

        }

        public override void Update(Player player, GameTime gt)
        {
            base.Update(player, gt);
            if (!lockOn)
                if (this.position.X > player.Position.X - 10 && this.position.X + sprite.Width < player.Position.X + player.BoundingBox.Width + 10)
                {
                    lockOn = true;
                    minXPos = player.Position.X - 5;
                    maxXPos = player.Position.X + player.BoundingBox.Width + 5;
                    this.velocity = Vector2.Zero;
                }
            if (lockOn)
            {
                if (goingRight)
                    position += new Vector2(5, 0);
                else position += new Vector2(-5, 0);

                if (this.position.X > maxXPos)
                    goingRight = false;
                if (this.position.X < minXPos)
                    goingRight = true;
                timer++;
            }
            if (timer > 40)
                this.velocity = new Vector2(0, 10);
        }
    }

    class FromRightAimingEnemy : Enemy
    {
        public FromRightAimingEnemy(Texture2D sprite, Vector2 position, Game g)
            : base(sprite, position, g)
        {
            this.velocity = new Vector2(-5, -1);
            this.health = 50;
            this.pickupDrop = PickupType.Score;
        }

        bool lockOn = false;
        protected int timer;

        protected override void MovementBehaviour()
        {
            position += velocity;
        }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        protected override void ShootingBehaviour(Player player, GameTime gt)
        {

        }

        public override void Update(Player player, GameTime gt)
        {
            base.Update(player, gt);
            if (!lockOn)
                if (this.position.X > player.Position.X - 10 && this.position.X + sprite.Width < player.Position.X + player.BoundingBox.Width + 10)
                {
                    lockOn = true;
                    this.velocity = Vector2.Zero;
                }
            if (lockOn)
                timer++;
            if (timer > 40)
                this.velocity = new Vector2(0, 10);
        }
    }
}
