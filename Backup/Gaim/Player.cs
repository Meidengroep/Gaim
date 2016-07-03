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
    class Player
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 middle;

        protected const float movementSpeed = 5;
        protected const float mouseFollowSpeed = 0.08f;

        protected int maxStructure;
        protected int maxShield;
        protected int maxEngine;

        protected int shield, structure, engine, shieldRecoveryRate, engineRecoveryRate, damageInvul;

        protected Texture2D straightTexture;
        protected Texture2D leftTexture;
        protected Texture2D rightTexture;
        protected Texture2D currentTexture;

        protected GraphicsDevice gd;

        protected Schieten schieten;

        protected DirectedParticleEngine engineParticleSystem;

        protected bool knalluh;

        public Player(Game g, Vector2 position, bool newGame, bool knalluh)
        {
            #region ParticleInit
            List<Texture2D>  fireParticles = new List<Texture2D>();
            fireParticles.Add(g.Content.Load<Texture2D>("Particles/Fire/Fire1"));
            fireParticles.Add(g.Content.Load<Texture2D>("Particles/Fire/Fire2"));
            fireParticles.Add(g.Content.Load<Texture2D>("Particles/Fire/Fire3"));
            fireParticles.Add(g.Content.Load<Texture2D>("Particles/Fire/Fire4"));
            this.engineParticleSystem = new DirectedParticleEngine(fireParticles, Vector2.Zero, 1, 1, Color.White, 0, 1, ParticleType.standaard);
            #endregion
            this.position = position;
            this.gd = g.GraphicsDevice;
            List<string> properties = new List<string>();
            List<string> items = new List<string>();
            LoadItems(g.Content, items);
            LoadShip(properties, g.Content, items[0]);
            LoadShield(items[1]);
            LoadEngine(items[2]);
            this.currentTexture = straightTexture;
            this.schieten = new Schieten(g, newGame, knalluh);
            this.middle = new Vector2(currentTexture.Width / 2, currentTexture.Height / 2);
            this.knalluh = knalluh;
        }

        #region Properties
        public int Shield
        {
            get { return this.shield; }
        }

        public int Structure
        {
            get { return this.structure; }
            set { this.structure = value; }
        }

        public int Engine
        {
            get { return this.engine; }
            set { this.engine = value; }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, currentTexture.Width, currentTexture.Height); }
        }

        public Schieten Schieten
        {
            get { return this.schieten; }
            set { this.schieten = value; }
        }

        public Vector2 Position
        {
            get { return this.position; }
        }

        protected Vector2 Origin
        {
            get { return new Vector2(currentTexture.Width / 2, currentTexture.Height / 2); }
        }

        public Vector2 MiddlePosition
        {
            get { return this.position + this.middle; }
        }

        public Vector2 Middle
        {
            get { return this.middle; }
        }
        #endregion

        #region Loading Items
        protected void LoadItems(ContentManager cm, List<string> items)
        {
            StreamReader streamReader = new StreamReader("Content/Save/Player.txt");
            string regel = streamReader.ReadLine();
            while (regel != null)
            {
                items.Add(regel);
                regel = streamReader.ReadLine();
            }
            streamReader.Close();
        }

        protected void LoadShield(string type)
        {
            switch (type)
            {
                case "CBarrierLow":
                    this.maxShield = 200;
                    this.shield = this.maxShield;
                    this.shieldRecoveryRate = 1;
                    break;
                case "CBarrierMedium":
                    this.maxShield = 10000;
                    this.shield = this.maxShield;
                    this.shieldRecoveryRate = 1;
                    break;
            }
        }

        protected void LoadEngine(string type)
        {
            switch (type)
            {
                case "StandardMicrofusion1":
                    this.maxEngine = 800;
                    this.engine = maxEngine;
                    this.engineRecoveryRate = 30;
                    break;
                case "StandardMicrofusion2":
                    this.maxEngine = 10000;
                    this.engine = maxEngine;
                    this.engineRecoveryRate = 30;
                    break;
            }
        }
        #endregion

        #region ShipInit
        private void LoadShip(List<string> properties, ContentManager cm, string shipType)
        {
            StreamReader fileLezer = new StreamReader("Content/Ships/" + shipType + ".txt");
            string regel = fileLezer.ReadLine();
            while (regel != null)
            {
                properties.Add(regel);
                regel = fileLezer.ReadLine();
            }
            fileLezer.Close();

            switch (shipType)
            {
                case "Phoenix":
                    this.straightTexture = cm.Load<Texture2D>("Ships/" + shipType + "/straight");
                    this.leftTexture = cm.Load<Texture2D>("Ships/" + shipType + "/left");
                    this.rightTexture = cm.Load<Texture2D>("Ships/" + shipType + "/right");
                    this.maxStructure = 500;
                    this.structure = maxStructure;
                    break;
                case "Test2":
                    this.straightTexture = cm.Load<Texture2D>("Ships/" + shipType + "/straight");
                    this.leftTexture = cm.Load<Texture2D>("Ships/" + shipType + "/left");
                    this.rightTexture = cm.Load<Texture2D>("Ships/" + shipType + "/right");
                    this.maxStructure = 500;
                    this.structure = maxStructure;
                    break;
            }
        }
        #endregion


        protected void KeyboardInput()
        {
            #region Keyboard Input
            /*if (Keyboard.GetState().GetPressedKeys().Length != 0)
            {
                foreach (Keys key in Keyboard.GetState().GetPressedKeys())
                    switch (key)
                    {
                        case Keys.A:
                            this.velocity.X = -movementSpeed;
                            break;
                        case Keys.D:
                            this.velocity.X = movementSpeed;
                            break;
                        case Keys.W:
                            this.velocity.Y = -movementSpeed;
                            break;
                        case Keys.S:
                            this.velocity.Y = movementSpeed;
                            break;
                        default:
                            break;
                    }
            }
            else this.velocity = Vector2.Zero;*/
            #endregion
        }

        public void Damage(int damage, bool inVul)
        {
            if (damageInvul <= 0)
            {
                if (shield > 0)
                {
                    if (damage < shield)
                        shield -= damage;
                    else
                    {
                        shield = 0;
                        structure -= (damage - shield);
                    }
                }
                else structure -= damage;
                damageInvul = 20;
            }
        }


        protected void MouseInput()
        {
            MouseState mouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            this.velocity = (mousePosition - this.position - this.Origin) * mouseFollowSpeed;
        }

        protected void Recovery()
        {
            this.shield += this.shieldRecoveryRate;
            this.engine += this.engineRecoveryRate;
            this.shield = (int)MathHelper.Clamp(shield, 0, maxShield);
            this.engine = (int)MathHelper.Clamp(engine, 0, maxEngine);
        }

        public virtual void Update(GameTime gt)
        {
            MouseInput();
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

        }

        public virtual void Draw(SpriteBatch sb)
        {
            schieten.Draw(sb);
            sb.Draw(currentTexture, position, Color.White);
            engineParticleSystem.Draw(sb);
        }
    }
}
