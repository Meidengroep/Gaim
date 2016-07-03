using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Gaim
{
    class PreviewSchieten : Schieten
    {
        public PreviewSchieten(bool newGame, bool preview)
            :base (newGame, preview)
        {
        }

        public void Update(Vector2 position, GameTime gt, bool knalluh, PreviewPlayer player)
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
            if ((Input.MouseLeftPressed || knalluh) && mainDelay <= 0 && player.Engine >= mainEngineUsage)
            {
                foreach (Bullet kogel in mainGun)
                {
                    if (kogel.BulletType != BulletType.Minigun)
                        kogel.Position = position;
                    else kogel.Position = position + new Vector2(8 * (float)Math.Sin((double)gt.TotalGameTime.TotalMilliseconds * 3), 0);
                    bullets.Add(kogel.DeepClone());
                }
                player.Engine -= mainEngineUsage;
                mainDelay = mainDelayReset;
            }
            if ((Input.MouseLeftPressed || knalluh) && altDelay <= 0 && player.Engine >= altEngineUsage)
            {
                foreach (Bullet kogel in altGun)
                {
                    kogel.Position = position;
                    bullets.Add(kogel.DeepClone());
                }
                player.Engine -= altEngineUsage;
                altDelay = altDelayReset;
            }
            #endregion

            #region Sidekicks
            if (leftSidekick != null && Input.MouseRightPressed && leftSidekickDelay <= 0 && player.Engine > leftSidekick.EngineUsage)
            {
                foreach (Bullet kogel in leftSidekick.kogels)
                {
                    kogel.Position = leftSidekick.Position;
                    bullets.Add((Bullet)kogel.Clone());
                }
                player.Engine -= leftSidekick.EngineUsage;
                leftSidekickDelay = leftSidekick.Delay;
            }
            if (rightSidekick != null && Input.MouseRightPressed && rightSidekickDelay <= 0 && player.Engine > rightSidekick.EngineUsage)
            {
                foreach (Bullet kogel in rightSidekick.kogels)
                {
                    kogel.Position = rightSidekick.Position;
                    bullets.Add((Bullet)kogel.Clone());
                }
                player.Engine -= rightSidekick.EngineUsage;
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