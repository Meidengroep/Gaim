using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Gaim
{
    class Background
    {
        public Background(string level, int numberOfBackgrounds, Game g)
        {
            this.scrollSpeed = 0.1f;
            this.backgrounds = new Texture2D[numberOfBackgrounds];
            this.bgPositions = new float[numberOfBackgrounds];
            this.LoadBackgrounds(g.Content, level);
            this.numberOfBackgrounds = numberOfBackgrounds;
            this.vpHeight = g.GraphicsDevice.Viewport.Height;
        }

        private Texture2D[] backgrounds;
        private float[] bgPositions;
        private int vpHeight;
        private float scrollSpeed;
        private float stackTop;
        private int numberOfBackgrounds;

        private void LoadBackgrounds(ContentManager cm, string level)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i] = cm.Load<Texture2D>("Backgrounds/L" + level + "/" + i.ToString());
                if (i - 1 >= 0)
                    bgPositions[i] = bgPositions[i - 1] - backgrounds[i].Height;
                if (i == 0)
                    bgPositions[i] -= 2 * scrollSpeed;
            }
            stackTop += bgPositions[numberOfBackgrounds];
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
            for (int n = 0; n < bgPositions.Length; n++)
            {
                if (bgPositions[n] >= vpHeight)
                {
                    bgPositions[n] = stackTop - backgrounds[n].Height;
                    stackTop -= backgrounds[n].Height;
                }
                bgPositions[n] += scrollSpeed *dt;
            }
            stackTop += scrollSpeed * dt;
        }

        public void Draw(SpriteBatch sb)
        {
            for (int m = 0; m < numberOfBackgrounds; m++)
                if (bgPositions[m] + backgrounds[m].Height > 0)
                    sb.Draw(backgrounds[m], new Vector2(0, bgPositions[m]), Color.White);
        }

    }
}
