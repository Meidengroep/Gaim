using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

enum ParticleType
{
    standaard = 0,
    stil = 1,
    touw = 2,
}

abstract class Particle : ICloneable
{
    public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
    public Vector2 Position { get; set; }        // The current position of the particle        5
    public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
    public float Angle { get; set; }            // The current angle of rotation of the particle
    public float AngularVelocity { get; set; }    // The speed that the angle is changing
    public Color Color { get; set; }            // The color of the particle
    public float Size { get; set; }                // The size of the particle
    public int TTL { get; set; }                // The 'time to live' of the particle

    public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
    {
        Texture = texture;
        Position = position;
        Velocity = velocity;
        Angle = angle;
        AngularVelocity = angularVelocity;
        Color = color;
        Size = size;
        TTL = ttl;
    }

    protected abstract void Behavior();

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void Update()
    {
        Behavior();
        TTL--;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

        spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
            Angle, origin, Size, SpriteEffects.None, 0f);
    }
}

class StandaardParticle : Particle
{
    public StandaardParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
        : base(texture, position, velocity, angle, angularVelocity, color, size, ttl)
    {
        this.maxsize = size;
        this.maxvelocity = velocity;
    }

    private float maxsize;
    private Vector2 maxvelocity;

    protected override void Behavior()
    {
        if (Size > 0)
            this.Size -= (maxsize / TTL);
        if (Velocity.Length() > 0.1f)
            Velocity -= (maxvelocity / TTL);
        Angle += AngularVelocity;
        Position += Velocity;
    }
}

class StilstaandeParticle : Particle
{
    public StilstaandeParticle(Texture2D texture, Vector2 position, Vector2 velocity,
        float angle, float angularVelocity, Color color, float size, int ttl)
        : base(texture, position, velocity, angle, angularVelocity, color, size, ttl)
    {
        this.Velocity = Vector2.Zero;
        this.AngularVelocity = 0;
        Random r = new Random();
        this.Angle = (float)((Math.PI * 2) * r.NextDouble());
        this.TTL = 10 + r.Next(10);
    }

    protected override void Behavior()
    {

    }
}

class TouwParticle : Particle
{
    public TouwParticle(Texture2D texture, Vector2 position, Vector2 velocity,
        float angle, float angularVelocity, Color color, float size, int ttl, float rotation)
        : base(texture, position, velocity, angle, angularVelocity, color, size, ttl)
    {
        this.Velocity = Vector2.Zero;
        this.AngularVelocity = 0;
        this.Angle = rotation;
        this.Size = 1;
        this.TTL = 100;
    }

    protected override void Behavior()
    {
    }
}

class ParticleEngine : ICloneable
{
    protected Random random;
    public Vector2 EmitterLocation { get; set; }
    public List<Particle> particles;
    protected List<Texture2D> textures;
    protected int total;
    protected float particleSnelheid, sizeFactor;
    protected Color color;
    protected int delay;
    protected int counter;
    protected ParticleType type;
    public int engineTtl;

    public ParticleEngine(List<Texture2D> textures, Vector2 location, int total, float particleSnelheid, Color color, int delay, float size, ParticleType type)
    {
        this.type = type;
        this.particleSnelheid = particleSnelheid;
        this.total = total;
        EmitterLocation = location;
        this.textures = textures;
        this.particles = new List<Particle>();
        random = new Random();
        this.color = color;
        this.delay = delay;
        this.sizeFactor = size;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    protected Particle GenerateNewParticle(float rotation)
    {
        Texture2D texture = textures[random.Next(textures.Count)];
        Vector2 position = EmitterLocation;
        Vector2 velocity = particleSnelheid * new Vector2(
                1f * (float)(random.NextDouble() * 2 - 1),
                1f * (float)(random.NextDouble() * 2 - 1));
        float angle = 0;
        float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
        float size = sizeFactor * (float)random.NextDouble();
        int ttl = 10 + random.Next(40);

        if (type == ParticleType.standaard)
        {
            return new StandaardParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        if (type == ParticleType.stil)
        {
            return new StilstaandeParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        if (type == ParticleType.touw)
        {
            return new TouwParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, rotation);
        }
        return new StandaardParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
    }

    public virtual void Update(bool addParticles, float rotation)
    {
        counter++;
        if (addParticles && (counter > delay))
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle(rotation));
            }

        for (int particle = 0; particle < particles.Count; particle++)
        {
            particles[particle].Update();
            if (particles[particle].TTL <= 0)
            {
                particles.RemoveAt(particle);
                particle--;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        spriteBatch.Begin(SpriteBlendMode.Additive);
        for (int index = 0; index < particles.Count; index++)
        {
            particles[index].Draw(spriteBatch);
        }
        spriteBatch.End();
        spriteBatch.Begin();
    }
}

class BurstParticleEngine : ParticleEngine
{
    public BurstParticleEngine(List<Texture2D> textures, Vector2 location, int total, float particleSnelheid, Color color, int delay, float size, ParticleType type, int bursts)
        : base(textures, location, total, particleSnelheid, color, delay, size, type)
    {
        this.counter = bursts;
        this.engineTtl = 52;
    }

    public override void Update(bool addParticles, float rotation)
    {
        if (addParticles && (counter > 0))
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle(rotation));
            }

        for (int particle = 0; particle < particles.Count; particle++)
        {
            particles[particle].Update();
            if (particles[particle].TTL <= 0)
            {
                particles.RemoveAt(particle);
                particle--;
            }
        }
        counter--;
        this.engineTtl--;
    }
}

class DirectedParticleEngine : ParticleEngine
{
    public DirectedParticleEngine(List<Texture2D> textures, Vector2 location, int total, float particleSnelheid, Color color, int delay, float size, ParticleType type)
        : base(textures, location, total, particleSnelheid, color, delay, size, type)
    {
    }

    protected Particle GenerateNewParticle(float rotation, Vector2 direction)
    {
        Texture2D texture = textures[random.Next(textures.Count)];
        Vector2 position = EmitterLocation;
        Vector2 velocity = direction;
        float angle = 0;
        float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
        float size = sizeFactor * (float)random.NextDouble();
        int ttl = 10 + random.Next(40);

        if (type == ParticleType.standaard)
        {
            return new StandaardParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        if (type == ParticleType.stil)
        {
            return new StilstaandeParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        if (type == ParticleType.touw)
        {
            return new TouwParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, rotation);
        }
        return new StandaardParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
    }

    public virtual void Update(bool addParticles, float rotation, Vector2 direction)
    {
        counter++;
        if (addParticles && (counter > delay))
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle(rotation, direction));
            }

        for (int particle = 0; particle < particles.Count; particle++)
        {
            particles[particle].Update();
            if (particles[particle].TTL <= 0)
            {
                particles.RemoveAt(particle);
                particle--;
            }
        }
    }   
}

class DirectedBurstParticleEngine : ParticleEngine
{
    public DirectedBurstParticleEngine(List<Texture2D> textures, Vector2 location, int total, float particleSnelheid, Color color, int delay, float size, ParticleType type, int bursts, Vector2 direction)
        : base(textures, location, total, particleSnelheid, color, delay, size, type)
    {
        this.counter = bursts;
        this.engineTtl = 52;
        this.direction = direction;
    }

    protected Vector2 direction;

    protected Particle GenerateNewParticle(float rotation, Vector2 direction)
    {
        Texture2D texture = textures[random.Next(textures.Count)];
        Vector2 position = EmitterLocation;
        Vector2 velocity = direction;
        float angle = 0;
        float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
        float size = sizeFactor * (float)random.NextDouble();
        int ttl = 10 + random.Next(40);

        if (type == ParticleType.standaard)
        {
            return new StandaardParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        if (type == ParticleType.stil)
        {
            return new StilstaandeParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        if (type == ParticleType.touw)
        {
            return new TouwParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, rotation);
        }
        return new StandaardParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
    }

    public override void Update(bool addParticles, float rotation)
    {
        if (addParticles && (counter > 0))
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle(rotation, direction));
            }

        for (int particle = 0; particle < particles.Count; particle++)
        {
            particles[particle].Update();
            if (particles[particle].TTL <= 0)
            {
                particles.RemoveAt(particle);
                particle--;
            }
        }
        counter--;
        this.engineTtl--;
    }
}