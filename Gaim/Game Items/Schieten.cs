using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Gaim
{
    class Schieten
    {
        public Schieten(bool newGame, bool preview)
        {
            #region Particles
            this.fireParticles = new List<Texture2D>();
            this.fireParticles.Add(Global.Game.Content.Load<Texture2D>("Particles/Fire/Fire1"));
            this.fireParticles.Add(Global.Game.Content.Load<Texture2D>("Particles/Fire/Fire2"));
            this.fireParticles.Add(Global.Game.Content.Load<Texture2D>("Particles/Fire/Fire3"));
            this.fireParticles.Add(Global.Game.Content.Load<Texture2D>("Particles/Fire/Fire4"));
            #endregion

            this.altGun = new List<Bullet>();
            this.mainGun = new List<Bullet>();

            this.loadedWeapons = new List<string>();
            LoadSaveWeapons(newGame, preview);

            this.main = loadedWeapons[0];
            this.mainLevel = loadedWeapons[1];
            this.alt = loadedWeapons[2];
            this.altLevel = loadedWeapons[3];
            this.leftSidekickTpe = loadedWeapons[4];
            this.rightSidekickType = loadedWeapons[5];
            MainWeaponInit();
            AltWeaponInit();
            SidekickInit();
        }

        #region Members
        protected string main, alt, mainLevel, altLevel, leftSidekickTpe, rightSidekickType;
        protected Texture2D mainSprite;
        protected Texture2D altSprite;
        public List<Bullet> bullets = new List<Bullet>();
        protected List<Bullet> mainGun, altGun;
        protected List<Texture2D> fireParticles;
        protected int mainDelay, altDelay, leftSidekickDelay, rightSidekickDelay, 
            mainDelayReset, altDelayReset, mainEngineUsage, altEngineUsage;
        protected List<string> loadedWeapons;
        protected Vector2 position;

        protected Sidekick leftSidekick;
        protected Sidekick rightSidekick;
        #endregion

        #region GunInit
        private void LoadSaveWeapons(bool newGame, bool preview)
        {
            string path;
            if (preview)
                path = "WeaponsTemp";
            else path = "Weapons";
            StreamReader fileLezer;
            if (newGame)
                fileLezer = new StreamReader("Content/Save/Newgame.txt");
            else fileLezer = new StreamReader("Content/Save/" + path + ".txt");
            string regel = fileLezer.ReadLine();
            while (regel != null)
            {
                loadedWeapons.Add(regel);
                regel = fileLezer.ReadLine();
            }
            fileLezer.Close();
        }

        private void MainWeaponInit()
        {
            switch (main)
            {
                case "Basic":
                    switch (mainLevel)
                    {
                        #region L1
                        case "1":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/lol");
                            mainGun.Add(new SingleShot(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 50;
                            this.mainDelay = 10;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L2
                        case "2":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/lol");
                            mainGun.Add(new SingleShot(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 150;
                            this.mainDelay = 10;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L3
                        case "3":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/lol");
                            mainGun.Add(new SingleShot(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot3Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot3Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 150;
                            this.mainDelay = 10;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L4
                        case "4":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/lol");
                            mainGun.Add(new SingleShot(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot3Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot3Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot4Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot4Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 150;
                            this.mainDelay = 10;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L5
                        case "5":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/lol");
                            mainGun.Add(new SingleShot(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot3Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot3Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot4Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot4Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot5Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new SingleShot5Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 150;
                            this.mainDelay = 10;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                    }
                    break;
                case "Laser" :
                    switch (mainLevel)
                    {
                        #region L1
                        case "1":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/LaserLong");
                            mainGun.Add(new Laser(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 1;
                            this.mainDelay = 1;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L2
                        case "2":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/LaserLong");
                            mainGun.Add(new Laser2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser2Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 2;
                            this.mainDelay = 1;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L3
                        case "3":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/LaserLong");
                            mainGun.Add(new Laser2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 100;
                            this.mainDelay = 1;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L4
                        case "4":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/LaserLong");
                            mainGun.Add(new Laser2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser4Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser4Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 10;
                            this.mainDelay = 1;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L5
                        case "5":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/LaserLong");
                            mainGun.Add(new Laser2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser4Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser4Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Laser(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 10;
                            this.mainDelay = 1;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                    }
                    break;
                case "Minigun":
                    switch (mainLevel)
                    {
                        #region L1
                        case "1":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/Minigun");
                            mainGun.Add(new Minigun(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 2;
                            this.mainDelay = 6;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L2
                        case "2":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/Minigun");
                            mainGun.Add(new Minigun2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Minigun2Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 2;
                            this.mainDelay = 6;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L3
                        case "3":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/Minigun");
                            mainGun.Add(new Minigun(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 2;
                            this.mainDelay = 5;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L4
                        case "4":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/Minigun");
                            mainGun.Add(new Minigun2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Minigun2Right(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 2;
                            this.mainDelay = 5;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                        #region L5
                        case "5":
                            this.mainSprite = Global.Game.Content.Load<Texture2D>("Bullets/Minigun");
                            mainGun.Add(new Minigun2Left(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Minigun2Right(Vector2.Zero, mainSprite, null));
                            mainGun.Add(new Minigun(Vector2.Zero, mainSprite, null));
                            mainEngineUsage = 2;
                            this.mainDelay = 5;
                            this.mainDelayReset = this.mainDelay;
                            break;
                        #endregion
                    }
                    break;
                default:
                    break;
            }
        }

        private void AltWeaponInit()
        {
            switch (alt)
            {
                case "Flamethrower":
                    switch (altLevel)
                    {
                        #region L1
                        case "1" :
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Vlammenwerper");
                            altGun.Add(new Flamethrower(Vector2.Zero, altSprite, fireParticles));
                            this.altDelay = 5;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 30;
                            break;
                        #endregion
                        #region L2
                        case "2" :
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Vlammenwerper");
                            altGun.Add(new Flamethrower2Left(Vector2.Zero, altSprite, fireParticles));
                            altGun.Add(new Flamethrower2Right(Vector2.Zero, altSprite, fireParticles));
                            this.altDelay = 5;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        #region L3
                        case "3":
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Vlammenwerper");
                            altGun.Add(new Flamethrower3(Vector2.Zero, altSprite, fireParticles));
                            this.altDelay = 3;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        #region L4
                        case "4":
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Vlammenwerper");
                            altGun.Add(new Flamethrower4Left(Vector2.Zero, altSprite, fireParticles));
                            altGun.Add(new Flamethrower4Right(Vector2.Zero, altSprite, fireParticles));
                            this.altDelay = 3;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        #region L5
                        case "5":
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Vlammenwerper");
                            altGun.Add(new Flamethrower5(Vector2.Zero, altSprite, fireParticles));
                            this.altDelay = 3;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        default:
                            break;
                    }
                    break;
                case "RearMissile":
                    switch (altLevel)
                    {
                        #region L1
                        case "1":
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Rocket");
                            altGun.Add(new RearMissile1Left(Vector2.Zero, altSprite, null));
                            altGun.Add(new RearMissile1Right(Vector2.Zero, altSprite, null));
                            this.altDelay = 15;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        #region L2
                        case "2":
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Rocket");
                            altGun.Add(new RearMissile2Left(Vector2.Zero, altSprite, null));
                            altGun.Add(new RearMissile2Right(Vector2.Zero, altSprite, null));
                            this.altDelay = 45;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        #region L3
                        case "3":
                            this.altSprite = Global.Game.Content.Load<Texture2D>("Bullets/Rocket");
                            altGun.Add(new RearMissile2Left(Vector2.Zero, altSprite, null));
                            altGun.Add(new RearMissile2Right(Vector2.Zero, altSprite, null));
                            altGun.Add(new RearMissile3Left(Vector2.Zero, altSprite, null));
                            altGun.Add(new RearMissile3Right(Vector2.Zero, altSprite, null));
                            this.altDelay = 45;
                            this.altDelayReset = this.altDelay;
                            altEngineUsage = 80;
                            break;
                        #endregion
                        default:
                            break;
                    }
                    break;
            }
        }

        private void SidekickInit()
        {
            switch (leftSidekickTpe)
            {
                case "ABomb" :
                    this.leftSidekick = new ABombSidekick(this.position, new Vector2(-50, 50));
                    break;
            }

            switch (rightSidekickType)
            {
                case "ABomb":
                    this.rightSidekick = new ABombSidekick(this.position, new Vector2(50, 50));
                    break;
            }
        }
        #endregion

        public void Update(Vector2 position, GameTime gt, bool knalluh)
        {
            this.position = position;

            if (leftSidekick != null)
                leftSidekick.Update(position);
            if (rightSidekick != null)
                rightSidekick.Update(position);

            #region ViewportDelete
            for (int updateKogel = 0; updateKogel < bullets.Count; updateKogel++)
            {
                bullets[updateKogel].Update(gt);
                if (bullets[updateKogel].BulletType != BulletType.Laser)
                {
                    if (bullets[updateKogel].Position.X + bullets[updateKogel].Sprite.Width > Global.ViewPortWidth
                        || bullets[updateKogel].Position.X < 0
                        || bullets[updateKogel].Position.Y + bullets[updateKogel].Sprite.Height > Global.ViewPortHeight
                        || bullets[updateKogel].Position.Y < 0
                        || bullets[updateKogel].Ttl <= 0)
                    {
                        if (bullets[updateKogel].ParticleEngine != null)
                            bullets[updateKogel].ParticleEngine.particles.Clear();
                        bullets.RemoveAt(updateKogel);
                        updateKogel--;
                    }
                }
                else
                if (bullets[updateKogel].Ttl <= 0)
                {
                    if (bullets[updateKogel].ParticleEngine != null)
                        bullets[updateKogel].ParticleEngine.particles.Clear();
                    bullets.RemoveAt(updateKogel);
                    updateKogel--;
                }
            }
            #endregion

            #region Delays
            if (mainDelay > 0)
                mainDelay--;
            if (altDelay > 0)
                altDelay--;
            if (leftSidekickDelay > 0)
                leftSidekickDelay--;
            if (rightSidekickDelay > 0)
                rightSidekickDelay--;
            #endregion

            #region Player Weapons
            if ((Input.MouseLeftPressed || knalluh) && mainDelay <= 0 && Global.Player.Engine >= mainEngineUsage)
            {
                foreach (Bullet kogel in mainGun)
                {
                    if (kogel.BulletType == BulletType.Minigun)
                        kogel.Position = position + new Vector2(8 * (float)Math.Sin((double)gt.TotalGameTime.TotalMilliseconds * 3), 0);
                    else if (kogel.BulletType == BulletType.Laser)
                        kogel.Position = Global.Player.MiddlePosition;
                    else kogel.Position = position;
                    bullets.Add(kogel.DeepClone());
                }
                Global.Player.Engine -= mainEngineUsage;
                mainDelay = mainDelayReset;
            }
            if ((Input.MouseLeftPressed || knalluh) && altDelay <= 0 && Global.Player.Engine >= altEngineUsage)
            {
                foreach (Bullet kogel in altGun)
                {
                    kogel.Position = position;
                    bullets.Add(kogel.DeepClone());
                }
                Global.Player.Engine -= altEngineUsage;
                altDelay = altDelayReset;
            }
            #endregion

            #region Sidekicks
            if (leftSidekick != null && Input.MouseRightPressed && leftSidekickDelay <= 0 && Global.Player.Engine > leftSidekick.EngineUsage)
            {
                foreach (Bullet kogel in leftSidekick.kogels)
                {
                    kogel.Position = leftSidekick.Position;
                    bullets.Add((Bullet)kogel.Clone());
                }
                Global.Player.Engine -= leftSidekick.EngineUsage;
                leftSidekickDelay = leftSidekick.Delay;
            }
            if (rightSidekick != null && Input.MouseRightPressed && rightSidekickDelay <= 0 && Global.Player.Engine > rightSidekick.EngineUsage)
            {
                foreach (Bullet kogel in rightSidekick.kogels)
                {
                    kogel.Position = rightSidekick.Position;
                    bullets.Add((Bullet)kogel.Clone());
                }
                Global.Player.Engine -= rightSidekick.EngineUsage;
                rightSidekickDelay = rightSidekick.Delay;
            }
            #endregion
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet drawKogel in bullets)
                drawKogel.Draw(spriteBatch);
            if (leftSidekick != null)
                leftSidekick.Draw(spriteBatch);
            if (rightSidekick != null)
                rightSidekick.Draw(spriteBatch);
        }
    }
}