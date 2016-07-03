using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

enum PickupType
{
    Score = 0,
    Structure = 1,
    None = 2,
}

namespace Gaim
{
    abstract class Pickup
    {
        public Pickup(Vector2 position)
        {
        }

        protected Animatie animatie;
        protected Vector2 velocity;
        protected Vector2 position;

        public Vector2 Position
        {
            get { return this.position; }
        }

        public Rectangle BoundingBox
        {
            get { return animatie.BoundingBox; }
                
                //return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

        /*public Texture2D Sprite
        {
            get { return this.sprite; }
        }*/

        public abstract void Effect();

        public virtual void Update(GameTime gt)
        {
            this.position += velocity;
            animatie.Update(position, gt);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            animatie.Draw(sb);
            //sb.Draw(sprite, position, Color.White);
        }
    }

    class ScorePickup : Pickup
    {
        public ScorePickup(Vector2 position)
            : base (position)
        {
            this.animatie = new Animatie(Global.Game.Content.Load<Texture2D>("AnimatieTest"), true);
            //this.sprite = cm.Load<Texture2D>("Ships/Phoenix/straight");
            this.velocity = new Vector2 (0, 6);
            this.position = position;
        }

        public override void Effect()
        {
            Global.Level.Score += 1000;
        }
    }

    class StructurePickup : Pickup
    {
        public StructurePickup( Vector2 position)
            : base(position)
        {
            this.animatie = new Animatie(Global.Game.Content.Load<Texture2D>("AnimatieTest"), true);
            //this.sprite = cm.Load<Texture2D>("Ships/Test2/straight");
            this.velocity = new Vector2(0, 6);
            this.position = position;
        }

        public override void Effect()
        {
            Global.Player.Structure += 1000;
        }
    }
}
