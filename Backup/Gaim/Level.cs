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
    partial class Level
    {
        protected Game g;

        protected Player player;
        protected Background bgManager;
        protected List<Enemy> enemies;
        protected List<Pickup> pickups;
        protected List<ParticleEngine> impactEffects;
        protected List<Texture2D> starImpact;
        protected int score;
        protected TimeSpan levelTime;
        public bool win = false;

        public Level(Game g, bool newGame, string levelNumber)
        {
            this.pickups = new List<Pickup>();
            this.impactEffects = new List<ParticleEngine>();
            this.starImpact = new List<Texture2D>();
            ImpactEffectsInit(g.Content);
            this.enemies = new List<Enemy>();
            this.g = g;
            LoadLevel(levelNumber, newGame);
            levelTime = TimeSpan.Zero;
        }

        private void ImpactEffectsInit(ContentManager cm)
        {
            starImpact.Add(cm.Load<Texture2D>("Particles/Starimpact/star1"));
            starImpact.Add(cm.Load<Texture2D>("Particles/Starimpact/star2"));
            starImpact.Add(cm.Load<Texture2D>("Particles/Starimpact/star3"));
        }

        public Player Player
        {
            get { return this.player; }
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        public void CreatePickup(PickupType type, Vector2 position)
        {
            switch (type)
            {
                case PickupType.Score:
                    pickups.Add(new ScorePickup(g.Content, position));
                    break;
                case PickupType.Structure:
                    pickups.Add(new StructurePickup(g.Content, position));
                    break;
            }
        }
        public void Update(GameTime gt, Game g)
        {
            levelTime += gt.ElapsedGameTime;
            for (int i = 0; i < spawnEnemyTimes.Count; i++)
                if (spawnEnemyTimes[i] < levelTime.Seconds)
                {
                    SpawnEnemy(spawnEnemyTypes[i], spawnEnemyLocationsX[i], spawnEnemyLocationsY[i]);
                    spawnEnemyTimes.RemoveAt(i);
                    spawnEnemyLocationsX.RemoveAt(i);
                    spawnEnemyLocationsY.RemoveAt(i);
                    spawnEnemyTypes.RemoveAt(i);
                    if (i != 0)
                        i--;
                }
                else i = spawnEnemyTimes.Count;

            Random r = new Random();
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(player, gt);
                if (enemies[i].Position.Y > 750)
                {
                    enemies.RemoveAt(i);
                    //enemies.Add(new TardEnemy(tardSprite, new Vector2(r.Next(0, 550), r.Next(-100, 0)), g));
                    i--;
                }

                else if (enemies[i].Health <= 0)
                {
                    CreatePickup(enemies[i].PickupType, enemies[i].Position);
                    enemies.RemoveAt(i);
                    //enemies.Add(new FiringTard(tardSprite, enemyBullet, new Vector2(r.Next(0, 550), r.Next(-100, 0)), g)); ;
                    i--;
                }

                else
                {
                    if (enemies[i].BoundingBox.Intersects(player.BoundingBox))
                    {
                        player.Damage(125, true);
                    }
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].CanShoot == true)
                    for (int f = 0; f < enemies[i].kogels.Count; f++)
                        if ((enemies[i].kogels[f].MiddlePosition - player.MiddlePosition).Length() < 100)
                            if (enemies[i].kogels[f].BoundingBox.Intersects(player.BoundingBox))
                            {
                                player.Damage(enemies[i].kogels[f].Damage, false);
                                enemies[i].kogels.RemoveAt(f);
                                f--;
                            }
            }

            for (int j = 0; j < player.Schieten.bullets.Count; j++)
            {
                for (int k = 0; k < enemies.Count; k++)
                {
                    if ((player.Schieten.bullets[j].MiddlePosition - enemies[k].MiddlePosition).Length() < 100)
                        if (player.Schieten.bullets[j].BoundingBox.Intersects(enemies[k].BoundingBox))
                        {
                            enemies[k].Health -= player.Schieten.bullets[j].Damage;
                            Vector2 impactPosition = new Vector2(
                                player.Schieten.bullets[j].BoundingBox.Center.X + enemies[k].BoundingBox.Center.X,
                                player.Schieten.bullets[j].BoundingBox.Center.Y + enemies[k].BoundingBox.Center.Y) / 2;
                            impactEffects.Add(new DirectedBurstParticleEngine(
                                starImpact, impactPosition, 5, 1f, Color.White, 0, 1.5f, ParticleType.standaard, 1, player.Schieten.bullets[j].Velocity * 0.1f));
                            player.Schieten.bullets.RemoveAt(j);
                            j--;
                            k = enemies.Count;
                        }
                }
            }

            for (int i = 0; i < pickups.Count; i++)
            {
                pickups[i].Update(gt);
                if (pickups[i].BoundingBox.Intersects(player.BoundingBox))
                {
                    pickups[i].Effect(this);
                    pickups.RemoveAt(i);
                    i--;
                }
                else if (pickups[i].Position.Y > 800)
                {
                    pickups.RemoveAt(i);
                    i--;
                }
            }

            for (int p = 0; p < impactEffects.Count; p++)
            {
                impactEffects[p].Update(true, 0);
                if (impactEffects[p].engineTtl <= 0)
                {
                    impactEffects.RemoveAt(p);
                    p--;
                }
            }

            player.Update(gt);
            bgManager.Update(gt);
            if (enemies.Count == 0 && spawnEnemyTimes.Count == 0)
                win = true;
        }

        public void Draw(SpriteBatch sb)
        {
            bgManager.Draw(sb);
            foreach (Pickup pickup in pickups)
                pickup.Draw(sb);
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].Draw(sb);
            for (int h = 0; h < impactEffects.Count; h++)
                impactEffects[h].Draw(sb);
            player.Draw(sb);
        }
    }
}
