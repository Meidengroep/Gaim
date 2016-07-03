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
    class PreviewPlayer : Player
    {
        public PreviewPlayer(Game g, Vector2 position, bool newGame, bool knalluh)
            : base(g, position, newGame, knalluh)
        {
            engineBar = new VerticalBar(g.Content.Load<Texture2D>("Overlays/Enginebar"), new Vector2(600, 520), this.engine);
        }

        protected VerticalBar engineBar;

        public override void Update(GameTime gt)
        {
            velocity.X = MathHelper.Clamp(velocity.X, -15, 15);
            velocity.Y = MathHelper.Clamp(velocity.Y, -15, 15);
            position += velocity;
            position.X = MathHelper.Clamp(position.X, 0, 600 - currentTexture.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, 750 - currentTexture.Height);

            schieten.Update(position + new Vector2(currentTexture.Width / 2, 0), gd, gt, this, knalluh);

            Recovery();
            if (damageInvul > 0)
                damageInvul--;

            #region Texture Assignment
            if (velocity.X > 3 && position.X != 600 - currentTexture.Width)
                currentTexture = rightTexture;
            else if (velocity.X < -3 && position.X != 0)
                currentTexture = leftTexture;
            else currentTexture = straightTexture;
            #endregion

            if (structure <= 0)
            {
                shield = maxShield;
                structure = maxStructure;
                engine = maxEngine;
            }
            structure = (int)MathHelper.Clamp(structure, -maxStructure, maxStructure);
            engineParticleSystem.EmitterLocation = new Vector2(position.X + Origin.X, position.Y + currentTexture.Height - 20);
            engineParticleSystem.Update(true, 0, new Vector2(0, 3));
            engineBar.Update(engine);
        }

        public override void Draw(SpriteBatch sb)
        {
            schieten.Draw(sb);
            sb.Draw(currentTexture, position, Color.White);
            engineParticleSystem.Draw(sb);
            engineBar.Draw(sb);
        }
    }
}
